using System;
using BabelFish.AST;
using BabelFish.Compiler;
using Sigil;
using sly.lexer;

namespace enquanto.Model
{
    internal class IntegerConstant : IExpression<EnquantoType>
    {
        public IntegerConstant(int value)
        {
            Value = value;
        }

        public EnquantoType Type { get => EnquantoType.INT; set { } }

        public int Value { get; set; }

        public TokenPosition Position { get; set; }

        public Scope<EnquantoType> CompilerScope { get; set; }

        public string Dump(string tab) => $"{tab}(INTEGER {Value})";

        public Emit<Func<int>> EmitByteCode(CompilerContext<EnquantoType> context, Emit<Func<int>> emiter)
        {
            emiter.LoadConstant(Value);
            return emiter;
        }

        public string Transpile(CompilerContext<EnquantoType> context) => Value.ToString();
    }
}