using System;
using BabelFish.AST;
using BabelFish.Compiler;
using Sigil;

namespace Brainfuck.Operators
{
    public class SkipStatement : AST, IStatement<BrainfuckType>
    {
        public override string Dump(string tab) => $"{tab}(SKIP)";

        public override Emit<Func<int>> EmitByteCode(CompilerContext<BrainfuckType> context, Emit<Func<int>> emiter)
        {
            emiter.Nop();
            return emiter;
        }

        public override string Transpile(CompilerContext<BrainfuckType> context) => ";";
    }
}
