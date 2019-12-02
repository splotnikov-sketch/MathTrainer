using System.Collections.Generic;
using System.Threading.Tasks;
using MathTrainer.Console.Commands.PrintConsole;
using MathTrainer.Console.Commands.QuizConfig;
using MathTrainer.Console.Commands.ReadConsole;
using MathTrainer.Console.Commands.RunQuiz;
using MathTrainer.Core.Base;
using MathTrainer.Core.Commands.GetProblems;
using MathTrainer.Core.Entities;
using MathTrainer.Core.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace MathTrainer.Console.Configurations.Ioc
{
    public static class IocHelper
    {
        public static IServiceCollection RegisterCommandHandlers(this IServiceCollection services)
        {
            services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
            services.AddSingleton<ICommandHandler<RunQuizCommand>, RunQuizCommandHandler>();
            services
               .AddSingleton<ICommandHandler<GetQuizConfigCommand, Task<QuizConfig>>,
                    GetQuizConfigCommandHandler>();
            services
               .AddSingleton<ICommandHandler<ReadConsoleCommand, Task<ConsoleResult>>, ReadConsoleCommandHandler>();
            services
               .AddSingleton<ICommandHandler<GetProblemsCommand, IEnumerable<Problem>>, GetProblemsCommandHandler>();
            services.AddSingleton<ICommandHandler<PrintConsoleCommand>, PrintConsoleCommandHandler>();

            return services;
        }
    }
}
