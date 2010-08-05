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
	public UnityNode targetNode;

	PointActor target;
	Arrive arrive;
	ASearch<UnityNode, UnityEdge> finder;
	CoherentPathFollower<UnityNode, UnityEdge> follower;

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
		if(!follower.Valid) {
			follower.Path = finder.Search(
				graph.Quantize(transform.position), targetNode);
			return;
		}
		
		float param = follower.GetNextParameter(transform.position);
		target.Point = follower.GetPosition(param + 2.0f) + Vector3.up;
	}
	
	public override SteeringBehavior Behavior
	{
		get { return arrive; }
	}	
}
