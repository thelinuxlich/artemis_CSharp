using UnityEngine;
using System.Collections;

using Artemis;

public class GameController : MonoBehaviour {

	EntityWorld World = new EntityWorld ();

	// Use this for initialization
	void Start () {

		World.InitializeAll (true);
	}

	// update the entity world.
	void Update()
	{
		World.Update ();
		World.Draw ();

		if (Input.GetKeyUp (KeyCode.A)) {
			var e = World.CreateEntity();
			e.AddComponent (new Component1 ());
			e.AddComponent (new Component2 ());
			e.AddComponent (new Component3 ());
			e.AddComponent (new Component3 ());
			e.AddComponent (new Component4 ());
			e.AddComponent (new Component5 ());
		}

		if (Input.GetKeyUp (KeyCode.C)) {
			World.Clear ();
		}
	}
}
