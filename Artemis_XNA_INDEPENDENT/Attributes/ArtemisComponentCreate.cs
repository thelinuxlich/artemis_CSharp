namespace Artemis.Attributes
{
    #region Using statements

    using global::System;

    #endregion Using statements

    /// <summary>Class ArtemisComponentCreate.</summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class ArtemisComponentCreate : Attribute
    {
    }
}