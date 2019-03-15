using sly.lexer;

namespace Brainfuck
{
    public enum BrainfuckToken
    {
        [Lexeme("[\\n\\r]+", true, true)] EOL,
        [Lexeme("[ \\t]+", true)] WHITE_SPACE,
        [Lexeme("[^\\n\\r \\t<>\\+\\-\\.,\\[\\]]+", true)] TEXT,

        [Lexeme(">")] GREATER_THAN = 1,
        [Lexeme("<")] LESSER_THAN,
        [Lexeme("\\+")] PLUS,
        [Lexeme("\\-")] MINUS,
        [Lexeme("\\.")] DOT,
        [Lexeme(",")] COMMA,
        [Lexeme("\\[")] OPEN_BRACKET,
        [Lexeme("\\]")] CLOSE_BRACKET
    }
}
