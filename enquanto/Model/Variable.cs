using System;
using BabelFish.AST;
using BabelFish.Compiler;
using Enquanto;
using FluentIL;
using Sigil;

namespace enquanto.Model
{
    internal class Variable : AST, IExpression<EnquantoType>
    {
        public Variable(string name)
        {
            Name = name;
        }

        public EnquantoType Type { get; set; }

        public string Name { get; }

        public override string Dump(string tab) => $"{tab}(VARIABLE {Name})";

        public override Emit<Func<int>> EmitByteCode(CompilerContext<EnquantoType> context, Emit<Func<int>> emiter)
        {
            emiter.LoadLocal(emiter.Locals[Name]);
            return emiter;
        }

        public override IEmitter EmitByteCode(CompilerContext<EnquantoType> context, IEmitter emiter)
        {
            var local = context.CurrentScope.GetLocal(Name);

            if (local == null)
            {
                throw new ContextException<EnquantoType>(Name, context);
            }

            emiter.LdLoc(local);

            return emiter;
        }

        public override string Transpile(CompilerContext<EnquantoType> context) => Name;
    }
}
