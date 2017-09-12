using System;
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
            var result = parser.Parse("MOV LEFT, ACC");
//            var result = parser.Parse(@"
//MOV LEFT, ACC
//ADD ACC
//MOV ACC, RIGHT
            //");

            Assert.True(result.IsEnded);
            Assert.False(result.IsError);
            Assert.IsInstanceOf<List<object>>(result.Result);

            var program = result.Result as List<object>;
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
    }
}
