namespace TIS100Sharp.Operands
{
    public abstract class Source<T> : Operand
    {
        public T Reference { get; protected set; }
    }
}
