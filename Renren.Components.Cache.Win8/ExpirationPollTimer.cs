// Thanks for Microsoft patterns & practices Enterprise Library Caching Application Block
// http://entlib.codeplex.com/

using System;
using System.Threading;
using Renren.Components.Tools;

namespace Renren.Components.Caching
{
	/// <summary>
	/// Represents an expiration poll timer.
	/// </summary>
    public sealed class ExpirationPollTimer : IDisposable
    {
        private BackgroundTimer pollTimer;
        private int expirationPollFrequencyInMilliSeconds;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expirationPollFrequencyInMilliSeconds"></param>
        public ExpirationPollTimer(int expirationPollFrequencyInMilliSeconds)
        {
            if (expirationPollFrequencyInMilliSeconds <= 0)
            {
                throw new ArgumentException("expirationPollFrequencyInMilliSeconds");
            }

            this.expirationPollFrequencyInMilliSeconds = expirationPollFrequencyInMilliSeconds;
        }

		/// <summary>
		/// Start the polling process.
		/// </summary>
		/// <param name="callbackMethod">The method to callback when a cycle has completed.</param>
        public void StartPolling(EventHandler<object> callbackMethod)
        {
            if (callbackMethod == null)
            {
                throw new ArgumentNullException("callbackMethod");
            }

            pollTimer = new BackgroundTimer();
            pollTimer.Interval = TimeSpan.FromMilliseconds(expirationPollFrequencyInMilliSeconds);
            pollTimer.Tick += callbackMethod;
            pollTimer.Start();
        }

		/// <summary>
		/// Stop the polling process.
		/// </summary>
        public void StopPolling()
        {
            if (pollTimer == null)
            {
                throw new InvalidOperationException("InvalidPollingStopOperation");
            }
            pollTimer.Stop();
            pollTimer = null;
        }

		void IDisposable.Dispose()
		{
            if (pollTimer != null)
            {
                pollTimer.Stop();
                pollTimer = null;
            }
		}		
	}
}
