using System.Collections.Generic;
using System.IO;
using MathTrainer.Console.Commands.RunQuiz;
using MathTrainer.Console.Configurations.Ioc;
using MathTrainer.Core.Base;
using MathTrainer.Core.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MathTrainer.Console
{
    class Program
    {
        public static IConfiguration Configuration { get; set; }

        static void Main(string[] args)
        {
            Configuration = GetConfiguration();

            var services = new ServiceCollection();

            var serviceProvider =
                services.AddSingleton(Configuration)
                        .AddLogging()
                        .AddOptions()
                        .RegisterCommandHandlers()
                        .BuildServiceProvider();

            //var section = Configuration.GetSection("QuizConfig");

            //services.AddOptions<QuizConfig>();

            //services.Configure<QuizConfig>(
            //                               options =>
            //                               {
            //                                   options.MaxNumber = 10;
            //                                   options.MaxTime = -1;
            //                                   options.NumberOfProblems = 30;
            //                                   options.ProblemTypes = new List<ProblemType>
            //                                                          {
            //                                                              ProblemType.Addition, ProblemType.Subtraction
            //                                                          };
            //                               });

            
            var quiz = serviceProvider.GetService<ICommandHandler<RunQuizCommand>>();

            quiz.Execute(new RunQuizCommand()).GetAwaiter().GetResult();

            System.Console.WriteLine("Press any key to quit...");
            System.Console.ReadLine();
        }

        private static IConfiguration GetConfiguration()
        {
            var configuration =
                new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", true)
                   .Build();

            return configuration;
        }
    }
}

