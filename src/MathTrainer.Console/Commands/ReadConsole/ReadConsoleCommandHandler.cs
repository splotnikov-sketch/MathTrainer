using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MathTrainer.Console.Constants;
using MathTrainer.Core.Base;
using MathTrainer.Core.Infrastructure;

namespace MathTrainer.Console.Commands.ReadConsole
{
    public class ReadConsoleCommandHandler : ICommandHandler<ReadConsoleCommand, Task<ConsoleResult>>
    {
        private readonly IAppLogger<ReadConsoleCommandHandler> _logger;

        public ReadConsoleCommandHandler(IAppLogger<ReadConsoleCommandHandler> logger)
        {
            _logger = logger;
        }

        public async Task<ConsoleResult> Execute(ReadConsoleCommand command)
        {
            var incorrectColor = command.IncorrectColor ?? ConsoleConstants.DefaultIncorrectColor;

            var values = command.Values != null
                             ? new HashSet<string>(command.Values.Select(x => x.ToLower()))
                             : new HashSet<string>();
            do
            {
                if (command.Prompt != null)
                {
                    await command.Prompt;
                }

                var readInputTaskAsync =
                    Task.Run(function:  () =>
                             {
                                 var result = System.Console.ReadLine();

                                 return result;
                             }, command.CancellationTokenSource.Token);



                var inputOrigin = await readInputTaskAsync;

                    //System.Console.ReadLine();
                var input = inputOrigin?.Trim().ToLower();

                var strValues = GetStrValues(input ?? string.Empty, values);

                if (strValues != null)
                {
                    var result = new ConsoleResult
                    {
                        StrValues = strValues
                    };

                    
                    return await Task.FromResult(result);
                }


                if (command.Range != null)
                {
                    var parsed = int.TryParse(input, out var res);
                  
                    if (!parsed && command.Range.Min == null && command.Range.Max == null)
                    {
                        var result = new ConsoleResult();
                        
                        return await Task.FromResult(result);
                    }

                    if (parsed)
                    {
                        var result = new ConsoleResult
                                     {
                                         IntValue = res
                                     };

                        
                        return await Task.FromResult(result);
                    }
                }

                PrintWrongInput();

            } while (true);


            void PrintWrongInput()
            {
                System.Console.ForegroundColor = incorrectColor;
                System.Console.WriteLine(ConsoleConstants.WrongInput);
            }
        }

        private static HashSet<string>? GetStrValues(string input, HashSet<string> availableValues)
        {
            if (availableValues == null || !availableValues.Any())
            {
                return null;
            }

            var inputs = new HashSet<string>(input.Split(ConsoleConstants.Delimiters).Select(x => x.ToLower()));

            return inputs.IsSubsetOf(availableValues) ? inputs : null;
        }
    }
}