namespace Artemis.Manager
{
    #region Using statements

    using global::System.Collections.Generic;
    using global::System.Threading;

    #endregion Using statements

    /// <summary>Class QueueManager.</summary>
    internal class QueueManager
    {
        /// <summary>The lock object.</summary>
        private static readonly object LockObject = new object();

        /// <summary>The entities to process each frame.</summary>
        public int EntitiesToProcessEachFrame = 50;

        /// <summary>The queue.</summary>
        public Queue<Entity> Queue;

        /// <summary>The ref count.</summary>
        public int RefCount;

        /// <summary>Initializes a new instance of the <see cref="QueueManager"/> class.</summary>
        public QueueManager()
        {
            this.Queue = new Queue<Entity>();
            this.RefCount = 0;

            this.AcquireLock();
            this.RefCount++;
            this.ReleaseLock();
        }

        /// <summary>Acquires the lock.</summary>
        public void AcquireLock()
        {
            Monitor.Enter(LockObject);
        }

        /// <summary>Releases the lock.</summary>
        public void ReleaseLock()
        {
            Monitor.Exit(LockObject);
        }
    }
}