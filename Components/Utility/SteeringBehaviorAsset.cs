using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SteeringBehaviorAsset : ScriptableObject {
	public enum PropertyType
	{
		BOOL,
		INT,
		FLOAT,
		VECTOR2,
		VECTOR3,
		VECTOR4,
		STRING,
		ACTOR
	}
		
	[System.Serializable]
	public class SteeringProperty
	{
		public string name;
		public PropertyType type;
	}
	
	[System.Serializable]
	public class LinkedProperty : SteeringProperty
	{
		public object defaultValue;
		public string link;
	}
	
	[System.Serializable]
	public class BehaviorDef
	{
		public string type;
		public float weight = 1.0f;
		public bool enabled = true;
		public LinkedProperty[] properties;
		
	}
		
	public SteeringProperty[] properties;	
	public BehaviorDef[] behaviors;
	
	public void AddBehavior(BehaviorDef behavior)
	{
		List<BehaviorDef> newBehaviors = new List<BehaviorDef>(behaviors);
		newBehaviors.Add(behavior);
		behaviors = newBehaviors.ToArray();
	}
	
	public void RemoveBehavior(int idx)
	{
		List<BehaviorDef> newBehaviors = new List<BehaviorDef>(behaviors);
		newBehaviors.RemoveAt(idx);
		behaviors = newBehaviors.ToArray();
	}
	
	public SteeringBehaviorAsset()
	{
		behaviors = new BehaviorDef[0];
		properties = new SteeringProperty[0];
	}
	
	public void AddProperty(SteeringProperty prop)
	{
		List<SteeringProperty> props = new List<SteeringProperty>(properties);
		props.Add(prop);
		properties = props.ToArray();
	}
	
	public void RemoveProperty(string name)
	{
		for(int i = 0; i < properties.Length; i ++)
		{
			if(properties[i].name.Equals(name)) {
				RemoveProperty(i);
				break;
			}
		}
	}

	public void RemoveProperty(int idx)
	{
		List<SteeringProperty> props = new List<SteeringProperty>(properties);
		props.RemoveAt(idx);
		properties = props.ToArray();
	}
	
	public void RemoveUnused()
	{
		List<string> remove = new List<string>();
		for(int i = 0; i < properties.Length; i ++)
		{
			string name = properties[i].name;
			int count = 0;
			foreach(BehaviorDef def in behaviors) {
				foreach(LinkedProperty prop in def.properties) {
					if(prop.link.Equals(name)) {
						count ++;
					}
				}
			}
			if(count <= 0) {
				remove.Add(name);
			}
		}
		foreach(string name in remove) {
			RemoveProperty(name);
		}
	}
}
