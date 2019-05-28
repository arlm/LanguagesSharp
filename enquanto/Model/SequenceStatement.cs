using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BabelFish.AST;
using BabelFish.Compiler;
using Sigil;
using sly.lexer;

namespace enquanto.Model
{
    internal class SequenceStatement : AST, IStatement<EnquantoType>
    {
        public SequenceStatement() => Statements = new List<IStatement<EnquantoType>>();

        public SequenceStatement(IStatement<EnquantoType> statement) => Statements = new List<IStatement<EnquantoType>> { statement };

        public SequenceStatement(IEnumerable<IStatement<EnquantoType>> sequence) => Statements = sequence.ToList();

        public List<IStatement<EnquantoType>> Statements { get; }

        public int Count => Statements.Count;

        public override string Dump(string tab)
        {
            var dump = new StringBuilder();
            dump.AppendLine($"{tab}(SEQUENCE [");
            Statements.ForEach(c => dump.AppendLine($"{c.Dump(tab + "\t")},"));
            dump.AppendLine($"{tab}] )");
            return dump.ToString();
        }

        public override Emit<Func<int>> EmitByteCode(CompilerContext<EnquantoType> context, Emit<Func<int>> emiter)
        {
            foreach (var stmt in Statements)
            {
                emiter = stmt.EmitByteCode(context, emiter);
            }

            return emiter;
        }

        public override string Transpile(CompilerContext<EnquantoType> context)
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

        public IStatement<EnquantoType> Get(int i) => Statements[i];

        public void Add(IStatement<EnquantoType> statement) => Statements.Add(statement);

        public void AddRange(List<IStatement<EnquantoType>> statements) => Statements.AddRange(statements);
    }
}