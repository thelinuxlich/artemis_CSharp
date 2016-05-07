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
			e.AddComponent (new Position ());
			e.AddComponent (new Velocity ());
			e.AddComponent (new View ());
			e.AddComponent (new Test ());
			e.AddComponent (new View ());
			e.AddComponent (new Empty ());
		}

		if (Input.GetKeyUp (KeyCode.S)) {
			World.Clear ();
		}

		if (Input.GetKeyUp (KeyCode.D)) {
			var entities = World.CurrentState;
			Debug.Log (entities.Count);

			Debug.Log (World.GetEntity (0).HasComponent<Position> ());
		}

		if (Input.GetKeyUp (KeyCode.W)) {
			World.GetEntity (0).AddComponent<Position> (new Position());
		}

		if (Input.GetKeyUp (KeyCode.F)) {
			var e = World.CreateEntity ();
			e.AddComponent (new Position ());
			e.GetComponent<Position> ().X = Random.Range(0,100);
			e.AddComponent<Velocity> (new Velocity());
		}

		if (Input.GetKeyUp (KeyCode.G)) {
			var entities = World.CurrentState;
			foreach (var entity in entities) {
				Debug.Log (entity.Key.GetComponent<Position> ().X);
			}
		}
	}
}
