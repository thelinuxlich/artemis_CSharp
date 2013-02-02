namespace Artemis
{
    #region Using statements

    using global::System;
    using global::System.Diagnostics;
    using global::System.Linq;
#if !XBOX && !WINDOWS_PHONE
    using global::System.Numerics;
#endif
#if XBOX || WINDOWS_PHONE
    using BigInteger = System.Int32;
#endif
    using Artemis.Manager;

    #endregion Using statements

    /// <summary>Specify a Filter class to filter what Entities (with what Components) a EntitySystem will Process.</summary>
    public class Aspect
    {
        /// <summary>Initializes a new instance of the <see cref="Aspect"/> class.</summary>
        protected Aspect()
        {
            this.OneTypesMap = 0;
            this.ExcludeTypesMap = 0;
            this.ContainsTypesMap = 0;
        }

        /// <summary>Gets or sets the contains types map.</summary>
        /// <value>The contains types map.</value>
        protected BigInteger ContainsTypesMap { get; set; }

        /// <summary>Gets or sets the exclude types map.</summary>
        /// <value>The exclude types map.</value>
        protected BigInteger ExcludeTypesMap { get; set; }

        /// <summary>Gets or sets the one types map.</summary>
        /// <value>The one types map.</value>
        protected BigInteger OneTypesMap { get; set; }

        /// <summary>All the specified types.</summary>
        /// <param name="types">The types.</param>
        /// <returns>The aspect to specified types.</returns>
        public static Aspect All(params Type[] types)
        {
            return new Aspect().GetAll(types);
        }

        /// <summary>Excludes the specified types.</summary>
        /// <param name="types">The types.</param>
        /// <returns>The exclude aspect to specified types.</returns>
        public static Aspect Exclude(params Type[] types)
        {
            return new Aspect().GetExclude(types);
        }

        /// <summary>Ones the specified types.</summary>
        /// <param name="types">The types.</param>
        /// <returns>The aspect to specified types.</returns>
        public static Aspect One(params Type[] types)
        {
            return new Aspect().GetOne(types);
        }

        /// <summary>Called by the EntitySystem to determine if the system is interested in the passed Entity</summary>
        /// <param name="entity">The entity.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        public virtual bool Interests(Entity entity)
        {
            if (!(this.ContainsTypesMap > 0 || this.ExcludeTypesMap > 0 || this.OneTypesMap > 0))
            {
                return false;
            }

            ////Little help
            ////10010 & 10000 = 10000
            ////10010 | 10000 = 10010
            ////10010 | 01000 = 11010

            ////1001 & 0000 = 0000 OK
            ////1001 & 0100 = 0000 NOK           
            ////0011 & 1001 = 0001 Ok

            return ((this.OneTypesMap      & entity.TypeBits) != 0                     || this.OneTypesMap      == 0) &&
                   ((this.ContainsTypesMap & entity.TypeBits) == this.ContainsTypesMap || this.ContainsTypesMap == 0) &&
                   ((this.ExcludeTypesMap  & entity.TypeBits) != this.ExcludeTypesMap  || this.ExcludeTypesMap  == 0);
        }

        /// <summary>Gets all.</summary>
        /// <param name="types">The types.</param>
        /// <returns>The aspect to specified types.</returns>
        public Aspect GetAll(params Type[] types)
        {
            Debug.Assert(types != null, "Types must not be null.");

            foreach (ComponentType componentType in types.Select(ComponentTypeManager.GetTypeFor))
            {
                this.ContainsTypesMap |= componentType.Bit;
            }

            return this;
        }

        /// <summary>Gets the exclude.</summary>
        /// <param name="types">The types.</param>
        /// <returns>The aspect to specified types.</returns>
        public Aspect GetExclude(params Type[] types)
        {
            Debug.Assert(types != null, "Types must not be null.");

            foreach (ComponentType componentType in types.Select(ComponentTypeManager.GetTypeFor))
            {
                this.ExcludeTypesMap |= componentType.Bit;
            }

            return this;
        }

        /// <summary>Gets the one.</summary>
        /// <param name="types">The types.</param>
        /// <returns>The aspect to specified types.</returns>
        public Aspect GetOne(params Type[] types)
        {
            Debug.Assert(types != null, "Types must not be null.");

            foreach (ComponentType componentType in types.Select(ComponentTypeManager.GetTypeFor))
            {
                this.OneTypesMap |= componentType.Bit;
            }

            return this;
        }
    }
}