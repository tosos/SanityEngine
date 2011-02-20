using UnityEngine;

namespace SanityEngine.DecisionMaking.FSM
{
	public class FSMEvent
	{
		int id;
		string name;
		
		public string Name
		{
			get { return name; }
		}
		
		internal FSMEvent(int id, string name)
		{
			this.id = id;
			this.name = name;
		}
		
		public override bool Equals (object obj)
		{
			if (obj == null)
				return false;
			if (ReferenceEquals (this, obj))
				return true;
			if (obj.GetType () != typeof(FSMEvent))
				return false;
			FSMEvent other = (FSMEvent)obj;
			return id == other.id;
		}


		public override int GetHashCode ()
		{
			return id.GetHashCode ();
		}

	}
}
