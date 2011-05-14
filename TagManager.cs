using System;
using System.Collections.Generic;
namespace Artemis
{
	public class TagManager {
		private World world;
		private Dictionary<String, Entity> entityByTag;
	
		public TagManager(World world) {
			this.world = world;
			entityByTag = new Dictionary<String, Entity>();
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
			return entityByTag[tag];
		}
	
	}
}

