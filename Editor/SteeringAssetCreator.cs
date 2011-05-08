using UnityEngine;
using UnityEditor;
using System.IO;

public class SteeringAssetCreator
{
	[MenuItem ("Assets/Create/Sanity Engine/Steering Behavior Asset")]
	public static void CreateWizard () {
		SteeringBehaviorAsset behavior = ScriptableObject.CreateInstance<SteeringBehaviorAsset>();
		string path = AssetDatabase.GetAssetPath (Selection.activeObject);
		if (path == "") {
			path = "Assets";
		} else if (Path.GetExtension(path) != "") {
			path = path.Replace(Path.GetFileName (AssetDatabase.GetAssetPath (Selection.activeObject)), "");
		}
		string assetPath = AssetDatabase.GenerateUniqueAssetPath(path + "/New Steering Behavior.asset");
		AssetDatabase.CreateAsset(behavior, assetPath);
		AssetDatabase.SetLabels(behavior, new string[] { "sanity", "steering" });
	}
}
