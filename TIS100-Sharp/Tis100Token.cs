using System;
namespace TIS100Sharp
{
    public enum Tis100Token
    {
		TEXT = 1,
		STRING,
		INT,
        OP,
        OP_JUMP,
        OP_1PARAM,
		OP_2PARAM,
		SOURCE,
		COMMA,
        COLON,
        EXCLAMATION,
        HASH,
		NIL,
        WHITE_SPACE,
        EOL
    }
}
