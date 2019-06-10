using System.Collections.Generic;
using BabelFish.Compiler;
using enquanto;
using FluentIL;

namespace Enquanto
{
    public static class Scope_FluentIL
    {
        private static readonly Dictionary<Scope<EnquantoType>, Dictionary<string, ILocal>> variables = new Dictionary<Scope<EnquantoType>, Dictionary<string, ILocal>>();

        public static void AddLocal(this Scope<EnquantoType> scope, string name, ILocal localRef)
        {
            if (variables.ContainsKey(scope))
            {
                if (!variables[scope].ContainsKey(name))
                {
                    variables[scope].Add(name, localRef);
                }
            }
            else
            {
                var dictionary = new Dictionary<string, ILocal>
                {
                    { name, localRef }
                };

                variables.Add(scope, dictionary);
            }
        }

        public static ILocal GetLocal(this Scope<EnquantoType> scope, string name)
        {
            if (variables.ContainsKey(scope))
            {
                if (variables[scope].ContainsKey(name))
                {
                    return variables[scope][name];
                }
            }

            return null;
        }
    }
}
