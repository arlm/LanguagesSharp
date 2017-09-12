using System.Collections.Generic;
using TIS100Sharp;
using TIS100Sharp.Operands;
using TIS100Sharp.Operators;
using NUnit.Framework;

namespace TIS100Test
{
    [TestFixture]
    public class FromManual
    {
        [Test]
        public void LeftDoubleRight()
        {
            var parser = Tis100Parser.Build();
            var result = parser.Parse(@"
MOV LEFT, ACC
ADD ACC
MOV ACC, RIGHT
            ");

            Assert.False(result.IsError);
            Assert.IsInstanceOf<List<Operator>>(result.Result);

            var program = result.Result as List<Operator>;
            Assert.AreEqual(3, program.Count);

            Assert.IsInstanceOf<Move>(program[0]);
            var move = program[0] as Move;
            Assert.AreEqual(Port.Available.LEFT, (move.Source as Port).Reference);
            Assert.AreEqual(Register.Available.ACC, (move.Destination as Register).Reference);

            Assert.IsInstanceOf<Add>(program[1]);
            var add = program[1] as Add;
            Assert.AreEqual(Register.Available.ACC, (add.Operand as Register).Reference);

            Assert.IsInstanceOf<Move>(program[2]);
			move = program[2] as Move;
            Assert.AreEqual(Register.Available.ACC, (move.Source as Register).Reference);
            Assert.AreEqual(Port.Available.RIGHT, (move.Destination as Port).Reference);
		}

        [Test]
        public void SequenceSorter()
        {
            var parser = Tis100Parser.Build();
            var result = parser.Parse(@"
START:
    MOV UP, ACC
    JGZ POSITIVE
    JLZ NEGATIVE
    JMP START
POSITIVE:
    MOV ACC, RIGHT
    JMP START
NEGATIVE:
    MOV ACC, LEFT
    JMP START
            ");

            Assert.False(result.IsError);
            Assert.IsInstanceOf<List<Operator>>(result.Result);

            var program = result.Result as List<Operator>;
            Assert.AreEqual(11, program.Count);

            Assert.IsInstanceOf<TIS100Sharp.Operators.Reference>(program[0]);
            var reference = program[0] as TIS100Sharp.Operators.Reference;
            Assert.AreEqual("START", reference.Name);

            Assert.IsInstanceOf<Move>(program[1]);
            var move = program[1] as Move;
            Assert.AreEqual(Port.Available.UP, (move.Source as Port).Reference);
            Assert.AreEqual(Register.Available.ACC, (move.Destination as Register).Reference);

            Assert.IsInstanceOf<Jump>(program[2]);
            var jump = program[2] as Jump;
			Assert.IsInstanceOf<TIS100Sharp.Operands.Reference>(jump.Operand);
            Assert.AreEqual("POSITIVE", (jump.Operand as TIS100Sharp.Operands.Reference).Name);
            Assert.AreEqual(Jump.ConditionType.GreaterThanZero, jump.Condition);

            Assert.IsInstanceOf<Jump>(program[3]);
            jump = program[3] as Jump;
            Assert.IsInstanceOf<TIS100Sharp.Operands.Reference>(jump.Operand);
            Assert.AreEqual("NEGATIVE", (jump.Operand as TIS100Sharp.Operands.Reference).Name);
            Assert.AreEqual(Jump.ConditionType.LesserThanZero, jump.Condition);

            Assert.IsInstanceOf<Jump>(program[4]);
            jump = program[4] as Jump;
            Assert.IsInstanceOf<TIS100Sharp.Operands.Reference>(jump.Operand);
            Assert.AreEqual("START", (jump.Operand as TIS100Sharp.Operands.Reference).Name);
            Assert.AreEqual(Jump.ConditionType.Inconditional, jump.Condition);

            Assert.IsInstanceOf<TIS100Sharp.Operators.Reference>(program[5]);
            reference = program[5] as TIS100Sharp.Operators.Reference;
            Assert.AreEqual("POSITIVE", reference.Name);

            Assert.IsInstanceOf<Move>(program[6]);
            move = program[6] as Move;
            Assert.AreEqual(Register.Available.ACC, (move.Source as Register).Reference);
            Assert.AreEqual(Port.Available.RIGHT, (move.Destination as Port).Reference);

            Assert.IsInstanceOf<Jump>(program[7]);
            jump = program[7] as Jump;
            Assert.IsInstanceOf<TIS100Sharp.Operands.Reference>(jump.Operand);
            Assert.AreEqual("START", (jump.Operand as TIS100Sharp.Operands.Reference).Name);
            Assert.AreEqual(Jump.ConditionType.Inconditional, jump.Condition);

            Assert.IsInstanceOf<TIS100Sharp.Operators.Reference>(program[8]);
            reference = program[8] as TIS100Sharp.Operators.Reference;
            Assert.AreEqual("NEGATIVE", reference.Name);

            Assert.IsInstanceOf<Move>(program[9]);
            move = program[9] as Move;
            Assert.AreEqual(Register.Available.ACC, (move.Source as Register).Reference);
            Assert.AreEqual(Port.Available.LEFT, (move.Destination as Port).Reference);

            Assert.IsInstanceOf<Jump>(program[10]);
            jump = program[10] as Jump;
            Assert.IsInstanceOf<TIS100Sharp.Operands.Reference>(jump.Operand);
            Assert.AreEqual("START", (jump.Operand as TIS100Sharp.Operands.Reference).Name);
            Assert.AreEqual(Jump.ConditionType.Inconditional, jump.Condition);
        }
    }
}
