using System;
using BabelFish.AST;
using BabelFish.Interpreter;
using enquanto.Model;

namespace enquanto
{
    public class Interpreter
    {
        private ExpressionEvaluator evaluator;

        private bool IsQuiet;

        public InterpreterContext<EnquantoType> Execute(INode<EnquantoType> ast, bool quiet)
        {
            IsQuiet = quiet;
            evaluator = new ExpressionEvaluator(quiet);
            return Execute(ast, new InterpreterContext<EnquantoType>());
        }

        public InterpreterContext<EnquantoType> Execute(INode<EnquantoType> ast)
        {
            evaluator = new ExpressionEvaluator();
            return Execute(ast, new InterpreterContext<EnquantoType>());
        }

        private InterpreterContext<EnquantoType> Execute(INode<EnquantoType> ast, InterpreterContext<EnquantoType> context)
        {
            switch (ast)
            {
                case AssignStatement assign:
                    Interprete(assign, context);
                    break;

                case SequenceStatement seq:
                    Interprete(seq, context);
                    break;

                case SkipStatement skip:
                    //Interprete(skip, context);
                    break;

                case PrintStatement print:
                    Interprete(print, context);
                    break;

                case IfStatement si:
                    Interprete(si, context);
                    break;

                case WhileStatement tantque:
                    Interprete(tantque, context);
                    break;
            }

            return context;
        }

        private void Interprete(AssignStatement ast, InterpreterContext<EnquantoType> context)
        {
            var val = evaluator.Evaluate(ast.Value, context);
            context.SetVariable(ast.VariableName, val);
        }

        private void Interprete(PrintStatement ast, InterpreterContext<EnquantoType> context)
        {
            var val = evaluator.Evaluate(ast.Value, context);
            if (!IsQuiet) Console.WriteLine(val.StringValue);
        }

        private void Interprete(SequenceStatement ast, InterpreterContext<EnquantoType> context)
        {
            for (var i = 0; i < ast.Count; i++)
            {
                var stmt = ast.Get(i);
                Execute(stmt, context);
            }
        }

        private void Interprete(IfStatement ast, InterpreterContext<EnquantoType> context)
        {
            var val = evaluator.Evaluate(ast.Condition, context);
            if (val.ValueType != EnquantoType.BOOL)
                throw new InterpreterException($"invalid condition type {ast.Condition.Dump("")}");

            if (val.BoolValue)
                Execute(ast.ThenStmt, context);
            else
                Execute(ast.ElseStmt, context);
        }

        private void Interprete(WhileStatement ast, InterpreterContext<EnquantoType> context)
        {
            var cond = evaluator.Evaluate(ast.Condition, context);
            if (cond.ValueType != EnquantoType.BOOL)
                throw new InterpreterException($"invalid condition type {ast.Condition.Dump("")}");
            while (cond.BoolValue)
            {
                Execute(ast.BlockStmt, context);
                cond = evaluator.Evaluate(ast.Condition, context);
            }
        }
    }
}
