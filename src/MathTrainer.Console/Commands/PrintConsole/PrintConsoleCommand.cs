

using System;
using MathTrainer.Core.Base;

namespace MathTrainer.Console.Commands.PrintConsole
{
    public class PrintConsoleCommand : ICommand
    {
        public ConsoleColor? Color { get; set; }
        public string? Text { get; set; }
        public bool IsNewLine { get; set; }
    }
}
