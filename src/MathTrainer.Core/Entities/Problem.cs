using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MathTrainer.Core.Entities
{
    public class ProblemAnswer
    {
        public int Answer { get; set; }
        public DateTime AnswerDate { get; set; }
        public TimeSpan Duration { get; set; }
        public bool IsCorrect { get; set; }
    }

    public class Problem
    {
        public ProblemType ProblemType { get; }
        public int First { get; }
        public int Second { get; }
        public int Result { get; set; }
        public int? NumInEquation { get; set; }

        //public int Answer {
        //    get
        //    {
        //        if (NumInEquation == null)
        //        {
        //            return Result;
        //        }

        //        if (NumInEquation == 1)
        //        {
        //            return 
        //        }
        //    }
        //}

        public ICollection<ProblemAnswer> Answers { get; set; }

        public Problem(ProblemType problemType, int first, int second, int result)
        {
            ProblemType = problemType;
            First = first;
            Second = second;
            Result = result;

            Answers = new Collection<ProblemAnswer>();
        }
    }
}