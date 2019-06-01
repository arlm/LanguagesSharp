using System;
using BabelFish.AST;
using BabelFish.Compiler;
using Sigil;
using sly.lexer;

namespace enquanto
{
    public abstract class AST : INode<EnquantoType>
    {
        public TokenPosition Position { get; set; }
        public Scope<EnquantoType> CompilerScope { get; set; }

        public abstract string Dump(string tab);
        public abstract Emit<Func<int>> EmitByteCode(CompilerContext<EnquantoType> context, Emit<Func<int>> emiter);
        public abstract string Transpile(CompilerContext<EnquantoType> context);
    }
}
