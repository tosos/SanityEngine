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
public class PathFollowerComponent : SteeringBehaviorComponent {
	public float lookAhead = 2.0f;
	public Vector3 targetOffset = Vector3.zero;

	PointActor target;
	Arrive arrive;
	ASearch<UnityNode, UnityEdge> finder;
	Path<UnityNode, UnityEdge> path;
	CoherentPathFollower<UnityNode, UnityEdge> follower;

	void Awake () {
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
		
		arrive.Weight = 0f;
		
		path = finder.Search(goal.NavMesh.Quantize(transform.position), goal);
		follower.Path = path;
		
		arrive.Weight = 1f;
	}
	
	void ClearGoalNode()
	{
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
	
	public override SteeringBehavior Behavior
	{
		get { return arrive; }
	}	
}
