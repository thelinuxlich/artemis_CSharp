using UnityEngine;
using System.Collections;

using Artemis;

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
			var entities = World.CurrentState;
			Debug.Log (entities.Count);
		}

		if (Input.GetKeyUp (KeyCode.F)) {
			var e = World.CreateEntity ();
			e.AddComponent (new PositionComponent ());
			e.GetComponent<PositionComponent> ().X = Random.Range(0,100);
		}

		if (Input.GetKeyUp (KeyCode.G)) {
			var entities = World.CurrentState;
			foreach (var entity in entities) {
				Debug.Log (entity.Key.GetComponent<PositionComponent> ().X);
			}
		}
	}
}
