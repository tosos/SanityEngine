using UnityEngine;
using UnityEditor;

public class SteeringAssetCreator
{
	[MenuItem ("Assets/Create/Sanity Engine/Steering Behavior Asset")]
	public static void CreateWizard () {
		SteeringBehaviorAsset behavior = ScriptableObject.CreateInstance<SteeringBehaviorAsset>();
		string path = AssetDatabase.GenerateUniqueAssetPath(
			"Assets/new steering behavior.asset");
		AssetDatabase.CreateAsset(behavior, path);
		AssetDatabase.SetLabels(behavior, new string[] { "sanity", "steering" });
	}
}
