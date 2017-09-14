namespace TIS100Sharp.Operands
{
    public class Literal : Operand
    {
        public int Value { get; private set; }

        public Literal(int value)
        {
            this.Value = value;
        }
    }
}
