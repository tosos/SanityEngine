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
	public UnityGraph graph;
	public DecisionMaker decisionMaker;

	PointActor target;
	Arrive arrive;
	ASearch<UnityNode, UnityEdge> finder;
	Path<UnityNode, UnityEdge> path;
	CoherentPathFollower<UnityNode, UnityEdge> follower;
	UnityNode lastGoal;

	void Awake () {
		Heuristic heuristic = new EuclideanHeuristic();
		
		finder = new ASearch<UnityNode, UnityEdge>(heuristic.Heuristic);
		follower = new CoherentPathFollower<UnityNode, UnityEdge>();
		arrive = new Arrive();
		arrive.TimeToTarget = 3;
		arrive.ArriveRadius = 1.0f;
		target = new PointActor(Vector3.zero);
		arrive.Target = target;
	}
	
	// Update is called once per frame
	void Update () {
		UnityNode goal = decisionMaker.GetMovementTarget();
		if(goal == null) {
			arrive.Weight = 0f;
			return;
		}
		if(!follower.Valid || goal != lastGoal) {
			arrive.Weight = 0f;
			path = finder.Search(graph.Quantize(transform.position), goal);
			follower.Path = path;
			lastGoal = goal;
			return;
		}
		
		arrive.Weight = 1f;
		float param = follower.GetNextParameter(transform.position);
		target.Point = follower.GetPosition(param + 2.0f) + Vector3.up;
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
