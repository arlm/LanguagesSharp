using System;
using BabelFish.AST;
using BabelFish.Compiler;
using Sigil;
using sly.lexer;

namespace enquanto.Model
{
    public class Variable : IExpression<EnquantoType>
    {
        public Variable(string name)
        {
            Name = name;
        }

        public EnquantoType Type { get; set; }

        public TokenPosition Position { get; set; }

        public Scope<EnquantoType> CompilerScope { get; set; }

        public string Name { get; }

        public string Dump(string tab) => $"{tab}(VARIABLE {Name})";

        public Emit<Func<int>> EmitByteCode(CompilerContext<EnquantoType> context, Emit<Func<int>> emiter)
        {
            emiter.LoadLocal(emiter.Locals[Name]);
            return emiter;
        }

        public string Transpile(CompilerContext<EnquantoType> context) => Name;
    }
}
