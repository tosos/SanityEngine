using UnityEngine;
using System.Collections;

public abstract class DecisionMaker : MonoBehaviour {
	public abstract UnityNode GetMovementTarget(); 
}
