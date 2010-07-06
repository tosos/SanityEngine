using System;

namespace FSM
{
    /// <summary>
    /// A transition from one state to another.
    /// </summary>
	class Transition<TData>
	{
		readonly string eventName;
        readonly State<TData> target;
        readonly Predicate<TData> pred;
		
        /// <summary>
        /// The name of the event triggering the transition.
        /// </summary>
		public string EventName
		{
			get { return eventName; }
		}
		
        /// <summary>
        /// The target state after the transition occurs.
        /// </summary>
        public State<TData> Target
		{
			get { return target; }
		}

        public Transition(string eventName, State<TData> target, Predicate<TData> pred)
		{
			this.eventName = eventName;
			this.target = target;
            this.pred = pred;
		}
		
		public bool Check(TData data)
		{
			return pred(data);
		}
	}
}
