using UnityEngine;
using System.Collections.Generic;
using SanityEngine.Actors;

namespace SanityEngine.DecisionMaking.BehaviorTree
{
	public class Action : BehaviorNode {
		public delegate bool ActionCallback(Actor actor);
		
		ActionCallback callback;

		public Action(ActionCallback callback)
		{
			if(callback == null) {
				throw new System.ArgumentException("Action callback must not be null");
			}
			
			this.callback = callback;			
		}

		public bool Tick (Actor actor)
		{
			return callback(actor);
		}
	}
}
