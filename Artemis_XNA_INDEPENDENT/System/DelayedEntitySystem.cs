namespace Artemis.System
{
    #region Using statements

    using global::System;
    using global::System.Collections.Generic;

    #endregion Using statements

    /// <summary>Class DelayedEntitySystem.</summary>
    public abstract class DelayedEntitySystem : EntitySystem
    {
        /// <summary>The accumulated delta.</summary>
        private float accumulatedDelta;

        /// <summary>The is running.</summary>
        private bool isRunning;

        /// <summary>Initializes a new instance of the <see cref="EntitySystem" /> class.</summary>
        /// <param name="types">The types.</param>
        protected DelayedEntitySystem(params Type[] types)
            : base(types)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="EntitySystem" /> class.</summary>
        /// <param name="aspect">The aspect.</param>
        protected DelayedEntitySystem(Aspect aspect)
            : base(aspect)
        {
        }

        /// <summary>Gets the initial time delay.</summary>
        /// <value>The initial time delay.</value>
        public float InitialTimeDelay { get; private set; }

        /// <summary>Gets the remaining time until processing.</summary>
        /// <returns>System.Single.</returns>
        public float GetRemainingTimeUntilProcessing()
        {
            if (this.isRunning)
            {
                return this.InitialTimeDelay - this.accumulatedDelta;
            }

            return 0.0f;
        }

        /// <summary>Determines whether this instance is running.</summary>
        /// <returns><see langword="true" /> if this instance is running; otherwise, <see langword="false" />.</returns>
        public bool IsRunning()
        {
            return this.isRunning;
        }

        /// <summary>Processes the entities.</summary>
        /// <param name="entities">The entities.</param>
        /// <param name="accumulatedDelta">The accumulated delta.</param>
        public abstract void ProcessEntities(SortedDictionary<int, Entity> entities, float accumulatedDelta);

        /// <summary>Starts the delayed run.</summary>
        /// <param name="milliseconds">The delay in milliseconds.</param>
        public void StartDelayedRun(float milliseconds)
        {
            this.InitialTimeDelay = milliseconds;
            this.accumulatedDelta = 0;
            this.isRunning = true;
        }

        /// <summary>
        /// <para>Stops this instance.</para>
        /// <para>Aborts running the system in the future and stops it.</para>
        /// <para>Call delayedRun() to start it again.</para>
        /// </summary>
        public void Stop()
        {
            this.isRunning = false;
            this.accumulatedDelta = 0;
        }

        /// <summary>Checks the processing.</summary>
        /// <returns><see langword="true" /> if this instance is enabled, <see langword="false" /> otherwise</returns>
        protected override bool CheckProcessing()
        {
            if (this.isRunning)
            {
                this.accumulatedDelta += this.EntityWorld.ElapsedTime;

                if (this.accumulatedDelta >= this.InitialTimeDelay)
                {
                    return this.IsEnabled;
                }
            }
            return false;
        }

        /// <summary>Processes the entities.</summary>
        /// <param name="entities">The entities.</param>
        protected override void ProcessEntities(SortedDictionary<int, Entity> entities)
        {
            this.ProcessEntities(entities, this.accumulatedDelta);
            this.Stop();
        }
    }
}