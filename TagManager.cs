using System;
namespace Artemis
{
	public class TagManager {
		private World world;
		private Dictionary<String, Entity> entityByTag;
	
		public TagManager(World world) {
			this.world = world;
			entityByTag = new Dictionary<String, Entity>();
		}
	
		public void register(String tag, Entity e) {
			entityByTag.Add(tag, e);
		}
	
		public void unregister(String tag) {
			entityByTag.Remove(tag);
		}
	
		public boolean isRegistered(String tag) {
			return entityByTag.containsKey(tag);
		}
	
		public Entity getEntity(String tag) {
			return entityByTag[tag];
		}
	
	}
}

