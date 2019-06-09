using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BabelFish.AST;
using BabelFish.Compiler;
using Sigil;

namespace Brainfuck.Operators
{
    public class SequenceStatement : AST, IStatement<BrainfuckType>
    {
        public SequenceStatement() => Statements = new List<IStatement<BrainfuckType>>();

        public SequenceStatement(IStatement<BrainfuckType> statement) => Statements = new List<IStatement<BrainfuckType>> { statement };

        public SequenceStatement(IEnumerable<IStatement<BrainfuckType>> sequence) => Statements = sequence.ToList();

        public List<IStatement<BrainfuckType>> Statements { get; }

        public int Count => Statements.Count;

        public override string Dump(string tab)
        {
            var dump = new StringBuilder();
            dump.AppendLine($"{tab}(SEQUENCE [");
            Statements.ForEach(c => dump.AppendLine($"{c.Dump(tab + "\t")},"));
            dump.AppendLine($"{tab}] )");
            return dump.ToString();
        }

        public override Emit<Func<int>> EmitByteCode(CompilerContext<BrainfuckType> context, Emit<Func<int>> emiter)
        {
            foreach (var stmt in Statements)
            {
                emiter = stmt.EmitByteCode(context, emiter);
            }

            return emiter;
        }

        public override string Transpile(CompilerContext<BrainfuckType> context)
        {
            var block = new StringBuilder("{\n");
            foreach (var stmt in Statements)
            {
                block.Append(stmt.Transpile(context));
                block.AppendLine(";");
            }

            block.AppendLine("}");
            return block.ToString();
        }

        public IStatement<BrainfuckType> Get(int i) => Statements[i];

        public void Add(IStatement<BrainfuckType> statement) => Statements.Add(statement);

        public void AddRange(List<IStatement<BrainfuckType>> statements) => Statements.AddRange(statements);
    }
}
