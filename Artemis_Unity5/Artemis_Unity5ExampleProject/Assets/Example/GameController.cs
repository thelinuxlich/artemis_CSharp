using UnityEngine;
using System.Collections;

using Artemis;
using System.Linq;

public class GameController : MonoBehaviour {

	EntityWorld World = new EntityWorld ();

	// Use this for initialization
	void Start () {

		World.InitializeAll (true);
	}

	void Update()
	{
		World.Update ();
		World.Draw ();


		if (Input.GetKeyUp (KeyCode.A)) {
			var e = World.CreateEntity();
			e.AddComponent (new PositionComponent ());
		}

		if (Input.GetKeyUp (KeyCode.S)) {
			World.Clear ();
		}

		if (Input.GetKeyUp (KeyCode.D)) {
			var entities = World.EntityManager.ActiveEntities;
			Debug.Log (entities.Count);
		}
	}
}
