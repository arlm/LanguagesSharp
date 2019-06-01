using System;
namespace BabelFish.Compiler
{
    internal class ByteCodeGenerationException : Exception
    {
        public object CompilationResult { get; set; }
        public string Value { get; set; }

        public ByteCodeGenerationException()
        {
        }

        public ByteCodeGenerationException(string message) : base(message)
        {
        }

        public ByteCodeGenerationException(string value, object compilationResult)
        {
            this.Value = value;
            this.CompilationResult = compilationResult;
        }

        public ByteCodeGenerationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
