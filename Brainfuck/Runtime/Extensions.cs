using System;
using System.Collections.Generic;
using Brainfuck.Operators;

namespace Brainfuck.Runtime
{
    public static class Extensions
    {
        public static void Execute(this IEnumerable<Operator> block)
        {
            foreach(var op in block)
            {
                op.Execute();
            }
        }
    }
}
