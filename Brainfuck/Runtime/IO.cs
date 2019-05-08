using System;
using System.IO;
using System.Text;

namespace Brainfuck.Runtime
{
    public class IO
    {
        public TextWriter Out => Console.Out;
        public TextReader In => Console.In;

        private static IO instance;

        public static IO Instance => instance ?? (instance = new IO());

        private IO(Encoding encoding = null) 
        {
            encoding = encoding ?? Encoding.ASCII;

            Console.InputEncoding = encoding;
            Console.OutputEncoding = encoding;
        }
    }
}
