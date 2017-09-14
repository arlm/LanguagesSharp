namespace TIS100Sharp.Operators
{
    public class Reference : Operator
    {
        public string Name { get; private set; }

        public Reference(string name)
        {
            this.Name = name;
        }
    }
}
