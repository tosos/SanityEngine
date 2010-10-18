using UnityEngine;
using System.Collections.Generic;
using SanityEngine.Actors;
using SanityEngine.Movement.SteeringBehaviors;
using SanityEngine.Movement.PathFollowing;
using SanityEngine.Structure.Graph;
using SanityEngine.Structure.Path;
using SanityEngine.Search.PathFinding;
using SanityEngine.Search.PathFinding.Algorithms;
using SanityEngine.Utility.Heuristics;

[AddComponentMenu("Sanity Engine/Movement/Path Follower")]
public class PathFollowerComponent : SteeringBehaviorComponent {
	public float lookAhead = 2.0f;
	public float minimumMovement = 0.001f;
	public float doneThreshold = 0.5f;
	public Vector3 targetOffset = Vector3.zero;
	public bool broadcastMessages = true;

	PointActor target;
	Arrive arrive;
	ASearch<UnityNode, UnityEdge> finder;
	Path<UnityNode, UnityEdge> path;
	UnityNode goalNode;
	CoherentPathFollower<UnityNode, UnityEdge> follower;
	bool atDestination;
	List<Component> listeners;

	public override SteeringBehavior Behavior
	{
		get { return arrive; }
	}	
	
	void Awake () {
		listeners = new List<Component>();
		
		Heuristic heuristic = new EuclideanHeuristic();
		
		finder = new ASearch<UnityNode, UnityEdge>(heuristic.Heuristic);
		follower = new CoherentPathFollower<UnityNode, UnityEdge>();
		arrive = new Arrive();
		arrive.ArriveRadius = lookAhead * 0.75f;
		arrive.Weight = 0f;
		target = new PointActor(Vector3.zero);
		arrive.Target = target;
	}
	
	void Update () {
		if(!follower.Valid || goalNode == null) {
			return;
		}
		
		follower.LookAhead = lookAhead;
		follower.Epsilon = minimumMovement;
		Vector3 pos = transform.position;
		float param = follower.GetNextParameter(pos);
		target.Point = follower.GetPosition(param + lookAhead) + targetOffset;
		
		if(!atDestination && Vector3.Distance(pos, goalNode.Position) < doneThreshold) {
			PathMessage("OnPathFollowerArrived");
		}
	}
	
	void SetGoalNode(UnityNode goal)
	{
		goalNode = goal;
		
		arrive.Weight = 0f;
		FindNewPath(goal);		
		arrive.Weight = 1f;
	}
	
	void ClearGoalNode()
	{
		atDestination = false;
		
		goalNode = null;
		follower.Path = null;
		arrive.Weight = 0f;
	}
	
	void OnDrawGizmosSelected()
	{
		if(path == null) {
			return;
		}

		Gizmos.color = Color.yellow;
		
		Gizmos.DrawSphere(target.Point, 0.2f);
		
		Gizmos.color = Color.white;
		for(int i=0;i<path.StepCount;i++) {
			UnityEdge edge = path.GetStep(i);
			Gizmos.DrawSphere(edge.Target.Position, 0.1f);
			Gizmos.DrawLine(edge.Source.Position, edge.Target.Position);
		}
	}
	
	void FindNewPath(UnityNode goal)
	{
		atDestination = false;
		
		if(goal == null || goal.NavMesh == null) {
			ClearGoalNode();
			return;
		}

		path = finder.Search(goal.NavMesh.Quantize(transform.position), goal);
		if(path == null) {
			ClearGoalNode();
			SendMessage("OnPathFollowerNoPath", SendMessageOptions.DontRequireReceiver);
			return;
		}
		follower.Path = path;
		SendMessage("OnPathFollowerNewPath", SendMessageOptions.DontRequireReceiver);
	}
	
	void PathMessage(string methodName)
	{
		if(broadcastMessages) {
			BroadcastMessage(methodName, SendMessageOptions.DontRequireReceiver);
		} else {
			SendMessage(methodName, SendMessageOptions.DontRequireReceiver);
		}
		
		for(int i = 0; i < listeners.Count; i ++) {
			listeners[i].SendMessage(methodName, this, SendMessageOptions.DontRequireReceiver);			
		}
	}
	
	/// <summary>
	/// Adds a listener, which will receive path follower events.
	/// </summary>
	/// <param name='listener'>the listener</param>
	public void AddListener(Component listener)
	{
		listeners.Add(listener);
	}

	/// <summary>
	/// Remove a listener.
	/// </summary>
	/// <param name='listener'>the listener</param>
	public void RemoveListener(Component listener)
	{
		listeners.Remove(listener);
	}
}
		