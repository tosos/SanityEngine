using UnityEngine;
using System.Collections.Generic;
using SanityEngine.Actors;
using SanityEngine.Movement.SteeringBehaviors;
using SanityEngine.Movement.PathFollowing;
using SanityEngine.Structure.Graph;
using SanityEngine.Structure.Graph.NavMesh;
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
	public float pathWidth = 5f;

	SteeringManagerComponent manager;
	PointActor target;
	ASearch finder;
	NavMesh navMesh;
	Path path;
	CoherentPathFollower follower;
	NavMeshNode goalNode;
	List<MonoBehaviour> listeners;
	float prevParam;
	Vector3 prevPoint;
	
	void Awake ()
	{
		listeners = new List<MonoBehaviour>();
	}

	void Start ()
	{
		prevParam = 0.0f;
		manager = GetComponent<SteeringManagerComponent>();
		Heuristic heuristic = new EuclideanHeuristic();
		
		finder = new ASearch(heuristic.Heuristic);
		follower = new CoherentPathFollower();
		manager[moveBehavior].SetEnabled(false);
		manager[moveBehavior].SetFloat("ArriveRadius", lookAhead * 0.75f);
		target = new PointActor(Vector3.zero);
		manager[moveBehavior].SetActor("Target", target);
	}
	
	void LateUpdate () {
		if(goalNode == null || path == null) {
			return;
		}
		
		if(Vector3.Distance(transform.position, goalNode.Position) < doneThreshhold) {
			SendPathMessage("OnPathArrived");
			ClearGoalNode();
			return;
		}

		if(path.StepCount < 1) {
			target.Point = goalNode.Position;
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
		
		float param = follower.GetNextParameter(transform.position, prevParam + epsilon, prevParam + lookAhead);
		Vector3 point = follower.GetPosition(param + lookAhead) + targetOffset;
		
		Vector3 toMe = transform.position - point;
		Vector3 dir = point - prevPoint;
		Vector3 side = Vector3.Cross(Vector3.up, dir);
		Vector3 offset = Vector3.Project(toMe, side);
		float mag = offset.magnitude;
		float radius = pathWidth * 0.5f;
		if(mag >= radius) {
			offset *= radius / mag;
		}
		
		target.Point = point + offset;
		prevParam = param;
		prevPoint = point;
	}
	
	void SetNavMesh(NavMesh navMesh)
	{
		this.navMesh = navMesh;
	}
	
	void SetGoalNode(NavMeshNode goal)
	{
		if(goal == null) {
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
			Edge edge = path.GetStep(i);
			NavMeshNode src = (NavMeshNode)edge.Source;
			NavMeshNode tgt = (NavMeshNode)edge.Target;
			Gizmos.DrawSphere(tgt.Position, 0.1f);
			Gizmos.DrawLine(src.Position, tgt.Position);
		}
	}
	
	void FindNewPath()
	{
		path = finder.Search(navMesh, navMesh.Quantize(transform.position), goalNode);
		if(path == null) {
			ClearGoalNode();
			SendPathMessage("OnPathNotFound");
			return;
		}
		
		prevParam = 0.0f;
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
