using System;
using BabelFish.Compiler;
using FluentIL;

namespace Enquanto
{
    public interface IFluentIL<T> where T : Enum
    {
        IEmitter EmitByteCode(CompilerContext<T> context, IEmitter emiter);
    }
}
