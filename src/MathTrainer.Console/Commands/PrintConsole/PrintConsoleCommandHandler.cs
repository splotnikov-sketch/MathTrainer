using System.Threading.Tasks;
using MathTrainer.Console.Constants;
using MathTrainer.Core.Base;
using MathTrainer.Core.Infrastructure;

namespace MathTrainer.Console.Commands.PrintConsole
{
    public class PrintConsoleCommandHandler : ICommandHandler<PrintConsoleCommand>
    {
        private readonly IAppLogger<PrintConsoleCommandHandler> _logger;

        public PrintConsoleCommandHandler(IAppLogger<PrintConsoleCommandHandler> logger)
        {
            _logger = logger;
        }

        public async Task Execute(PrintConsoleCommand command)
        {
            var task = Task.Run(() =>
                                {
                                    System.Console.ForegroundColor =
                                        command.Color ?? ConsoleConstants.DefaultInfoColor;

                                    if (command.IsNewLine)
                                    {
                                        System.Console.WriteLine(command?.Text);
                                    }
                                    else
                                    {
                                        System.Console.Write(command?.Text);
                                    }
                                });


            await task;
        }
    }
}