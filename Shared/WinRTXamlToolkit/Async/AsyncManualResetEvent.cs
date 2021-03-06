﻿using System.Threading;
using System.Threading.Tasks;

// Thanks for Xmal toolkit
// http://winrtxamltoolkit.codeplex.com/

namespace Renren.Components.Async
{
    /// <summary>
    /// Notifies one or more waiting awaiters that an event has occurred
    /// </summary>
    public class AsyncManualResetEvent
    {
        private volatile TaskCompletionSource<bool> _tcs = new TaskCompletionSource<bool>();

        /// <summary>
        /// Waits the async.
        /// </summary>
        /// <returns></returns>
        public Task WaitAsync()
        {
            return _tcs.Task;
        }

        //public void Set() { m_tcs.TrySetResult(true); }
        /// <summary>
        /// Sets the state of the event to signaled, allowing one or more waiting awaiters to proceed.
        /// </summary>
        public void Set()
        {
            var tcs = _tcs;
            Task.Factory.StartNew(s => ((TaskCompletionSource<bool>)s).TrySetResult(true),
                tcs, CancellationToken.None, TaskCreationOptions.PreferFairness, TaskScheduler.Default);
            tcs.Task.Wait();
        }

        /// <summary>
        /// Sets the state of the event to nonsignaled, causing awaiters to block.
        /// </summary>
        public void Reset()
        {
            while (true)
            {
                var tcs = _tcs;
                if (!tcs.Task.IsCompleted ||
                    Interlocked.CompareExchange(ref _tcs, new TaskCompletionSource<bool>(), tcs) == tcs)
                    return;
            }
        } 
    }
}
