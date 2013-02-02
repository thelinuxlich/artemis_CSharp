namespace Artemis.Attributes
{
    #region Using statements

    using global::System;

    #endregion Using statements

    /// <summary>Class ArtemisEntityTemplate.</summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class ArtemisEntityTemplate : Attribute
    {
        /// <summary>Initializes a new instance of the <see cref="ArtemisEntityTemplate"/> class.</summary>
        /// <param name="name">The name.</param>
        public ArtemisEntityTemplate(string name)
        {
            this.Name = name;
        }

        /// <summary>Gets the name.</summary>
        /// <value>The name.</value>
        public string Name { get; private set; }
    }
}