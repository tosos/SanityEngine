using System;
using System.Collections.Generic;

namespace FSM
{
    /// <summary>
    /// A state in a finite state machine.
    /// </summary>
    /// <typeparam name="TData">The data type used for transition guard checks.</typeparam>
	public class State<TData>
	{
		readonly string name;
		Dictionary<string, List<Transition<TData>>> transitions = new Dictionary<string, List<Transition<TData>>>();
		
        /// <summary>
        /// The name of the state.
        /// </summary>
		public string Name
		{
			get { return name; }
		}
		
		internal State (string name)
		{
			this.name = name;
		}
		
		internal void AddTransition(Transition<TData> transition)
		{
			List<Transition<TData>> ts = null;
			if(!transitions.ContainsKey(transition.EventName)) {
				ts = new List<Transition<TData>>();
				transitions[transition.EventName] = ts;
			} else {
				ts = transitions[transition.EventName];
			}
			ts.Add(transition);
		}
		
		internal State<TData> TriggerEvent(string eventName, TData data)
		{
			if(!transitions.ContainsKey(eventName)) {
				return null;
			}

            foreach (Transition<TData> transition in transitions[eventName])
            {
				if(transition.Check(data)) {
					return transition.Target;
				}
			}
			return null;
		}
	}
}
