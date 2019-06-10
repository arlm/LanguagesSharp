using System;
using System.Diagnostics;
using System.Text;
using BabelFish.AST;
using FluentIL;
using Sigil;
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

        public string Dump(string code)
        {
            string dump = null;

            try
            {
                var result = enquantoParser.Parse(code);

                if (result.IsOk)
                {
                    var ast = result.Result;

                    dump = ast.Dump(string.Empty);
                }
            }
            catch
            {
                dump = null;
            }

            return dump;
        }

        public string TranspileToCSharp(string code)
        {
            string sharpCode = null;

            try
            {
                var result = enquantoParser.Parse(code);

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

        public bool CreateConsoleApp(string code, string fileName, string moduleName)
        {
            try
            {
                var result = enquantoParser.Parse(code);
                var ast = result.Result as AST;

                var checker = new SemanticChecker();
                var context = checker.SemanticCheck(ast);

                string typeName = GetClassName(Guid.NewGuid().ToString());

                var type = new TypeFactory(fileName, moduleName)
                    .NewType(typeName)
                    .Public()
                    .Class();

                var main = type
                    .NewMethod("main")
                    .Static()
                    .HideBySig()
                    .Param(typeof(string[]), "args");

                ast.EmitByteCode(context, main.Body());

                //type.Save();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public Func<int> CompileToFunction(string code)
        {
            Func<int> function = null;

            try
            {
                var result = enquantoParser.Parse(code);

                if (result.IsOk)
                {
                    var ast = result.Result;

                    var checker = new SemanticChecker();
                    var context = checker.SemanticCheck(ast);

                    var emiter = Emit<Func<int>>.NewDynamicMethod("Method" + Guid.NewGuid());

                    emiter = ast.EmitByteCode(context, emiter);

                    function = emiter.CreateDelegate();
                    object res = function.Invoke();
                }
            }
            catch (Exception ex)
            {
				Debug.WriteLine(ex.ToString());
                function = null;
            }

            return function;
        }
    }
}