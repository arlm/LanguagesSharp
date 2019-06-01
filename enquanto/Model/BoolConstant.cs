using System;
using BabelFish.AST;
using BabelFish.Compiler;
using Sigil;

namespace enquanto.Model
{
    internal class BoolConstant : AST, IExpression<EnquantoType>
    {
        public BoolConstant(bool value)
        {
            Value = value;
        }

        public bool Value { get; set; }

        public EnquantoType Type { get => EnquantoType.BOOL; set { } }

        public override string Dump(string tab) => $"{tab}(BOOL {Value})";

        public override Emit<Func<int>> EmitByteCode(CompilerContext<EnquantoType> context, Emit<Func<int>> emiter)
        {
            emiter.LoadConstant(Value);
            return emiter;
        }

        public override string Transpile(CompilerContext<EnquantoType> context) => Value.ToString();
    }
}