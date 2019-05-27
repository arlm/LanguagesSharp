using BabelFish.AST;
using BabelFish.Compiler;
using enquanto.Model;

namespace enquanto
{
    internal class ExpressionTyper
    {
        private readonly Signatures signatures;

        public ExpressionTyper()
        {
            signatures = new Signatures();
        }

        public EnquantoType TypeExpression(IExpression<EnquantoType> expr, CompilerContext<EnquantoType> context)
        {
            switch (expr)
            {
                case BoolConstant @bool:
                    return TypeExpression(@bool, context);

                case IntegerConstant @int:
                    return TypeExpression(@int, context);

                case StringConstant @string:
                    return TypeExpression(@string, context);

                case BinaryOperation binary:
                    return TypeExpression(binary, context);

                case Neg neg:
                    return TypeExpression(neg, context);

                case Not not:
                    return TypeExpression(not, context);

                case Variable variable:
                    {
                        var varType = context.GetVariableType(variable.Name);

                        if (varType == EnquantoType.NONE)
                        {
                            throw new TypingException($" variable {variable.Name} {variable.Position} is not intialized");
                        }

                        variable.CompilerScope = context.CurrentScope;
                        return varType;
                    }
            }

            throw new SignatureException($"unknow expression type ({expr.GetType().Name})");
        }

        public EnquantoType TypeExpression(BoolConstant boolConst, CompilerContext<EnquantoType> context)
        {
            boolConst.CompilerScope = context.CurrentScope;
            return EnquantoType.BOOL;
        }

        public EnquantoType TypeExpression(StringConstant stringConst, CompilerContext<EnquantoType> context)
        {
            stringConst.CompilerScope = context.CurrentScope;
            return EnquantoType.STRING;
        }

        public EnquantoType TypeExpression(IntegerConstant intConst, CompilerContext<EnquantoType> context)
        {
            intConst.CompilerScope = context.CurrentScope;
            return EnquantoType.INT;
        }

        public EnquantoType TypeExpression(BinaryOperation operation, CompilerContext<EnquantoType> context)
        {
            operation.CompilerScope = context.CurrentScope;
            var left = TypeExpression(operation.Left, context);
            operation.Left.Type = left;
            var right = TypeExpression(operation.Right, context);
            operation.Right.Type = right;
            var resultType = signatures.CheckBinaryOperationTyping(operation.Operator, left, right);

            return resultType;
        }


        public EnquantoType TypeExpression(Neg neg, CompilerContext<EnquantoType> context)
        {
            var positiveVal = TypeExpression(neg.Value, context);
            neg.CompilerScope = context.CurrentScope;

            if (positiveVal != EnquantoType.INT)
            {
                throw new SignatureException($"invalid operation type({positiveVal}) : {neg.Dump("")}");
            }

            return EnquantoType.INT;
        }

        public EnquantoType TypeExpression(Not not, CompilerContext<EnquantoType> context)
        {
            var positiveVal = TypeExpression(not.Value, context);
            not.CompilerScope = context.CurrentScope;

            if (positiveVal != EnquantoType.BOOL)
            {
                throw new SignatureException($"invalid operation type({positiveVal}) : {not.Dump("")}");
            }

            return EnquantoType.BOOL;
        }
    }
}