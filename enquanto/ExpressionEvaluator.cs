using System;
using System.Collections.Generic;
using BabelFish.AST;
using BabelFish.Interpreter;
using enquanto.Model;

namespace enquanto
{
    internal class ExpressionEvaluator
    {
        private readonly Dictionary<BinaryOperator, List<Signature<EnquantoType>>> binaryOperationSignatures;

        public ExpressionEvaluator(bool quiet = false)
        {
            IsQuiet = quiet;

            binaryOperationSignatures = new Dictionary<BinaryOperator, List<Signature<EnquantoType>>>
            {
                {
                    BinaryOperator.ADD, new List<Signature<EnquantoType>>
                    {
                        new Signature<EnquantoType>(EnquantoType.INT, EnquantoType.INT, EnquantoType.INT)
                    }
                },
                {
                    BinaryOperator.SUB, new List<Signature<EnquantoType>>
                    {
                        new Signature<EnquantoType>(EnquantoType.INT, EnquantoType.INT, EnquantoType.INT)
                    }
                },
                {
                    BinaryOperator.DIVIDE, new List<Signature<EnquantoType>>
                    {
                        new Signature<EnquantoType>(EnquantoType.INT, EnquantoType.INT, EnquantoType.INT)
                    }
                },
                {
                    BinaryOperator.MULTIPLY, new List<Signature<EnquantoType>>
                    {
                        new Signature<EnquantoType>(EnquantoType.INT, EnquantoType.INT, EnquantoType.INT)
                    }
                },
                {
                    BinaryOperator.AND, new List<Signature<EnquantoType>>
                    {
                        new Signature<EnquantoType>(EnquantoType.BOOL, EnquantoType.BOOL, EnquantoType.BOOL)
                    }
                },
                {
                    BinaryOperator.OR, new List<Signature<EnquantoType>>
                    {
                        new Signature<EnquantoType>(EnquantoType.BOOL, EnquantoType.BOOL, EnquantoType.BOOL)
                    }
                },
                {
                    BinaryOperator.LESSER, new List<Signature<EnquantoType>>
                    {
                        new Signature<EnquantoType>(EnquantoType.INT, EnquantoType.INT, EnquantoType.BOOL),
                        new Signature<EnquantoType>(EnquantoType.STRING, EnquantoType.STRING, EnquantoType.BOOL),
                        new Signature<EnquantoType>(EnquantoType.BOOL, EnquantoType.BOOL, EnquantoType.BOOL)
                    }
                },
                {
                    BinaryOperator.GREATER, new List<Signature<EnquantoType>>
                    {
                        new Signature<EnquantoType>(EnquantoType.INT, EnquantoType.INT, EnquantoType.BOOL),
                        new Signature<EnquantoType>(EnquantoType.STRING, EnquantoType.STRING, EnquantoType.BOOL),
                        new Signature<EnquantoType>(EnquantoType.BOOL, EnquantoType.BOOL, EnquantoType.BOOL)
                    }
                },
                {
                    BinaryOperator.EQUALS, new List<Signature<EnquantoType>>
                    {
                        new Signature<EnquantoType>(EnquantoType.INT, EnquantoType.INT, EnquantoType.BOOL),
                        new Signature<EnquantoType>(EnquantoType.STRING, EnquantoType.STRING, EnquantoType.BOOL),
                        new Signature<EnquantoType>(EnquantoType.BOOL, EnquantoType.BOOL, EnquantoType.BOOL)
                    }
                },
                {
                    BinaryOperator.DIFFERENT, new List<Signature<EnquantoType>>
                    {
                        new Signature<EnquantoType>(EnquantoType.INT, EnquantoType.INT, EnquantoType.BOOL),
                        new Signature<EnquantoType>(EnquantoType.STRING, EnquantoType.STRING, EnquantoType.BOOL),
                        new Signature<EnquantoType>(EnquantoType.BOOL, EnquantoType.BOOL, EnquantoType.BOOL)
                    }
                },
                {
                    BinaryOperator.CONCAT, new List<Signature<EnquantoType>>
                    {
                        new Signature<EnquantoType>(EnquantoType.ANY, EnquantoType.ANY, EnquantoType.STRING)
                    }
                }
            };
        }

        public bool IsQuiet { get; }

        public EnquantoType CheckBinaryOperationTyping(BinaryOperator oper, EnquantoType left, EnquantoType right)
        {
            EnquantoType result;

            if (binaryOperationSignatures.ContainsKey(oper))
            {
                var signatures = binaryOperationSignatures[oper];
                var res = signatures.Find(sig => sig.Match(left, right));

                result = res?.Result ?? throw new InterpreterException($"invalid operation {left} {oper} {right}");
            }
            else
            {
                result = left;
            }

            return result;
        }

        public TypedValue<EnquantoType> Evaluate(IExpression<EnquantoType> expr, InterpreterContext<EnquantoType> context)
        {
            switch (expr)
            {
                case BoolConstant b:
                    return Evaluate(b, context);

                case IntegerConstant i:
                    return Evaluate(i, context);

                case StringConstant s:
                    return Evaluate(s, context);

                case BinaryOperation binary:
                    return Evaluate(binary, context);

                case Neg neg:
                    return Evaluate(neg, context);

                case Not not:
                    return Evaluate(not, context);

                case Variable variable:
                    return context.GetVariable(variable.Name);
            }

            throw new InterpreterException($"unknow expression type ({expr.GetType().Name})");
        }

