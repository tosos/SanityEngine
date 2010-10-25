using UnityEngine;
using System.Collections;
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
	public Vector3 targetOffset = Vector3.zero;
	public string moveBehavior;

	SteeringManagerComponent manager;
	PointActor target;
	ASearch<UnityNode, UnityEdge> finder;
	Path<UnityNode, UnityEdge> path;
	CoherentPathFollower<UnityNode, UnityEdge> follower;

	void Start () {
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
		if(!follower.Valid) {
			return;
		}
		
		follower.LookAhead = lookAhead;
		float param = follower.GetNextParameter(transform.position);
		target.Point = follower.GetPosition(param + lookAhead) + targetOffset;
	}
	
	void SetGoalNode(UnityNode goal)
	{
		if(goal == null || goal.NavMesh == null) {
			ClearGoalNode();
			return;
		}
		
		manager[moveBehavior].SetEnabled(false);
		
		path = finder.Search(goal.NavMesh.Quantize(transform.position), goal);
		follower.Path = path;
		
		manager[moveBehavior].SetEnabled(true);
	}
	
	void ClearGoalNode()
	{
		follower.Path = null;
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
}
