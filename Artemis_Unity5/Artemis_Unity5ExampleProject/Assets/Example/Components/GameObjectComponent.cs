using Artemis.Interface;
using UnityEngine;

public class GameObjectComponent : IComponent, IReset {
	public GameObject GameObject;

	public void Reset(params object[] args)
	{
		GameObject = null;
	}
}