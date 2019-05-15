using System;
using BabelFish.Compiler;
using Sigil;
using sly.lexer;

namespace BabelFish.AST
{
    public interface INode<T> where T : Enum
    {
        TokenPosition Position { get; set; }

        Scope<T> CompilerScope { get; set; }
        string Dump(string tab);

        string Transpile(CompilerContext<T> context);
        Emit<Func<int>> EmitByteCode(CompilerContext<T> context, Emit<Func<int>> emiter);
    }
}