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
		public string defaultValue;
		public string link;
	}
	
	[System.Serializable]
	public class BehaviorDef
	{
		public string type;
		public string name;
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
		string name = props[idx].name;
		props.RemoveAt(idx);
		properties = props.ToArray();
		
		foreach(BehaviorDef def in behaviors) {
			foreach(LinkedProperty prop in def.properties) {
				if(name.Equals(prop.link)) {
					prop.link = "";
				}
			}
		}
	}
	
	public void RemoveUnused()
	{
		List<string> remove = new List<string>();
		for(int i = 0; i < properties.Length; i ++)
		{
			string name = properties[i].name;
			if(CountUsed(name) <= 0) {
				remove.Add(name);
			}
		}
		foreach(string name in remove) {
			RemoveProperty(name);
		}
	}
	
	public void RenameProperty(string oldName, string newName)
	{
		for(int i = 0; i < properties.Length; i ++)
		{
			if(oldName.Equals(properties[i].name)) {
				properties[i].name = newName;
			}
		}		
		foreach(BehaviorDef def in behaviors) {
			foreach(LinkedProperty prop in def.properties) {
				if(prop.link.Equals(oldName)) {
					prop.link = newName;
				}
			}
		}
	}
	
	public int CountUsed(string name)
	{
		int count = 0;
		foreach(BehaviorDef def in behaviors) {
			foreach(LinkedProperty prop in def.properties) {
				if(prop.link != null && prop.link.Equals(name)) {
					count ++;
				}
			}
		}
		return count;
	}
	
	public static Vector2 ParseVector2(string val)
	{
		string[] vals = val.Substring(1, val.Length - 2).Split(
			new char[]{','}, System.StringSplitOptions.None);
		float x = System.Single.Parse(vals[0]);
		float y = System.Single.Parse(vals[1]);
		return new Vector2(x, y);
	}

	public static Vector3 ParseVector3(string val)
	{
		string[] vals = val.Substring(1, val.Length - 2).Split(
			new char[]{','}, System.StringSplitOptions.None);
		float x = System.Single.Parse(vals[0]);
		float y = System.Single.Parse(vals[1]);
		float z = System.Single.Parse(vals[2]);
		return new Vector3(x, y, z);
	}

	public static Vector4 ParseVector4(string val)
	{
		string[] vals = val.Substring(1, val.Length - 2).Split(
			new char[]{','}, System.StringSplitOptions.None);
		float x = System.Single.Parse(vals[0]);
		float y = System.Single.Parse(vals[1]);
		float z = System.Single.Parse(vals[2]);
		float w = System.Single.Parse(vals[3]);
		return new Vector4(x, y, z, w);
	}
}
