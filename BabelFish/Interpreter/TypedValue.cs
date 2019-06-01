using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BabelFish.Compiler;

namespace BabelFish.Interpreter
{
	public class TypedValue<T> where T : Enum
	{
		public TypedValue(T valType, object value)
		{
			ValueType = valType;
			Value = value;
		}

		public TypedValue(string value)
		{
            ValueType = GetTypeEnumValue(value.GetType());
            Value = value;
        }

		public TypedValue(int value)
		{
            ValueType = GetTypeEnumValue(value.GetType());
            Value = value;
        }

		public TypedValue(bool value)
		{
			ValueType = GetTypeEnumValue(value.GetType());
			Value = value;
		}

		public T ValueType { get; }

		public int IntValue => (int)Value;

		public bool BoolValue => (bool)Value;

		public string StringValue => Value.ToString();

		public object Value { get; }

        public override string ToString() => $"{StringValue}({ValueType})";

        private static T GetTypeEnumValue(Type castTo)
        {
            var type = typeof(T);
            var properties = type.GetFields(BindingFlags.Public | BindingFlags.Static);

            var values = from p in properties
                           let bc = p.GetCustomAttributes(typeof(ByteCodeAttribute), false).Cast<ByteCodeAttribute>()
                           where bc.Any(a => a.ByteCode == ByteCode.IL && a.Type == castTo)
                           select p.GetRawConstantValue();

            return values.Cast<T>().First();
        }
    }
}
