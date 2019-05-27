using System;
using System.Text;
using BabelFish.AST;
using BabelFish.Compiler;
using Sigil;
using sly.lexer;
using sly.parser;
using sly.parser.generator;

namespace enquanto
{
    public class Compiler
    {
        private readonly Parser<EnquantoToken, INode<EnquantoType>> enquantoParser;

        public Compiler()
        {
            var parser = new Parser();
            var builder = new ParserBuilder<EnquantoToken, INode<EnquantoType>>();
            var buildResult = builder.BuildParser(parser, ParserType.EBNF_LL_RECURSIVE_DESCENT, "statement");
            enquantoParser = buildResult.Result;
        }

        private string GetNameSpace(string id)
        {
            return $"NS{id.Replace("-", "")}";
        }

        private string GetClassName(string id)
        {
            return $"Class{id.Replace("-", "")}";
        }

        private string GetCSharpCode(string code, string id)
        {
            var classCode = new StringBuilder();
            classCode.AppendLine("using System;");
            classCode.AppendLine("using csly.whileLang.compiler;");
            classCode.AppendLine($"namespace {GetNameSpace(id)} {{");
            classCode.AppendLine($"     public class {GetClassName(id)} : WhileFunction {{");
            classCode.AppendLine("         public void Run() {");
            classCode.AppendLine(code);
            classCode.AppendLine("         }");
            classCode.AppendLine("      }");
            classCode.AppendLine("}");
            return classCode.ToString();
        }

        public string TranspileToCSharp(string whileCode)
        {
            string sharpCode = null;

            try
            {
                var result = enquantoParser.Parse(whileCode);

                if (result.IsOk)
                {
                    var ast = result.Result;

                    var checker = new SemanticChecker();

                    var context = checker.SemanticCheck(ast);

                    sharpCode = ast.Transpile(context);
                    sharpCode = GetCSharpCode(sharpCode, Guid.NewGuid().ToString());
                }
            }
            catch
            {
                sharpCode = null;
            }


            return sharpCode;
        }

        public Func<int> CompileToFunction(string whileCode)
        {
            Func<int> function = null;

            try
            {
                var result = enquantoParser.Parse(whileCode);
                if (result.IsOk)
                {
                    var ast = result.Result;

                    var checker = new SemanticChecker();

                    var context = checker.SemanticCheck(ast);

                    var emiter = Emit<Func<int>>.NewDynamicMethod("Method" + Guid.NewGuid());

                    emiter = ast.EmitByteCode(context, emiter);
                    //emiter.LoadConstant(42);                    
                    //emiter.Return();
                    function = emiter.CreateDelegate();
                    object res = function.Invoke();
                }
            }
            catch
            {
                function = null;
            }

            return function;
        }
    }
}