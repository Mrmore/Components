using System;
using System.Threading;
using System.Threading.Tasks;

namespace Renren.Components.Caching
{
    /// <summary>
    /// Purpose of this class is to encapsulate the behavior of how ICacheItemRefreshActions
    /// are invoked in the background.
    /// </summary>
    public static class RefreshActionInvoker
    {
        /// <summary>
        /// Invokes the refresh action on a thread pool thread
        /// </summary>
        /// <param name="removedCacheItem">Cache item being removed. Must never be null.</param>
		/// <param name="removalReason">The reason the item was removed.</param>	
		/// <param name="instrumentationProvider">The instrumentation provider.</param>
		public static void InvokeRefreshAction(CacheItem removedCacheItem, CacheItemRemovedReason removalReason)
        {
            if (removedCacheItem == null) throw new ArgumentNullException("removedCacheItem");

            if (removedCacheItem.RefreshAction == null)
            {
                return;
            }

			try
			{
                RefreshActionData refreshActionData =
                    new RefreshActionData(removedCacheItem.RefreshAction, removedCacheItem.Key,removalReason);
                refreshActionData.InvokeOnThreadPoolThread();
			}
			catch (Exception e)
			{
				
			}            
        }

        private class RefreshActionData
        {
            private ICacheItemRefreshAction refreshAction;
            private string keyToRefresh;
            private CacheItemRemovedReason removalReason;

			public RefreshActionData(ICacheItemRefreshAction refreshAction, string keyToRefresh, CacheItemRemovedReason removalReason)
            {
                this.refreshAction = refreshAction;
                this.keyToRefresh = keyToRefresh;
                this.removalReason = removalReason;
            }

            public ICacheItemRefreshAction RefreshAction
            {
                get { return refreshAction; }
            }

            public string KeyToRefresh
            {
                get { return keyToRefresh; }
            }

            public CacheItemRemovedReason RemovalReason
            {
                get { return removalReason; }
            }

            public void InvokeOnThreadPoolThread()
            {
                //ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadPoolRefreshActionInvoker));
                Task.Run(() =>
                    {
                        try
                        {
                            RefreshAction.Refresh(KeyToRefresh, RemovalReason);
                        }
                        catch (Exception e)
                        {
                        }
                    });
            }

            //private void ThreadPoolRefreshActionInvoker()
            //{
            //    try
            //    {
            //        RefreshAction.Refresh(KeyToRefresh, RemovedData, RemovalReason);
            //    }
            //    catch (Exception e)
            //    {
            //    }
            //}
        }
    }
}
