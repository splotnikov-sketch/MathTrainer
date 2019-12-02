using System.Collections.Generic;
using MathTrainer.Core.Base;
using MathTrainer.Core.Entities;

namespace MathTrainer.Core.Commands.GetProblems
{
    public class GetProblemsCommand: ICommand<IEnumerable<Problem>>
    {
        public int  MaxNumber { get; set; }
        public IList<ProblemType> ProblemTypes { get; set; }
        public int? NumberOfProblems { get; set; }
    }
}