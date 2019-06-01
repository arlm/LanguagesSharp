using BabelFish.AST;
using BabelFish.Compiler;
using enquanto.Model;

namespace enquanto
{
    internal class SemanticChecker
    {
        private ExpressionTyper expressionTyper;

        public CompilerContext<EnquantoType> SemanticCheck(INode<EnquantoType> ast)
        {
            expressionTyper = new ExpressionTyper();
            return SemanticCheck(ast, new CompilerContext<EnquantoType>());
        }

        private CompilerContext<EnquantoType> SemanticCheck(INode<EnquantoType> ast, CompilerContext<EnquantoType> context)
        {
            switch (ast)
            {
                case AssignStatement assign:
                    SemanticCheck(assign, context);
                    break;

                case SequenceStatement seq:
                    SemanticCheck(seq, context);
                    break;

                case SkipStatement skip:
                    break;

                case PrintStatement print:
                    SemanticCheck(print, context);
                    break;

                case ReturnStatement ret:
                    SemanticCheck(ret, context);
                    break;

                case IfStatement si:
                    SemanticCheck(si, context);
                    break;

                case WhileStatement tantque:
                    SemanticCheck(tantque, context);
                    break;
}

            return context;
        }

        private void SemanticCheck(AssignStatement ast, CompilerContext<EnquantoType> context)
        {
            var valType = expressionTyper.TypeExpression(ast.Value, context);
            var varExists = context.VariableExists(ast.VariableName);
            //ast.CompilerScope = context.CurrentScope;

            if (varExists)
            {
                var varType = context.GetVariableType(ast.VariableName);

                if (varType != valType)
                {
                    throw new TypingException($"bad assignment : {valType} expecting {varType}");
                }
            }
            else
            {
                var creation = context.SetVariableType(ast.VariableName, valType);
                ast.IsVariableCreation = creation;
                ast.CompilerScope = context.CurrentScope;
            }
        }

        private void SemanticCheck(PrintStatement ast, CompilerContext<EnquantoType> context)
        {
            var val = expressionTyper.TypeExpression(ast.Value, context);
        }

        private void SemanticCheck(ReturnStatement ast, CompilerContext<EnquantoType> context)
        {
            var valType = expressionTyper.TypeExpression(ast.Value, context);

            if (valType != EnquantoType.INT)
            {
                throw new TypingException($"bad return type : {valType} expecting INT");
            }
        }

        private void SemanticCheck(SequenceStatement ast, CompilerContext<EnquantoType> context)
        {
            context.OpenNewScope();
            ast.CompilerScope = context.CurrentScope;

            for (var i = 0; i < ast.Count; i++)
            {
                var stmt = ast.Get(i);
                SemanticCheck(stmt, context);
            }

            context.CloseScope();
        }

        private void SemanticCheck(IfStatement ast, CompilerContext<EnquantoType> context)
        {
            var val = expressionTyper.TypeExpression(ast.Condition, context);

            if (val != EnquantoType.BOOL)
            {
                throw new SignatureException($"invalid condition type {ast.Condition.Dump("")} at {ast.Position}");
            }

            ast.CompilerScope = context.CurrentScope;

            context.OpenNewScope();
            SemanticCheck(ast.ThenStmt);
            context.CloseScope();

            context.OpenNewScope();
            SemanticCheck(ast.ElseStmt);
            context.CloseScope();
        }

        private void SemanticCheck(WhileStatement ast, CompilerContext<EnquantoType> context)
        {
            var cond = expressionTyper.TypeExpression(ast.Condition, context);

            if (cond != EnquantoType.BOOL)
            {
                throw new SignatureException($"invalid condition type {ast.Condition.Dump("")} at {ast.Position}");
            }

            ast.CompilerScope = context.CurrentScope;

            context.OpenNewScope();
            SemanticCheck(ast.BlockStmt, context);
            context.CloseScope();
        }
    }
}