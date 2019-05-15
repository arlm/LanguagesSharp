using System;

namespace BabelFish.AST
{
    public interface IStatement<T> : INode<T> where T : Enum
    {
    }
}