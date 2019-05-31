using System;

namespace BabelFish.AST
{
    public class TypingException : Exception
    {
        public TypingException(string message) : base(message)
        {
        }
    }
}
