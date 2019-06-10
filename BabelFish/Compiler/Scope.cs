using System;
using System.Collections.Generic;
using System.Text;

namespace BabelFish.Compiler
{
    public class Scope<T> where T : Enum
    {
        public int Id;

        public Scope<T> ParentScope;

        public List<Scope<T>> scopes;

        private readonly Dictionary<string, T> variables;

        public Scope()
        {
            Id = 0;
            ParentScope = null;
            variables = new Dictionary<string, T>();
            scopes = new List<Scope<T>>();
        }

        protected Scope(Scope<T> parent, int id)
        {
            Id = id;
            ParentScope = parent;
            variables = new Dictionary<string, T>();
            scopes = new List<Scope<T>>();
        }

        public string Path
        {
            get
            {
                string path = ParentScope == null ? Id.ToString() : $"{ParentScope.Path}.{Id}";
                return path;
            }
        }

        public Scope<T> CreateNewScope()
        {
            var scope = new Scope<T>(this, scopes.Count);
            scopes.Add(scope);
            return scope;
        }

        /// <summary>
        ///     Set a variable type
        /// </summary>
        /// <param name="name">varaible name</param>
        /// <param name="variableType">variable type</param>
        /// <returns>true if variable is a new variable in scope</returns>
        public bool SetVariableType(string name, T variableType)
        {
            var creation = !variables.ContainsKey(name);
            variables[name] = variableType;
            return creation;
        }

        public T GetVariableType(string name)
        {
            var varType = default(T);

            if (variables.ContainsKey(name))
            {
                varType = variables[name];
            }
            else if (ParentScope != null)
            {
                varType = ParentScope.GetVariableType(name);
            }

            return varType;
        }

        public bool ExistsVariable(string name)
        {
            var exists = variables.ContainsKey(name);

            if (!exists && ParentScope != null)
            {
                exists = ParentScope.ExistsVariable(name);
            }

            return exists;
        }

        public string Dump(string tab = "")
        {
            var dmp = new StringBuilder();
            dmp.AppendLine($"{tab}scope({Path}) {{");
            dmp.AppendLine($"{tab}\t[");

            foreach (var pair in variables)
            {
                dmp.AppendLine($"{tab}\t{pair.Key}={pair.Value}");
            }

            dmp.AppendLine($"{tab}\t],");

            foreach (var scope in scopes)
            {
                dmp.AppendLine($"{scope.Dump(tab + "\t")},");
            }

            dmp.AppendLine($"{tab}}}");
            return dmp.ToString();
        }
    }
}