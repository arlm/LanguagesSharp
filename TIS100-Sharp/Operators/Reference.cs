using System;
namespace TIS100Sharp.Operands
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
