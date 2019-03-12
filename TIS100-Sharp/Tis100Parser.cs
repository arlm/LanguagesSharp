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
        public static Parser<Tis100Token, List<Operator>> Build()
		{
            var parserInstance = new Tis100Parser();
			var builder = new ParserBuilder<Tis100Token, List<Operator>>();

			var parser = builder.BuildParser(parserInstance, ParserType.EBNF_LL_RECURSIVE_DESCENT,  "program");

            return parser.Result;
		}

        [Production("program : operator * ")]
        public List<Operator> ProgramValue(List<object> tokens)
        {
            return tokens.Cast<Operator>().ToList();
        }

		[Production("reference : REFERENCE ")]
        public Operands.Reference ReferenceValue(Token<Tis100Token> referenceToken)
		{
            return new Operands.Reference(referenceToken.StringWithoutQuotes);
		}

		[Production("value : INT ")]
        public Literal IntValue(Token<Tis100Token> intToken)
		{
            return new Literal(intToken.IntValue);
		}

		[Production("value : NIL [d] ")]
		public Literal NillValue()
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

		[Production("operator : REFERENCE COLON [d] ")]
        public Operators.Reference LabelValue(Token<Tis100Token> labelToken)
		{
            return new Operators.Reference(labelToken.StringWithoutQuotes);
		}

		//[Production("comment : HASH [d] ")]
  //      public string CommentValue(List<Token<Tis100Token>> textTokens, object terminator)
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

		[Production("operator : OP_2PARAM value COMMA [d] value ")]
        public Operator DoubleOperatorValue(Token<Tis100Token> operandToken, Operand source, Operand destination)
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
