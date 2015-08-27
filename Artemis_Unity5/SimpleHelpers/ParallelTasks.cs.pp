#region *   License     *
/*
    SimpleHelpers - ParallelTasks   

    Copyright © 2015 Khalid Salomão

    Permission is hereby granted, free of charge, to any person
    obtaining a copy of this software and associated documentation
    files (the “Software”), to deal in the Software without
    restriction, including without limitation the rights to use,
    copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the
    Software is furnished to do so, subject to the following
    conditions:

    The above copyright notice and this permission notice shall be
    included in all copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND,
    EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
    OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
    NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
    HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
    WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
    FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
    OTHER DEALINGS IN THE SOFTWARE. 

    License: http://www.opensource.org/licenses/mit-license.php
    Website: https://github.com/khalidsalomao/SimpleHelpers.Net
 */
#endregion

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace $rootnamespace$.SimpleHelpers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ParallelTasks<T> : IDisposable
    {
        private List<Task> m_threads;
        private BlockingCollection<T> m_tasks;
        private int _maxNumberOfThreads = 0;
        private Action<T> _action;

        /// <summary>
        /// Processes the specified items.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="numberOfThreads">The number of threads.</param>
        /// <param name="action">The action.</param>
        /// <remarks>The internal queue has a default bounded capacity of twice the number of threads.</remarks>
        public static void Process (IEnumerable<T> items, int numberOfThreads, Action<T> action)
        {
            Process (items, numberOfThreads, numberOfThreads, 0, action);
        }

        /// <summary>
        /// Processes the specified items.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="initialNumberOfThreads">The initial number of threads.</param>
        /// <param name="maxNumberOfThreads">The max number of threads.</param>
        /// <param name="action">The action.</param>
        /// <remarks>The internal queue has a default bounded capacity of twice the number of threads.</remarks>
        public static void Process (IEnumerable<T> items, int initialNumberOfThreads, int maxNumberOfThreads, Action<T> action)
        {
            Process (items, initialNumberOfThreads, maxNumberOfThreads, 0, action);
        }

        /// <summary>
        /// Processes the specified items.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="initialNumberOfThreads">The initial number of threads.</param>
        /// <param name="maxNumberOfThreads">The max number of threads.</param>
        /// <param name="queueBoundedCapacity">
        /// The queue bounded capacity.<para/>
        /// A negative value will make the queue without upper bound.<para/>
        /// The default value is twice the number of threads.<para/>
        /// </param>
        /// <param name="action">The action.</param>
        public static void Process (IEnumerable<T> items, int initialNumberOfThreads, int maxNumberOfThreads, int queueBoundedCapacity, Action<T> action)
        {
            using (var mgr = new ParallelTasks<T> (initialNumberOfThreads, maxNumberOfThreads, queueBoundedCapacity, action))
            {
                foreach (var i in items)
                    mgr.AddTask (i);
                mgr.CloseAndWait ();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParallelTasks" /> class.
        /// </summary>
        /// <param name="numberOfThreads">The number of threads.</param>
        /// <param name="action">The action.</param>
        /// <remarks>The internal queue has a default bounded capacity of twice the number of threads.</remarks>
        public ParallelTasks (int numberOfThreads, Action<T> action)
        {
            // create task queue
            Initialize (numberOfThreads, 0, action);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParallelTasks" /> class.
        /// </summary>
        /// <param name="initialNumberOfThreads">The initial number of threads.</param>
        /// <param name="maxNumberOfThreads">The max number of threads.</param>
        /// <param name="action">The action.</param>
        /// <remarks>The internal queue has a default bounded capacity of twice the number of threads.</remarks>
        public ParallelTasks (int initialNumberOfThreads, int maxNumberOfThreads, Action<T> action)
        {
            if (maxNumberOfThreads < initialNumberOfThreads)
                throw new ArgumentOutOfRangeException ("maxNumberOfThreads");
                _maxNumberOfThreads = maxNumberOfThreads;

            // create task queue
            Initialize (initialNumberOfThreads, 0, action);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParallelTasks" /> class.
        /// </summary>
        /// <param name="initialNumberOfThreads">The initial number of threads.</param>
        /// <param name="maxNumberOfThreads">The max number of threads.</param>
        /// <param name="queueBoundedCapacity">
        /// The queue bounded capacity.<para/>
        /// A negative value will make the queue without upper bound.<para/>
        /// The default value is twice the number of threads.<para/>
        /// </param>
        /// <param name="action">The action.</param>
        public ParallelTasks (int initialNumberOfThreads, int maxNumberOfThreads, int queueBoundedCapacity, Action<T> action)
        {
            if (maxNumberOfThreads < initialNumberOfThreads)
                throw new ArgumentOutOfRangeException ("maxNumberOfThreads");
			_maxNumberOfThreads = maxNumberOfThreads;

            // create task queue
            Initialize (initialNumberOfThreads, queueBoundedCapacity, action);
        }
 
        private void Initialize (int numberOfThreads, int queueBoundedCapacity, Action<T> action)
        {
            // sanity check            
            if (numberOfThreads < 0)
                throw new ArgumentOutOfRangeException ("Number of threads cannot negative.", "numberOfThreads");
            if (action == null)
                throw new ArgumentNullException ("action");

            // adjust blocking collection capacity
            if (queueBoundedCapacity == 0)
                queueBoundedCapacity = (_maxNumberOfThreads * 2) + 1;
            if (queueBoundedCapacity == 1)
                queueBoundedCapacity++;

            // register action
            _action = action;
			
            // create task queue
            m_tasks = (queueBoundedCapacity > 0) ? new BlockingCollection<T> (queueBoundedCapacity) : new BlockingCollection<T>();

            // create all threads
            m_threads = new List<Task> (numberOfThreads);
            CreateThreads (numberOfThreads, action);
        }
 
        private void CreateThreads (int numberOfThreads, Action<T> action)
        {
            for (var i = 0; i < numberOfThreads; i++)
            {
                Task thread = Task.Run (() =>
                {
                    foreach (var t in m_tasks.GetConsumingEnumerable ())
                    {
                        action (t);
                    }
                });

                lock (m_threads) 
                    m_threads.Add (thread);
            }
        }

        /// <summary>
        /// Adds the task.<para/>
        /// This method is thread safe.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <remarks>
        /// This method is thread safe.<para/>
        /// Any call to Add may block until space is available to store the provided item in the processing queue.
        /// </remarks>
        /// <exception cref="ObjectDisposedException">If this instance is Disposed, any subsequent call may raise this exception.</exception>
        /// <exception cref="InvalidOperationException">If this instance is Disposed, any subsequent call may raise this exception.</exception>
        public void AddTask (T task)
        {
            // check if we have room for threads
            if (m_threads.Count < _maxNumberOfThreads)
            {
                // if there is no thread, we must start one rigth now
                if (m_threads.Count == 0)
                    CreateThreads (1, _action);
                // signal for thread creation
                StartMaintenance ();
            }
            // finally add the task
            m_tasks.Add (task);
        }

        /// <summary>
        /// Gets the queue count.
        /// </summary>
        /// <value>The queue count.</value>
        public int QueueCount
        {
            get { return m_tasks != null ? m_tasks.Count : 0; }
        }

        /// <summary>
        /// Gets the thread count.
        /// </summary>
        /// <value>The thread count.</value>
        public int ThreadCount
        {
            get { return m_threads != null ? m_threads.Count : 0; }
        }

        /// <summary>
        /// Remove all waiting items from processing queue.
        /// </summary>
        public void Clear ()
        {
            if (m_tasks != null)
            {
                while (m_tasks.Count > 0)
                {
                    T item;
                    m_tasks.TryTake (out item);
                }
            }
        }

        /// <summary>
        /// Closes this instance by waiting all threads to complete processing all waiting items.
        /// </summary>
        public void CloseAndWait ()
        {
            Close (true);
        }

        /// <summary>
        /// Closes the specified wait for work to finish.
        /// </summary>
        /// <param name="waitForWorkToFinish">The wait for work to finish.</param>
        public void Close (bool waitForWorkToFinish)
        {
            if (m_tasks != null)
            {
                // signal that there is no more items
                m_tasks.CompleteAdding ();

                // check thread creation task
                ExecuteMaintenance (null);
                StopMaintenance ();

                // wait for work completion
                if (waitForWorkToFinish)
                {
                    foreach (var thread in m_threads)
                        thread.Wait ();
                }
                else
                {
                    foreach (var thread in m_threads)
                        thread.Wait (0);
                }
                
                // clean up
                m_tasks.Dispose ();
                m_tasks = null;
                m_threads.Clear ();
                m_threads = null;
            }
        }

        /// <summary>
        /// Closes this instance by waiting all threads to complete processing all waiting items.
        /// </summary>
        public void Dispose ()
        {
            Close (true);
            // no need to call dispose again by GC
            GC.SuppressFinalize (this);
        }

        ~ParallelTasks ()
        {
            Close (false);
        }

        #region *   Async Thread/Workers Creation  *

        private System.Threading.Timer m_maintenanceTask = null;
        private readonly object m_lock = new object ();
        private int m_executing = 0;
        private int m_idleCounter = 0;

        private void StartMaintenance ()
        {
            if (m_maintenanceTask == null)
            {
                lock (m_lock)
                {
                    if (m_maintenanceTask == null)
                    {
                        m_maintenanceTask = new System.Threading.Timer (ExecuteMaintenance, null, 0, 100);
                    }
                }
            }
        }

        private void StopMaintenance ()
        {
            lock (m_lock)
            {
                if (m_maintenanceTask != null)
                    m_maintenanceTask.Dispose ();
                m_maintenanceTask = null;
            }
        }

        private void ExecuteMaintenance (object state)
        {
            // check if a step is already executing
            if (System.Threading.Interlocked.CompareExchange (ref m_executing, 1, 0) != 0)
                return;
            // try to fire OnExpiration event
            try
            {
                // stop timed task we have a full thread pool
                if (m_threads.Count >= _maxNumberOfThreads)
                {
                    StopMaintenance ();
                }
                else if (m_tasks.Count == 0)
                {
                    // after 3 tries with empty queue, stop timer
                    if (m_idleCounter++ > 2)
                        StopMaintenance ();
                }
                else
                {                    
                    // create threads while there is tasks to be processed
                    do
                        CreateThreads (1, _action);
                    while (m_threads.Count < _maxNumberOfThreads && m_tasks.Count > 0);
                    // clear idle queue marker
                    m_idleCounter = 0;
                }   
            }
            finally
            {
                // release lock
                System.Threading.Interlocked.Exchange (ref m_executing, 0);
            }           
        }

        #endregion
    }
}
