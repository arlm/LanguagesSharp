namespace TIS100Sharp.Runtime
{
    public class Port
    {
        private int? value;

        public Node Node1 { get; private set; }
        public Node Node2 { get; private set; }

        public Port(Node node1, Node node2)
        {
            this.Node1 = node1;
            this.Node2 = node2;
        }
    }
}
