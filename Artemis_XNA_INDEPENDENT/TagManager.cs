using System;
using System.Collections.Generic;
namespace Artemis
{
	public sealed class TagManager {
		private EntityWorld world;
		private Dictionary<String, Entity> entityByTag = new Dictionary<String, Entity>();
	
		internal TagManager(EntityWorld world) {
			this.world = world;
		}
	
		internal void Register(String tag, Entity e) {
            System.Diagnostics.Debug.Assert(e != null);
            System.Diagnostics.Debug.Assert(!String.IsNullOrEmpty(tag));
			entityByTag.Add(tag, e);
		}

        internal void Unregister(String tag)
        {
            System.Diagnostics.Debug.Assert(!String.IsNullOrEmpty(tag));
			entityByTag.Remove(tag);
		}
	
		public bool IsRegistered(String tag) {
            System.Diagnostics.Debug.Assert(!String.IsNullOrEmpty(tag));
			return entityByTag.ContainsKey(tag);
		}
	
		public Entity GetEntity(String tag) {
            System.Diagnostics.Debug.Assert(!String.IsNullOrEmpty(tag));
            Entity e;
            entityByTag.TryGetValue(tag, out e);
            if (e != null && e.isActive)
            {
                return e;
            }
            else
            {
                Unregister(tag);
                return null;
            }
		}
	
		public String GetTagOfEntity(Entity e) {
            System.Diagnostics.Debug.Assert(e != null);
			String tag = "";
			foreach (var pair in entityByTag)
			{
				if(pair.Value.Equals(e)) {
					tag = pair.Key;
					break;
				}
			}
			return tag;
		}
	}
}

