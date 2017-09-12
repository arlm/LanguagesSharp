using System;
namespace TIS100Sharp
{
    public enum Tis100Token
    {
		TEXT = 1,
		STRING,
		INT,
		OP,
		SOURCE,
		COMMA,
		COLON,
        HASH,
		NIL,
        WHITE_SPACE,
        EOL
    }
}
