using System;
using System.Text;
using BabelFish.AST;
using BabelFish.Compiler;
using FluentIL;
using Sigil;

namespace enquanto.Model
{
    internal class Not : AST, IExpression<EnquantoType>
    {
        public Not(IExpression<EnquantoType> value)
        {
            Value = value;
        }

        public IExpression<EnquantoType> Value { get; set; }

        public EnquantoType Type { get => EnquantoType.BOOL; set { } }

        public string Name { get; }

        public override string Dump(string tab)
        {
            var dmp = new StringBuilder();
            dmp.AppendLine($"{tab}(NOT");
            dmp.AppendLine(Value.Dump(tab + "\t"));
            dmp.AppendLine($"{tab})");
            return dmp.ToString();
        }

        public override Emit<Func<int>> EmitByteCode(CompilerContext<EnquantoType> context, Emit<Func<int>> emiter)
        {
            emiter = Value.EmitByteCode(context, emiter);
            emiter.Negate();
            return emiter;
        }

        public override IEmitter EmitByteCode(CompilerContext<EnquantoType> context, IEmitter emiter)
        {
            emiter = (Value as AST).EmitByteCode(context, emiter);
            emiter.Neg();
            return emiter;
        }

        public override string Transpile(CompilerContext<EnquantoType> context) => $"! {Value.Transpile(context)}";
    }
}