using System;
using System.Text;
using BabelFish.AST;
using BabelFish.Compiler;
using Sigil;
using sly.lexer;

namespace enquanto.Model
{
    internal class PrintStatement : AST, IStatement<EnquantoType>
    {
        public PrintStatement(IExpression<EnquantoType> value)
        {
            Value = value;
        }

        public IExpression<EnquantoType> Value { get; set; }

        public override string Dump(string tab)
        {
            var dmp = new StringBuilder();
            dmp.AppendLine($"{tab}(PRINT ");
            dmp.AppendLine($"{Value.Dump("\t" + tab)}");
            dmp.AppendLine($"{tab})");
            return dmp.ToString();
        }

        public override Emit<Func<int>> EmitByteCode(CompilerContext<EnquantoType> context, Emit<Func<int>> emiter)
        {
            var mi = typeof(Console).GetMethod("WriteLine", new[] { typeof(string) });

            emiter = Value.EmitByteCode(context, emiter);
            emiter.Call(mi);

            return emiter;
        }

        public override string Transpile(CompilerContext<EnquantoType> context) => $"System.Console.WriteLine({Value.Transpile(context)});";
    }
}