using sly.lexer;

namespace TIS100Sharp
{
    public enum Tis100Token
    {
        NOP = 1,

        [Lexeme(GenericToken.Int)]
        INT,

        [Lexeme(GenericToken.KeyWord, "NOP", "SWP", "SAV", "NEG")]
        [Lexeme(GenericToken.KeyWord, "nop", "swp", "sav", "neg")]
        OP,

        [Lexeme(GenericToken.KeyWord, "JMP", "JEZ", "JNZ", "JGZ", "JLZ")]
        [Lexeme(GenericToken.KeyWord, "jmp", "jez", "jnz", "jgz", "jlz")]
        OP_JUMP,

        [Lexeme(GenericToken.KeyWord, "ADD", "SUB", "JRO")]
        [Lexeme(GenericToken.KeyWord, "add", "sub", "jro")]
        OP_1PARAM,

        [Lexeme(GenericToken.KeyWord, "MOV")]
        [Lexeme(GenericToken.KeyWord, "mov")]
        OP_2PARAM,

        [Lexeme(GenericToken.KeyWord, "ANY", "LAST", "ACC", "UP", "DOWN", "LEFT", "RIGHT")]
        [Lexeme(GenericToken.KeyWord, "any", "last", "acc", "up", "down", "left", "right")]
        SOURCE,

        [Lexeme(GenericToken.SugarToken, ",")]
        COMMA,

        [Lexeme(GenericToken.Identifier, IdentifierType.AlphaNumeric)]
        REFERENCE,

        [Lexeme(GenericToken.SugarToken, ":")]
        COLON,

        [Lexeme(GenericToken.SugarToken, "!")]
        EXCLAMATION,

        [Comment("#", null, null)]
        //[Comment("#", "/*", "*/")]
        HASH,

        [Lexeme(GenericToken.KeyWord, "NIL")]
        [Lexeme(GenericToken.KeyWord, "nil")]
        NIL,

        EOF = 0
    }
}
