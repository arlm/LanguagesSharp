using System;
using BabelFish.AST;
using BabelFish.Compiler;
using FluentIL;
using Sigil;

namespace enquanto.Model
{
    internal class StringConstant : AST, IExpression<EnquantoType>
    {
        public StringConstant(string value)
        {
            Value = value;
        }

        public string Value { get; set; }

        public EnquantoType Type { get => EnquantoType.STRING; set { } }

        public override string Dump(string tab) => $"{tab}(STRING {Value})";

        public override Emit<Func<int>> EmitByteCode(CompilerContext<EnquantoType> context, Emit<Func<int>> emiter)
        {
            emiter.LoadConstant(Value);
            return emiter;
        }

        public override IEmitter EmitByteCode(CompilerContext<EnquantoType> context, IEmitter emiter)
        {
            emiter.LdStr(Value);
            return emiter;
        }

        public override string Transpile(CompilerContext<EnquantoType> context) => $"\"{Value}\"";
    }
}