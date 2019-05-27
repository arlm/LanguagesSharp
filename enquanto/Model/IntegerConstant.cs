using System;
using BabelFish.AST;
using BabelFish.Compiler;
using Sigil;
using sly.lexer;

namespace enquanto.Model
{
    internal class IntegerConstant : AST, IExpression<EnquantoType>
    {
        public IntegerConstant(int value)
        {
            Value = value;
        }

        public EnquantoType Type { get => EnquantoType.INT; set { } }

        public int Value { get; set; }

        public override string Dump(string tab) => $"{tab}(INTEGER {Value})";

        public override Emit<Func<int>> EmitByteCode(CompilerContext<EnquantoType> context, Emit<Func<int>> emiter)
        {
            emiter.LoadConstant(Value);
            return emiter;
        }

        public override string Transpile(CompilerContext<EnquantoType> context) => Value.ToString();
    }
}