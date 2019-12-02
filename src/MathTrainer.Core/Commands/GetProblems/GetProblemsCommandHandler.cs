using System;
using System.Collections.Generic;
using MathTrainer.Core.Base;
using MathTrainer.Core.Entities;
using MathTrainer.Core.Infrastructure;

namespace MathTrainer.Core.Commands.GetProblems
{
    public class GetProblemsCommandHandler : ICommandHandler<GetProblemsCommand, IEnumerable<Problem>>
    {
        private readonly IAppLogger<GetProblemsCommandHandler> _logger;

        public GetProblemsCommandHandler(IAppLogger<GetProblemsCommandHandler> logger)
        {
            _logger = logger;
        }

        public IEnumerable<Problem> Execute(GetProblemsCommand command)
        {
            var rnd = new Random();


            for (var i = 0; i <= (command.NumberOfProblems ?? 1) - 1; i++)
            {
                var rndProblemType = rnd.Next(command.ProblemTypes.Count);
                var problemType = command.ProblemTypes[rndProblemType];

                switch (problemType)
                {
                    case ProblemType.Addition:
                        yield return GetAddProblem();
                        break;
                   
                    case ProblemType.Subtraction:
                        yield return GetSubtractProblem();
                        break;
                   
                    default:
                        yield return GetAddProblem();
                        break;
                }
            }
        
            Problem GetAddProblem()
            {
                var first = rnd.Next(1, command.MaxNumber);
                var second = rnd.Next(1, command.MaxNumber);

                return
                    new Problem(ProblemType.Addition, first, second, first + second);
            }

            Problem GetSubtractProblem()
            {
                int first, second;
                do
                {
                    first = rnd.Next(1, command.MaxNumber);
                    second = rnd.Next(1, command.MaxNumber);
                } while (first <= second);
                
                
                return 
                    new Problem(ProblemType.Subtraction, first, second, first-second);
            }
        }
    }
}