        public TypedValue<EnquantoType> Evaluate(BoolConstant boolConst, InterpreterContext<EnquantoType> context) => new TypedValue<EnquantoType>(boolConst.Value);

        public TypedValue<EnquantoType> Evaluate(StringConstant stringConst, InterpreterContext<EnquantoType> context) => new TypedValue<EnquantoType>(stringConst.Value);

        public TypedValue<EnquantoType> Evaluate(IntegerConstant intConst, InterpreterContext<EnquantoType> context) => new TypedValue<EnquantoType>(intConst.Value);

        public TypedValue<EnquantoType> Evaluate(BinaryOperation operation, InterpreterContext<EnquantoType> context)
        {
            var left = Evaluate(operation.Left, context);
            var right = Evaluate(operation.Right, context);
            var resultType = CheckBinaryOperationTyping(operation.Operator, left.ValueType, right.ValueType);
            object value;

            switch (operation.Operator)
            {
                case BinaryOperator.ADD:
                    {
                        value = left.IntValue + right.IntValue;
                        break;
                    }
                case BinaryOperator.SUB:
                    {
                        value = left.IntValue - right.IntValue;
                        break;
                    }
                case BinaryOperator.MULTIPLY:
                    {
                        value = left.IntValue * right.IntValue;
                        break;
                    }
                case BinaryOperator.DIVIDE:
                    {
                        value = left.IntValue / right.IntValue;
                        break;
                    }
                case BinaryOperator.AND:
                    {
                        value = left.BoolValue && right.BoolValue;
                        break;
                    }
                case BinaryOperator.OR:
                    {
                        value = left.BoolValue || right.BoolValue;
                        break;
                    }
                case BinaryOperator.GREATER:
                    {
                        switch (left.ValueType)
                        {
                            case EnquantoType.BOOL:
                                {
                                    value = left.BoolValue && right.BoolValue == false;
                                    break;
                                }
                            case EnquantoType.INT:
                                {
                                    value = left.IntValue > right.IntValue;
                                    break;
                                }
                            case EnquantoType.STRING:
                                {
                                    value = string.Compare(left.StringValue, right.StringValue, StringComparison.CurrentCulture) == 1;
                                    break;
                                }
                            default:
                                {
                                    value = false;
                                    break;
                                }
                        }

                        break;
                    }
                case BinaryOperator.LESSER:
                    {
                        switch (left.ValueType)
                        {
                            case EnquantoType.BOOL:
                                {
                                    value = left.BoolValue == false && right.BoolValue;
                                    break;
                                }
                            case EnquantoType.INT:
                                {
                                    value = left.IntValue < right.IntValue;
                                    break;
                                }
                            case EnquantoType.STRING:
                                {
                                    value = string.Compare(left.StringValue, right.StringValue, StringComparison.CurrentCulture) == -1;
                                    break;
                                }
                            default:
                                {
                                    value = false;
                                    break;
                                }
                        }

                        break;
                    }
                case BinaryOperator.EQUALS:
                    {
                        switch (left.ValueType)
                        {
                            case EnquantoType.BOOL:
                                {
                                    value = left.BoolValue == right.BoolValue;
                                    break;
                                }
                            case EnquantoType.INT:
                                {
                                    value = left.IntValue == right.IntValue;
                                    break;
                                }
                            case EnquantoType.STRING:
                                {
                                    value = left.StringValue == right.StringValue;
                                    break;
                                }
                            default:
                                {
                                    value = false;
                                    break;
                                }
                        }

                        break;
                    }
                case BinaryOperator.DIFFERENT:
                    {
                        switch (left.ValueType)
                        {
                            case EnquantoType.BOOL:
                                {
                                    value = left.BoolValue != right.BoolValue;
                                    break;
                                }
                            case EnquantoType.INT:
                                {
                                    value = left.IntValue != right.IntValue;
                                    break;
                                }
                            case EnquantoType.STRING:
                                {
                                    value = left.StringValue != right.StringValue;
                                    break;
                                }
                            default:
                                {
                                    value = false;
                                    break;
                                }
                        }

                        break;
                    }
                case BinaryOperator.CONCAT:
                    {
                        value = left.StringValue + right.StringValue;
                        break;
                    }
                default:
                    {
                        value = null;
                        break;
                    }
            }

            return new TypedValue<EnquantoType>(resultType, value);
        }


        public TypedValue<EnquantoType> Evaluate(Neg neg, InterpreterContext<EnquantoType> context)
        {
            var positiveVal = Evaluate(neg.Value, context);

            if (positiveVal.ValueType != EnquantoType.INT)
            {
                throw new InterpreterException($"invalid operation - {positiveVal.StringValue}");
            }

            return new TypedValue<EnquantoType>(-positiveVal.IntValue);
        }

        public TypedValue<EnquantoType> Evaluate(Not not, InterpreterContext<EnquantoType> context)
        {
            var positiveVal = Evaluate(not.Value, context);

            if (positiveVal.ValueType != EnquantoType.BOOL)
            {
                throw new InterpreterException($"invalid operation NOT {positiveVal.StringValue}");
            }

            return new TypedValue<EnquantoType>(!positiveVal.BoolValue);
        }
    }
}
