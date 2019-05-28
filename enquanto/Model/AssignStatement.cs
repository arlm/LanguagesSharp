using System;
using System.Text;
using BabelFish.AST;
using BabelFish.Compiler;
using Sigil;
using sly.lexer;

namespace enquanto.Model
{
    internal class AssignStatement : AST, IStatement<EnquantoType>
    {
        public AssignStatement(string variableName, IExpression<EnquantoType> value)
        {
            VariableName = variableName;
            Value = value;
        }

        public string VariableName { get; set; }

        public IExpression<EnquantoType> Value { get; set; }

        public bool IsVariableCreation { get; internal set; }

        public override string Dump(string tab)
        {
            var dmp = new StringBuilder();
            dmp.AppendLine($"{tab}(ASSIGN");
            dmp.AppendLine($"{tab}\t{VariableName}");
            dmp.AppendLine(Value.Dump(tab + "\t"));
            dmp.AppendLine($"{tab})");
            return dmp.ToString();
        }

        public override Emit<Func<int>> EmitByteCode(CompilerContext<EnquantoType> context, Emit<Func<int>> emiter)
        {
			Local local = null;

			if(IsVariableCreation)
			{
				var type = TypeConverter<EnquantoType>.Emit(CompilerScope.GetVariableType(VariableName));
				local = emiter.DeclareLocal(type, VariableName );
			}
			else
			{
				local = emiter.Locals[VariableName];
			}

            Value.EmitByteCode(context, emiter);
            emiter.StoreLocal(local);

            return emiter;
        }

        public override string Transpile(CompilerContext<EnquantoType> context)
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