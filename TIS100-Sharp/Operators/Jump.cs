namespace TIS100Sharp.Operators
{
    public class Jump : Operator
    {
        public Operand Operand { get; private set; }
        public ConditionType Condition { get; private set; }

        public Jump(Operand op, ConditionType type = ConditionType.Inconditional)
		{
            this.Operand = op;
            this.Condition = type;
		}

        public enum ConditionType 
        {
            Inconditional,
            Zero,
            NotZero,
            GreaterThanZero,
            LesserThanZero,
            Offset
        }
    }
}
