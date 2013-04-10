// Thanks for Microsoft patterns & practices Enterprise Library Caching Application Block
// http://entlib.codeplex.com/

using Renren.Components.Async;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Renren.Components.Caching
{
    /// <summary>
    /// Represents a cache scavenger that runs on a background thread.
    /// </summary>
    /// <remarks>
    /// The <see cref="BackgroundScheduler"/> will make its best effort to avoid scheduling a new scavenge request 
    /// when it is safe to assume that it's not necessary. Since <see cref="ScavengerTask.NumberOfItemsToBeScavenged"/> 
    /// elements are scavenged each time, there should be at least one scavenge request every 
    /// <see cref="ScavengerTask.NumberOfItemsToBeScavenged"/> elements the cache over the 
    /// <see cref="CacheCapacityScavengingPolicy.MaximumItemsAllowedBeforeScavenging"/> threshold.
    /// <para/>
    /// Each time a scheduled scavenge task is processed the counter used to avoid superfluous scavenges is reset to 
    /// zero, so the next scavenge request will result in a new scheduled scavenge task.
    /// </remarks>
    public class BackgroundScheduler : ICacheScavenger
    {
        private ExpirationTask expirationTask;
        private ScavengerTask scavengerTask;

        private readonly AsyncLock scavengeExpireLock = new AsyncLock();
        private int scavengePending = 0;

        /// <summary>
        /// Initialize a new instance of the <see cref="BackgroundScheduler"/> with a <see cref="ExpirationTask"/> and 
        /// a <see cref="ScavengerTask"/>.
        /// </summary>
        /// <param name="expirationTask">The expiration task to use.</param>
        /// <param name="scavengerTask">The scavenger task to use.</param>
        /// <param name="instrumentationProvider">The instrumentation provider to use.</param>
        public BackgroundScheduler(ExpirationTask expirationTask, ScavengerTask scavengerTask)
        {
            this.expirationTask = expirationTask;
            this.scavengerTask = scavengerTask;
        }

        /// <summary>
        /// Queues a message that the expiration timeout has expired.
        /// </summary>
        /// <param name="notUsed">Ignored.</param>
        public void ExpirationTimeoutExpired(object sender,object notUsed)
        {
            Task.Run(async () =>
            {
                try
                {
                    using (var releaser = await scavengeExpireLock.LockAsync())
                    {
                        await expirationTask.DoExpirations();
                    }
                }
                catch (Exception e)
                {

                }
            });
        }

        /// <summary>
        /// Starts the scavenging process.
        /// </summary>
        public void StartScavenging()
        {
            int pendingScavengings = Interlocked.Increment(ref scavengePending);
            if (pendingScavengings == 1)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        using (var releaser = await scavengeExpireLock.LockAsync())
                        {
                            await Scavenge();
                        }
                    }
                    catch (Exception e)
                    {

                    }
                });
            }
        }

        internal async Task StartScavengingIfNeeded()
        {
            bool bIsScavengingNeeded = await scavengerTask.IsScavengingNeeded();
            if (bIsScavengingNeeded)
            {
                StartScavenging();
            }
        }

        internal async Task Scavenge()
        {
            int pendingScavengings = Interlocked.Exchange(ref scavengePending, 0);
            int timesToScavenge = ((pendingScavengings - 1) / scavengerTask.NumberOfItemsToBeScavenged) + 1;
            while (timesToScavenge > 0)
            {
                await scavengerTask.DoScavenging();
                --timesToScavenge;
            }
        }

        internal async Task Expire()
        {
            await expirationTask.DoExpirations();
        }

    }
}
