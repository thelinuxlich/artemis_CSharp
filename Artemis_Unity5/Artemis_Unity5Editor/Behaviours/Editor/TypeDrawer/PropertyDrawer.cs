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

	using Artemis.Interface;
	using Artemis;

	#endregion

	/// <summary>
	/// Property Drawer.
	/// </summary>
	public static class PropertyDrawer
	{
		public static void DrawPropertyList(IComponent Component, FieldInfo[] FieldInfoList)
		{
			foreach (FieldInfo FieldInfo in FieldInfoList) {
				PropertyDrawerStyle.BeginPropertyList ();
				DrawProperty (Component, FieldInfo);
				PropertyDrawerStyle.EndPropertyList ();
			}
		}

		public static void DrawPropertyList(IComponent Component, PropertyInfo[] PropertyList)
		{
			foreach (PropertyInfo PropertyInfo in PropertyList) {
				PropertyDrawerStyle.BeginPropertyList ();
				DrawProperty (Component, PropertyInfo);
				PropertyDrawerStyle.EndPropertyList ();
			}
		}

		public static void DrawProperty(IComponent Component, FieldInfo FieldInfo)
		{
			if (TypeDrawerManager.Supports (FieldInfo.FieldType)) {

				PropertyDrawerStyle.BeginProperty (FieldInfo.Name);
				// Get value on the target instance.
				object value = FieldInfo.GetValue (Component);
				object nvalue = TypeDrawerManager.Draw (FieldInfo.FieldType, value);

				FieldInfo.SetValue (Component, Convert.ChangeType (nvalue, FieldInfo.FieldType));

				if (GUILayout.Button ("x", GUILayout.Width (19), GUILayout.Height (14))) {
					FieldInfo.SetValue (Component, Convert.ChangeType (TypeDrawerManager.DefaultOf(FieldInfo.FieldType), FieldInfo.FieldType));
				}

				PropertyDrawerStyle.EndProperty ();
			}
		}

		public static void DrawProperty(IComponent Component, PropertyInfo PropertyInfo)
		{
			if (TypeDrawerManager.Supports (PropertyInfo.PropertyType)) {
				
				PropertyDrawerStyle.BeginProperty (PropertyInfo.Name);
				// Get value on the target instance.
				object value = PropertyInfo.GetValue (Component, null);
				object nvalue = TypeDrawerManager.Draw (PropertyInfo.PropertyType, value);

				PropertyInfo.SetValue (Component, Convert.ChangeType (nvalue, PropertyInfo.PropertyType), null);

				if (GUILayout.Button ("x", GUILayout.Width (19), GUILayout.Height (14))) {
					PropertyInfo.SetValue (Component, Convert.ChangeType (TypeDrawerManager.DefaultOf(PropertyInfo.PropertyType), PropertyInfo.PropertyType));
				}

				PropertyDrawerStyle.EndProperty ();
			}
		}
	}
}