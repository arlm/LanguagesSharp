using sly.lexer;

namespace Brainfuck
{
    public enum BrainfuckToken
    {
        [Lexeme(">")] GREATER_THAN = 1,
        [Lexeme("<")] LESSER_THAN,
        [Lexeme("\\+")] PLUS,
        [Lexeme("\\-")] MINUS,
        [Lexeme("\\.")] DOT,
        [Lexeme(",")] COMMA,
        [Lexeme("\\[")] OPEN_BRACKET,
        [Lexeme("\\]")] CLOSE_BRACKET,

        [Lexeme("[\\n\\r]+", true, true)] EOL,
        [Lexeme("[ \\t]+", true)] WHITE_SPACE,
        [Lexeme("[^<>\\+\\-\\.,\\[\\]]+", true)] TEXT,
        EOF = 0
    }
}
