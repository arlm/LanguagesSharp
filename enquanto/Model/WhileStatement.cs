using System;
using System.Text;
using BabelFish.AST;
using BabelFish.Compiler;
using FluentIL;
using Sigil;

namespace enquanto.Model
{
    internal class WhileStatement : AST, IStatement<EnquantoType>
    {
        public WhileStatement(IExpression<EnquantoType> condition, IStatement<EnquantoType> blockStmt)
        {
            Condition = condition;
            BlockStmt = blockStmt;
        }

        public IExpression<EnquantoType> Condition { get; set; }

        public IStatement<EnquantoType> BlockStmt { get; set; }

        public override string Dump(string tab)
        {
            var dmp = new StringBuilder();
            dmp.AppendLine($"{tab}(WHILE");

            dmp.AppendLine($"{tab + "\t"}(COND");
            dmp.AppendLine(Condition.Dump("\t\t" + tab));
            dmp.AppendLine($"{tab + "\t"})");

            dmp.AppendLine($"{tab + "\t"}(BLOCK");
            dmp.AppendLine(BlockStmt.Dump("\t\t" + tab));
            dmp.AppendLine($"{tab})");

            dmp.AppendLine($"{tab})");
            return dmp.ToString();
        }

        public override Emit<Func<int>> EmitByteCode(CompilerContext<EnquantoType> context, Emit<Func<int>> emiter)
        {
            var loopLabel = emiter.DefineLabel();
            var outLabel = emiter.DefineLabel();

            emiter.MarkLabel(loopLabel);

            Condition.EmitByteCode(context, emiter);

            emiter.BranchIfFalse(outLabel);

            BlockStmt.EmitByteCode(context, emiter);

            emiter.Branch(loopLabel);
            emiter.MarkLabel(outLabel);
            return emiter;
        }

        public override IEmitter EmitByteCode(CompilerContext<EnquantoType> context, IEmitter emiter)
        {
            var guid = Guid.NewGuid().ToString().Replace("-", "");
            var outLabel = $"End_Loop_{guid}";
            string loopLabel = $"Loop_{guid}";

            emiter.DefineLabel(loopLabel, out var loop);
            emiter.DefineLabel(outLabel, out var @out);

            emiter.MarkLabel(loop);

            (Condition as AST).EmitByteCode(context, emiter);

            emiter.BrFalse(@out);

            (BlockStmt as AST).EmitByteCode(context, emiter);

            emiter.Br(loop);

            emiter.MarkLabel(@out);

            return emiter;
        }

        public override string Transpile(CompilerContext<EnquantoType> context)
        {
            var code = new StringBuilder();
            code.AppendLine($"while({Condition.Transpile(context)}) {{ ");
            code.AppendLine(BlockStmt.Transpile(context));
            code.AppendLine("}");
            return code.ToString();
        }
    }
}