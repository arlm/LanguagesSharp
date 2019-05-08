using System;
using System.Collections.Generic;
using NUnit.Framework;
using Brainfuck;
using Brainfuck.Operators;
using Brainfuck.Runtime;
using System.Linq;

namespace BrainfuckTest
{
    /// <summary>
    /// From Wikipedia <see cref="https://en.wikipedia.org/wiki/Brainfuck#Examples"/>
    /// </summary>
    [TestFixture]
    public class FromWikipedia
    {

        [Test]
        public void AddTwoValues()
        {
            // Add the current cell's value to the next cell:
            // Each time the loop is executed, the current cell is decremented,
            // the data pointer moves to the right, that next cell is incremented,
            // and the data pointer moves left again.
            // This sequence is repeated until the starting cell is 0.

            var parser = BrainfuckParser.Build();

            CheckAST(parser.Parse("[->+<]"));
            CheckAST(parser.Parse(" [  -\t> \t+\n<\r]\n\r"));
            CheckAST(parser.Parse("Testing how brainfuck works [ this should be ignored -&> 5 + 4 < 10\"]\""));

            var parseResult = parser.Parse(@"
            Let's do a test?
[ This is not a comment and should support all characters (including Unicode and Emojis)
    for (int i = 0; i - 1 > 0; i = i + 1)
    {
        bool j = i < 0;

        if (j)
        {
            break;
        }
    }
]");
            var block = CheckAST(parseResult);

            Pointer.Initialize(new byte[] { 0, 0 });
            block.Execute();
            Assert.AreEqual(new byte[] { 0, 0 }, Pointer.Instance.Buffer);

            Pointer.Initialize(new byte[] { 1, 0 });
            block.Execute();
            Assert.AreEqual(new byte[] { 0, 1 }, Pointer.Instance.Buffer);

            Pointer.Initialize(new byte[] { 2, 3 });
            block.Execute();
            Assert.AreEqual(new byte[] { 0, 5 }, Pointer.Instance.Buffer);
        }

        [Test]
        public void AdditionProgram()
        {
            var parser = BrainfuckParser.Build();

            var parseResult = parser.Parse(@"
                ++       Cell c0 = 2
                > +++++  Cell c1 = 5

                [        Start your loops with your cell pointer on the loop counter (c1 in our case)
                < +      Add 1 to c0
                > -      Subtract 1 from c1
                ]        End your loops with the cell pointer on the loop counter

                At this point our program has added 5 to 2 leaving 7 in c0 and 0 in c1
                BUT we cannot output this value to the terminal since it's not ASCII encoded!

                To display the ASCII character ""7"" we must add 48 to the value 7!
                48 = 6 * 8 so let's use another loop to help us!

                ++++++++  c1 = 8 and this will be our loop counter again
                [
                < ++++++Add 6 to c0
                > -Subtract 1 from c1
                ]
                < .Print out c0 which has the value 55 which translates to ""7""!
");

            Assert.False(parseResult.IsError, string.Join(", ", parseResult.Errors?.Select(e => e.ErrorMessage).ToArray() ?? Enumerable.Empty<string>()));
            Assert.True(parseResult.IsOk);

            var block = parseResult.Result as List<Operator>;

            block.Execute();
            Assert.AreEqual(0x37, Pointer.Instance.Buffer[0]);
            Assert.AreEqual(0, Pointer.Instance.Buffer[1]);
        }

        [Test]
        public void HelloWorld()
        {
            var parser = BrainfuckParser.Build();

            var parseResult = parser.Parse(@"
                [ This program prints ""Hello World!"" and a newline to the screen, its
                  length is 106 active command characters. [It is not the shortest.]

                  This loop is an ""initial comment loop"", a simple way of adding a comment
                  to a BF program such that you don't have to worry about any command
                  characters.Any ""."", "","", ""+"", ""-"", ""<"" and "">"" characters are simply
                  ignored, the ""["" and ""]"" characters just have to be balanced.This
                  loop and the commands it contains are ignored because the current cell
                  defaults to a value of 0; the 0 value causes this loop to be skipped.
                ]
                ++++++++Set Cell #0 to 8
                [

                > ++++Add 4 to Cell #1; this will always set Cell #1 to 4
                    [                   as the cell will be cleared by the loop
                > ++Add 2 to Cell #2
                        > +++Add 3 to Cell #3
                        > +++Add 3 to Cell #4
                        > +Add 1 to Cell #5
                        <<<< -Decrement the loop counter in Cell #1
                    ]                   Loop till Cell #1 is zero; number of iterations is 4
                    > +Add 1 to Cell #2
                    > +Add 1 to Cell #3
                    > -Subtract 1 from Cell #4
                    >> +Add 1 to Cell #6
                    [<]                 Move back to the first zero cell you find; this will
                                        be Cell #1 which was cleared by the previous loop
                    < -Decrement the loop Counter in Cell #0
                ]                       Loop till Cell #0 is zero; number of iterations is 8

                The result of this is:
                Cell No :   0   1   2   3   4   5   6
                Contents:   0   0  72 104  88  32   8
                Pointer:    ^
");

            Assert.False(parseResult.IsError, string.Join(", ", parseResult.Errors?.Select(e => e.ErrorMessage).ToArray() ?? Enumerable.Empty<string>()));
            Assert.True(parseResult.IsOk);

            var block = parseResult.Result as List<Operator>;

            Pointer.Initialize();
            block.Execute();
            Assert.AreEqual(0, Pointer.Instance.Buffer[0]);
            Assert.AreEqual(0, Pointer.Instance.Buffer[1]);
            Assert.AreEqual(72, Pointer.Instance.Buffer[2]);
            Assert.AreEqual(104, Pointer.Instance.Buffer[3]);
            Assert.AreEqual(88, Pointer.Instance.Buffer[4]);
            Assert.AreEqual(32, Pointer.Instance.Buffer[5]);
            Assert.AreEqual(8, Pointer.Instance.Buffer[6]);

            parseResult = parser.Parse(@"
                [ This program prints ""Hello World!"" and a newline to the screen, its
                  length is 106 active command characters. [It is not the shortest.]

                  This loop is an ""initial comment loop"", a simple way of adding a comment
                  to a BF program such that you don't have to worry about any command
                  characters.Any ""."", "","", ""+"", ""-"", ""<"" and "">"" characters are simply
                  ignored, the ""["" and ""]"" characters just have to be balanced.This
                  loop and the commands it contains are ignored because the current cell
                  defaults to a value of 0; the 0 value causes this loop to be skipped.
                ]
                ++++++++Set Cell #0 to 8
                [

                > ++++Add 4 to Cell #1; this will always set Cell #1 to 4
                    [                   as the cell will be cleared by the loop
                > ++Add 2 to Cell #2
                        > +++Add 3 to Cell #3
                        > +++Add 3 to Cell #4
                        > +Add 1 to Cell #5
                        <<<< -Decrement the loop counter in Cell #1
                    ]                   Loop till Cell #1 is zero; number of iterations is 4
                    > +Add 1 to Cell #2
                    > +Add 1 to Cell #3
                    > -Subtract 1 from Cell #4
                    >> +Add 1 to Cell #6
                    [<]                 Move back to the first zero cell you find; this will
                                        be Cell #1 which was cleared by the previous loop
                    < -Decrement the loop Counter in Cell #0
                ]                       Loop till Cell #0 is zero; number of iterations is 8

                The result of this is:
                Cell No :   0   1   2   3   4   5   6
                Contents:   0   0  72 104  88  32   8
                Pointer:    ^


                >>.Cell #2 has value 72 which is 'H'
                > ---.Subtract 3 from Cell #3 to get 101 which is 'e'
                +++++++..++ +.Likewise for 'llo' from Cell #3
                >>.Cell #5 is 32 for the space
                < -.Subtract 1 from Cell #4 for 87 to give a 'W'
                <.                      Cell #3 was set to 'o' from the end of 'Hello'
                +++.------.--------.Cell #3 for 'rl' and 'd'
                >> +.Add 1 to Cell #5 gives us an exclamation point
                > ++.And finally a newline from Cell #6
");

            Assert.False(parseResult.IsError, string.Join(", ", parseResult.Errors?.Select(e => e.ErrorMessage).ToArray() ?? Enumerable.Empty<string>()));
            Assert.True(parseResult.IsOk);

            block = parseResult.Result as List<Operator>;

            Pointer.Initialize();
            block.Execute();
            Assert.AreEqual(0, Pointer.Instance.Buffer[0]);
            Assert.AreEqual(0, Pointer.Instance.Buffer[1]);
            Assert.AreEqual(72, Pointer.Instance.Buffer[2]);
            Assert.AreEqual(100, Pointer.Instance.Buffer[3]);
            Assert.AreEqual(87, Pointer.Instance.Buffer[4]);
            Assert.AreEqual(33, Pointer.Instance.Buffer[5]);
            Assert.AreEqual(10, Pointer.Instance.Buffer[6]);
                      
            parseResult = parser.Parse("++++++++[>++++[>++>+++>+++>+<<<<-]>+>+>->>+[<]<-]");

            Assert.False(parseResult.IsError, string.Join(", ", parseResult.Errors?.Select(e => e.ErrorMessage).ToArray() ?? Enumerable.Empty<string>()));
            Assert.True(parseResult.IsOk);

            block = parseResult.Result as List<Operator>;

            Pointer.Initialize();
            block.Execute();
            Assert.AreEqual(0, Pointer.Instance.Buffer[0]);
            Assert.AreEqual(0, Pointer.Instance.Buffer[1]);
            Assert.AreEqual(72, Pointer.Instance.Buffer[2]);
            Assert.AreEqual(104, Pointer.Instance.Buffer[3]);
            Assert.AreEqual(88, Pointer.Instance.Buffer[4]);
            Assert.AreEqual(32, Pointer.Instance.Buffer[5]);
            Assert.AreEqual(8, Pointer.Instance.Buffer[6]);

            parseResult = parser.Parse(">>.>---.+++++++..+++.>>.<-.<.+++.------.--------.>>+.>++.");

            Assert.False(parseResult.IsError, string.Join(", ", parseResult.Errors?.Select(e => e.ErrorMessage).ToArray() ?? Enumerable.Empty<string>()));
            Assert.True(parseResult.IsOk);

            block = parseResult.Result as List<Operator>;

            block.Execute();
            Assert.AreEqual(0, Pointer.Instance.Buffer[0]);
            Assert.AreEqual(0, Pointer.Instance.Buffer[1]);
            Assert.AreEqual(72, Pointer.Instance.Buffer[2]);
            Assert.AreEqual(100, Pointer.Instance.Buffer[3]);
            Assert.AreEqual(87, Pointer.Instance.Buffer[4]);
            Assert.AreEqual(33, Pointer.Instance.Buffer[5]);
            Assert.AreEqual(10, Pointer.Instance.Buffer[6]);
        }

        private static Block CheckAST(sly.parser.ParseResult<BrainfuckToken, object> result)
        {
            Assert.False(result.IsError, string.Join(", ", result.Errors?.Select(e => e.ErrorMessage).ToArray() ?? Enumerable.Empty<string>()));
            Assert.True(result.IsOk);

            Assert.IsInstanceOf<List<Operator>>(result.Result);

            var program = result.Result as List<Operator>;
            Assert.AreEqual(1, program.Count);

            Assert.IsInstanceOf<Block>(program[0]);
            var block = program[0] as Block;
            Assert.AreEqual(4, block.Closure.Count);
            Assert.AreEqual(BrainfuckToken.MINUS, block.Closure[0].Token);
            Assert.AreEqual(BrainfuckToken.GREATER_THAN, block.Closure[1].Token);
            Assert.AreEqual(BrainfuckToken.PLUS, block.Closure[2].Token);
            Assert.AreEqual(BrainfuckToken.LESSER_THAN, block.Closure[3].Token);
            return block;
        }
    }
}
