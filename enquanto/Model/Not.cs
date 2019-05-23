using System;
using System.Text;
using BabelFish.AST;
using BabelFish.Compiler;
using Sigil;
using sly.lexer;

namespace enquanto.Model
{
    public class Not : IExpression<EnquantoType>
    {
        public Not(IExpression<EnquantoType> value)
        {
            Value = value;
        }

        public IExpression<EnquantoType> Value { get; set; }

        public EnquantoType Type { get => EnquantoType.BOOL; set { } }

        public TokenPosition Position { get; set; }

        public Scope<EnquantoType> CompilerScope { get; set; }

        public string Name { get; }

        public string Dump(string tab)
        {
            var dmp = new StringBuilder();
            dmp.AppendLine($"{tab}(NOT");
            dmp.AppendLine(Value.Dump(tab + "\t"));
            dmp.AppendLine($"{tab})");
            return dmp.ToString();
        }

        public Emit<Func<int>> EmitByteCode(CompilerContext<EnquantoType> context, Emit<Func<int>> emiter)
        {
            emiter = Value.EmitByteCode(context, emiter);
            emiter.Negate();
            return emiter;
        }

        public string Transpile(CompilerContext<EnquantoType> context) => $"! {Value.Transpile(context)}";
    }
}