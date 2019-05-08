using System.Collections.Generic;

namespace TIS100Sharp.Runtime.Nodes
{
    public class StackMemory : Node
    {
        public Stack<int> Data { get; private set; }

        public StackMemory(int capacity)
        {
            this.Data = new Stack<int>(capacity);
        }
    }
}
