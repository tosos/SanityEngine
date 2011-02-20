using UnityEngine;
using System.Collections.Generic;
using SanityEngine.Actors;

namespace SanityEngine.DecisionMaking.BehaviorTree
{
	public class Sequence : BehaviorNode {
		private List<BehaviorNode> children = new List<BehaviorNode>();
		
		public bool Tick (Actor actor)
		{
			if(children.Count <= 0) {
				throw new System.InvalidOperationException("Sequence node has no children");
			}
			
			for(int i = 0;i < children.Count; i ++) {
				BehaviorNode child = children[i];
				if(!child.Tick(actor)) {
					return false;
				}
			}
			return true;
		}

		public void AddChild(BehaviorNode child)
		{
			children.Add(child);
		}

		public void InsertChild(int index, BehaviorNode child)
		{
			children.Insert(index, child);
		}
		
		public void RemoveChild(int index)
		{
			children.RemoveAt(index);
		}
		
		public void RemoveChild(BehaviorNode child)
		{
			children.Remove(child);
		}
	}
}
