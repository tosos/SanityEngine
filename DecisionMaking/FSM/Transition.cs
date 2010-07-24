//
// Copyright (C) 2010 The Sanity Engine Development Team
//
// This source code is licensed under the terms of the
// MIT License.
//
// For more information, see the file LICENSE

using System;

namespace SanityEngine.DecisionMaking.FSM
{
    /// <summary>
    /// A transition from one state to another.
    /// </summary>
	class Transition<TData>
	{
		readonly string eventName;
        readonly State<TData> target;
        readonly Predicate<TData> pred;
		Action<TData> action;
		
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

        public Transition(string eventName, State<TData> target,
			Predicate<TData> pred, Action<TData> action)
		{
			this.eventName = eventName;
			this.target = target;
            this.pred = pred;
			this.action = action;
		}
		
		public bool Check(TData data)
		{
			if(pred != null) {
				return pred(data);
			}
			return true;
		}
		
		public void FireAction(TData data)
		{
			if(action != null) {
				action(data);
			}
		}
	}
}
