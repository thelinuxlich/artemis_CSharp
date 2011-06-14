using System;
using System.Collections.Generic;

namespace Artemis
{
	public struct ImportData
	{
		public String tag;
		public String groupName;
		public List<Component> components;
		
		public ImportData(String tag,String groupName,List<Component> components) {
			this.tag = tag;
			this.groupName = groupName;
			this.components = components;
		}
		
		public ImportData() {}
	}
}

