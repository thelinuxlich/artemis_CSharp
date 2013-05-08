#region File description

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueueManager.cs" company="GAMADU.COM">
//     Copyright © 2013 GAMADU.COM. All rights reserved.
//
//     Redistribution and use in source and binary forms, with or without modification, are
//     permitted provided that the following conditions are met:
//
//        1. Redistributions of source code must retain the above copyright notice, this list of
//           conditions and the following disclaimer.
//
//        2. Redistributions in binary form must reproduce the above copyright notice, this list
//           of conditions and the following disclaimer in the documentation and/or other materials
//           provided with the distribution.
//
//     THIS SOFTWARE IS PROVIDED BY GAMADU.COM 'AS IS' AND ANY EXPRESS OR IMPLIED
//     WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
//     FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL GAMADU.COM OR
//     CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
//     CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
//     SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
//     ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
//     NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
//     ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//
//     The views and conclusions contained in the software and documentation are those of the
//     authors and should not be interpreted as representing official policies, either expressed
//     or implied, of GAMADU.COM.
// </copyright>
// <summary>
//   Class QueueManager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion File description

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
        private readonly object lockObject = new object();

        /// <summary>Initializes a new instance of the <see cref="QueueManager"/> class.</summary>
        public QueueManager()
        {
            this.EntitiesToProcessEachFrame = 50;
            this.Queue = new Queue<Entity>();
            this.RefCount = 0;

            this.AcquireLock();
            ++this.RefCount;
            this.ReleaseLock();
        }

        /// <summary>Gets or sets the entities to process each frame.</summary>
        /// <value>The entities to process each frame.</value>
        public int EntitiesToProcessEachFrame { get; set; }

        /// <summary>Gets or sets the queue.</summary>
        /// <value>The queue.</value>
        public Queue<Entity> Queue { get; set; }

        /// <summary>Gets or sets the ref count.</summary>
        /// <value>The ref count.</value>
        public int RefCount { get; set; }

        /// <summary>Acquires the lock.</summary>
        public void AcquireLock()
        {
            Monitor.Enter(this.lockObject);
        }

        /// <summary>Releases the lock.</summary>
        public void ReleaseLock()
        {
            Monitor.Exit(this.lockObject);
        }
    }

    /// <summary>Class QueueManager that is independent of the entity concept.</summary>
    /// <typeparam name="T">The Type T.</typeparam>
    internal class QueueManager<T>
    {
        /// <summary>The lock object.</summary>
        private readonly object lockObject = new object();

        /// <summary>Initializes a new instance of the <see cref="QueueManager{T}"/> class.</summary>
        public QueueManager()
        {
            this.EntitiesToProcessEachFrame = 50;
            this.Queue = new Queue<T>();
            this.RefCount = 0;

            this.AcquireLock();
            ++this.RefCount;
            this.ReleaseLock();
        }

        /// <summary>Gets or sets the entities to process each frame.</summary>
        /// <value>The entities to process each frame.</value>
        public int EntitiesToProcessEachFrame { get; set; }

        /// <summary>Gets or sets the queue.</summary>
        /// <value>The queue.</value>
        public Queue<T> Queue { get; set; }

        /// <summary>Gets or sets the ref count.</summary>
        /// <value>The ref count.</value>
        public int RefCount { get; set; }

        /// <summary>Acquires the lock.</summary>
        public void AcquireLock()
        {
            Monitor.Enter(this.lockObject);
        }

        /// <summary>Releases the lock.</summary>
        public void ReleaseLock()
        {
            Monitor.Exit(this.lockObject);
        }
    }
}