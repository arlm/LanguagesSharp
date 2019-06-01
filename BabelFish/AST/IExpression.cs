using System;

namespace BabelFish.AST
{
    public interface IExpression<T> : INode<T> where T : Enum
    {
        T Type { get; set; }
    }
}