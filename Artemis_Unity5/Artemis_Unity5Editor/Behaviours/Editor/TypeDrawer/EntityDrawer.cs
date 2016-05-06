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
	/// Entity Drawer.
	/// </summary>
	public static class EntityDrawer
	{
		static bool Initialized = false;

		static Bag<bool> components = new Bag<bool>();

		public static void Initialize()
		{
			if (!Initialized) {
				Initialized = true;
				TypeDrawer.Initialize ();
			}
		}

		public static void DrawEntity(Entity Entity)
		{
			foreach (IComponent Component in Entity.Components) {
				DrawComponent (Entity, Component);
			}
		}

		public static void DrawComponent(Entity Entity, IComponent Component)
		{
			Type ComponentType = Component.GetType ();
			BindingFlags BindingFlags = BindingFlags.Public | BindingFlags.Instance;
		

			EditorGUILayout.BeginVertical(GUI.skin.box);
			{
				int ComponentId = ComponentTypeManager.GetTypeFor (ComponentType).Id;

				EditorGUILayout.BeginHorizontal();
				{
					//EditorGUILayout.LabelField(ComponentType.Name, EditorStyles.boldLabel);

					EditorGUI.indentLevel = 1;
					components.Set (ComponentId, EditorGUILayout.Foldout (components[ComponentId], ComponentType.Name));
					EditorGUI.indentLevel = 0;

					if (GUILayout.Button ("-", GUILayout.Width (19), GUILayout.Height (14))) {
						Entity.RemoveComponent (ComponentTypeManager.GetTypeFor (ComponentType));
					}
				}
				EditorGUILayout.EndHorizontal();

				if (components [ComponentId]) {
					DrawProperties (Component);
				}
		
			}
			EditorGUILayout.EndVertical();
		}

		public static void DrawProperties(IComponent Component)
		{
			Type ComponentType = Component.GetType ();
			foreach (PropertyInfo PropertyInfo in ComponentType.GetProperties()) {
				EditorGUILayout.BeginVertical ();
				DrawProperty (Component, PropertyInfo);
				EditorGUILayout.EndVertical ();
			}
		}

		public static void DrawProperty(IComponent Component, PropertyInfo PropertyInfo)
		{
			if (TypeDrawer.Supports (PropertyInfo.PropertyType)) {
				EditorGUILayout.BeginHorizontal ();
				// Get value on the target instance.
				object value = PropertyInfo.GetValue (Component, null);

				EditorGUILayout.LabelField (PropertyInfo.Name, GUILayout.MaxWidth (50));

				object nvalue = TypeDrawer.Draw (PropertyInfo.PropertyType, value);
				PropertyInfo.SetValue (Component, Convert.ChangeType (nvalue, PropertyInfo.PropertyType), null);
				EditorGUILayout.EndHorizontal ();
			}
		}
	}
}