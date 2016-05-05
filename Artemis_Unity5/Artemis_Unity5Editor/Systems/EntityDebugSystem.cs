#region File description

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityDebugSystem.cs" company="GAMADU.COM">
//     Copyright © 2013 GAMADU.COM. All rights reserved.
//
//     Redistribution and use in source and binary forms, with or without modification, are
//     permitted provided that the following conditions are met:
//
//        1. Redistributions of source code must retain the above copyright notice, this list of
//           conditions and the following disclaimer.
//
//        2. Redistributions in binary form must reproduce the above copyright notice, this list
//           of conditions and the following disclaimer in the documentation and/or other materials
//           provided with the distribution.
//
//     THIS SOFTWARE IS PROVIDED BY GAMADU.COM 'AS IS' AND ANY EXPRESS OR IMPLIED
//     WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
//     FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL GAMADU.COM OR
//     CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
//     CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
//     SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
//     ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
//     NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
//     ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//
//     The views and conclusions contained in the software and documentation are those of the
//     authors and should not be interpreted as representing official policies, either expressed
//     or implied, of GAMADU.COM.
// </copyright>
// <summary>
//   Class EntityDebugSystem.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion File description

namespace Artemis_Unity5Editor
{
	#region Using statements

	using Artemis.Manager;
	using Artemis.Attributes;
	using Artemis.System;
	using Artemis;

	using UnityEngine;

	#endregion Using statements

	/// <summary>
	/// Entity Debug System.
	/// </summary>
	[ArtemisEntitySystem(ExecutionType = ExecutionType.Synchronous, GameLoopType = GameLoopType.Update, Layer = 0)]
	public class EntityDebugSystem : EntitySystem
	{
		GameObject Container = new GameObject ("EntityWorld (Active Entities: 0)");

		public override void LoadContent ()
		{
			this.EntityWorld.EntityManager.AddedEntityEvent += AddedEntityEvent;
			this.EntityWorld.EntityManager.RemovedEntityEvent += RemovedEntityEvent;
		}

		void RemovedEntityEvent (Entity entity)
		{
			Print ();

			Debug.Log ("Entity " + entity.UniqueId + " Removed.");
		}

		void AddedEntityEvent (Entity entity)
		{
			GameObject entityObj = new GameObject ("Entity " + entity.Id);
			entityObj.transform.parent = Container.transform;
			entityObj.AddComponent<EntityBehaviour> ();
			var script = entityObj.GetComponent<EntityBehaviour> ();
			script.EntityManager = this.EntityWorld.EntityManager;
			script.Entity = entity;

			Print ();

			Debug.Log ("Entity " + entity.UniqueId + " Added.");
		}

		void Print()
		{
			Container.name = "EntityWorld (Active Entities: " + this.entityWorld.CurrentState.Count;
			Container.name += " Total Created: " + this.entityWorld.EntityManager.TotalCreated;
			Container.name += " Total Removed: " + this.entityWorld.EntityManager.TotalRemoved + ")";
		}
	}
}