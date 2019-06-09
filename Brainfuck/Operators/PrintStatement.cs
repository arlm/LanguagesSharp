using System;
using System.Reflection;
using System.Text;
using BabelFish.AST;
using BabelFish.Compiler;
using Brainfuck.Runtime;
using Sigil;

namespace Brainfuck.Operators
{
    public class PrintStatement:AST, IStatement<BrainfuckType>
    {
public override string Dump(string tab)
        {
            var dmp = new StringBuilder();
            dmp.AppendLine($"{tab}(PRINT ");
            dmp.AppendLine($"{tab + "\t"}Pointer.Data");
            dmp.AppendLine($"{tab})");
            return dmp.ToString();
        }

        public override Emit<Func<int>> EmitByteCode(CompilerContext<BrainfuckType> context, Emit<Func<int>> emiter)
        {
            var type = typeof(Runtime.Pointer);
            var pi = type.GetProperty("Instance", BindingFlags.Static);
            var mi = pi.GetGetMethod();
            emiter.Call(mi);

            pi = type.GetProperty("Data");
            mi = pi.GetGetMethod();
            emiter.Call(mi);

            emiter.Convert<char>();

            mi = typeof(Console).GetMethod("Write", new[] { typeof(char) });
            emiter.Call(mi);

            return emiter;
        }

        public override string Transpile(CompilerContext<BrainfuckType> context) => $"System.Console.WriteLine(unchecked((char)Pointer.Instance.Data));";
    }
}
