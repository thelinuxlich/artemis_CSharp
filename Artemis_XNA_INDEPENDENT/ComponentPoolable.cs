namespace Artemis
{
    #region Using statements

    using Artemis.Interface;

    #endregion Using statements

    /// <summary>Class ComponentPoolable.</summary>
    public abstract class ComponentPoolable : IComponent
    {
        /// <summary>The pool id.</summary>
        internal int PoolId;

        /// <summary>Initializes a new instance of the <see cref="ComponentPoolable"/> class.</summary>
        protected ComponentPoolable()
        {
            this.PoolId = 0;
        }

        /// <summary>Cleans up.</summary>
        public virtual void CleanUp()
        {
        }

        /// <summary>Initializes this instance.</summary>
        public virtual void Initialize()
        {
        }
    }
}