using UnityEngine;
using System.Collections;
using SanityEngine.Movement.SteeringBehaviors.Flocking;

public class FlockComponent : MonoBehaviour {
	Flock flock;
	
	public Flock Flock
	{
		get { return flock; }
	}
	
	void Awake() {
		flock = new Flock();
	}
	
	void Start() {
		GameObjectActor[] actors = GetComponentsInChildren<GameObjectActor>();
		foreach(GameObjectActor actor in actors) {
			flock.AddToFlock(actor);
		}
	}
}
