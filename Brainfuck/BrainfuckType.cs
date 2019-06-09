using System;
using BabelFish.Compiler;

namespace Brainfuck
{
    public enum BrainfuckType
    {
        [ByteCode(ByteCode.IL, Type = typeof(object))]
        ANY = 0,

        [ByteCode(ByteCode.IL, Type = typeof(byte))]
        BYTE,

        [ByteCode(ByteCode.IL, Type = typeof(DBNull))]
        NONE
    }
}