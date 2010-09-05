using UnityEngine;
using UnityEditor;

public class SteeringAssetCreator
{
	[MenuItem ("Assets/Create/Sanity Engine/Steering Behavior Asset")]
	public static void CreateWizard () {
		SteeringBehaviorAsset behavior = new SteeringBehaviorAsset();
		string path = AssetDatabase.GenerateUniqueAssetPath(
			"Assets/new steering behavior.asset");
		AssetDatabase.CreateAsset(behavior, path);
	}
}
