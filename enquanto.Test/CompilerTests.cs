using System.Linq;
using BabelFish.AST;
using enquanto;
using enquanto.Model;
using NUnit.Framework;
using sly.buildresult;
using sly.parser;
using sly.parser.generator;

namespace Tests
{
    public class CompilerTests
    {
        private static BuildResult<Parser<EnquantoToken, INode<EnquantoType>>> Parser;

        public BuildResult<Parser<EnquantoToken, INode<EnquantoType>>> BuildParser()
        {
            if (Parser == null)
            {
                var enquantoParser = new Parser();
                var builder = new ParserBuilder<EnquantoToken, INode<EnquantoType>>();
                Parser = builder.BuildParser(enquantoParser, ParserType.EBNF_LL_RECURSIVE_DESCENT, "statement");
            }

            return Parser;
        }

        [Test]
        public void TestAssignAdd()
        {
            var buildResult = BuildParser();
            Assert.False(buildResult.IsError, string.Join(";", buildResult.Errors?.Select(e => e.Message)  ?? Enumerable.Empty<string>()));
            var parser = buildResult.Result;
            var result = parser.Parse("(a:=1+1)");
            Assert.False(result.IsError, string.Join(";", result.Errors?.Select(e => e.ToString()) ?? Enumerable.Empty<string>()));
            Assert.NotNull(result.Result);

            Assert.That(result.Result, Is.TypeOf<SequenceStatement>());
            var seq = result.Result as SequenceStatement;
            Assert.That(seq.Get(0), Is.TypeOf<AssignStatement>());
            var assign = seq.Get(0) as AssignStatement;
            Assert.That(assign.VariableName, Is.EqualTo("a"));
            var val = assign.Value;
            Assert.That(val, Is.TypeOf<BinaryOperation>());
            var bin = val as BinaryOperation;
            Assert.That(bin.Operator, Is.EqualTo(BinaryOperator.ADD));
            Assert.That((bin.Left as IntegerConstant)?.Value, Is.EqualTo(1));
            Assert.That((bin.Right as IntegerConstant)?.Value, Is.EqualTo(1));
        }

        [Test]
        public void TestBuildParser()
        {
            var buildResult = BuildParser();
            Assert.False(buildResult.IsError, string.Join(";", buildResult.Errors?.Select(e => e.Message)  ?? Enumerable.Empty<string>()));
            Assert.IsNotNull(buildResult.Result);
        }

        [Test]
        public void TestCounterProgram()
        {
            var buildResult = BuildParser();
            Assert.False(buildResult.IsError, string.Join(";", buildResult.Errors?.Select(e => e.Message)  ?? Enumerable.Empty<string>()));
            var parser = buildResult.Result;
            var result = parser.Parse("(a:=0; enquanto a < 10 faça (imprima a; a := a +1 ))");
            Assert.False(result.IsError, string.Join(";", result.Errors?.Select(e => e.ToString()) ?? Enumerable.Empty<string>()));
            Assert.NotNull(result.Result);
        }

        [Test]
        public void TestCounterProgramExec()
        {
            var buildResult = BuildParser();
            Assert.False(buildResult.IsError, string.Join(";", buildResult.Errors?.Select(e => e.Message)  ?? Enumerable.Empty<string>()));
            var parser = buildResult.Result;
            var result = parser.Parse("(a:=0; enquanto a < 10 faça (imprima a; a := a +1 ))");
            Assert.False(result.IsError, string.Join(";", result.Errors?.Select(e => e.ToString()) ?? Enumerable.Empty<string>()));
            Assert.NotNull(result.Result);
        }

        [Test]
        public void TestFactorialProgramExec()
        {
            var program = @"
(
    r:=1;
    i:=1;
    enquanto i < 11 faça 
    ( 
    r := r * i;
    imprima r;
    imprima i;
    i := i + 1 )
)";
            var buildResult = BuildParser();
            Assert.False(buildResult.IsError, string.Join(";", buildResult.Errors?.Select(e => e.Message)  ?? Enumerable.Empty<string>()));
            var parser = buildResult.Result;
            var result = parser.Parse(program);
            Assert.False(result.IsError, string.Join(";", result.Errors?.Select(e => e.ToString()) ?? Enumerable.Empty<string>()));
            Assert.NotNull(result.Result);
        }

