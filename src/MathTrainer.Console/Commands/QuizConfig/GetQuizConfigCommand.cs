using System;
using System.Threading;
using System.Threading.Tasks;
using MathTrainer.Core.Base;

namespace MathTrainer.Console.Commands.QuizConfig
{
    public class GetQuizConfigCommand : ICommand<Task<Core.Entities.QuizConfig>>
    {
        public ConsoleColor? InfoColor { get; set; }
        public ConsoleColor? IncorrectColor { get; set; }
        public CancellationTokenSource CancellationTokenSource { get; set; }
    }
}