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
	public delegate bool Guard();
	
    /// <summary>
    /// A finite state machine.
    /// </summary>
    /// <typeparam name="TData">The data type used for transition guard checks.</typeparam>
    public class FiniteStateMachine
	{
        Dictionary<string, State> states = new Dictionary<string, State>();
        State startState;
        State currentState;
		int nextEvent = 0;
		Dictionary<string, FSMEvent> registeredEvents = new Dictionary<string, FSMEvent>();

        /// <summary>
        /// The current state.
        /// </summary>
        public State CurrentState
		{
			get { return currentState; }
		}
		
        /// <summary>
        /// Create a finite state machine.
        /// </summary>
        /// <param name="data">The data used for predicate guard checks</param>
		public FiniteStateMachine ()
		{
		}
		
		public FSMEvent RegisterEvent(string name)
		{
			if(registeredEvents.ContainsKey(name)) {
				throw new ArgumentException("Event '" + name + "' already registered");
			}
			
			FSMEvent newEvent = new FSMEvent(nextEvent ++, name);
			registeredEvents[name]  = newEvent;
			return newEvent;
		}
		
        /// <summary>
        /// Add a state.
        /// </summary>
        /// <param name="name">The name of the state</param>
        /// <param name="startState"><code>true</code> if this state is the start state</param>
		public void AddState(string name, bool startState)
		{
			AddState(name, startState, null, null, null);
		}
		
        /// <summary>
        /// Add a state.
        /// </summary>
        /// <param name="name">The name of the state</param>
        /// <param name="startState"><code>true</code> if this state is the start state</param>
        /// <param name="tickAction">An action called when the state is ticked</param>
        /// <param name="enterAction">An action called when entering the state</param>
        /// <param name="exitAction">An action called when exiting the state</param>
		public void AddState(string name, bool startState, Action tickAction, Action enterAction, Action exitAction)
		{
			State newState = new State(name, tickAction, enterAction, exitAction);
			states.Add(name, newState);
			if(startState) {
				if(this.startState != null) {
					throw new ArgumentException("Start state already set!");
				}
				this.startState = newState;
			}
		}
		
        /// <summary>
        /// Add a transition.
        /// </summary>
        /// <param name="source">The source state name.</param>
        /// <param name="target">The target state name.</param>
        /// <param name="eventName">The event name that triggers this transition.</param>
		public void AddTransition(string source, string target, FSMEvent trigger)
		{
			AddTransition(source, target, trigger, null, null);
		}
		
        /// <summary>
        /// Add a transition.
        /// </summary>
        /// <param name="source">The source state name.</param>
        /// <param name="target">The target state name.</param>
        /// <param name="eventName">The event name that triggers this transition.</param>
        /// <param name="guard">The guard predicate for this transition.</param>
		public void AddTransition(string source, string target, FSMEvent trigger,
            Guard guard)
		{
			AddTransition(source, target, trigger, guard, null);
		}
		
        /// <summary>
        /// Add a transition.
        /// </summary>
        /// <param name="source">The source state name.</param>
        /// <param name="target">The target state name.</param>
        /// <param name="eventName">The event name that triggers this transition.</param>
        /// <param name="action">An action callback to be called if this transition is triggered.</param>
		public void AddTransition(string source, string target, FSMEvent trigger,
            Action action)
		{
			AddTransition(source, target, trigger, null, action);
		}
		
        /// <summary>
        /// Add a transition.
        /// </summary>
        /// <param name="source">The source state name.</param>
        /// <param name="target">The target state name.</param>
        /// <param name="eventName">The event name that triggers this transition.</param>
        /// <param name="predicate">The guard predicate for this transition.</param>
        /// <param name="action">An action callback to be called if this transition is triggered.</param>
		public void AddTransition(string source, string target, FSMEvent trigger,
            Guard guard, Action action)
		{
			if(!states.ContainsKey(source)) {
				throw new ArgumentException("No such source state found");
			}
			
			if(!states.ContainsKey(target)) {
				throw new ArgumentException("No such target state found");
			}
			
			states[source].AddTransition(new Transition(trigger,
				states[target], guard, action));
		}
		
		/// <summary>
		/// Enters the start state.
		/// </summary>
		public void Start()
		{
			if(startState == null) {
				throw new InvalidOperationException("No start state set!");
			}
			
			if(currentState != null) {
				throw new InvalidOperationException("FSM already in a state");
			}
			
			startState.FireEnterAction();
			currentState = startState;
		}
		
        /// <summary>
        /// Reset this FSM to the start state.
        /// </summary>
		public void Reset()
		{		
			if(currentState != null) {
				currentState.FireExitAction();
				currentState = null;
			}
			
			Start();
		}
		
		/// <summary>
		/// Tick the current state's update action (if any).
		/// </summary>
		public void Tick()
		{
			if(currentState == null) {
				throw new InvalidOperationException("No current state (you may need to call Start)");
			}
			
			currentState.Tick();
		}
		
        /// <summary>
        /// Trigger an event. If a transition exists for this event name in
        /// the current state, the state will change to the transition's
        /// target state.
        /// </summary>
        /// <param name="eventName">The event name.</param>
		public void TriggerEvent(FSMEvent evt)
		{
			if(currentState == null) {
				throw new InvalidOperationException("No current state (you may need to call Start)");
			}
			
			if(!registeredEvents.ContainsKey(evt.Name)) {
				throw new InvalidOperationException("Event is not registered");
			}

            State newState = currentState.TriggerEvent(evt);
			if(newState != null) {
				currentState = newState;
			}
		}
	}
}
