namespace Artemis.Manager
{
    #region Using statements

    using global::System.Collections.Generic;
    using global::System.Threading;

    #endregion Using statements

    /// <summary>Class QueueManager.</summary>
    internal class QueueManager
    {
        /// <summary>The entities to process each frame.</summary>
        public static int EntitiesToProcessEachFrame = 50;

        /// <summary>The lock object.</summary>
        private static readonly object LockObject = new object();

        /// <summary>Initializes a new instance of the <see cref="QueueManager"/> class.</summary>
        public QueueManager()
        {
            this.Queue = new Queue<Entity>();
            this.RefCount = 0;

            this.AcquireLock();
            this.RefCount++;
            this.ReleaseLock();
        }

        /// <summary>Gets or sets the queue.</summary>
        /// <value>The queue.</value>
        public Queue<Entity> Queue { get; set; }

        /// <summary>Gets or sets the ref count.</summary>
        /// <value>The ref count.</value>
        public int RefCount { get; set; }

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