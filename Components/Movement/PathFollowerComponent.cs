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
public class PathFollowerComponent : MonoBehaviour {
	public float lookAhead = 2.0f;
	public float epsilon = 0.001f;
	public float doneThreshhold = 0.1f;
	public Vector3 targetOffset = Vector3.zero;
	public string moveBehavior;
	public bool broadcastMessages = true;

	SteeringManagerComponent manager;
	PointActor target;
	ASearch<UnityNode, UnityEdge> finder;
	Path<UnityNode, UnityEdge> path;
	CoherentPathFollower<UnityNode, UnityEdge> follower;
	UnityNode goalNode;
	List<MonoBehaviour> listeners;
	
	void Awake ()
	{
		listeners = new List<MonoBehaviour>();
	}

	void Start ()
	{
		manager = GetComponent<SteeringManagerComponent>();
		Heuristic heuristic = new EuclideanHeuristic();
		
		finder = new ASearch<UnityNode, UnityEdge>(heuristic.Heuristic);
		follower = new CoherentPathFollower<UnityNode, UnityEdge>();
		manager[moveBehavior].SetEnabled(false);
		manager[moveBehavior].SetFloat("ArriveRadius", lookAhead * 0.75f);
		target = new PointActor(Vector3.zero);
		manager[moveBehavior].SetActor("Target", target);
	}
	
	void Update () {
		if(goalNode == null || path == null) {
			return;
		}
		
		if(Vector3.Distance(transform.position, goalNode.Position) < doneThreshhold) {
			SendPathMessage("OnPathArrived");
			ClearGoalNode();
			return;
		}
		
		if(path.Graph.HasChanged) {
			// XXX replace with callbacks
			if(!path.TestValidity()) {
				FindNewPath();
			}
			path.Graph.ResetChanges();
			return;
		}
		
		float param = follower.GetNextParameter(transform.position, epsilon, lookAhead);
		target.Point = follower.GetPosition(param + lookAhead) + targetOffset;
	}
	
	void SetGoalNode(UnityNode goal)
	{
		if(goal == null || goal.NavMesh == null) {
			ClearGoalNode();
			return;
		}
		
		manager[moveBehavior].SetEnabled(false);

		this.goalNode = goal;
		
		FindNewPath();
				
		manager[moveBehavior].SetEnabled(true);
	}
	
	void ClearGoalNode()
	{
		goalNode = null;
		path = null;
		manager[moveBehavior].SetEnabled(false);
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
	
	void FindNewPath()
	{
		path = finder.Search(goalNode.NavMesh.Quantize(transform.position), goalNode);
		if(path == null) {
			ClearGoalNode();
			SendPathMessage("OnPathNotFound");
			return;
		}
		
		follower.SetPath(path);
		
		SendPathMessage("OnPathNew");
	}
	
	void SendPathMessage(string message)
	{
		if(broadcastMessages) {
			BroadcastMessage(message, SendMessageOptions.DontRequireReceiver);
		} else {
			SendMessage(message, SendMessageOptions.DontRequireReceiver);
		}

		for(int i = 0; i < listeners.Count; i ++) {
			listeners[i].SendMessage(message, this, SendMessageOptions.DontRequireReceiver);
		}
	}
	
	public void AddListener(MonoBehaviour listener)
	{
		listeners.Add(listener);
	}

	public void RemoveListener(MonoBehaviour listener)
	{
		listeners.Remove(listener);
	}
}
