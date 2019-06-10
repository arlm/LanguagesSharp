using System;
using System.Text;
using BabelFish.AST;
using BabelFish.Compiler;
using FluentIL;
using Sigil;

namespace enquanto.Model
{
    internal class IfStatement : AST, IStatement<EnquantoType>
    {
        public IfStatement(IExpression<EnquantoType> condition, IStatement<EnquantoType> thenStmt, IStatement<EnquantoType> elseStmt)
        {
            Condition = condition;
            ThenStmt = thenStmt;
            ElseStmt = elseStmt;
        }

        public IExpression<EnquantoType> Condition { get; set; }

        public IStatement<EnquantoType> ThenStmt { get; set; }

        public IStatement<EnquantoType> ElseStmt { get; set; }

        public override string Dump(string tab)
        {
            var dmp = new StringBuilder();
            dmp.AppendLine($"{tab}(IF");

            dmp.AppendLine($"{tab + "\t"}(COND");
            dmp.AppendLine(Condition.Dump("\t\t" + tab));
            dmp.AppendLine($"{tab + "\t"})");

            dmp.AppendLine($"{tab + "\t"}(THEN");
            dmp.AppendLine(ThenStmt.Dump("\t\t" + tab));
            dmp.AppendLine($"{tab})");

            dmp.AppendLine($"{tab + "\t"}(ELSE");
            dmp.AppendLine(ElseStmt.Dump("\t\t" + tab));
            dmp.AppendLine($"{tab + "\t"})");

            dmp.AppendLine($"{tab})");
            return dmp.ToString();
        }

        public override Emit<Func<int>> EmitByteCode(CompilerContext<EnquantoType> context, Emit<Func<int>> emiter)
        {
            var thenLabel = emiter.DefineLabel();
            var elseLabel = emiter.DefineLabel();
            var endLabel = emiter.DefineLabel();

            Condition.EmitByteCode(context, emiter);

            emiter.BranchIfTrue(thenLabel);
            emiter.Branch(elseLabel);
            emiter.MarkLabel(thenLabel);

            ThenStmt.EmitByteCode(context, emiter);

            emiter.Branch(endLabel);
            emiter.MarkLabel(elseLabel);

            ElseStmt.EmitByteCode(context, emiter);

            emiter.Branch(endLabel);
            emiter.MarkLabel(endLabel);
            return emiter;
        }

        public override IEmitter EmitByteCode(CompilerContext<EnquantoType> context, IEmitter emiter)
        {
            emiter.DefineLabel(out var thenLabel);
            emiter.DefineLabel(out var elseLabel);
            emiter.DefineLabel(out var endLabel);

            (Condition as AST).EmitByteCode(context, emiter);

            emiter.BrTrue(thenLabel);
            emiter.Br(elseLabel);
            emiter.MarkLabel(thenLabel);

            (ThenStmt as AST).EmitByteCode(context, emiter);

            emiter. Br(endLabel);
            emiter.MarkLabel(elseLabel);

            (ElseStmt as AST).EmitByteCode(context, emiter);

            emiter.Br(endLabel);
            emiter.MarkLabel(endLabel);

            return emiter;
        }

        public override string Transpile(CompilerContext<EnquantoType> context)
        {
            var code = new StringBuilder();
            code.AppendLine($"if({Condition.Transpile(context)}) {{ ");
            code.AppendLine(ThenStmt.Transpile(context));
            code.AppendLine("}");

            if (ElseStmt != null)
            {
                code.AppendLine("else {");
                code.AppendLine(ElseStmt.Transpile(context));
                code.AppendLine("}");
            }

            return code.ToString();
        }
    }
}