using System;
using System.Text;
using BabelFish.AST;
using BabelFish.Compiler;
using FluentIL;
using Sigil;

namespace enquanto.Model
{
    internal class ReturnStatement : AST, IStatement<EnquantoType>
    {
        public ReturnStatement(IExpression<EnquantoType> value)
        {
            Value = value;
        }

        public IExpression<EnquantoType> Value { get; set; }

        public override string Dump(string tab)
        {
            var dmp = new StringBuilder();
            dmp.AppendLine($"{tab}(RETURN ");
            dmp.AppendLine($"{Value.Dump("\t" + tab)}");
            dmp.AppendLine($"{tab})");
            return dmp.ToString();
        }

        public override Emit<Func<int>> EmitByteCode(CompilerContext<EnquantoType> context, Emit<Func<int>> emiter)
        {
            emiter = Value.EmitByteCode(context, emiter);
            emiter.Return();
            return emiter;
        }

        public override IEmitter EmitByteCode(CompilerContext<EnquantoType> context, IEmitter emiter)
        {
            emiter = (Value as AST).EmitByteCode(context, emiter);
            emiter.Ret();
            return emiter;
        }

        public override string Transpile(CompilerContext<EnquantoType> context) => $"return {Value.Transpile(context)};";
    }
}