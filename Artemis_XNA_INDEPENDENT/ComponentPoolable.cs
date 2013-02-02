namespace Artemis
{
    #region Using statements

    using Artemis.Interface;

    #endregion Using statements

    /// <summary>Class ComponentPool-able.</summary>
    public abstract class ComponentPoolable : IComponent
    {
        /// <summary>Initializes a new instance of the <see cref="ComponentPoolable"/> class.</summary>
        protected ComponentPoolable()
        {
            this.PoolId = 0;
        }

        /// <summary>Gets or sets the pool id.</summary>
        /// <value>The pool id.</value>
        internal int PoolId { get; set; }

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