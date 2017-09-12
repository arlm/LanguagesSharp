using System;
namespace TIS100Sharp.Operands
{
    public class Register : Source<Register.Available>
    {
        public Register(string name)
        {
            if (Enum.IsDefined(typeof(Available), name))
            {
                Enum.Parse(typeof(Available), name);
            }
            else
            {
                Reference = Available.None;
            }
        }

        public enum Available
        {
            None,
            ACC,
            BAK
        }
    }
}
