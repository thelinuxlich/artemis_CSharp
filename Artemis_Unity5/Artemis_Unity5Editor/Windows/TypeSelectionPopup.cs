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

	using global::System.Collections.Generic;
	using global::System.Reflection;
	using global::System;

	using Artemis.Interface;
	using Artemis;

	#endregion

	/// <summary>
	/// Type Selection Popup.
	/// </summary>
	public abstract class TypeSelectionPopup : PopupWindowContent
	{
		List<Type> Type = new List<Type>(); 
	
		Type TypeFilter;

		bool[] TypeState;

		Vector2 Scroll;

		string Search="";

		public TypeSelectionPopup(Type Filter)
		{
			this.TypeFilter = Filter;
		}

		public abstract void TypeConfirmed(Type Type);

		public override Vector2 GetWindowSize()
		{
			return new Vector2(200,150);
		}

		public override void OnGUI(Rect rect)
		{
			Search = EditorGUILayout.TextField (Search);

			Scroll = EditorGUILayout.BeginScrollView (Scroll);
			for (int i = 0; i < Type.Count; i++) 
			{
				if (Type [i].Name.Contains (Search) || String.IsNullOrEmpty (Search)) {
					EditorGUILayout.BeginHorizontal ();
					TypeState [i] = EditorGUILayout.Toggle (TypeState [i]);
					GUILayout.Label (Type [i].Name);
					EditorGUILayout.EndHorizontal ();
				}
			}
			EditorGUILayout.EndScrollView ();

			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if (GUILayout.Button ("Add Component(s)")) {
				for (int i = 0; i < TypeState.Length; i++) {
					if (TypeState [i]) {
						TypeConfirmed (Type [i]);
					}
				}

				this.editorWindow.Close ();
			}
			GUILayout.FlexibleSpace ();
			EditorGUILayout.EndHorizontal ();
		}

		public override void OnOpen()
		{
			Type = GetTypeList();
			TypeState = new bool[Type.Count];
		}

		private List<Type> GetTypeList()
		{
			List<Type> TypeList = new List<Type> ();
			Assembly[] Libraries = AppDomain.CurrentDomain.GetAssemblies ();
			foreach(var Library in Libraries){
				var LibraryTypes = Library.GetTypes ();
				foreach (var LibraryType in LibraryTypes) {
					if (TypeFilter.IsAssignableFrom (LibraryType) && LibraryType.IsClass
						&& !LibraryType.IsAbstract) {
						TypeList.Add (LibraryType);
					}
				}
			}

			return TypeList;
		}
	}
}