using System;
namespace TIS100Sharp.Operators
{
    public class Subtract : Operator
    {
		public Operand Operand { get; private set; }

		public Subtract(Operand op)
		{
			this.Operand = op;
		}
    }
}
