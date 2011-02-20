using UnityEngine;
using System.Collections.Generic;
using SanityEngine.Actors;
using SanityEngine.Utility.Containers;

namespace SanityEngine.DecisionMaking.BehaviorTree
{
	public class PrioritySelector : BehaviorNode {
		public delegate int GetPriority(int ID);
		
		private class Comparer : IComparer<int>
		{
			GetPriority priorityCallback;
			public Comparer(GetPriority callback)
			{
				priorityCallback = callback;
			}
			
			public int Compare (int x, int y)
			{
				int px = priorityCallback(x);
				int py = priorityCallback(y);
				
				return px.CompareTo(py);
			}
		}
		
		private Dictionary<int, BehaviorNode> children = new Dictionary<int, BehaviorNode>();
		private Comparer comparer;
		
		public PrioritySelector(GetPriority priorityCallback)
		{
			if(priorityCallback == null) {
				throw new System.InvalidOperationException("Priority callback must not be null");
			}
			
			this.comparer = new Comparer(priorityCallback);
		}
		
		public bool Tick (Actor actor)
		{
			List<int> ids = new List<int>(children.Keys);
			
			ids.Sort(comparer);

			for(int i = 0; i < ids.Count; i ++) {
				int id = ids[i];
				
				BehaviorNode child = children[id];
				if(child.Tick(actor)) {
					return true;
				}
			}
			return false;
		}
		
		public void AddChild(int id, BehaviorNode child)
		{
			children[id] = child;
		}
		
		public void RemoveChild(int id)
		{
			children.Remove(id);
		}
	}
}
