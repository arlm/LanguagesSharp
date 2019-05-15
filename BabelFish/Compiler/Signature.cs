using System;
namespace BabelFish.Compiler
{
    public class Signature<T> where T : Enum
    {
        private readonly T Left;
        public T Result;
        private readonly T Right;

        public Signature(T left, T right, T result)
        {
            Left = left;
            Right = right;
            Result = result;
        }

        public bool Match(T l, T r)
        {
            return (Left.Equals(default(T)) || l.Equals(Left)) &&
                   (Right.Equals(default(T)) || r.Equals(Right));
        }
    }
}
