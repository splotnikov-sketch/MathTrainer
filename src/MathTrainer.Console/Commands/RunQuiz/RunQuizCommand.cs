using System;
using MathTrainer.Core.Base;

namespace MathTrainer.Console.Commands.RunQuiz
{
    public class RunQuizCommand: ICommand
    {
        public ConsoleColor? InfoColor { get; set; }
        public ConsoleColor? IncorrectColor { get; set; }
    }
}
