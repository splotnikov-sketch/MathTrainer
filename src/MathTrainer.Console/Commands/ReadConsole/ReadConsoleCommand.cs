using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MathTrainer.Core.Base;

namespace MathTrainer.Console.Commands.ReadConsole
{
    public class Range
    {
        public int? Min { get; set; }
        public int? Max { get; set; }
    }

    public class ReadConsoleCommand: ICommand<Task<ConsoleResult>>
    {
        public Task? Prompt { get; set; }
        public IList<string>? Values { get; set; }
        public Range? Range { get; set; }
        public ConsoleColor? InfoColor { get; set; }
        public ConsoleColor? IncorrectColor { get; set; }
        public char[] Delimiters { get; set; }
        public CancellationTokenSource CancellationTokenSource { get; set; }
    }
}
