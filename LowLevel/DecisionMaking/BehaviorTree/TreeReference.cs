using UnityEngine;
using System.Collections.Generic;
using SanityEngine.Actors;

namespace SanityEngine.DecisionMaking.BehaviorTree
{
	public class TreeReference : BehaviorNode {
		BehaviorTree subTree;
		
		public void SetSubTree(BehaviorTree tree)
		{
			this.subTree = tree;
		}
		
		public bool Tick (Actor actor)
		{
			if(subTree == null) {
				throw new System.InvalidOperationException("No subtree set");
			}
			
			return subTree.Tick(actor);
		}
	}
}
