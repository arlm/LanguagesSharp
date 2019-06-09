using System;
using BabelFish.AST;
using BabelFish.Compiler;
using Sigil;
using sly.lexer;

namespace Brainfuck
{
    public abstract class AST : INode<BrainfuckType>
    {
        public TokenPosition Position { get; set; }
        public Scope<BrainfuckType> CompilerScope { get; set; }

        public abstract string Dump(string tab);
        public abstract Emit<Func<int>> EmitByteCode(CompilerContext<BrainfuckType> context, Emit<Func<int>> emiter);
        public abstract string Transpile(CompilerContext<BrainfuckType> context);
    }
}
