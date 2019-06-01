using System;
using System.Collections.Generic;
using System.Linq;
using BabelFish.AST;

namespace BabelFish.Compiler
{
	public static class TypeConverter<T> where T : Enum
    {
        public static string Transpile(T fromType, Language toLanguage) => GetAttributes<LanguageAttribute>(fromType).First(a => a.Language == toLanguage).Type;

        public static Type Emit(T fromType) => GetAttributes<ByteCodeAttribute>(fromType).First(b => b.ByteCode == ByteCode.IL).Type ?? throw new NullReferenceException("IL type not set");

        public static byte[] Emit(T fromType, ByteCode toByteCode) => GetAttributes<ByteCodeAttribute>(fromType).First(b => b.ByteCode == toByteCode).OpCode ?? throw new NullReferenceException("OpCode not set");

        public static bool ParseBoolean(T fromType) => GetAttributes<BooleanValueAttribute>(fromType).First().Value;

        private static IEnumerable<A> GetAttributes<A>(T p) where A: Attribute
        {
            var value = typeof(T).GetField(Enum.GetName(typeof(T), p));
            return Attribute.GetCustomAttributes(value, typeof(A)).Cast<A>();
        }
    }
}
