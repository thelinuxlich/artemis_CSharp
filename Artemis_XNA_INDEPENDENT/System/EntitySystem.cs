namespace Artemis.System
{
    #region Using statements

    using global::System;
    using global::System.Collections.Generic;
    using global::System.Diagnostics;
#if !XBOX && !WINDOWS_PHONE
    using global::System.Numerics;
#endif
#if XBOX || WINDOWS_PHONE
    using BigInteger = System.Int32;
#endif
    using Artemis.Blackboard;

    #endregion Using statements

    /// <summary>
    /// <para>Base of all Entity Systems.</para>
    /// <para>Provide basic functionalities.</para>
    /// </summary>
    public abstract class EntitySystem
    {
        /// <summary>The actives.</summary>
        private readonly SortedDictionary<int, Entity> actives;

        /// <summary>Initializes static members of the <see cref="EntitySystem"/> class.</summary>
        static EntitySystem()
        {
            BlackBoard = new BlackBoard();
        }

        /// <summary>Initializes a new instance of the <see cref="EntitySystem"/> class.</summary>
        protected EntitySystem()
        {
            this.SystemBit = 0;
            this.actives = new SortedDictionary<int, Entity>();
            this.Aspect = null;
            this.IsEnabled = true;
        }

        /// <summary>Initializes a new instance of the <see cref="EntitySystem"/> class.</summary>
        /// <param name="types">The types.</param>
        protected EntitySystem(params Type[] types)
        {
            this.SystemBit = 0;
            this.actives = new SortedDictionary<int, Entity>();
            this.Aspect = Aspect.All(types);
            this.IsEnabled = true;
        }

        /// <summary>Initializes a new instance of the <see cref="EntitySystem"/> class.</summary>
        /// <param name="aspect">The aspect.</param>
        protected EntitySystem(Aspect aspect)
        {
            Debug.Assert(aspect != null, "Aspect must not be null.");

            this.SystemBit = 0;
            this.actives = new SortedDictionary<int, Entity>();
            this.Aspect = aspect;
            this.IsEnabled = true;
        }

        /// <summary>Gets or sets the black board.</summary>
        /// <value>The black board.</value>
        public static BlackBoard BlackBoard { get; protected set; }

        /// <summary>Gets or sets a value indicating whether this instance is enabled.</summary>
        /// <value><see langword="true" /> if this instance is enabled; otherwise, <see langword="false" />.</value>
        public bool IsEnabled { get; set; }

        /// <summary>Gets or sets the entity world.</summary>
        /// <value>The entity world.</value>
        public EntityWorld EntityWorld { get; protected internal set; }

        /// <summary>Gets or sets the system bit. Setter only.</summary>
        /// <value>The system bit.</value>
        internal BigInteger SystemBit { private get; set; }

        /// <summary>Gets or sets the aspect.</summary>
        /// <value>The aspect.</value>
        protected Aspect Aspect { get; set; }

        /// <summary>Gets the merged types.</summary>
        /// <param name="requiredType">Type of the required.</param>
        /// <param name="otherTypes">The other types.</param>
        /// <returns>An array of all related types.</returns>
        public static Type[] GetMergedTypes(Type requiredType, params Type[] otherTypes)
        {
            Debug.Assert(requiredType != null, "RequiredType must not be null.");

            Type[] types = new Type[1 + otherTypes.Length];
            types[0] = requiredType;
            for (int index = 0, j = otherTypes.Length; j > index; ++index)
            {
                types[index + 1] = otherTypes[index];
            }

            return types;
        }

        /// <summary>
        /// <para>Initializes this instance.</para>
        /// <para>Override to implement code that gets executed</para>
        /// <para>when systems are initialized.</para>
        /// </summary>
        public virtual void Initialize()
        {
        }

        /// <summary>Called when [added].</summary>
        /// <param name="entity">The entity.</param>
        public virtual void OnAdded(Entity entity)
        {
        }

        /// <summary>Called when [change].</summary>
        /// <param name="entity">The entity.</param>
        public virtual void OnChange(Entity entity)
        {
            Debug.Assert(entity != null, "Entity must not be null.");

            bool contains = (this.SystemBit & entity.SystemBits) == this.SystemBit;
            ////bool interest = (typeFlags & entity.TypeBits) == typeFlags;
            bool interest = this.Aspect.Interests(entity);

            if (interest && !contains)
            {
                this.Add(entity);
            }
            else if (!interest && contains)
            {
                this.Remove(entity);
            }
            else if (interest && entity.IsEnabled)
            {
                this.Enable(entity);
            }
            else if (interest && entity.IsEnabled == false)
            {
                this.Disable(entity);
            }
        }

        /// <summary>Called when [disabled].</summary>
        /// <param name="entity">The entity.</param>
        public virtual void OnDisabled(Entity entity)
        {
        }

        /// <summary>Called when [enabled].</summary>
        /// <param name="entity">The entity.</param>
        public virtual void OnEnabled(Entity entity)
        {
        }

        /// <summary>Called when [removed].</summary>
        /// <param name="entity">The entity.</param>
        public virtual void OnRemoved(Entity entity)
        {
        }

        /// <summary>Processes this instance.</summary>
        public virtual void Process()
        {
            if (this.CheckProcessing())
            {
                this.Begin();
                this.ProcessEntities(this.actives);
                this.End();
            }
        }

        /// <summary>Toggles this instance.</summary>
        public void Toggle()
        {
            this.IsEnabled = !this.IsEnabled;
        }

        /// <summary>Adds the specified entity.</summary>
        /// <param name="entity">The entity.</param>
        protected void Add(Entity entity)
        {
            Debug.Assert(entity != null, "Entity must not be null.");
            entity.AddSystemBit(this.SystemBit);
            if (entity.IsEnabled)
            {
                this.Enable(entity);
            }

            this.OnAdded(entity);
        }

        /// <summary>Begins this instance processing.</summary>
        protected virtual void Begin()
        {
        }

        /// <summary>Checks the processing.</summary>
        /// <returns><see langword="true" /> if this instance is enabled, <see langword="false" /> otherwise</returns>
        protected virtual bool CheckProcessing()
        {
            return this.IsEnabled;
        }

        /// <summary>Ends this instance processing.</summary>
        protected virtual void End()
        {
        }

        /// <summary>Interests in the specified entity.</summary>
        /// <param name="entity">The entity.</param>
        /// <returns><see langword="true" /> if any interests in entity, <see langword="false" /> otherwise</returns>
        protected bool Interests(Entity entity)
        {
            return this.Aspect.Interests(entity);
        }

        /// <summary>Processes the entities.</summary>
        /// <param name="entities">The entities.</param>
        protected virtual void ProcessEntities(SortedDictionary<int, Entity> entities)
        {
        }

        /// <summary>Removes the specified entity.</summary>
        /// <param name="entity">The entity.</param>
        protected void Remove(Entity entity)
        {
            Debug.Assert(entity != null, "Entity must not be null.");
            entity.RemoveSystemBit(this.SystemBit);
            if (entity.IsEnabled)
            {
                this.Disable(entity);
            }

            this.OnRemoved(entity);
        }

        /// <summary>Disables the specified entity.</summary>
        /// <param name="entity">The entity.</param>
        private void Disable(Entity entity)
        {
            Debug.Assert(entity != null, "Entity must not be null.");

            if (!this.actives.ContainsKey(entity.Id))
            {
                return;
            }

            this.actives.Remove(entity.Id);
            this.OnDisabled(entity);
        }

        /// <summary>Enables the specified entity.</summary>
        /// <param name="entity">The entity.</param>
        private void Enable(Entity entity)
        {
            Debug.Assert(entity != null, "Entity must not be null.");

            if (this.actives.ContainsKey(entity.Id))
            {
                return;
            }

            this.actives.Add(entity.Id, entity);
            this.OnEnabled(entity);
        }
    }
}