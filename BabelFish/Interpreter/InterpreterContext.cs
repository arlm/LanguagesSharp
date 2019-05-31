using System;
using System.Collections.Generic;
using System.Text;

namespace BabelFish.Interpreter
{
	public class InterpreterContext<T> where T : Enum
    {
		public Dictionary<string, TypedValue<T>> variables;

		public InterpreterContext()
		{
			variables = new Dictionary<string, TypedValue<T>>();
		}

		public void SetVariable(string name, TypedValue<T> value)
		{
			variables[name] = value;
		}

		public void SetVariable(string name, string value)
		{
			variables[name] = new TypedValue<T>(value);
		}

		public void SetVariable(string name, int value)
		{
			variables[name] = new TypedValue<T>(value);
		}

		public void SetVariable(string name, bool value)
		{
			variables[name] = new TypedValue<T>(value);
		}

		public TypedValue<T> GetVariable(string name)
		{
			return variables.ContainsKey(name) ? variables[name] : null;
		}

		public override string ToString()
		{
			var dmp = new StringBuilder();
			foreach(var pair in variables) dmp.AppendLine($"{pair.Key}={pair.Value}");
			return dmp.ToString();
		}
	}
}
