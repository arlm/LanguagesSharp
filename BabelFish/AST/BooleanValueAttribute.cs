using System;
using System.Collections.Generic;
using System.Text;

namespace BabelFish.AST
{
	public class BooleanValueAttribute : Attribute
	{
		public bool Value
		{
			get;
		}

		public BooleanValueAttribute( bool value)
		{
			this.Value = value;
		}
	}
}
