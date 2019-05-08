using System;
using Brainfuck.Runtime;

namespace Brainfuck.Operators
{
    public class Operator
    {
        public BrainfuckToken Token { get; private set; }

        public Operator(BrainfuckToken token) => this.Token = token;

        public virtual void Execute()
        {
            switch (Token)
            {
                case BrainfuckToken.GREATER_THAN:
                    Pointer.Instance.Increment();
                    break;
                case BrainfuckToken.LESSER_THAN:
                    Pointer.Instance.Decrement();
                    break;
                case BrainfuckToken.PLUS:
                    Pointer.Instance.IncrementData();
                    break;
                case BrainfuckToken.MINUS:
                    Pointer.Instance.DecrementData();
                    break;
                case BrainfuckToken.DOT:
                    IO.Instance.Out.Write(Pointer.Instance.Data);
                    //Console.Out.Write(Pointer.Instance.Data);
                    break;
                case BrainfuckToken.COMMA:
                    var input = IO.Instance.In.Read();
                    //var input = Console.In.Read();
                    Pointer.Instance.Data = unchecked((byte)input);
                    break;
                default:
                    throw new ArgumentException($"Not possible to run Token: {Token:G}", nameof(Token));
            }
        }
    }
}
