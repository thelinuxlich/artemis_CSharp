using System;
using System.Collections.Generic;
namespace Artemis
{
	public sealed class TagManager {
		private EntityWorld world;
		private Dictionary<String, Entity> entityByTag = new Dictionary<String, Entity>();
	
		public TagManager(EntityWorld world) {
			this.world = world;
		}
	
		public void Register(String tag, Entity e) {
			entityByTag.Add(tag, e);
		}
	
		public void Unregister(String tag) {
			entityByTag.Remove(tag);
		}
	
		public bool IsRegistered(String tag) {
			return entityByTag.ContainsKey(tag);
		}
	
		public Entity GetEntity(String tag) {
            Entity e;
            entityByTag.TryGetValue(tag, out e);
            if (e == null || e.IsActive())
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

