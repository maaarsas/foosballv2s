using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace foosballv2s.Source.Services.FoosballWebService
{
    public static class TaskHelper
    {
        public static Task<T> RunWithRetry<T>(Func<Task<T>> func, int maxTries = 3)
        {
            return RunWithRetryInternal(func, maxTries - 1,
                new TaskCompletionSource<T>(), Enumerable.Empty<Exception>());
        }
        
        /// <summary>
        /// Send HTTP request that retries the action if something goes wrong
        /// </summary>
        /// <param name="func"></param>
        /// <param name="remainingTries"></param>
        /// <param name="tcs"></param>
        /// <param name="previousExceptions"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static Task<T> RunWithRetryInternal<T>(
            Func<Task<T>> func, int remainingTries,
            TaskCompletionSource<T> tcs, 
            IEnumerable<Exception> previousExceptions)
        {
            func().ContinueWith(previousTask =>
                {
                    if (previousTask.IsFaulted)
                    {
                        var exceptions = previousExceptions.Concat(
                            previousTask.Exception.Flatten().InnerExceptions);
                        if (remainingTries > 0)
                            RunWithRetryInternal(func, remainingTries - 1,
                                tcs, exceptions);
                        else
                            tcs.SetException(exceptions);
                    }
                    else
                    {
                        tcs.SetResult(previousTask.Result);
                    }
                }, 
                TaskContinuationOptions.ExecuteSynchronously);
            return tcs.Task;
        }
    }
}