        [Test]
        public void TestFactorialProgramExecAsIL()
        {
            var program = @"
(
    r:=1;
    i:=1;
    enquanto i < 11 faça 
    ( 
    r := r * i;
    imprima """".r;
    imprima """".i;
    i := i + 1 );
retorne r
)";
            var compiler = new Compiler();
            var func = compiler.CompileToFunction(program);
            Assert.NotNull(func);
            var f = func();
            Assert.That(f, Is.EqualTo(3628800));
        }

        [Test]
        public void TestIfThenElse()
        {
            var buildResult = BuildParser();
            Assert.False(buildResult.IsError, string.Join(";", buildResult.Errors?.Select(e => e.Message)  ?? Enumerable.Empty<string>()));
            var parser = buildResult.Result;
            var result = parser.Parse("se verdadeiro então (a := \"hello\") senão (b := \"world\")");
            Assert.False(result.IsError, string.Join(";", result.Errors?.Select(e => e.ToString()) ?? Enumerable.Empty<string>()));
            Assert.NotNull(result.Result);

            Assert.That(result.Result, Is.TypeOf<SequenceStatement>());
            var seq = result.Result as SequenceStatement;
            Assert.That(seq.Get(0), Is.TypeOf<IfStatement>());
            var si = seq.Get(0) as IfStatement;
            var cond = si.Condition;
            Assert.That(cond, Is.TypeOf<BoolConstant>());
            Assert.True((cond as BoolConstant).Value);
            var s = si.ThenStmt;

            Assert.That(si.ThenStmt, Is.TypeOf<SequenceStatement>());
            var thenBlock = si.ThenStmt as SequenceStatement;
            Assert.That(thenBlock.Count, Is.EqualTo(1));
            Assert.That(thenBlock.Get(0), Is.TypeOf<AssignStatement>());
            var thenAssign = thenBlock.Get(0) as AssignStatement;
            Assert.That(thenAssign.VariableName, Is.EqualTo("a"));
            Assert.That(thenAssign.Value, Is.TypeOf<StringConstant>());
            Assert.That((thenAssign.Value as StringConstant).Value, Is.EqualTo("hello"));

            Assert.That(si.ElseStmt, Is.TypeOf<SequenceStatement>());
            var elseBlock = si.ElseStmt as SequenceStatement;
            Assert.That(elseBlock.Count, Is.EqualTo(1));
            Assert.That(elseBlock.Get(0), Is.TypeOf<AssignStatement>());
            var elseAssign = elseBlock.Get(0) as AssignStatement;
            Assert.That(elseAssign.VariableName, Is.EqualTo("b"));
            Assert.That(elseAssign.Value, Is.TypeOf<StringConstant>());
            Assert.That((elseAssign.Value as StringConstant).Value, Is.EqualTo("world"));
        }

        [Test]
        public void TestInfiniteWhile()
        {
            var buildResult = BuildParser();
            Assert.False(buildResult.IsError, string.Join(";", buildResult.Errors?.Select(e => e.Message)  ?? Enumerable.Empty<string>()));
            var parser = buildResult.Result;
            var result = parser.Parse("enquanto verdadeiro faça (pule)");
            Assert.False(result.IsError, string.Join(";", result.Errors?.Select(e => e.ToString()) ?? Enumerable.Empty<string>()));
            Assert.NotNull(result.Result);

            Assert.That(result.Result, Is.TypeOf<SequenceStatement>());
            var seq = result.Result as SequenceStatement;
            Assert.That(seq.Get(0), Is.TypeOf<WhileStatement>());
            var whil = seq.Get(0) as WhileStatement;
            var cond = whil.Condition;
            Assert.That(cond, Is.TypeOf<BoolConstant>());
            Assert.True((cond as BoolConstant).Value);
            var s = whil.BlockStmt;
            Assert.That(whil.BlockStmt, Is.TypeOf<SequenceStatement>());
            var seqBlock = whil.BlockStmt as SequenceStatement;
            Assert.That(seqBlock.Count, Is.EqualTo(1));
            Assert.That(seqBlock.Get(0), Is.TypeOf<SkipStatement>());
        }

        [Test]
        public void TestPrintBoolExpression()
        {
            var buildResult = BuildParser();
            Assert.False(buildResult.IsError, string.Join(";", buildResult.Errors?.Select(e => e.Message)  ?? Enumerable.Empty<string>()));
            var parser = buildResult.Result;
            var result = parser.Parse("imprima verdadeiro e falso");
            Assert.False(result.IsError, string.Join(";", result.Errors?.Select(e => e.ToString()) ?? Enumerable.Empty<string>()));
            Assert.NotNull(result.Result);

            Assert.That(result.Result, Is.TypeOf<SequenceStatement>());
            var seq = result.Result as SequenceStatement;
            Assert.That(seq.Get(0), Is.TypeOf<PrintStatement>());
            var print = seq.Get(0) as PrintStatement;
            var expr = print.Value;
            Assert.That(expr, Is.TypeOf<BinaryOperation>());
            var bin = expr as BinaryOperation;
            Assert.That(bin.Operator, Is.EqualTo(BinaryOperator.AND));
            Assert.True((bin.Left as BoolConstant)?.Value);
            Assert.False((bin.Right as BoolConstant)?.Value);
        }

