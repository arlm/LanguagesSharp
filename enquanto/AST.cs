using System;
using BabelFish.AST;
using BabelFish.Compiler;
using Enquanto;
using FluentIL;
using Sigil;
using sly.lexer;

namespace enquanto
{
    public abstract class AST : INode<EnquantoType>, IFluentIL<EnquantoType>
    {
        public TokenPosition Position { get; set; }
        public Scope<EnquantoType> CompilerScope { get; set; }

        public abstract string Dump(string tab);
        public abstract Emit<Func<int>> EmitByteCode(CompilerContext<EnquantoType> context, Emit<Func<int>> emiter);
        public abstract IEmitter EmitByteCode(CompilerContext<EnquantoType> context, IEmitter emiter);
        public abstract string Transpile(CompilerContext<EnquantoType> context);
    }
}
