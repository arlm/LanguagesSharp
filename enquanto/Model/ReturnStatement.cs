using System;
using System.Text;
using BabelFish.AST;
using BabelFish.Compiler;
using Sigil;
using sly.lexer;

namespace enquanto.Model
{
    internal class ReturnStatement : IStatement<EnquantoType>
    {
        public ReturnStatement(IExpression<EnquantoType> value)
        {
            Value = value;
        }

        public IExpression<EnquantoType> Value { get; set; }

        public TokenPosition Position { get; set; }

        public Scope<EnquantoType> CompilerScope { get; set; }

        public string Dump(string tab)
        {
            var dmp = new StringBuilder();
            dmp.AppendLine($"{tab}(RETURN ");
            dmp.AppendLine($"{Value.Dump("\t" + tab)}");
            dmp.AppendLine($"{tab})");
            return dmp.ToString();
        }

        public Emit<Func<int>> EmitByteCode(CompilerContext<EnquantoType> context, Emit<Func<int>> emiter)
        {
            emiter = Value.EmitByteCode(context, emiter);
            emiter.Return();
            return emiter;
        }

        public string Transpile(CompilerContext<EnquantoType> context) => $"return {Value.Transpile(context)};";
    }
}