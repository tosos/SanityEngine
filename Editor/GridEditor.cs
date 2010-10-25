using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Grid))]
public class GridEditor : Editor {
	public bool edgeFoldout = true;
	
	public override void OnInspectorGUI()
	{
		Grid comp = (Grid)target;
		SerializedObject serObj = new SerializedObject(target);
		SerializedProperty maskProp = serObj.FindProperty("layerMask");
		comp.samplingType = (Grid.SamplerType)EditorGUILayout.EnumPopup(
			"Sampling Type", comp.samplingType);
		comp.gridType = (Grid.GridType)
			EditorGUILayout.EnumPopup("Grid Type", comp.gridType);
		comp.size = EditorGUILayout.Vector3Field("Size", comp.size);
		comp.xRes = EditorGUILayout.IntField("X Resolution", comp.xRes);
		comp.yRes = EditorGUILayout.IntField("Y Resolution", comp.yRes);
		EditorGUILayout.PropertyField(maskProp);
		serObj.ApplyModifiedProperties();
		
		// Edge parameters
		edgeFoldout = EditorGUILayout.Foldout(edgeFoldout, "Edges");
		if(edgeFoldout) {
			EditorGUI.indentLevel = 1;
			
			comp.maxEdgeHeightChange = EditorGUILayout.FloatField("Max Height Change",
				comp.maxEdgeHeightChange);
			comp.edgeRaycast = EditorGUILayout.Toggle("Edge Raycast",
				comp.edgeRaycast);
			comp.edgeCostAlgorithm = (Grid.EdgeCostAlgorithm)
				EditorGUILayout.EnumPopup("Edge Cost Algorithm",
				comp.edgeCostAlgorithm);
			
			EditorGUI.indentLevel = 0;
		}
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
		
		int x, y;
		Grid.GridCell cell = comp.GetSelectedCell(out x, out y);
		if(cell != null) {
			EditorGUILayout.Separator();
			EditorGUILayout.BeginVertical("box");
			EditorGUILayout.LabelField("Cell", "(" + x +"," + y + ")");
			comp.SetCellHeight(x, y, EditorGUILayout.FloatField("Height", cell.height));
			EditorGUI.indentLevel ++;
			foreach(Grid.GridEdge edge in cell.edges) {
				EditorGUILayout.LabelField("Target", "(" + edge.targetX +"," + edge.targetY + ")");
				edge.cost = EditorGUILayout.FloatField("Cost", edge.cost);
				if(edge.cost < 0f) {
					edge.cost = 0f;
				}
				EditorGUILayout.Separator();
			}
			EditorGUI.indentLevel --;
			EditorGUILayout.EndVertical();
		}
		
		if(GUI.changed) {
			EditorUtility.SetDirty(target);
		}
	}
	
	void OnSceneGUI()
	{
		Grid comp = (Grid)target;
		
		if(comp.IsInitialized) {
			Vector3 cellSize = comp.GetCellSize();
			float size = Mathf.Min(cellSize.x, Mathf.Min(cellSize.y, cellSize.z)) * 0.5f;
			for (int y = 0; y < comp.yRes; y++) {
				for (int x = 0; x < comp.xRes; x++) {
					Vector3 pos = comp.GetWorldCellPosition(x, y);
					if(comp.IsSelected(x, y)) {
						Handles.color = new Color(1.0f, 0.5f, 0.0f);
						Vector3 newPos = Handles.Slider(pos, comp.transform.up);
						float heightAdj = Vector3.Dot(comp.transform.up, newPos - pos);
						comp.SetCellHeight(x, y, comp.GetCellHeight(x, y) + heightAdj);
						Handles.color = new Color(0.0f, 1.0f, 1.0f, 0.1f);
					} else {
						Handles.color = new Color(1.0f, 1.0f, 1.0f, 0.1f);
					}
					if(Handles.Button(pos, comp.transform.rotation, size, size, Handles.CubeCap))
					{
						comp.SetSelected(x, y);
					}
				}
			}
		}
	}
}