        [Test]
        public void TestBoolExpression()
        {
            var buildResult = BuildParser();
            Assert.False(buildResult.IsError, string.Join(";", buildResult.Errors?.Select(e => e.Message) ?? Enumerable.Empty<string>()));
            var parser = buildResult.Result;
            var result = parser.Parse("a := verdadeiro e falso");
            Assert.False(result.IsError, string.Join(";", result.Errors?.Select(e => e.ToString()) ?? Enumerable.Empty<string>()));
            Assert.NotNull(result.Result);

            Assert.That(result.Result, Is.TypeOf<SequenceStatement>());
            var seq = result.Result as SequenceStatement;
            Assert.That(seq.Get(0), Is.TypeOf<AssignStatement>());
            var assignment = seq.Get(0) as AssignStatement;
            var expr = assignment.Value;
            Assert.That(expr, Is.TypeOf<BinaryOperation>());
            var bin = expr as BinaryOperation;
            Assert.That(bin.Operator, Is.EqualTo(BinaryOperator.AND));
            Assert.True((bin.Left as BoolConstant)?.Value);
            Assert.False((bin.Right as BoolConstant)?.Value);
        }

        [Test]
        public void TestSkip()
        {
            var buildResult = BuildParser();
            Assert.False(buildResult.IsError, string.Join(";", buildResult.Errors?.Select(e => e.Message)  ?? Enumerable.Empty<string>()));
            var parser = buildResult.Result;
            var result = parser.Parse("pule");
            Assert.False(result.IsError, string.Join(";", result.Errors?.Select(e => e.ToString()) ?? Enumerable.Empty<string>()));
            Assert.NotNull(result.Result);

            Assert.That(result.Result, Is.TypeOf<SequenceStatement>());
            var seq = result.Result as SequenceStatement;
            Assert.That(seq.Get(0), Is.TypeOf<SkipStatement>());
        }

        [Test]
        public void TestSkipAssignSequence()
        {
            var buildResult = BuildParser();
            Assert.False(buildResult.IsError, string.Join(";", buildResult.Errors?.Select(e => e.Message) ?? Enumerable.Empty<string>()));
            var parser = buildResult.Result;
            var result = parser.Parse("(a:=1; b:=2; c:=3)");
            Assert.False(result.IsError, string.Join(";", result.Errors?.Select(e => e.ToString()) ?? Enumerable.Empty<string>()));
            Assert.NotNull(result.Result);
            Assert.That(result.Result, Is.TypeOf<SequenceStatement>());
            var seq = result.Result as SequenceStatement;
            Assert.That(seq.Count, Is.EqualTo(3));

            string[] names = { "a", "b", "c" };
            for (var i = 0; i < names.Length; i++)
            {
                Assert.That(seq.Get(i), Is.TypeOf<AssignStatement>());
                var assign = seq.Get(i) as AssignStatement;
                Assert.That(assign.VariableName, Is.EqualTo(names[i]));
                Assert.That((assign.Value as IntegerConstant).Value, Is.EqualTo(i + 1));
            }
        }

        [Test]
        public void TestSkipSkipSequence()
        {
            var buildResult = BuildParser();
            Assert.False(buildResult.IsError, string.Join(";", buildResult.Errors?.Select(e => e.Message)  ?? Enumerable.Empty<string>()));
            var parser = buildResult.Result;
            var result = parser.Parse("(pule; pule; pule)");
            Assert.False(result.IsError, string.Join(";", result.Errors?.Select(e => e.ToString()) ?? Enumerable.Empty<string>()));
            Assert.NotNull(result.Result);
            Assert.That(result.Result, Is.TypeOf<SequenceStatement>());
            var seq = result.Result as SequenceStatement;
            Assert.That(seq.Count, Is.EqualTo(3));
            Assert.That(seq.Get(0), Is.TypeOf<SkipStatement>());
            Assert.That(seq.Get(1), Is.TypeOf<SkipStatement>());
            Assert.That(seq.Get(2), Is.TypeOf<SkipStatement>());
        }
    }
}