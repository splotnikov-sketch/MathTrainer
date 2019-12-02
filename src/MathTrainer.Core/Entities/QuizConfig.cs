using System.Collections.Generic;

namespace MathTrainer.Core.Entities
{
    public class QuizConfig
    {
        public int MaxNumber { get; set; }
        public IList<ProblemType> ProblemTypes { get; set; }
        public int NumberOfProblems { get; set; }
        public int? MaxTime { get; set; }
        public bool? IsEquationsAllowed { get; set; }
        public int RetryAttempts { get; set; }
    }
}
