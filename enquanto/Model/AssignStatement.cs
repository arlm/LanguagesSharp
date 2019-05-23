using System;
using System.Text;
using BabelFish.AST;
using BabelFish.Compiler;
using Sigil;
using sly.lexer;

namespace enquanto.Model
{
    internal class AssignStatement : IStatement<EnquantoType>
    {
        public AssignStatement(string variableName, IExpression<EnquantoType> value)
        {
            VariableName = variableName;
            Value = value;
        }

        public string VariableName { get; set; }

        public IExpression<EnquantoType> Value { get; set; }

        public bool IsVariableCreation { get; internal set; }

        public Scope<EnquantoType> CompilerScope { get; set; }

        public TokenPosition Position { get; set; }

        public string Dump(string tab)
        {
            var dmp = new StringBuilder();
            dmp.AppendLine($"{tab}(ASSIGN");
            dmp.AppendLine($"{tab}\t{VariableName}");
            dmp.AppendLine(Value.Dump(tab + "\t"));
            dmp.AppendLine($"{tab})");
            return dmp.ToString();
        }

        public Emit<Func<int>> EmitByteCode(CompilerContext<EnquantoType> context, Emit<Func<int>> emiter)
        {
            using (Local local = IsVariableCreation
                ? emiter.DeclareLocal(TypeConverter<EnquantoType>.Emit(CompilerScope.GetVariableType(VariableName)),
                    VariableName)
                : emiter.Locals[VariableName])
            {
                Value.EmitByteCode(context, emiter);
                emiter.StoreLocal(local);
            }

            return emiter;
        }

        public string Transpile(CompilerContext<EnquantoType> context)
        {
            var code = new StringBuilder();

            if (IsVariableCreation)
            {
                var type = TypeConverter<EnquantoType>.Transpile(CompilerScope.GetVariableType(VariableName), Language.CSharp);
                code.AppendLine(
                    $"{type} {VariableName};");
            }

            code.AppendLine($"{VariableName} = {Value.Transpile(context)};");

            return code.ToString();
        }
    }
}