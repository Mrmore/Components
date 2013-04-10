using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Renren.Components.Network.Http
{
    /// <summary>
    /// One implementation of arbitration container of tokens
    /// It's in charge of deciding which token should be processed next
    /// </summary>
    public class TokenArbitrationContainer : ITokenArbitrationContainer<INetworkAsyncToken>
    {
        /// <summary>
        /// Work note definition
        /// </summary>
        class WorkNote
        {
            /// <summary>
            /// The container used to load the tokens
            /// </summary>
            public Queue<INetworkAsyncToken> Container { get; set; }

            /// <summary>
            /// The locker belongs to this work note
            /// </summary>
            public object NoteLocker { get; set; }
        }

        private SemaphoreSlim _syncupFactor = null;

        private IDictionary<NetworkPriority, WorkNote> _workNotesTable =
            new Dictionary<NetworkPriority, WorkNote>();

        /// <summary>
        /// The internal constructor
        /// </summary>
        /// <param name="factor">The root sync up locker</param>
        internal TokenArbitrationContainer(SemaphoreSlim factor)
        {
            this._workNotesTable.Add(NetworkPriority.High,
                new WorkNote() { Container = new Queue<INetworkAsyncToken>(), 
                    NoteLocker = new object() });

            this._workNotesTable.Add(NetworkPriority.Low,
                new WorkNote() { Container = new Queue<INetworkAsyncToken>(), 
                    NoteLocker = new object() });

            this._workNotesTable.Add(NetworkPriority.Normal, 
                new WorkNote() { Container = new Queue<INetworkAsyncToken>(), 
                    NoteLocker = new object() });

            this._syncupFactor = factor;
        }

        /// <summary>
        /// Add a new token to container
        /// </summary>
        /// <param name="token">The new token</param>
        public void AddToken(INetworkAsyncToken token)
        {
            lock (_workNotesTable[token.Priority.Priority].NoteLocker)
            {
                _workNotesTable[token.Priority.Priority].Container.Enqueue(token);
            }

            token.SetStatus(NetworkStatus.Waitting);
            _syncupFactor.Release();
        }

        /// <summary>
        /// Get the next token should be processed
        /// </summary>
        /// <returns>The token need processing</returns>
        public INetworkAsyncToken Next()
        {
            _syncupFactor.Wait();
            INetworkAsyncToken target = null;

            // Try high priority queue
            target = this.containerOperator(NetworkPriority.High,
                (m) =>
                {
                    m.OrderByDescending((t) => t.Priority.Grayness);
                    return m.Dequeue();
                });

            if (target != null)
            {
                return target;
            }

            // Try normal priority queue
            target = this.containerOperator(NetworkPriority.Normal,
                (m) =>
                {
                    m.OrderByDescending((t) => t.Priority.Grayness);
                    return m.Dequeue();
                });
            if (target != null)
            {
                return target;
            }

            // Try low priority queue
            target = this.containerOperator(NetworkPriority.Low,
                (m) =>
                {
                    m.OrderByDescending((t) => t.Priority.Grayness);
                    return m.Dequeue();
                });

            return target;
        }

        /// <summary>
        /// It's a container operation wrapper using specified function
        /// </summary>
        /// <typeparam name="T">The return value type</typeparam>
        /// <param name="priority">The priority</param>
        /// <param name="fun">The function refer to specified token</param>
        /// <returns></returns>
        private T containerOperator<T>(NetworkPriority priority, Func<Queue<INetworkAsyncToken>, T> fun)
        {
            T target = default(T);
            lock (this._workNotesTable[priority].NoteLocker)
            {
                if (this._workNotesTable[priority].Container.Count > 0)
                {
                    target = fun(this._workNotesTable[priority].Container);
                }
            }
            return target;
        }

        /// <summary>
        /// It's a container operation wrapper using specified action
        /// </summary>
        /// <param name="priority">The netwrok priority</param>
        /// <param name="action">The action refer to this token</param>
        private void containerOperator(NetworkPriority priority, Action<Queue<INetworkAsyncToken>> action)
        {
            lock (this._workNotesTable[priority].NoteLocker)
            {
                action(this._workNotesTable[priority].Container);
            }
        }

        /// <summary>
        /// Clear operation wrapper
        /// </summary>
        public void Clear()
        {
            this.containerOperator(NetworkPriority.High, (m) => m.Clear() );
        }

        /// <summary>
        /// Try to update the token's priority in this container
        /// </summary>
        /// <param name="token"></param>
        /// <param name="newPriority"></param>
        public void Update(INetworkAsyncToken token, TransitionPriority newPriority)
        {
            lock (this._workNotesTable[token.Priority.Priority].NoteLocker)
            {
                if (this._workNotesTable[token.Priority.Priority].Container.Contains(token))
                {
                    lock (this._workNotesTable[newPriority.Priority])
                    {
                        this._workNotesTable[token.Priority.Priority].Container.ToList().Remove(token);
                        token.Priority = newPriority;
                        this._workNotesTable[newPriority.Priority].Container.Enqueue(token);
                    }
                }
            }
        }

        /// <summary>
        /// Try to remove the specified token in this container
        /// </summary>
        /// <param name="token"></param>
        public void Remove(INetworkAsyncToken token)
        {
            lock (this._workNotesTable[token.Priority.Priority].NoteLocker)
            {
                if (this._workNotesTable[token.Priority.Priority].Container.Contains(token))
                {
                    this._workNotesTable[token.Priority.Priority].Container.ToList().Remove(token);
                }
            }
        }

        /// <summary>
        /// Returns a value indicating whether the specified token is in this container
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool Contains(INetworkAsyncToken token)
        {
            return this._workNotesTable[token.Priority.Priority].Container.Contains(token);
        }
    }
}
