namespace Artemis.Attributes
{
    #region Using statements

    using global::System;

    using Artemis.Manager;

    #endregion Using statements

    /// <summary>Class ArtemisEntitySystem.</summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class ArtemisEntitySystem : Attribute
    {
        /// <summary>Initializes a new instance of the <see cref="ArtemisEntitySystem"/> class.</summary>
        public ArtemisEntitySystem()
        {
            this.ExecutionType = ExecutionType.UpdateSynchronous;
            this.Layer = 0;
        }

        /// <summary>Gets or sets the type of the execution.</summary>
        /// <value>The type of the execution.</value>
        public ExecutionType ExecutionType { get; set; }

        /// <summary>Gets or sets the layer.</summary>
        /// <value>The layer.</value>
        public int Layer { get; set; }
    }
}