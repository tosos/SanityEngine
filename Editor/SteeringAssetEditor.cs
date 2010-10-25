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
		EditorGUIUtility.LookLikeInspector();
		
		SteeringBehaviorAsset sab = (SteeringBehaviorAsset)target;
		EditorGUILayout.BeginVertical();

		showProperties = EditorGUILayout.Foldout(showProperties,
			"Properties");
		if(showProperties) {
			EditorGUI.indentLevel++;
			int idx = 0;
			foreach(SteeringBehaviorAsset.SteeringProperty prop
				in sab.properties)
			{
				EditorGUILayout.BeginVertical("Box");
				string newName = EditorGUILayout.TextField("Name", prop.name);
				if(!newName.Equals(prop.name)) {
					sab.RenameProperty(prop.name,
						GetUniqueLinkedPropName(sab, newName));
				}
				EditorGUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				int count = sab.CountUsed(prop.name);
				GUILayout.Label(count + " Link" + (count == 1 ? "" : "s"),
					EditorStyles.miniLabel);
				EditorGUILayout.Space();
				if(GUILayout.Button("Delete", EditorStyles.miniButton)) {
					sab.RemoveProperty(idx);
				}
				EditorGUILayout.EndHorizontal();				
				
				idx ++;
				EditorGUILayout.EndVertical();
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
				EditorGUILayout.BeginVertical("Box");
				EditorGUILayout.LabelField("Type", def.name);
				def.weight = EditorGUILayout.FloatField("Weight", def.weight);
				def.enabled = EditorGUILayout.Toggle("Enabled", def.enabled);
				EditorGUI.indentLevel++;
				foreach(SteeringBehaviorAsset.LinkedProperty prop in
					def.properties)
				{
					EditorGUILayout.BeginVertical("Box");
					EditorGUI.indentLevel++;
					EditorGUILayout.LabelField("Built-in Property Name",
						prop.name);
					EditorGUILayout.LabelField("Type", prop.type.ToString());
					prop.defaultValue = DefaultField(
						prop.defaultValue, prop.type);
					List<string> existingProps = new List<string>();
					existingProps.Add("(None)");
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
					if(link < 0) {
						link = 0;
					}
					link = EditorGUILayout.Popup("Linked To", link, linkNames);
					if(link >= 1 && link < linkNames.Length - 1) {
						prop.link = linkNames[link];
					} else if(link == linkNames.Length - 1) {
						prop.link = NewLinkProperty(sab, prop);
					} else {
						prop.link = "";
					}
					EditorGUILayout.EndVertical();
					EditorGUILayout.Separator();
					EditorGUI.indentLevel--;
				}
				EditorGUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				if(GUILayout.Button("Delete", EditorStyles.miniButton)) {
					sab.RemoveBehavior(idx);
					sab.RemoveUnused();
				}
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.EndVertical();

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
	
	private string DefaultField(string val,
		SteeringBehaviorAsset.PropertyType t)
	{
		if(val == null) {
			return null;
		}
		object result = null;
		switch(t) {
		case SteeringBehaviorAsset.PropertyType.BOOL:
			bool vb = System.Boolean.Parse(val);
			result = EditorGUILayout.Toggle("Enabled", vb);
			break;
		case SteeringBehaviorAsset.PropertyType.INT:
			int vi = System.Int32.Parse(val);
			result = EditorGUILayout.IntField("Default Value", vi);
			break;
		case SteeringBehaviorAsset.PropertyType.FLOAT:
			float vf = System.Single.Parse(val);
			result = EditorGUILayout.FloatField("Default Value", vf);
			break;
		case SteeringBehaviorAsset.PropertyType.STRING:
			result = EditorGUILayout.TextField("Default Value", val);
			break;
		case SteeringBehaviorAsset.PropertyType.VECTOR2:
			Vector2 v2 = ParseVector2(val);
			result = EditorGUILayout.Vector2Field("Default Value", v2);
			break;
		case SteeringBehaviorAsset.PropertyType.VECTOR3:
			Vector3 v3 = ParseVector3(val);
			result = EditorGUILayout.Vector3Field("Default Value", v3);
			break;
		case SteeringBehaviorAsset.PropertyType.VECTOR4:
			Vector4 v4 = ParseVector4(val);
			result = EditorGUILayout.Vector4Field("Default Value", v4);
			break;
		}
		return result == null ? null : result.ToString();
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
	
	Vector2 ParseVector2(string val)
	{
		string[] vals = val.Substring(1, val.Length - 2).Split(
			new char[]{','}, System.StringSplitOptions.None);
		float x = System.Single.Parse(vals[0]);
		float y = System.Single.Parse(vals[1]);
		return new Vector2(x, y);
	}

	Vector3 ParseVector3(string val)
	{
		string[] vals = val.Substring(1, val.Length - 2).Split(
			new char[]{','}, System.StringSplitOptions.None);
		float x = System.Single.Parse(vals[0]);
		float y = System.Single.Parse(vals[1]);
		float z = System.Single.Parse(vals[2]);
		return new Vector3(x, y, z);
	}

	Vector4 ParseVector4(string val)
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
