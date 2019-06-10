using System;
using BabelFish.Compiler;

namespace BabelFish.AST
{
    public class ContextException<T> : Exception where T : Enum
    {
        public ContextException(string message) : base(message)
        {
        }

        public ContextException(string variableName, CompilerContext<T> context) :
             base($"Variable {variableName} not present in context.\n{context.Dump()}")
        {

        }
    }
}
