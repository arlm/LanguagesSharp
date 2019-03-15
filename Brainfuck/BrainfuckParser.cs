﻿using System.Collections.Generic;
using System.Linq;
using Brainfuck.Operators;
using sly.lexer;
using sly.parser;
using sly.parser.generator;

namespace Brainfuck
{
    public class BrainfuckParser
    {
        public static Parser<BrainfuckToken, List<Operator>> Build()
        {
            var parserInstance = new BrainfuckParser();
            var builder = new ParserBuilder<BrainfuckToken, List<Operator>>();
            var result = builder.BuildParser(parserInstance, ParserType.EBNF_LL_RECURSIVE_DESCENT, "program");

            return result.Result;
        }

        [Production("program : operation* ")]
        public List<Operator> ProgramValue(List<object> tokens) => (tokens.Any() ? tokens.Cast<Operator>() : Enumerable.Empty<Operator>()).ToList();

        [Production("operation : pointer ")]
        [Production("operation : data ")]
        [Production("operation : io ")]
        [Production("operation : while ")]
        public Operator Operation(Operator op) => op;

        [Production("pointer : GREATER_THAN ")]
        public Operator IncrementPointer(Token<BrainfuckToken> token) => new Operator(token.TokenID);

        [Production("pointer : LESSER_THAN ")]
        public Operator DecrementPointer(Token<BrainfuckToken> token) => new Operator(token.TokenID);

        [Production("data : PLUS ")]
        public Operator IncrementData(Token<BrainfuckToken> token) => new Operator(token.TokenID);

        [Production("data : MINUS ")]
        public Operator DecrementData(Token<BrainfuckToken> token) => new Operator(token.TokenID);

        [Production("io : DOT ")]
        public Operator OutputData(Token<BrainfuckToken> token) => new Operator(token.TokenID);

        [Production("io : COMMA ")]
        public Operator InputData(Token<BrainfuckToken> token) => new Operator(token.TokenID);

        [Production("while : OPEN_BRACKET [d] CLOSE_BRACKET [d] ")]
        public Block JumpIfZero() => new Block(Enumerable.Empty<Operator>());

        [Production("while : OPEN_BRACKET [d] operation+ CLOSE_BRACKET [d] ")]
        public Block JumpIfZero(List<object> closure) => new Block(closure.Cast<Operator>());
    }
}
