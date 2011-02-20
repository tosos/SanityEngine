using UnityEngine;
using System.Collections;
using SanityEngine.Actors;

namespace SanityEngine.DecisionMaking.BehaviorTree
{
	public class BehaviorTree {
		private BehaviorNode root;
		
		public void SetRootNode(BehaviorNode node)
		{
			this.root = node;
		}
		
		public bool Tick(Actor actor)
		{
			if(root == null) {
				throw new System.InvalidOperationException("No root node set");
			}
			
			return root.Tick(actor);
		}
	}
}
