using System;
using System.Reflection;
using System.Text;
using BabelFish.AST;
using BabelFish.Compiler;
using Sigil;

namespace Brainfuck.Operators
{
    public sealed class BlockStatement : AST, IStatement<BrainfuckType>
    {
        public IStatement<BrainfuckType> BlockStmt { get; private set; }

        public BlockStatement(IStatement<BrainfuckType> blockStmt)
        {
            this.BlockStmt = blockStmt;
        }

        public override string Dump(string tab)
        {
            var dmp = new StringBuilder();
            dmp.AppendLine($"{tab}(WHILE");

            dmp.AppendLine($"{tab + "\t"}(COND");
            dmp.AppendLine($"{tab + "\t\t"}Pointer.Data != 0");
            dmp.AppendLine($"{tab + "\t"})");

            dmp.AppendLine($"{tab + "\t"}(BLOCK");
            dmp.AppendLine(BlockStmt.Dump("\t\t" + tab));
            dmp.AppendLine($"{tab})");

            dmp.AppendLine($"{tab})");
            return dmp.ToString();
        }

        public override Emit<Func<int>> EmitByteCode(CompilerContext<BrainfuckType> context, Emit<Func<int>> emiter)
        {
            var loopLabel = emiter.DefineLabel();
            var outLabel = emiter.DefineLabel();

            emiter.MarkLabel(loopLabel);

            var type = typeof(Runtime.Pointer);
            var pi = type.GetProperty("Instance", BindingFlags.Static);
            var mi = pi.GetGetMethod();
            emiter.Call(mi);

            pi = type.GetProperty("Data");
            mi = pi.GetGetMethod();
            emiter.Call(mi);

            emiter.BranchIfFalse(outLabel);
            BlockStmt.EmitByteCode(context, emiter);
            emiter.Branch(loopLabel);
            emiter.MarkLabel(outLabel);
            return emiter;
        }

        public override string Transpile(CompilerContext<BrainfuckType> context)
        {
            var code = new StringBuilder();
            code.AppendLine($"while(Pointer.Instance.Data != 0) {{ ");
            code.AppendLine(BlockStmt.Transpile(context));
            code.AppendLine("}");
            return code.ToString();
        }
    }
}
