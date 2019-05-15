using System;

namespace BabelFish.Compiler
{
    public class SignatureException : TypingException
    {
        public SignatureException(string message) : base(message)
        {
        }
    }
}
