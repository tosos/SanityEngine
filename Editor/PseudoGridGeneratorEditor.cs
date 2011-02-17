using UnityEngine;
using UnityEditor;
using System.Collections;

//[CustomEditor(typeof(PseudoGridGenerator))]
public class PseudoGridGeneratorEditor {// : Editor {
#if false
	public bool edgeFoldout = true;
	
	public override void OnInspectorGUI()
	{
		PseudoGridGenerator comp = (PseudoGridGenerator)target;
		comp.samplingType = (PseudoGridGenerator.SamplerType)EditorGUILayout.EnumPopup(
			"Sampling Type", comp.samplingType);
		comp.gridType = (PseudoGridGenerator.GridType)
			EditorGUILayout.EnumPopup("Grid Type", comp.gridType);
		comp.xRes = EditorGUILayout.IntField("X Resolution", comp.xRes);
		comp.yRes = EditorGUILayout.IntField("Y Resolution", comp.yRes);
		comp.maxHeight = EditorGUILayout.FloatField("Max Cell Height",
			comp.maxHeight);
		
		// Edge parameters
		edgeFoldout = EditorGUILayout.Foldout(edgeFoldout, "Edges");
		if(edgeFoldout) {
			EditorGUI.indentLevel = 1;
			
			comp.maxSlope = EditorGUILayout.FloatField("Max Slope",
				comp.maxSlope);
			comp.edgeRaycast = EditorGUILayout.Toggle("Edge Raycast",
				comp.edgeRaycast);
			comp.edgeCostAlgorithm = (PseudoGridGenerator.EdgeCostAlgorithm)
				EditorGUILayout.EnumPopup("Edge Cost Algorithm",
				comp.edgeCostAlgorithm);
			
			EditorGUI.indentLevel = 0;
		}
		comp.noHitNoCell = EditorGUILayout.Toggle("No hit no cell",
			comp.noHitNoCell);
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
		comp.drawGrid = EditorGUILayout.Toggle("Draw Grid",
			comp.drawGrid);
		if(GUI.changed) {
			EditorUtility.SetDirty(target);
		}
	}
#endif
}