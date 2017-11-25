using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Java.Net;

namespace foosballv2s.Source.Services.FoosballWebService
{
    public class TaskHelper<T>
    {
        private T returnObject;

        public void SetFaultTaskReturnObject(T returnObject)
        {
            this.returnObject = returnObject;
        }
        
        public Task<T> RunWithRetry(Func<Task<T>> func, int maxTries = 5)
        {
            try
            {
                return RunWithRetryInternal(func, maxTries - 1,
                    new TaskCompletionSource<T>(), Enumerable.Empty<Exception>());
            }
            catch (Exception e)
            {
                return Task.FromResult<T>(returnObject);
            }
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
        private Task<T> RunWithRetryInternal(
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