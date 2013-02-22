namespace Artemis.System
{
    #region Using statements

    using global::System;

    #endregion Using statements

    /// <summary>Class IntervalEntitySystem.</summary>
    public abstract class IntervalEntitySystem : EntitySystem
    {
        /// <summary>The interval.</summary>
        private readonly float interval;

        /// <summary>The accumulated delta.</summary>
        private float accumulatedDelta;

        /// <summary>Initializes a new instance of the <see cref="IntervalEntitySystem"/> class.</summary>
        /// <param name="interval">The interval.</param>
        /// <param name="types">The types.</param>
        protected IntervalEntitySystem(float interval, params Type[] types)
            : base(types)
        {
            this.interval = interval;
        }

        /// <summary>Initializes a new instance of the <see cref="IntervalEntitySystem"/> class.</summary>
        /// <param name="interval">The interval.</param>
        /// <param name="aspect">The aspect.</param>
        protected IntervalEntitySystem(int interval, Aspect aspect)
            : base(aspect)
        {
            this.interval = interval;
        }

        /// <summary>Checks the processing.</summary>
        /// <returns><see langword="true" /> if this instance is enabled, <see langword="false" /> otherwise</returns>
        protected override bool CheckProcessing()
        {
            this.accumulatedDelta += this.EntityWorld.ElapsedTime;
            if (this.accumulatedDelta >= this.interval)
            {
                this.accumulatedDelta -= this.interval;
                return this.IsEnabled;
            }

            return false;
        }
    }
}