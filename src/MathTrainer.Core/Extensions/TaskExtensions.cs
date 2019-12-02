using System;
using System.Threading;
using System.Threading.Tasks;

namespace MathTrainer.Core.Extensions
{
    public static class TaskExtensions
    {
        public static async Task<TResult> WithTimeout<TResult>(this Task<TResult> task, TimeSpan timeout, CancellationTokenSource cancellationTokenSource)
        {
            var token = cancellationTokenSource.Token;
            token.ThrowIfCancellationRequested();

            var winner = await Task.WhenAny(task, Task.Delay(timeout, token));

            if (winner == task)
            {
                return await task;
            }

            cancellationTokenSource.Cancel();
            throw new TimeoutException();

        }
    }
}