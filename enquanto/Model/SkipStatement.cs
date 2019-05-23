using System;
using BabelFish.AST;
using BabelFish.Compiler;
using Sigil;
using sly.lexer;

namespace enquanto.Model
{
    public class SkipStatement : INode<EnquantoType>
    {
        public TokenPosition Position { get; set; }

        public Scope<EnquantoType> CompilerScope { get; set; }

        public string Dump(string tab) => $"{tab}(SKIP)";

        public Emit<Func<int>> EmitByteCode(CompilerContext<EnquantoType> context, Emit<Func<int>> emiter)
        {
            emiter.Nop();
            return emiter;
        }

        public string Transpile(CompilerContext<EnquantoType> context) => ";";
    }
}