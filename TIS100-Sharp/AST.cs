using System;
using BabelFish.AST;
using BabelFish.Compiler;
using Sigil;
using sly.lexer;

namespace TIS100Sharp
{
    public abstract class AST : INode<TIS100Type>
    {
        public TokenPosition Position { get; set; }
        public Scope<TIS100Type> CompilerScope { get; set; }

        public abstract string Dump(string tab);
        public abstract Emit<Func<int>> EmitByteCode(CompilerContext<TIS100Type> context, Emit<Func<int>> emiter);
        public abstract string Transpile(CompilerContext<TIS100Type> context);
    }
}
