using System.Collections.Generic;

namespace TIS100Sharp.Runtime.Nodes
{
    public class RandomAccessMemory : Node
    {
        public List<int> Data { get; private set; }

        public RandomAccessMemory(int capacity)
        {
            this.Data = new List<int>(capacity);
        }
    }
}
