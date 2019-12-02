using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MathTrainer.Console.Commands.PrintConsole;
using MathTrainer.Console.Commands.QuizConfig;
using MathTrainer.Console.Commands.ReadConsole;
using MathTrainer.Console.Constants;
using MathTrainer.Core.Base;
using MathTrainer.Core.Commands.GetProblems;
using MathTrainer.Core.Entities;
using MathTrainer.Core.Extensions;
using MathTrainer.Core.Infrastructure;
using Range = MathTrainer.Console.Commands.ReadConsole.Range;

namespace MathTrainer.Console.Commands.RunQuiz
{
    public class RunQuizCommandHandler : ICommandHandler<RunQuizCommand>
    {
        private readonly IAppLogger<RunQuizCommandHandler> _logger;
        private readonly ICommandHandler<PrintConsoleCommand> _printConsole;
        private readonly ICommandHandler<GetQuizConfigCommand, Task<Core.Entities.QuizConfig>> _getQuizConfig;
        private readonly ICommandHandler<GetProblemsCommand, IEnumerable<Problem>> _getProblems;
        private readonly ICommandHandler<ReadConsoleCommand, Task<ConsoleResult>> _getConsoleResult;

        public RunQuizCommandHandler(
            IAppLogger<RunQuizCommandHandler> logger,
            ICommandHandler<PrintConsoleCommand> printConsole,
                                     ICommandHandler<GetQuizConfigCommand, Task<Core.Entities.QuizConfig>> getQuizConfig,
                                     ICommandHandler<GetProblemsCommand, IEnumerable<Problem>> getProblems,
                                     ICommandHandler<ReadConsoleCommand, Task<ConsoleResult>> getConsoleResult)
        {
            _printConsole = printConsole;
            _getQuizConfig = getQuizConfig;
            _getProblems = getProblems;
            _getConsoleResult = getConsoleResult;
            _logger = logger;
        }

        public async Task Execute(RunQuizCommand command)
        {

            var infoColor = command.InfoColor ?? ConsoleConstants.DefaultInfoColor;
            var correctColor = ConsoleColor.Green;
            var incorrectColor = command.IncorrectColor ?? ConsoleConstants.DefaultIncorrectColor;

            var correct = 0;
            var inCorrect = 0;

            var (problems, span) = GetProblems(infoColor, incorrectColor);

            foreach (var problem in problems)
            {
                var problemPrompt = $"{problem.First} {ConsoleConstants.MapProblemTypeToPrint[problem.ProblemType]} {problem.Second} = ";

                var printProblem = _printConsole.Execute(new PrintConsoleCommand
                {
                    Text = problemPrompt,
                    Color = infoColor
                });

                using var cancellationTokenSource = new CancellationTokenSource();

                var resultAsync =
                    _getConsoleResult.Execute(new ReadConsoleCommand
                    {
                        Prompt = printProblem,
                        Range = new Range(),
                        CancellationTokenSource = cancellationTokenSource
                    });


                try
                {
                    var result = await resultAsync.WithTimeout(span, cancellationTokenSource);

                    if (result.IntValue.HasValue && result.IntValue.Value == problem.Result)
                    {
                        cancellationTokenSource.Cancel();
                        System.Console.ForegroundColor = correctColor;
                        System.Console.WriteLine("Correct");
                        correct++;
                    }
                    else
                    {
                        cancellationTokenSource.Cancel();
                        System.Console.ForegroundColor = incorrectColor;
                        System.Console.WriteLine("Incorrect");
                        inCorrect++;
                    }

                }
                catch (TimeoutException ex)
                {
                    cancellationTokenSource.Cancel();
                    System.Console.ForegroundColor = incorrectColor;
                    System.Console.WriteLine("Timeout");
                    inCorrect++;
                }
                finally
                {
                    await Task.Delay(TimeSpan.FromSeconds(3));
                    cancellationTokenSource.Dispose();
                    System.Console.Clear();
                }


            }

            System.Console.ForegroundColor = infoColor;
            System.Console.Write($"Correct results: {correct}, incorrect results: {inCorrect} {Environment.NewLine}");

        }


        private (IEnumerable<Problem> problems, TimeSpan span) GetProblems(ConsoleColor infoColor, ConsoleColor incorrectColor)
        {

            using var cancellationTokenSource = new CancellationTokenSource();

            var quizConfigAsync = _getQuizConfig.Execute(new GetQuizConfigCommand
            {
                InfoColor = infoColor,
                IncorrectColor = incorrectColor,
                CancellationTokenSource = cancellationTokenSource
            });

            var quizConfig = quizConfigAsync.GetAwaiter().GetResult();

            foreach (var problemType in quizConfig.ProblemTypes ?? Enumerable.Empty<ProblemType>())
            {
                System.Console.WriteLine(problemType.ToString());
            }

            var problems = _getProblems.Execute(new GetProblemsCommand
            {
                ProblemTypes = quizConfig.ProblemTypes,
                MaxNumber = quizConfig.MaxNumber,
                NumberOfProblems = quizConfig.NumberOfProblems,
            });

            System.Console.Clear();

            var timeout = quizConfig.MaxTime == null || quizConfig.MaxTime == -1 ? int.MaxValue : quizConfig.MaxTime.Value * 1000;
            var span = TimeSpan.FromMilliseconds(timeout);

            return (problems, span);
        }

    }
}