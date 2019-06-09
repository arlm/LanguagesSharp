using System;
using BabelFish.AST;
using BabelFish.Compiler;
using Sigil;

namespace Brainfuck.Operators
{
    public class ByteConstant : AST, IExpression<BrainfuckType>
    {
        public ByteConstant(byte value)
        {
            Value = value;
        }

        public BrainfuckType Type { get => BrainfuckType.BYTE; set { } }

        public int Value { get; set; }

        public override string Dump(string tab) => $"{tab}(INTEGER {Value})";

        public override Emit<Func<int>> EmitByteCode(CompilerContext<BrainfuckType> context, Emit<Func<int>> emiter)
        {
            emiter.LoadConstant(Value);
            return emiter;
        }

        public override string Transpile(CompilerContext<BrainfuckType> context) => Value.ToString();

    }
}
