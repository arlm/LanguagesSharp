using System.Collections.Generic;
using BabelFish.AST;
using enquanto.Model;

namespace enquanto
{
    internal class Signatures
    {
        private readonly Dictionary<BinaryOperator, List<Signature<EnquantoType>>> binaryOperationSignatures;

        public Signatures()
        {
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

        public EnquantoType CheckBinaryOperationTyping(BinaryOperator oper, EnquantoType left, EnquantoType right)
        {
            EnquantoType result;
            if (binaryOperationSignatures.ContainsKey(oper))
            {
                var signatures = binaryOperationSignatures[oper];
                var res = signatures.Find(sig => sig.Match(left, right));
                if (res != null)
                    result = res.Result;
                else
                    throw new SignatureException($"invalid operation {left} {oper} {right}");
            }
            else
            {
                result = left;
            }

            return result;
        }
    }
}
