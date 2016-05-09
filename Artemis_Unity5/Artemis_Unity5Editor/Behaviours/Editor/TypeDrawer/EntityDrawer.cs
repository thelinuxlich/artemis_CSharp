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
	
		static Bag<bool> Folded = new Bag<bool> ();

		static Rect buttonRect;

		public static void Initialize()
		{
			if (!Initialized) {
				Initialized = true;
				TypeDrawerManager.Initialize ();
			}
		}

		public static void DrawEntityList(Bag<Entity> EntityList, int Total)
		{
			if (Folded.Count < Total) {
				for (int i = 0; i < Total; i++) {
					Folded.Add (false);
				}
			}

			if (GUILayout.Button ("Delete Entities"))
			{
				for (int i = 0; i < EntityList.Count; i++) {
					EntityList [i].Delete ();
				}
			}
			EditorGUILayout.Space ();

			EntityDrawerStyle.BeginEntityList ();
			for (int i = 0; i < EntityList.Count; i++) {
				DrawEntity (EntityList[i]);
			}
			EntityDrawerStyle.EndEntityList ();

			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Add Component",GUILayout.Width(200)))
			{
				PopupWindow.Show(buttonRect, new EntityTypeSelectionPopup(EntityList));
			}

			if (Event.current.type == EventType.Repaint) buttonRect = GUILayoutUtility.GetLastRect();
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal ();
		}
			
		public static void DrawEntity(Entity Entity)
		{
	
			EntityDrawerStyle.BeginEntity ();
			EntityDrawerStyle.BeginEntityHeader ();
			Folded[Entity.Id] = EditorGUILayout.Foldout (Folded [Entity.Id], "Entity " + Entity.Id + " / " + Entity.UniqueId);
			EntityDrawerStyle.EndEntityHeader ();

			if (Folded [Entity.Id]) {


				EditorGUILayout.Space ();
				if (GUILayout.Button ("Delete Entity"))
				{
					Entity.Delete ();
				}


				EditorGUILayout.Space ();
				ComponentDrawer.DrawComponentList (Entity, Entity.Components);
				EditorGUILayout.Space ();


			
				GUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();

				if (GUILayout.Button("Add Component",GUILayout.Width(200)))
				{
					PopupWindow.Show(buttonRect, new EntityTypeSelectionPopup(Entity));
				}

				if (Event.current.type == EventType.Repaint) buttonRect = GUILayoutUtility.GetLastRect();

				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
				EditorGUILayout.Space ();
	
			}
			EntityDrawerStyle.EndEntity ();
		}
	}
}