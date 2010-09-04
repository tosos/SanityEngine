using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using SanityEngine.Actors;
using SanityEngine.Movement.SteeringBehaviors;
using SanityEngine.Movement.SteeringBehaviors.Flocking;

public class SteeringBehaviorProxy
{
	class PropProxy
	{
		public readonly PropertyInfo property;
		public readonly SteeringBehavior behavior;
		
		public PropProxy(PropertyInfo prop, SteeringBehavior behavior)
		{
			this.property = prop;
			this.behavior = behavior;
		}
	}
	public readonly SteeringBehavior[] behaviors;
	
	float[] baseWeights;
	Dictionary<string, List<PropProxy>> properties =
		new Dictionary<string, List<PropProxy>>();
	
	public SteeringBehaviorProxy(SteeringBehaviorAsset asset)
	{
		Assembly assm = typeof(SteeringBehavior).Assembly;
		List<SteeringBehavior> behaviors = new List<SteeringBehavior>();
		foreach(SteeringBehaviorAsset.BehaviorDef def in asset.behaviors) {
			System.Type type = assm.GetType(def.type);
    	    SteeringBehavior behavior = (SteeringBehavior)
    	    	System.Activator.CreateInstance(type);
    	    behavior.Weight = def.weight;
    	    behavior.Enabled = def.enabled;
    	    behaviors.Add(behavior);
	        foreach(SteeringBehaviorAsset.SteeringProperty prop
	        	in asset.properties)
    	    {
    	    	List<PropProxy> props = null;
    	    	if(properties.ContainsKey(prop.name)) {
    	    		props = properties[prop.name];
    	    	} else {
    	    		props = new List<PropProxy>();
    	    		properties[prop.name] = props;
    	    	}
        		props.Add(new PropProxy(type.GetProperty(prop.name),
        			behavior));
        	}
		}
		this.behaviors = behaviors.ToArray();
		this.baseWeights = new float[this.behaviors.Length];
		for(int i = 0; i < baseWeights.Length; i ++) {
			baseWeights[i] = this.behaviors[i].Weight;
		}
	}
	
	public void SetEnabled(bool enabled)
	{
		foreach(SteeringBehavior behavior in behaviors)
		{
			behavior.Enabled = enabled;
		}
	}

	public void SetEnabled(int idx, bool enabled)
	{
		behaviors[idx].Enabled = enabled;
	}

	public void SetWeightScale(float weightScale)
	{
		for(int i = 0; i < behaviors.Length; i ++) {
			behaviors[i].Weight = baseWeights[i] * weightScale;
		}
	}

	public void SetWeight(int idx, float weight)
	{
		behaviors[idx].Weight = baseWeights[idx] = weight;
	}
	
	public void SetBool(string name, bool val)
	{
		SetProperty(name, val);
	}

	public void SetInt(string name, int val)
	{
		SetProperty(name, val);
	}

	public void SetFloat(string name, float val)
	{
		SetProperty(name, val);
	}

	public void SetVector2(string name, Vector2 val)
	{
		SetProperty(name, val);
	}

	public void SetVector3(string name, Vector3 val)
	{
		SetProperty(name, val);
	}

	public void SetVector4(string name, Vector4 val)
	{
		SetProperty(name, val);
	}

	public void SetString(string name, string val)
	{
		SetProperty(name, val);
	}

	public void SetActor(string name, Actor val)
	{
		SetProperty(name, val);
	}
	
	void SetProperty(string name, object val)
	{
		List<PropProxy> props = properties[name];
		foreach(PropProxy prop in props) {
			prop.property.SetValue(prop.behavior, val, null);
		}
	}
}
