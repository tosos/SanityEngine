using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;
using SanityEngine.Actors;
using SanityEngine.Movement.SteeringBehaviors;
using SanityEngine.Movement.SteeringBehaviors.Flocking;

public class NewSteeringBehavior : EditorWindow {
	public SteeringBehaviorAsset asset;
	
	List<System.Type> classes;
	List<string> names;
	int chosenType = -1;
	float weight = 1.0f;
	bool enabled = true;
		
	public NewSteeringBehavior()
	{
		classes = new List<System.Type>();
		names = new List<string>();
		
		System.Type behaviorType = typeof(SteeringBehavior);
		Assembly asm = behaviorType.Assembly;
    	System.Type[] types = asm.GetTypes();
	    foreach(System.Type type in types) {
    	    if(type.IsSubclassOf(typeof(FlockingBehavior))) {
    	    	continue;
    	    }
    	    
    	    if(!type.IsSubclassOf(behaviorType) || type.IsAbstract) {
        	  	continue;
	        }
	        
	        classes.Add(type);
	        names.Add(type.Name);
    	}
	}
	
	void OnGUI()
	{
		chosenType = EditorGUILayout.Popup("Type", chosenType, names.ToArray());
		weight = EditorGUILayout.FloatField("Weight", weight);
		enabled = EditorGUILayout.Toggle("Initially Enabled", enabled);
		EditorGUILayout.Space();
		if(GUILayout.Button("Create")) {
			CreateBehavior();
		}
	}
	
	void CreateBehavior()
	{
		System.Type type = classes[chosenType];

		SteeringBehaviorAsset.BehaviorDef behavior =
			new SteeringBehaviorAsset.BehaviorDef();
		behavior.type = type.FullName;	
		behavior.weight = weight;
		behavior.enabled = enabled;	 
        
        List<SteeringBehaviorAsset.LinkedProperty> props =
            new List<SteeringBehaviorAsset.LinkedProperty>();
        object impl = System.Activator.CreateInstance(type);
	    PropertyInfo[] properties = type.GetProperties(
	    	BindingFlags.Instance | BindingFlags.Public);
    	foreach(PropertyInfo prop in properties) {
    		if(prop.Name == "Weight" || prop.Name == "Enabled") {
    			continue;
    		}
    		
    	    if(!prop.CanWrite) {
    	    	continue;
            }
            
            object def = null;
			SteeringBehaviorAsset.LinkedProperty linkedProp
				= new SteeringBehaviorAsset.LinkedProperty();
			linkedProp.name = prop.Name;
			linkedProp.type = GetPropertyType(prop.PropertyType, out def);
          	if(prop.CanRead) {
               	def = prop.GetValue(impl, null);
	        }
			linkedProp.defaultValue = def;
			props.Add(linkedProp);
   		}
   		behavior.properties = props.ToArray();
   		asset.AddBehavior(behavior);
   		Close();
	}
	
	SteeringBehaviorAsset.PropertyType GetPropertyType(System.Type type,
		out object def)
	{
		if(typeof(Actor).IsAssignableFrom(type)) {
			def = null;
			return SteeringBehaviorAsset.PropertyType.ACTOR;
		}
		if(type == typeof(bool)) {
			def = new System.Boolean();
			return SteeringBehaviorAsset.PropertyType.BOOL;
		}
		if(type == typeof(int)) {
			def = new System.Int32();
			return SteeringBehaviorAsset.PropertyType.INT;
		}
		if(type == typeof(float)) {
			def = new System.Single();
			return SteeringBehaviorAsset.PropertyType.FLOAT;
		}
		if(type == typeof(Vector2)) {
			def = new Vector2();
			return SteeringBehaviorAsset.PropertyType.VECTOR2;
		}
		if(type == typeof(Vector3)) {
			def = new Vector3();
			return SteeringBehaviorAsset.PropertyType.VECTOR3;
		}
		if(type == typeof(Vector4)) {
			def = new Vector4();
			return SteeringBehaviorAsset.PropertyType.VECTOR4;
		}
		if(type == typeof(string)) {
			def = "";
			return SteeringBehaviorAsset.PropertyType.STRING;
		}
		throw new System.ArgumentException("Unsupported type");
	}
}
