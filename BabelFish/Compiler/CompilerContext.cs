using System;

namespace BabelFish.Compiler
{
    public class CompilerContext<T> where T : Enum
    {
        public CompilerContext()
        {
            RootScope = new Scope<T>();
            CurrentScope = RootScope;
        }

        public Scope<T> RootScope { get; protected set; }

        public Scope<T> CurrentScope { get; protected set; }


        public string Dump()
        {
            return RootScope.Dump("");
        }

        public override string ToString()
        {
            return Dump();
        }

        #region variable management

        public bool SetVariableType(string name, T variableType)
        {
            return CurrentScope.SetVariableType(name, variableType);
        }

        public T GetVariableType(string name)
        {
            return CurrentScope.GetVariableType(name);
        }

        public bool VariableExists(string name)
        {
            return CurrentScope.ExistsVariable(name);
        }

        #endregion

        #region scope management

        public Scope<T> OpenNewScope()
        {
            var scope = CurrentScope.CreateNewScope();
            CurrentScope = scope;
            return scope;
        }

        public Scope<T> CloseScope()
        {
            CurrentScope = CurrentScope?.ParentScope;
            return CurrentScope;
        }

        #endregion
    }
}