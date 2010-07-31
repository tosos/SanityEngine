using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(GridGenerator))]
public class GridGeneratorEditor : Editor {
	public bool edgeFoldout = true;
	
	public override void OnInspectorGUI()
	{
		GridGenerator comp = (GridGenerator)target;
		comp.samplingType = (GridGenerator.SamplerType)EditorGUILayout.EnumPopup(
			"Sampling Type", comp.samplingType);
		comp.gridType = (GridGenerator.GridType)
			EditorGUILayout.EnumPopup("Grid Type", comp.gridType);
		
		// Edge parameters
		edgeFoldout = EditorGUILayout.Foldout(edgeFoldout, "Edges");
		if(edgeFoldout) {
			EditorGUI.indentLevel = 1;
			
			comp.maxSlope = EditorGUILayout.FloatField("Max Slope",
				comp.maxSlope);
			comp.edgeRaycast = EditorGUILayout.Toggle("Edge Raycast",
				comp.edgeRaycast);
			comp.edgeCostAlgorithm = (GridGenerator.EdgeCostAlgorithm)
				EditorGUILayout.EnumPopup("Edge Cost Algorithm",
				comp.edgeCostAlgorithm);
			
			EditorGUI.indentLevel = 0;
		}
		comp.noHitNoCell = EditorGUILayout.Toggle("No hit no cell", comp.noHitNoCell);
		if(GUILayout.Button("Generate")) {
			foreach(Collider obj in FindObjectsOfType(typeof(Collider)))
			{
				obj.isTrigger = !obj.isTrigger;
				obj.isTrigger = !obj.isTrigger;
				EditorUtility.SetDirty(obj);
			}
			EditorUtility.SetDirty(target);
			comp.FindCells();
		}
		if(GUI.changed) {
			EditorUtility.SetDirty(target);
		}
	}
}