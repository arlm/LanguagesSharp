using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using BabelFish.AST;
using BabelFish.Compiler;
using Sigil;

namespace Brainfuck.Operators
{
    public class Operation : AST, IStatement<BrainfuckType>
    {
        public MemoryOperator Operator { get; private set; }

        public Operation(MemoryOperator @operator) => this.Operator = @operator;

        public override string Dump(string tab)
        {
            var dmp = new StringBuilder();
            dmp.AppendLine($"{tab}(OPERATION [{Operator}]");
            dmp.AppendLine($"{tab})");
            return dmp.ToString();
        }

        public override Emit<Func<int>> EmitByteCode(CompilerContext<BrainfuckType> context, Emit<Func<int>> emiter)
        {
            var type = typeof(Runtime.Pointer);

            var pi = type.GetProperty("Instance", BindingFlags.Static);
            var mi = pi.GetGetMethod();
            emiter.Call(mi);

            switch (Operator)
            {
                case MemoryOperator.INCREMENT_POINTER:
                    mi = type.GetMethod("Increment");
                    emiter.Call(mi);
                    break;

                case MemoryOperator.DECREMENT_POINTER:
                    mi = type.GetMethod("Decrement");
                    emiter.Call(mi);
                    break;

                case MemoryOperator.INCREMENT_VALUE:
                    mi = type.GetMethod("IncrementData");
                    emiter.Call(mi);
                    break;

                case MemoryOperator.DECREMENT_VALUE:
                    mi = type.GetMethod("DecrementData");
                    emiter.Call(mi);
                    break;
            }

            return emiter;
        }

        public override string Transpile(CompilerContext<BrainfuckType> context)
        {
            var operators = new Dictionary<MemoryOperator, string>
            {
                {MemoryOperator.INCREMENT_POINTER, "Pointer.Instance.Increment()"},
                {MemoryOperator.DECREMENT_POINTER, "Pointer.Instance.Decrement()"},
                {MemoryOperator.INCREMENT_VALUE, "Pointer.Instance.IncrementData()"},
                {MemoryOperator.DECREMENT_VALUE, "Pointer.Instance.DecrementData()"},
            };

            var code = new StringBuilder();
            code.Append(operators[Operator]);
            return code.ToString();
        }
    }
}
