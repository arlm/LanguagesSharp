using System.Collections.Generic;

namespace TIS100Sharp.Runtime.Nodes
{
    public class BasicExecution : Node
    {
        public int ACC { get; set; }
        public int BAK { get; internal set; }

        private LinkedListNode<Operator> ExecutionPointer;

        public LinkedList<Operator> Instructions { get; protected set; }

        protected BasicExecution(IEnumerable<Operator> instructions)
        {
            this.Instructions = new LinkedList<Operator>(instructions);
        }
    }
}
