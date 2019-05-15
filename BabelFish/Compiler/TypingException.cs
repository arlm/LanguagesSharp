using System;

namespace BabelFish.Compiler
{
    public class TypingException : Exception
    {
        public TypingException(string message) : base(message)
        {
        }
    }
}
