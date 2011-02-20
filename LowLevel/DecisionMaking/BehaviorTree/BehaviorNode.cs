using UnityEngine;
using System.Collections.Generic;
using SanityEngine.Actors;

namespace SanityEngine.DecisionMaking.BehaviorTree
{
	public interface BehaviorNode {
		bool Tick(Actor actor);
	}
}
