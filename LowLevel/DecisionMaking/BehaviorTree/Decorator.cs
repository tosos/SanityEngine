using UnityEngine;
using System.Collections.Generic;
using SanityEngine.Actors;

namespace SanityEngine.DecisionMaking.BehaviorTree
{
	public class Decorator : BehaviorNode {
		public delegate bool DecoratorCallback(Actor actor);
		
		DecoratorCallback callback;
		
		BehaviorNode child;
		
		public Decorator(DecoratorCallback callback)
		{
			if(callback == null) {
				throw new System.ArgumentException("Decorator callback must not be null");
			}
			this.callback = callback;			
		}
		
		public void SetChild(BehaviorNode child)
		{
			this.child = child;
		}

		public bool Tick (Actor actor)
		{
			if(child == null) {
				throw new System.InvalidOperationException("Decorator node has no child");
			}
			
			if(callback(actor)) {
				return child.Tick(actor);
			}
			return false;
		}
	}
}
