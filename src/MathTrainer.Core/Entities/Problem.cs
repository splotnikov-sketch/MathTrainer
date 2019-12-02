namespace MathTrainer.Core.Entities
{
    public class Problem
    {
        public int First { get; }
        public int Second { get; }
        public ProblemType ProblemType { get; }
        public int Result { get; set; }

        public Problem(ProblemType problemType, int first, int second, int result)
        {
            ProblemType = problemType;
            First = first;
            Second = second;
            Result = result;
        }

        public Problem()
        {
        }
    }
}