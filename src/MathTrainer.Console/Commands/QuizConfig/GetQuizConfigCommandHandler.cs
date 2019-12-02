using System;
using System.Linq;
using System.Threading.Tasks;
using MathTrainer.Console.Commands.PrintConsole;
using MathTrainer.Console.Commands.ReadConsole;
using MathTrainer.Console.Constants;
using MathTrainer.Core.Base;
using MathTrainer.Core.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Range = MathTrainer.Console.Commands.ReadConsole.Range;

namespace MathTrainer.Console.Commands.QuizConfig
{

    public class GetQuizConfigCommandHandler : ICommandHandler<GetQuizConfigCommand, Task<Core.Entities.QuizConfig>>
    {
        private readonly IAppLogger<GetQuizConfigCommandHandler> _logger;
        private readonly ICommandHandler<PrintConsoleCommand> _printConsoleCommandHandler;
        private readonly ICommandHandler<ReadConsoleCommand, Task<ConsoleResult>> _getConsoleResult;
        private readonly IConfiguration _configuration;

        public GetQuizConfigCommandHandler(
            IAppLogger<GetQuizConfigCommandHandler> logger,
            IConfiguration configuration,
            ICommandHandler<PrintConsoleCommand> printConsoleCommandHandler,
            ICommandHandler<ReadConsoleCommand, Task<ConsoleResult>> getConsoleResult, 
            IOptions<Core.Entities.QuizConfig> quizConfig)
        {
            _logger = logger;
            _configuration = configuration;
            _printConsoleCommandHandler = printConsoleCommandHandler;
            _getConsoleResult = getConsoleResult;
        }

        public async Task<Core.Entities.QuizConfig> Execute(GetQuizConfigCommand command)
        {
            var infoColor = command?.InfoColor ?? ConsoleConstants.DefaultInfoColor;
            var incorrectColor = command?.IncorrectColor ?? ConsoleConstants.DefaultIncorrectColor;

            await PrintDefault(_configuration, infoColor);

            var maxNumber = await GetConsoleInt("Max number to use (> 1): ", ConsoleConstants.MaxNumberDefault, 1);

            var numberOfProblems = await GetConsoleInt("Number of problems: ", ConsoleConstants.NumberOfProblemDefault, 1);

            var maxTime = await GetConsoleInt("Max time to solve [1 sec - 60 sec], [-1 => unlimited]: ", ConsoleConstants.NumberOfProblemDefault, -1, 60);




            async Task<int> GetConsoleInt(string prompt, int defaultNumber, int? min = null, int? max = null)
            {
                var promptAsync =
                    _printConsoleCommandHandler.Execute(new PrintConsoleCommand
                    {
                        Color = infoColor,
                        Text = prompt
                    });

                var consoleResult =
                    await _getConsoleResult.Execute(new ReadConsoleCommand
                    {
                        Prompt = promptAsync,
                        Range = new Range
                        {
                            Min = min,
                            Max = max
                        },
                        InfoColor = infoColor,
                        IncorrectColor = incorrectColor,
                        CancellationTokenSource = command.CancellationTokenSource
                    });

                var result = consoleResult.IntValue ?? defaultNumber;

                return
                    result;
            }



            var problemTypesPrompt =
                  ConsoleConstants.MapAbbreviationToProblemType
                                  .Aggregate(string.Empty, (result, next) =>
                                                               string.IsNullOrEmpty(result)
                                                                   ? $"{next.Key} - {next.Value}"
                                                                   : string.Concat(result, $"{Environment.NewLine}{next.Key} - {next.Value}")
                                            );
            var problemTypesPromptPrint =
                _printConsoleCommandHandler.Execute(new PrintConsoleCommand
                {
                    Color = infoColor,
                    Text = $"Type of problems: {Environment.NewLine}{problemTypesPrompt}{Environment.NewLine}"
                });

            var problemTypes = (await
                                    _getConsoleResult.Execute(new ReadConsoleCommand
                                    {
                                        Prompt = problemTypesPromptPrint,
                                        Values = ConsoleConstants
                                                                          .MapAbbreviationToProblemType.Select(x => x.Key)
                                                                          .ToList(),
                                        InfoColor = infoColor,
                                        IncorrectColor = incorrectColor,
                                        CancellationTokenSource =
                                                                      command.CancellationTokenSource
                                    })).StrValues;


            return
                await Task.FromResult(

                                      new Core.Entities.QuizConfig
                                      {
                                          MaxNumber = maxNumber,
                                          NumberOfProblems = numberOfProblems,
                                          ProblemTypes = problemTypes
                                                        .Select(x => ConsoleConstants.MapAbbreviationToProblemType
                                                                    [x.ToUpper()]).ToList(),
                                          MaxTime = maxTime
                                      });
        }

        private async Task PrintDefault(IConfiguration configuration, ConsoleColor infoColor)
        {
            var quizKeyConfig = "Quiz";

            var maxNumber = configuration[$"{quizKeyConfig}:MaxNumber"];
            var numberOfProblems = configuration[$"{quizKeyConfig}:NumberOfProblems"];
            var maxTime = configuration[$"{quizKeyConfig}:MaxTime"];
            var problemTypes = 
                configuration.GetSection($"{quizKeyConfig}:ProblemTypes").AsEnumerable();
            var problemTypesInfo = string.Join(" ", problemTypes.Select(x => x.Value));
            
            var retryAttempts = configuration[$"{quizKeyConfig}:RetryAttempts"];
            var isEquationsAllowed = configuration[$"{quizKeyConfig}:IsEquationsAllowed"];

            var defaultConfigInfo =
                string.Concat(
                              $"Max Number: {maxNumber}{Environment.NewLine}",
                              $"Number of problems: {numberOfProblems}{Environment.NewLine}",
                              $"Max Time: {maxTime}{Environment.NewLine}",
                              $"Problem Types: {problemTypesInfo}{Environment.NewLine}",
                              $"Retry Attempts: {retryAttempts}{Environment.NewLine}",
                              $"Is Equations Allowed: {isEquationsAllowed}{Environment.NewLine}"
                             );
            
            var quizConfig = new Core.Entities.QuizConfig();

            await _printConsoleCommandHandler.Execute(new PrintConsoleCommand
                                  {
                                      Text = defaultConfigInfo,
                                      Color = infoColor,
                                      IsNewLine = true
            });
        }
    }
}