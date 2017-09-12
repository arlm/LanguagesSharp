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

			var parser = builder.BuildParser<Tis100Token>(parserInstance, ParserType.EBNF_LL_RECURSIVE_DESCENT,  "program");

            return parser;
		}

        [LexerConfiguration]
        public ILexer<Tis100Token> BuildJsonLexer(ILexer<Tis100Token> lexer)
		{
			lexer.AddDefinition(new TokenDefinition<Tis100Token>(Tis100Token.EOL, "[\\n\\r]+", true, true));
			lexer.AddDefinition(new TokenDefinition<Tis100Token>(Tis100Token.WHITE_SPACE, "[ \\t]+", true));
            lexer.AddDefinition(new TokenDefinition<Tis100Token>(Tis100Token.INT, "(-?[0-9]+)"));
            lexer.AddDefinition(new TokenDefinition<Tis100Token>(Tis100Token.SOURCE, "\\b(ANY|LAST|ACC|UP|DOWN|LEFT|RIGHT)\\b"));
            lexer.AddDefinition(new TokenDefinition<Tis100Token>(Tis100Token.NIL, "\\b(NIL)\\b"));
            lexer.AddDefinition(new TokenDefinition<Tis100Token>(Tis100Token.OP, "\\b(NOP|SWP|SAV|NEG)\\b"));
            lexer.AddDefinition(new TokenDefinition<Tis100Token>(Tis100Token.OP_JUMP, "\\b(JMP|JEZ|JNZ|JGZ|JLZ)\\b"));
            lexer.AddDefinition(new TokenDefinition<Tis100Token>(Tis100Token.OP_1PARAM, "\\b(ADD|SUB|JRO)\\b"));
            lexer.AddDefinition(new TokenDefinition<Tis100Token>(Tis100Token.OP_2PARAM, "\\b(MOV)\\b"));
            lexer.AddDefinition(new TokenDefinition<Tis100Token>(Tis100Token.COMMA, ","));
            lexer.AddDefinition(new TokenDefinition<Tis100Token>(Tis100Token.COLON, ":"));
            lexer.AddDefinition(new TokenDefinition<Tis100Token>(Tis100Token.EXCLAMATION, "!"));
            lexer.AddDefinition(new TokenDefinition<Tis100Token>(Tis100Token.HASH, "#"));
            lexer.AddDefinition(new TokenDefinition<Tis100Token>(Tis100Token.STRING, "\\b([A-Z_]+)\\b"));
			return lexer;
		}

        [Production("program : operator * ")]
        public List<Operator> ProgramValue(List<object> tokens)
        {
            return tokens.Cast<Operator>().ToList();
        }

		[Production("reference : STRING ")]
        public Operands.Reference StringValue(Token<Tis100Token> stringToken)
		{
            return new Operands.Reference(stringToken.StringWithoutQuotes);
		}

		[Production("value : INT ")]
        public Literal IntValue(Token<Tis100Token> intToken)
		{
            return new Literal(intToken.IntValue);
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
        public Operators.Reference LabelValue(Token<Tis100Token> labelToken, object forget)
		{
            return new Operators.Reference(labelToken.StringWithoutQuotes);
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

		[Production("operator : OP_1PARAM value ")]
        public Operator SingleOperatorValue(Token<Tis100Token> operandToken, Operand source)
		{
			switch (operandToken.StringWithoutQuotes)
			{
				case "ADD":
					return new Add(source);
				case "SUB":
					return new Subtract(source);
				case "JRO":
                    return new Jump(source, Jump.ConditionType.Offset);
                default:
					throw new UnknownOperator(operandToken.StringWithoutQuotes);
			}
		}

        [Production("operator : OP_JUMP reference ")]
        public Operator JumpOperatorValue(Token<Tis100Token> operandToken, Operand source)
        {
            switch (operandToken.StringWithoutQuotes)
            {
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
                default:
                    throw new UnknownOperator(operandToken.StringWithoutQuotes);
            }
        }

		[Production("operator : OP_2PARAM value COMMA value ")]
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
