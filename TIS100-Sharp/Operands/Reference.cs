namespace TIS100Sharp.Operands
{
    public class Reference : Operand
    {
        public string Name { get; private set; }

        public Reference(string name)
        {
            this.Name = name;
        }
    }
}
