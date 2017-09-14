namespace TIS100Sharp.Runtime
{
    public abstract class Node
    {
        public Port UP { get; protected set; }
        public Port DOWN { get; protected set; }
        public Port LEFT { get; protected set; }
        public Port RIGHT { get; protected set; }

        public void LinkUp(Node node) => this.UP = new Port(this, node);
        public void LinkDown(Node node) => this.DOWN = new Port(this, node);
        public void LinkLeft(Node node) => this.LEFT = new Port(this, node);
        public void LinkRight(Node node) => this.RIGHT = new Port(this, node);
    }
}
