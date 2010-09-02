using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;

[CustomEditor(typeof(SteeringBehaviorAsset))]
public class SteeringAssetEditor : Editor {	
	private bool showBehaviors = true;
	private bool showProperties = true;
	
	public override void OnInspectorGUI()
	{
		SteeringBehaviorAsset sab = (SteeringBehaviorAsset)target;
		EditorGUILayout.BeginVertical();

		showProperties = EditorGUILayout.Foldout(showProperties,
			"Properties");
		if(showProperties) {
			EditorGUI.indentLevel++;
			foreach(SteeringBehaviorAsset.SteeringProperty prop
				in sab.properties)
			{
				EditorGUILayout.LabelField("Name", prop.name);
				
				EditorGUILayout.Separator();
			}
			EditorGUI.indentLevel--;
		}
		showBehaviors = EditorGUILayout.Foldout(showBehaviors,
			"Behavior Definitions");
		if(showBehaviors) {
			EditorGUI.indentLevel++;
			int idx = 0;
			foreach(SteeringBehaviorAsset.BehaviorDef def in sab.behaviors) {
				EditorGUILayout.LabelField("Type", def.type);
				def.weight = EditorGUILayout.FloatField("Weight", def.weight);
				def.enabled = EditorGUILayout.Toggle("Enabled", def.enabled);
				EditorGUI.indentLevel++;
				foreach(SteeringBehaviorAsset.LinkedProperty prop in
					def.properties)
				{
					EditorGUI.indentLevel++;
					EditorGUILayout.LabelField("Name", prop.name);
					EditorGUILayout.LabelField("Type", prop.type.ToString());
					prop.defaultValue = DefaultField(
						prop.defaultValue, prop.type);
					List<string> existingProps = new List<string>();
					foreach(SteeringBehaviorAsset.SteeringProperty p2
						in sab.properties)
					{
						if(p2.type == prop.type) {
							existingProps.Add(p2.name);
						}
					}
					existingProps.Add("-- Add New --");
					string[] linkNames = existingProps.ToArray();
					int link = existingProps.IndexOf(prop.link);
					link = EditorGUILayout.Popup("Link", link, linkNames);
					if(link >= 0 && link < linkNames.Length - 1) {
						prop.link = linkNames[link];
					} else if(link == linkNames.Length - 1) {
						prop.link = NewLinkProperty(sab, prop);
					}
					EditorGUILayout.Separator();
					EditorGUI.indentLevel--;
				}
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("");
				if(GUILayout.Button("Delete")) {
					sab.RemoveBehavior(idx);
					sab.properties = new SteeringBehaviorAsset.LinkedProperty[0];
				}
				EditorGUILayout.EndHorizontal();				

				EditorGUILayout.Separator();
				
				EditorGUI.indentLevel--;
				idx ++;
			}
			EditorGUI.indentLevel--;
		}
		if(GUILayout.Button("Add Behavior")) {
			NewSteeringBehavior win = (NewSteeringBehavior)
				EditorWindow.GetWindow(typeof(NewSteeringBehavior));
			win.asset = sab;
		}
		EditorGUILayout.EndVertical();		
		
		if(GUI.changed) {
			sab.RemoveUnused();
			EditorUtility.SetDirty(target);
		}		
	}
	
	private object DefaultField(object val, SteeringBehaviorAsset.PropertyType t)
	{
		if(val == null) {
			return null;
		}
		object result = null;
		switch(t) {
		case SteeringBehaviorAsset.PropertyType.BOOL:
			bool vb = (bool)val;
			result = EditorGUILayout.Toggle("Enabled", vb);
			break;
		case SteeringBehaviorAsset.PropertyType.INT:
			int vi = (int)val;
			result = EditorGUILayout.IntField("Default Value", vi);
			break;
		case SteeringBehaviorAsset.PropertyType.FLOAT:
			float vf = (float)val;
			result = EditorGUILayout.FloatField("Default Value", vf);
			break;
		case SteeringBehaviorAsset.PropertyType.STRING:
			string vs = (string)val;
			result = EditorGUILayout.TextField("Default Value", vs);
			break;
		case SteeringBehaviorAsset.PropertyType.VECTOR2:
			Vector2 v2 = (Vector2)val;
			result = EditorGUILayout.Vector2Field("Default Value", v2);
			break;
		case SteeringBehaviorAsset.PropertyType.VECTOR3:
			Vector3 v3 = (Vector3)val;
			result = EditorGUILayout.Vector3Field("Default Value", v3);
			break;
		case SteeringBehaviorAsset.PropertyType.VECTOR4:
			Vector4 v4 = (Vector4)val;
			result = EditorGUILayout.Vector4Field("Default Value", v4);
			break;
		}
		return result;
	}
	
	string NewLinkProperty(SteeringBehaviorAsset sab,
		SteeringBehaviorAsset.SteeringProperty prop)
	{
		SteeringBehaviorAsset.SteeringProperty newProp = 
			new SteeringBehaviorAsset.SteeringProperty();
		newProp.name = GetUniqueLinkedPropName(sab, prop.name);
		newProp.type = prop.type;
		sab.AddProperty(newProp);
		return newProp.name;
	}
	
	string GetUniqueLinkedPropName(SteeringBehaviorAsset sab, string name)
	{
		List<string> names = new List<string>();
		foreach(SteeringBehaviorAsset.SteeringProperty prop in sab.properties)
		{
			names.Add(prop.name);
		}
		names.Sort();
		int i = 1;
		string newName = name;
		while(names.BinarySearch(newName) >= 0) {
			newName = name + (i++);
		}
		return newName;
	}
}
