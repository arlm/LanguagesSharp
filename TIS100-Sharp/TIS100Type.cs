using System;
using BabelFish.Compiler;

namespace TIS100Sharp
{
    public enum TIS100Type
    {
        [ByteCode(ByteCode.IL, Type = typeof(object))]
        ANY = 0,

        [ByteCode(ByteCode.IL, Type = typeof(bool))]
        BOOL,

        [ByteCode(ByteCode.IL, Type = typeof(int))]
        INT,

        [ByteCode(ByteCode.IL, Type = typeof(string))]
        STRING,

        [ByteCode(ByteCode.IL, Type = typeof(DBNull))]
        NONE
    }
}
