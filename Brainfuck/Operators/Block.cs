using System.Collections.Generic;
using System.Linq;
using Brainfuck.Runtime;

namespace Brainfuck.Operators
{
    public sealed class Block : Operator
    {
        public List<Operator> Closure { get; private set; }

        public Block(IEnumerable<Operator> tokens) : base(BrainfuckToken.OPEN_BRACKET)
        {
            this.Closure = tokens.ToList();
        }

        public override void Execute()
        {
            while (Pointer.Instance.Data != 0)
            {
                foreach (var op in Closure)
                {
                    op.Execute();
                }
            }
        }
    }
}
