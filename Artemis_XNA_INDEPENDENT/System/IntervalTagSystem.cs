namespace Artemis.System
{
    /// <summary>Class IntervalTagSystem.</summary>
    public abstract class IntervalTagSystem : TagSystem
    {
        /// <summary>The interval.</summary>
        private readonly float interval;

        /// <summary>The accumulated delta.</summary>
        private float accumulatedDelta;

        /// <summary>Initializes a new instance of the <see cref="IntervalTagSystem"/> class.</summary>
        /// <param name="interval">The interval.</param>
        /// <param name="tag">The tag.</param>
        protected IntervalTagSystem(float interval, string tag)
            : base(tag)
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