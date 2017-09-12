using System;
namespace TIS100Sharp.Operands
{
    public class Port : Source<Port.Available>
    {
		public Port(string name)
		{
			if (Enum.IsDefined(typeof(Available), name))
			{
                Reference = (Available) Enum.Parse(typeof(Available), name);
			}
			else
			{
				Reference = Available.None;
			}
		}

		public enum Available
		{
			None,
			ANY,
			LAST,
            LEFT,
            RIGHT,
            UP,
            DOWN
		}
    }
}
