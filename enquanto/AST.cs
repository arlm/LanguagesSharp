using System;
using BabelFish.AST;
using BabelFish.Compiler;
using Sigil;
using sly.lexer;

namespace enquanto
{
    public class AST : INode<EnquantoToken>
    {
        public AST()
        {
        }

        public TokenPosition Position { get; set; }
        public Scope<EnquantoToken> CompilerScope { get; set; }

        public string Dump(string tab)
        {
            throw new NotImplementedException();
        }

        public Emit<Func<int>> EmitByteCode(CompilerContext<EnquantoToken> context, Emit<Func<int>> emiter)
        {
            throw new NotImplementedException();
        }

        public string Transpile(CompilerContext<EnquantoToken> context)
        {
            throw new NotImplementedException();
        }
    }
}
