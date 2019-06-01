using System;
namespace BabelFish.Compiler
{
    public class ByteCodeAttribute : Attribute
    {
        public ByteCode ByteCode { get; }
        public Type Type { get; set; }
        public byte[] OpCode { get; set; }

        public ByteCodeAttribute(ByteCode byteCode)
        {
            this.ByteCode = byteCode;
        }
    }
}
