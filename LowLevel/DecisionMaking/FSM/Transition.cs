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
	class Transition
	{
		readonly FSMEvent trigger;
        readonly State target;
        readonly Guard guard;
		Action action;
		
        /// <summary>
        /// The name of the event triggering the transition.
        /// </summary>
		public FSMEvent Trigger
		{
			get { return trigger; }
		}
		
        /// <summary>
        /// The target state after the transition occurs.
        /// </summary>
        public State Target
		{
			get { return target; }
		}

        public Transition(FSMEvent trigger, State target,
			Guard guard, Action action)
		{
			this.trigger = trigger;
			this.target = target;
			this.guard = guard;
			this.action = action;
		}
		
		public bool CheckGuard()
		{
			if(guard != null) {
				return guard();
			}
			return true;
		}
		
		public void FireAction()
		{
			if(action != null) {
				action();
			}
		}
	}
}
