namespace TIS100Sharp.Operators
{
    public class Add : Operator
    {
		public Operand Operand { get; private set; }

        public Add(Operand op)
        {
            this.Operand = op;
        }
    }
}
