using System;

namespace MathTrainer.Core
{
    public static class MathHelper
    {
        public static void MathTest()
        {
            var result = string.Empty;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Max :");
            
            var res = ReadUntilExpected1(x => int.TryParse((string) x, out var parsed));

            //do
            //{

            //    Console.ForegroundColor = ConsoleColor.Yellow;

            //    var problem = GetSubtractProblem(10);

            //    Console.Write($"{problem.first} - {problem.second}: ");

            //    result = Console.ReadLine();

            //    if (int.TryParse(result, out var resultInt))
            //    {
            //        if (resultInt == problem.first - problem.second)
            //        {
            //            PrintCorrect(resultInt);
            //        }
            //        else
            //        {
            //            PrintIncorrect(resultInt);
            //        }
            //    }
            //    else
            //    {
            //        if (result.ToLower() != "q")
            //        {
            //            return;
            //        }

            //    }


            //} while (result.ToLower() != "q");
        }

        private static object ReadUntilExpected1(params Func<string, object>[] funcs)
        {
            do
            {
                var input = Console.ReadLine();

                foreach (var func in funcs)
                {
                    var result = func(input);
                    if (result != null)
                    {
                        return result;
                    }
                }

                Print("Unrecognized. Try again", ConsoleColor.Red);

            } while (false);

            return null;
        }



        static void PrintCorrect(int result)
        {
            Print($"{result} is correct", ConsoleColor.Green);
        }

        static void PrintIncorrect(int result)
        {
            Print($"{result} is incorrect", ConsoleColor.Red);
        }

        static void Print(string toPrint, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(toPrint);
            Console.WriteLine();
        }

        static (bool isInt, int parsed) IsInt(string input)
        {
            var isInt = int.TryParse(input, out var parsed);

            var result = (isInt: isInt, parsed: parsed);

            return result;
        }

        //static (string strResult, int intResult) ReadUntilExpected()
        //{
        //    var result = (strResult: string.Empty, intResult: -1);

        //    do
        //    {
        //        var input = Console.ReadLine();

        //        if (string.IsNullOrWhiteSpace(input?.Trim()))
        //        {
        //            Print("Unrecognized. Try again", ConsoleColor.Red);
        //            continue;
        //        }

        //        if (Commands.ContainsKey(input.ToUpper()))
        //        {
        //            result.strResult = input;
        //            return result;
        //        }

        //        if (int.TryParse(input, out var parsed))
        //        {
        //            result.intResult = parsed;
        //            return result;
        //        }

        //        Print("Unrecognized. Try again", ConsoleColor.Red);

        //    } while (false);

        //    return result;
        //}
    }
}
