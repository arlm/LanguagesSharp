using BabelFish.Interpreter;
using enquanto;
using enquanto.Model;
using NUnit.Framework;
using sly.buildresult;
using sly.parser;
using sly.parser.generator;

namespace Tests
{
    public class IntrerpreterTests
    {
        private static BuildResult<Parser<EnquantoToken, AST>> Parser;


        public BuildResult<Parser<EnquantoToken, AST>> BuildParser()
        {
            if (Parser == null)
            {
                var whileParser = new Parser();
                var builder = new ParserBuilder<EnquantoToken, AST>();
                Parser = builder.BuildParser(whileParser, ParserType.EBNF_LL_RECURSIVE_DESCENT, "statement");
            }

            return Parser;
        }


        public bool CheckIntVariable(InterpreterContext<EnquantoType> context, string variable, int value)
        {
            var ok = false;
            if (context.GetVariable(variable) != null)
            {
                var v = context.GetVariable(variable).IntValue;
                ok = v == value;
            }

            return ok;
        }

        [Test]
        public void TestAssignAdd()
        {
            var buildResult = BuildParser();
            Assert.False(buildResult.IsError);
            var parser = buildResult.Result;
            var result = parser.Parse("(a:=1+1)");
            Assert.False(result.IsError);
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
            Assert.False(buildResult.IsError);
        }

        [Test]
        public void TestCounterProgram()
        {
            var buildResult = BuildParser();
            Assert.False(buildResult.IsError);
            var parser = buildResult.Result;
            var result = parser.Parse("(a:=0; enquanto a < 10 faça (imprima a; a := a +1 ))");
            Assert.False(result.IsError);
            Assert.NotNull(result.Result);
        }

        [Test]
        public void TestCounterProgramExec()
        {
            var buildResult = BuildParser();
            Assert.False(buildResult.IsError);
            var parser = buildResult.Result;
            var result = parser.Parse("(a:=0; enquanto a < 10 faça (imprima a; a := a +1 ))");
            Assert.False(result.IsError);
            Assert.NotNull(result.Result);
            var interpreter = new Interpreter();
            var context = interpreter.Execute(result.Result, true);
            Assert.That(context.variables, Has.Exactly(1).Items);
            Assert.True(CheckIntVariable(context, "a", 10));
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
            Assert.False(buildResult.IsError);
            var parser = buildResult.Result;
            var result = parser.Parse(program);
            Assert.False(result.IsError);
            Assert.NotNull(result.Result);
            var interpreter = new Interpreter();
            var context = interpreter.Execute(result.Result, true);
            Assert.That(context.variables.Count, Is.EqualTo(2));
            Assert.True(CheckIntVariable(context, "i", 11));
            Assert.True(CheckIntVariable(context, "r", 3628800));
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
            Assert.False(buildResult.IsError);
            var parser = buildResult.Result;
            var result = parser.Parse("se verdadeiro então (a := \"hello\") senão (b := \"world\")");
            Assert.False(result.IsError);
            Assert.NotNull(result.Result);

            Assert.That(result.Result, Is.TypeOf<SequenceStatement>());
            var seq = result.Result as SequenceStatement;
            Assert.That(seq.Get(0), Is.TypeOf<IfStatement>());
            var si = seq.Get(0) as IfStatement;
            var cond = si.Condition;
            Assert.That(cond, Is.TypeOf<BoolConstant>());
            Assert.True((cond as BoolConstant).Value);
            var s = si.ThenStmt;

            Assert.That(s, Is.TypeOf<SequenceStatement>());
            var thenBlock = s as SequenceStatement;
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
            Assert.False(buildResult.IsError);
            var parser = buildResult.Result;
            var result = parser.Parse("enquanto verdadeiro faça (pule)");
            Assert.False(result.IsError);
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
            Assert.False(buildResult.IsError);
            var parser = buildResult.Result;
            var result = parser.Parse("imprima verdadeiro e falso");
            Assert.False(result.IsError);
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
        public void TestSkip()
        {
            var buildResult = BuildParser();
            Assert.False(buildResult.IsError);
            var parser = buildResult.Result;
            var result = parser.Parse("pule");
            Assert.False(result.IsError);
            Assert.NotNull(result.Result);

            Assert.That(result.Result, Is.TypeOf<SequenceStatement>());
            var seq = result.Result as SequenceStatement;
            Assert.That(seq.Get(0), Is.TypeOf<SkipStatement>());
        }

        [Test]
        public void TestSkipAssignSequence()
        {
            var buildResult = BuildParser();
            Assert.False(buildResult.IsError);
            var parser = buildResult.Result;
            var result = parser.Parse("(a:=1; b:=2; c:=3)");
            Assert.False(result.IsError);
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
            Assert.False(buildResult.IsError);
            var parser = buildResult.Result;
            var result = parser.Parse("(pule; pule; pule)");
            Assert.False(result.IsError);
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