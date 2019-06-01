using System;

namespace BabelFish.Interpreter
{
	public class InterpreterException : Exception
	{
		public InterpreterException(string message) : base(message)
		{
		}
	}
}
