using System;
namespace TIS100Sharp.Operators
{
    public class Move : Operator
    {
		public Operand Source { get; private set; }
		public Operand Destination { get; private set; }

        public Move(Operand source, Operand destination)
        {
            this.Source = source;
            this.Destination = destination;
        }
    }
}
