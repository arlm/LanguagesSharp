using System.Collections.Generic;
using System.Linq;
using Brainfuck.Operators;
using sly.lexer;
using sly.parser;
using sly.parser.generator;

namespace Brainfuck
{
    public class BrainfuckParser
    {
        public static Parser<BrainfuckToken> Build()
        {
            BrainfuckParser parserInstance = new BrainfuckParser();
            ParserBuilder builder = new ParserBuilder();

            var parser = builder.BuildParser<BrainfuckToken>(parserInstance, ParserType.EBNF_LL_RECURSIVE_DESCENT, "program");

            return parser;
        }

        [LexerConfiguration]
        public ILexer<BrainfuckToken> BuildLexer(ILexer<BrainfuckToken> lexer)
        {
            lexer.AddDefinition(new TokenDefinition<BrainfuckToken>(BrainfuckToken.EOL, "[\\n\\r]+", true, true));
            lexer.AddDefinition(new TokenDefinition<BrainfuckToken>(BrainfuckToken.WHITE_SPACE, "[ \\t]+", true));
            lexer.AddDefinition(new TokenDefinition<BrainfuckToken>(BrainfuckToken.TEXT, "[^\\n\\r \\t<>\\+\\-\\.\\,\\[\\]]+", true));
            lexer.AddDefinition(new TokenDefinition<BrainfuckToken>(BrainfuckToken.GREATER_THAN, ">"));
            lexer.AddDefinition(new TokenDefinition<BrainfuckToken>(BrainfuckToken.LESSER_THAN, "<"));
            lexer.AddDefinition(new TokenDefinition<BrainfuckToken>(BrainfuckToken.PLUS, "\\+"));
            lexer.AddDefinition(new TokenDefinition<BrainfuckToken>(BrainfuckToken.MINUS, "\\-"));
            lexer.AddDefinition(new TokenDefinition<BrainfuckToken>(BrainfuckToken.DOT, "\\."));
            lexer.AddDefinition(new TokenDefinition<BrainfuckToken>(BrainfuckToken.COMMA, "\\,"));
            lexer.AddDefinition(new TokenDefinition<BrainfuckToken>(BrainfuckToken.OPEN_BRACKET, "\\["));
            lexer.AddDefinition(new TokenDefinition<BrainfuckToken>(BrainfuckToken.CLOSE_BRACKET, "\\]"));
            return lexer;
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

        [Production("while : OPEN_BRACKET CLOSE_BRACKET ")]
        public Block JumpIfZero(Token<BrainfuckToken> open, Token<BrainfuckToken> close) => new Block(Enumerable.Empty<Operator>());

        [Production("while : OPEN_BRACKET operation+ CLOSE_BRACKET ")]
        public Block JumpIfZero(Token<BrainfuckToken> open, List<object> closure, Token<BrainfuckToken> close) => new Block(closure.Cast<Operator>());
    }
}
