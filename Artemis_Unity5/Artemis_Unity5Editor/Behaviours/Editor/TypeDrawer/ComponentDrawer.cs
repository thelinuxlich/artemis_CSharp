#region File description

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityWorld.cs" company="GAMADU.COM">
//     Copyright © 2013 GAMADU.COM. All rights reserved.
//     Redistribution and use in source and binary forms, with or without modification, are
//     permitted provided that the following conditions are met:
//        1. Redistributions of source code must retain the above copyright notice, this list of
//           conditions and the following disclaimer.
//        2. Redistributions in binary form must reproduce the above copyright notice, this list
//           of conditions and the following disclaimer in the documentation and/or other materials
//           provided with the distribution.
//     THIS SOFTWARE IS PROVIDED BY GAMADU.COM 'AS IS' AND ANY EXPRESS OR IMPLIED
//     WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
//     FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL GAMADU.COM OR
//     CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
//     CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
//     SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
//     ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
//     NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
//     ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//     The views and conclusions contained in the software and documentation are those of the
//     authors and should not be interpreted as representing official policies, either expressed
//     or implied, of GAMADU.COM.
// </copyright>
// <summary>
//   The Entity World Class. Main interface of the Entity System.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#endregion File description

namespace Artemis_Unity5Editor.Editor
{
	#region Using statements

	using UnityEditor;
	using UnityEngine;

	using global::System.Reflection;
	using global::System;

	using Artemis.Manager;
	using Artemis.Interface;
	using Artemis.Utils;
	using Artemis;

	#endregion

	/// <summary>
	/// Component Drawer.
	/// </summary>
	public static class ComponentDrawer
	{
		static Bag<bool> Folded = new Bag<bool>();

		public static void DrawComponentList(Entity Entity, Bag<IComponent> ComponentList)
		{
			ComponentDrawerStyle.BeginComponentList ();
			for (int i = 0; i < ComponentList.Count; i++) {
				DrawComponent (Entity, ComponentList[i]);
			}
			ComponentDrawerStyle.EndComponentList ();
		}

		public static void DrawComponent(Entity Entity, IComponent Component)
		{
			Type ComponentType = Component.GetType ();
			BindingFlags BindingFlags = BindingFlags.Public | BindingFlags.Instance;
			int ComponentId = ComponentTypeManager.GetTypeFor (ComponentType).Id;

			PropertyInfo[] PropertyInfo = ComponentType.GetProperties (BindingFlags);
			FieldInfo[] FieldInfo = ComponentType.GetFields (BindingFlags);

			ComponentDrawerStyle.BeginComponent ();
			ComponentDrawerStyle.BeginComponentHeader ();


			if (PropertyInfo.Length > 0 || FieldInfo.Length > 0) {
				Folded [ComponentId] = EditorGUILayout.Foldout (Folded [ComponentId], ComponentType.Name);
			} else {
				EditorGUILayout.LabelField (ComponentType.Name);
			}

			var bgColor = GUI.backgroundColor;

			GUI.backgroundColor = Entity.CanReset(ComponentType) ? bgColor:Color.red;

			if (GUILayout.Button ("+", GUILayout.Width (19), GUILayout.Height (14))) {
				Entity.ResetComponent (ComponentType);
			}

			GUI.backgroundColor = bgColor;

			if (GUILayout.Button ("-", GUILayout.Width (19), GUILayout.Height (14))) {
				Entity.RemoveComponent (ComponentTypeManager.GetTypeFor (ComponentType));
			}
				
			ComponentDrawerStyle.EndComponentHeader();

			if (Folded [ComponentId]) {
				PropertyDrawer.DrawPropertyList (Component, FieldInfo);
				PropertyDrawer.DrawPropertyList (Component, PropertyInfo);
			}

			ComponentDrawerStyle.EndComponent();
		}
	}
}