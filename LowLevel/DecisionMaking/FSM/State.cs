//
// Copyright (C) 2010 The Sanity Engine Development Team
//
// This source code is licensed under the terms of the
// MIT License.
//
// For more information, see the file LICENSE

using System;
using System.Collections.Generic;

namespace SanityEngine.DecisionMaking.FSM
{
    /// <summary>
    /// A state in a finite state machine.
    /// </summary>
    /// <typeparam name="TData">The data type used for transition guard checks.</typeparam>
	public class State
	{
		readonly string name;
		readonly Action tickAction;
		readonly Action enterAction;
		readonly Action exitAction;
		Dictionary<FSMEvent, List<Transition>> transitions = new Dictionary<FSMEvent, List<Transition>>();
		
        /// <summary>
        /// The name of the state.
        /// </summary>
		public string Name
		{
			get { return name; }
		}
		
		internal State (string name, Action tickAction, Action enterAction, Action exitAction)
		{
			this.name = name;
			this.tickAction = tickAction;
			this.enterAction = enterAction;
			this.exitAction = exitAction;
		}
				
		internal void AddTransition(Transition transition)
		{
			List<Transition> ts = null;
			if(!transitions.ContainsKey(transition.Trigger)) {
				ts = new List<Transition>();
				transitions[transition.Trigger] = ts;
			} else {
				ts = transitions[transition.Trigger];
			}
			ts.Add(transition);
		}

		internal void Tick()
		{
			if(tickAction != null) {
				tickAction();
			}
		}
		
		internal State TriggerEvent(FSMEvent trigger)
		{
			if(!transitions.ContainsKey(trigger)) {
				return null;
			}

            foreach (Transition transition in transitions[trigger])
            {
				if(transition.CheckGuard()) {
					FireExitAction();
					transition.FireAction();
					State newState = transition.Target;
					newState.FireEnterAction();
					return newState;
				}
			}
			return null;
		}

		public void FireEnterAction()
		{
			if(enterAction != null) {
				enterAction();
			}
		}

		public void FireExitAction()
		{
			if(exitAction != null) {
				exitAction();
			}
		}
	}
}
