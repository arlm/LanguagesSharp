﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using BabelFish.AST;
using BabelFish.Compiler;
using Sigil;
using sly.lexer;

namespace enquanto.Model
{
    internal class BinaryOperation : AST, IExpression<EnquantoType>
    {
        public IExpression<EnquantoType> Left { get; set; }

        public BinaryOperator Operator { get; set; }

        public IExpression<EnquantoType> Right { get; set; }

        public EnquantoType Type { get; set; }

        public BinaryOperation(IExpression<EnquantoType> left, BinaryOperator oper, IExpression<EnquantoType> right)
        {
            Left = left;
            Operator = oper;
            Right = right;
        }

        [ExcludeFromCodeCoverage]
        public override string Dump(string tab)
        {
            var dmp = new StringBuilder();
            dmp.AppendLine($"{tab}(OPERATION [{Operator}]");
            dmp.AppendLine($"{Left.Dump("\t" + tab)},");
            dmp.AppendLine(Right.Dump("\t" + tab));
            dmp.AppendLine($"{tab})");
            return dmp.ToString();
        }

        public override Emit<Func<int>> EmitByteCode(CompilerContext<EnquantoType> context, Emit<Func<int>> emiter)
        {
            if (Operator == BinaryOperator.CONCAT) return EmitConcat(context, emiter);
            emiter = Left.EmitByteCode(context, emiter);
            emiter = Right.EmitByteCode(context, emiter);
            switch (Operator)
            {
                case BinaryOperator.ADD:
                    {
                        emiter = emiter.Add();
                        break;
                    }
                case BinaryOperator.SUB:
                    {
                        emiter = emiter.Subtract();
                        break;
                    }
                case BinaryOperator.MULTIPLY:
                    {
                        emiter = emiter.Multiply();
                        break;
                    }
                case BinaryOperator.DIVIDE:
                    {
                        emiter = emiter.Divide();
                        break;
                    }
                case BinaryOperator.EQUALS:
                    {
                        emiter = emiter.CompareEqual();
                        break;
                    }
                case BinaryOperator.DIFFERENT:
                    {
                        emiter = emiter.CompareEqual();
                        emiter = emiter.Not();
                        break;
                    }
                case BinaryOperator.OR:
                    {
                        emiter = emiter.Or();
                        break;
                    }
                case BinaryOperator.AND:
                    {
                        emiter = emiter.And();
                        break;
                    }
                case BinaryOperator.LESSER:
                    {
                        emiter = emiter.CompareLessThan();
                        break;
                    }
                case BinaryOperator.GREATER:
                    {
                        emiter = emiter.CompareGreaterThan();
                        break;
                    }
            }

            return emiter;
        }

        public override string Transpile(CompilerContext<EnquantoType> context)
        {
            var operators = new Dictionary<BinaryOperator, string>
            {
                {BinaryOperator.ADD, " + "},
                {BinaryOperator.SUB, " - "},
                {BinaryOperator.MULTIPLY, " * "},
                {BinaryOperator.DIVIDE, " / "},
                {BinaryOperator.EQUALS, " == "},
                {BinaryOperator.DIFFERENT, " != "},
                {BinaryOperator.OR, " || "},
                {BinaryOperator.AND, " && "},
                {BinaryOperator.LESSER, " < "},
                {BinaryOperator.GREATER, " + "},
                {BinaryOperator.CONCAT, " + "}
            };

            var code = new StringBuilder();
            code.Append(Left.Transpile(context));
            code.Append(operators[Operator]);
            code.Append(Right.Transpile(context));
            return code.ToString();
        }

        private Emit<Func<int>> EmitConcat(CompilerContext<EnquantoType> context, Emit<Func<int>> emiter)
        {
            Left.EmitByteCode(context, emiter);
            if (Left.Type != EnquantoType.STRING)
            {
                var t = TypeConverter<EnquantoType>.Emit(Left.Type);
                emiter.Box(t);
            }

            Right.EmitByteCode(context, emiter);
            if (Right.Type != EnquantoType.STRING)
            {
                var t = TypeConverter<EnquantoType>.Emit(Right.Type);
                emiter.Box(t);
            }

            var mi = typeof(string).GetMethod("Concat", new[] { typeof(object), typeof(object) });
            emiter.Call(mi);
            return emiter;
        }
    }
}
