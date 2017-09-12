using System;
using System.Collections.Generic;
using System.Linq;
using sly.lexer;
using sly.parser;
using sly.parser.generator;
using TIS100Sharp.Operands;
using TIS100Sharp.Operators;

namespace TIS100Sharp
{
    public class Tis100Parser
    {
        public static Parser<Tis100Token> Build()
		{
            Tis100Parser parserInstance = new Tis100Parser();
			ParserBuilder builder = new ParserBuilder();

			var parser = builder.BuildParser<Tis100Token>(parserInstance, ParserType.EBNF_LL_RECURSIVE_DESCENT,  "operator");

            return parser;
		}

        [LexerConfiguration]
        public ILexer<Tis100Token> BuildJsonLexer(ILexer<Tis100Token> lexer)
		{
			lexer.AddDefinition(new TokenDefinition<Tis100Token>(Tis100Token.STRING, "([A-Z_]*?)"));
			lexer.AddDefinition(new TokenDefinition<Tis100Token>(Tis100Token.INT, "(-?[0-9]+)"));
            lexer.AddDefinition(new TokenDefinition<Tis100Token>(Tis100Token.NIL, "(NIL)"));
            lexer.AddDefinition(new TokenDefinition<Tis100Token>(Tis100Token.OP, "([A-Z]){3}"));
            lexer.AddDefinition(new TokenDefinition<Tis100Token>(Tis100Token.SOURCE, "(ANY|LAST|ACC|UP|DOWN|LEFT|RIGHT)"));
			lexer.AddDefinition(new TokenDefinition<Tis100Token>(Tis100Token.COMMA, ","));
            lexer.AddDefinition(new TokenDefinition<Tis100Token>(Tis100Token.COLON, ":"));
			lexer.AddDefinition(new TokenDefinition<Tis100Token>(Tis100Token.HASH, "#"));
            lexer.AddDefinition(new TokenDefinition<Tis100Token>(Tis100Token.WHITE_SPACE, "[ \\t]+", true));
			lexer.AddDefinition(new TokenDefinition<Tis100Token>(Tis100Token.EOL, "[\\n\\r]+", true, true));
			return lexer;
		}

		[Production("value : STRING ")]
		public string StringValue(Token<Tis100Token> stringToken)
		{
            return stringToken.StringWithoutQuotes;
		}

		[Production("value : INT ")]
		public int IntValue(Token<Tis100Token> intToken)
		{
			return intToken.IntValue;
		}

		[Production("value : NIL ")]
		public Literal NillValue(object forget)
		{
			return null;
		}

		[Production("value : SOURCE ")]
        public Operand SourceValue(Token<Tis100Token> sourceToken)
		{
            if (Enum.IsDefined(typeof(Port.Available), sourceToken.StringWithoutQuotes))
            {
                return new Port(sourceToken.StringWithoutQuotes);
            }

            return new Register(sourceToken.StringWithoutQuotes);
        }

		[Production("operator : STRING COLON ")]
        public Reference LabelValue(Token<Tis100Token> labelToken, object forget)
		{
            return new Reference(labelToken.StringWithoutQuotes);
		}

		//[Production("comment : HASH TEXT* EOL")]
  //      public string CommentValue(object forget, List<Token<Tis100Token>> textTokens, object terminator)
		//{
  //          return string.Join(" ", textTokens.Select(t => t.StringWithoutQuotes));
		//}

		[Production("operator : OP ")]
        public Operator SimpleOperatorValue(Token<Tis100Token> operandToken)
		{
			switch (operandToken.StringWithoutQuotes)
			{
                case "NOP":
                    return new Add(null);
				case "SWP":
					return new Swap();
				case "SAV":
                    return new Move(new Register("ACC"), new Register("BAK"));
				case "NEG":
                    return new Negate();
				default:
					throw new UnknownOperator(operandToken.StringWithoutQuotes);
			}
		}

		[Production("operator : OP value ")]
        public Operator SingleOperatorValue(Token<Tis100Token> operandToken, Operand source)
		{
			switch (operandToken.StringWithoutQuotes)
			{
				case "ADD":
					return new Add(source);
				case "SUB":
					return new Subtract(source);
				case "JMP":
					return new Jump(source);
				case "JEZ":
                    return new Jump(source, Jump.ConditionType.Zero);
				case "JNZ":
                    return new Jump(source, Jump.ConditionType.NotZero);
				case "JGZ":
                    return new Jump(source, Jump.ConditionType.GreaterThanZero);
				case "JLZ":
                    return new Jump(source, Jump.ConditionType.LesserThanZero);
				case "JRO":
                    return new Jump(source, Jump.ConditionType.Offset);
                default:
					throw new UnknownOperator(operandToken.StringWithoutQuotes);
			}
		}

		[Production("operator : OP value COMMA value ")]
        public Operator DoubleOperatorValue(Token<Tis100Token> operandToken, Operand source, object forget, Operand destination)
		{
            switch (operandToken.StringWithoutQuotes)
            {
                case "MOV":
                    return new Move(source, destination);
                default:
                    throw new UnknownOperator(operandToken.StringWithoutQuotes);
            }
		}
    }

    public class ParsingError : Exception
    {
        public ParsingError(string message) : base(message) {}
    }

	public class UnknownOperator : ParsingError
	{
        public UnknownOperator(string op) : base($"Unknown operator: {op}") { }
	}
}
