using System;
using System.Collections.Generic;
using MathTrainer.Core.Entities;

namespace MathTrainer.Console.Constants
{
    public static class ConsoleConstants
    {
        public static string WrongInput = "Wrong input, please try again...";
        public static ConsoleColor DefaultInfoColor = ConsoleColor.Yellow;
        public static ConsoleColor DefaultIncorrectColor = ConsoleColor.Red;
        public static int MaxNumberDefault = 10;
        public static int NumberOfProblemDefault = 20;

        public static Dictionary<ProblemType, string> MapProblemTypeToPrint = 
            new Dictionary<ProblemType, string>
            {
                {ProblemType.Addition, "+" },
                {ProblemType.Subtraction, "-" },
                {ProblemType.Division, "-" },
                {ProblemType.Multiplication, "*" },
            };
        
        public static IDictionary<string, string> Commands =
            new Dictionary<string, string>
            {
                {"R", "Result"},
                {"S", "Skip"},
                {"Q", "Quit"}
            };

        public static Dictionary<string, ProblemType> MapAbbreviationToProblemType =
            new Dictionary<string, ProblemType>
            {
                { "A", ProblemType.Addition },
                { "S", ProblemType.Subtraction },
                { "M", ProblemType.Multiplication },
                { "D", ProblemType.Division }
            };

        public static char[] Delimiters = {',', ' ', '|'};
    }
}
