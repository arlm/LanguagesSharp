using System.Collections.Generic;
using System.Linq;
using BabelFish.AST;
using Brainfuck.Operators;
using sly.parser;
using sly.parser.generator;
using sly.lexer;

namespace Brainfuck
{
    public class Parser
    {
        public static Parser<BrainfuckToken, INode<BrainfuckType>> Build()
        {
            var parserInstance = new Parser();
            var builder = new ParserBuilder<BrainfuckToken, INode<BrainfuckType>>();
            var result = builder.BuildParser(parserInstance, ParserType.EBNF_LL_RECURSIVE_DESCENT, "sequence");

            return result.Result;
        }

        [Production("sequence : statement sequence ")]
        public SequenceStatement Sequence(IStatement<BrainfuckType> first, SequenceStatement next)
        {
            var sequence = new SequenceStatement(first);
            sequence?.AddRange(next?.Statements);
            return sequence;
        }

        [Production("sequence : statement ")]
        public SequenceStatement Sequence(IStatement<BrainfuckType> statement) => statement is SequenceStatement ?  statement as SequenceStatement : new SequenceStatement(statement);

        [Production("primStatement : GREATER_THAN ")]
        public INode<BrainfuckType> IncrementPointer(Token<BrainfuckToken> op) => new Operation(MemoryOperator.INCREMENT_POINTER);

        [Production("primStatement : LESSER_THAN ")]
        public INode<BrainfuckType> DecrementPointer(Token<BrainfuckToken> op) => new Operation(MemoryOperator.DECREMENT_POINTER);

        [Production("primStatement : PLUS ")]
        public INode<BrainfuckType> IncrementData(Token<BrainfuckToken> op) => new Operation(MemoryOperator.INCREMENT_VALUE);

        [Production("primStatement : MINUS ")]
        public INode<BrainfuckType> DecrementData(Token<BrainfuckToken> op) => new Operation(MemoryOperator.DECREMENT_VALUE);

        [Production("primStatement : DOT ")]
        public INode<BrainfuckType> OutputData(Token<BrainfuckToken> op) => new PrintStatement();

        [Production("primStatement : COMMA ")]
        public INode<BrainfuckType> InputData(Token<BrainfuckToken> op) => new ReadStatement();

        [Production("statement : primStatement+")]
        public IStatement<BrainfuckType> Statement(IEnumerable<INode<BrainfuckType>> ops) => new SequenceStatement(ops.Cast<IStatement<BrainfuckType>>());

        [Production("statement : OPEN_BRACKET [d] sequence CLOSE_BRACKET [d] ")]
        public IStatement<BrainfuckType> JumpIfZero(SequenceStatement statement) => new BlockStatement(statement);

        [Production("statement : OPEN_BRACKET [d] CLOSE_BRACKET [d] ")]
        public IStatement<BrainfuckType> JumpIfZero() => new BlockStatement(new SkipStatement());
    }
} 
