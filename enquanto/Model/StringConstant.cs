using System;
using BabelFish.AST;
using BabelFish.Compiler;
using Sigil;
using sly.lexer;

namespace enquanto.Model
{
    internal class StringConstant : IExpression<EnquantoType>
    {
        public StringConstant(string value)
        {
            Value = value;
        }

        public string Value { get; set; }

        public EnquantoType Type { get => EnquantoType.STRING; set { } }

        public TokenPosition Position { get; set; }

        public Scope<EnquantoType> CompilerScope { get; set; }

        public string Dump(string tab) => $"{tab}(STRING {Value})";

        public Emit<Func<int>> EmitByteCode(CompilerContext<EnquantoType> context, Emit<Func<int>> emiter)
        {
            emiter.LoadConstant(Value);
            return emiter;
        }

        public string Transpile(CompilerContext<EnquantoType> context) => $"\"{Value}\"";
    }
}