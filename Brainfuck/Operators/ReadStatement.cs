using System;
using System.Reflection;
using System.Text;
using BabelFish.AST;
using BabelFish.Compiler;
using Sigil;

namespace Brainfuck.Operators
{
    public class ReadStatement: AST, IStatement<BrainfuckType>
    {
        public override string Dump(string tab)
        {
            var dmp = new StringBuilder();
            dmp.AppendLine($"{tab}(READ INPUT ");
            dmp.AppendLine($"{tab})");
            return dmp.ToString();
        }

        public override Emit<Func<int>> EmitByteCode(CompilerContext<BrainfuckType> context, Emit<Func<int>> emiter)
        {
            var mi = typeof(Console).GetMethod("Read");
            emiter.Call(mi);
            emiter.Convert<byte>();

            var type = typeof(Runtime.Pointer);
            var pi = type.GetProperty("Instance", BindingFlags.Static);
            mi = pi.GetGetMethod();
            emiter.Call(mi);

            pi = type.GetProperty("Data");
            mi = pi.GetSetMethod();
            emiter.Call(mi);

            return emiter;
        }

        public override string Transpile(CompilerContext<BrainfuckType> context) => $"Pointer.Instance.Data = unchecked((byte)System.Console.Read());";
    }
}
