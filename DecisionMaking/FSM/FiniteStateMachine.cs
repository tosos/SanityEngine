using System;
using System.Collections.Generic;

namespace FSM
{
    /// <summary>
    /// A finite state machine.
    /// </summary>
    /// <typeparam name="TData">The data type used for transition guard checks.</typeparam>
    public class FiniteStateMachine<TData>
	{
        Dictionary<string, State<TData>> states = new Dictionary<string, State<TData>>();
        State<TData> startState;
        State<TData> currentState;
        TData data;

        /// <summary>
        /// The current state.
        /// </summary>
        public State<TData> CurrentState
		{
			get { return currentState; }
		}
		
        /// <summary>
        /// Create a finite state machine.
        /// </summary>
        /// <param name="data">The data used for predicate guard checks</param>
		public FiniteStateMachine (TData data)
		{
            this.data = data;
		}
		
        /// <summary>
        /// Add a state.
        /// </summary>
        /// <param name="name">The name of the state</param>
        /// <param name="startState"><code>true</code> if this state is the start state</param>
		public void AddState(string name, bool startState)
		{
			State<TData> newState = new State<TData>(name);
			states.Add(name, newState);
			if(startState) {
				if(this.startState != null) {
					throw new ArgumentException("Start state already set!");
				}
				this.startState = this.currentState = newState;
			}
		}
		
        /// <summary>
        /// Add a transition.
        /// </summary>
        /// <param name="source">The source state name.</param>
        /// <param name="target">The target state name.</param>
        /// <param name="eventName">The event name that triggers this transition.</param>
        /// <param name="predicate">The guard predicate for this transition.</param>
		public void AddTransition(string source, string target, string eventName,
            Predicate<TData> predicate)
		{
			if(!states.ContainsKey(source)) {
				throw new ArgumentException("No such source state found");
			}
			
			if(!states.ContainsKey(target)) {
				throw new ArgumentException("No such target state found");
			}
			
			states[source].AddTransition(new Transition<TData>(eventName, states[target], predicate));
		}
		
        /// <summary>
        /// Reset this FSM to the start state.
        /// </summary>
		public void Reset()
		{
			if(startState == null) {
				throw new InvalidOperationException("No start state set!");
			}
			
			currentState = startState;
		}
		
        /// <summary>
        /// Trigger an event. If a transition exists for this event name in
        /// the current state, the state will change to the transition's
        /// target state.
        /// </summary>
        /// <param name="eventName">The event name.</param>
		public void TriggerEvent(string eventName)
		{
			if(currentState == null) {
				throw new InvalidOperationException("No current (or start?) state set!");
			}

            State<TData> newState = currentState.TriggerEvent(eventName, data);
			if(newState != null) {
				currentState = newState;
			}
		}
	}
}
