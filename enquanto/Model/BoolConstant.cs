using System;
using BabelFish.AST;
using BabelFish.Compiler;
using Sigil;
using sly.lexer;

namespace enquanto.Model
{
    internal class BoolConstant : IExpression<EnquantoType>
    {
        public BoolConstant(bool value)
        {
            Value = value;
        }

        public bool Value { get; set; }

        public Scope<EnquantoType> CompilerScope { get; set; }

        public TokenPosition Position { get; set; }

        public EnquantoType Type { get => EnquantoType.BOOL; set { } }

        public string Dump(string tab) => $"{tab}(BOOL {Value})";

        public Emit<Func<int>> EmitByteCode(CompilerContext<EnquantoType> context, Emit<Func<int>> emiter)
        {
            emiter.LoadConstant(Value);
            return emiter;
        }

        public string Transpile(CompilerContext<EnquantoType> context) => Value.ToString();
    }
}