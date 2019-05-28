using BabelFish.AST;
using sly.lexer;

namespace enquanto
{
	public enum EnquantoToken
    {
        #region keywords 0 -> 19

        [Lexeme("(então)")] THEN = 1,

        [Lexeme("(senão)")] ELSE = 2,

        [Lexeme("(se)")] IF = 3,

        [Lexeme("(enquanto)")] WHILE = 4,

        [Lexeme("(faça)")] DO = 5,

        [Lexeme("(pule)")] SKIP = 6,

        [Lexeme("(verdadeiro)")] [BooleanValue(true)] TRUE = 7,

        [Lexeme("(falso)")] [BooleanValue(false)] FALSE = 8,

        [Lexeme("(não)")] NOT = 9,

        [Lexeme("(e)")] AND = 10,

        [Lexeme("(ou)")] OR = 11,

        [Lexeme("(imprima)")] PRINT = 12,

        [Lexeme("(retorne)")] RETURN = 13,

        #endregion

        #region literals 20 -> 29

        [Lexeme("[a-zA-Z]+")] IDENTIFIER = 20,

        [Lexeme("\"[^\"]*\"")] STRING = 21,

        [Lexeme("[0-9]+")] INT = 22,

        #endregion

        #region operators 30 -> 49

        [Lexeme(">")] GREATER = 30,

        [Lexeme("<")] LESSER = 31,

        [Lexeme("==")] EQUALS = 32,

        [Lexeme("!=")] DIFFERENT = 33,

        [Lexeme("\\.")] CONCAT = 34,

        [Lexeme(":=")] ASSIGN = 35,

        [Lexeme("\\+")] PLUS = 36,

        [Lexeme("\\-")] MINUS = 37,


        [Lexeme("\\*")] TIMES = 38,

        [Lexeme("\\/")] DIVIDE = 39,

        #endregion

        #region sugar 50 ->

        [Lexeme("\\(")] LPAREN = 50,

        [Lexeme("\\)")] RPAREN = 51,

        [Lexeme(";")] SEMICOLON = 52,

        [Lexeme("[ \\t]+", true)] WS = 53,

        [Lexeme("[\\n\\r]+", true, true)] EOL = 54,

        EOF = 0

        #endregion
    }
}
