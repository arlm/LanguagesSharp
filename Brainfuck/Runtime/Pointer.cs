using System.Collections.Generic;
using System.Linq;

namespace Brainfuck.Runtime
{
    public class Pointer
    {
        private const int SIZE = 30000;

        private uint index;
        private static Pointer instance;

        public byte[] Buffer { get; private set; }

        public byte Data
        {
            get => Buffer[index];
            set => Buffer[index] = value;
        }

        public static Pointer Instance => instance ?? (instance = new Pointer(SIZE));

        internal static void Initialize(int size = SIZE) => instance = new Pointer(SIZE);

        internal static void Initialize(IEnumerable<byte> vector) => instance = new Pointer { Buffer = vector.ToArray() };

        private Pointer(int size) => this.Buffer = new byte[size];
        private Pointer() {}

        public void Increment()
        {
            index++;
        }

        public void Decrement()
        {
            index--;
        }

        public void IncrementData()
        {
            Buffer[index]++;
        }

        public void DecrementData()
        {
            Buffer[index]--;
        }
    }
}
