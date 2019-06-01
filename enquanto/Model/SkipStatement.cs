using System;
using BabelFish.AST;
using BabelFish.Compiler;
using Sigil;

namespace enquanto.Model
{
    internal class SkipStatement : AST, IStatement<EnquantoType>
    {
        public override string Dump(string tab) => $"{tab}(SKIP)";

        public override Emit<Func<int>> EmitByteCode(CompilerContext<EnquantoType> context, Emit<Func<int>> emiter)
        {
            emiter.Nop();
            return emiter;
        }

        public override string Transpile(CompilerContext<EnquantoType> context) => ";";
    }
}