//
// Copyright (C) 2010 The Sanity Engine Development Team
//
// This source code is licensed under the terms of the
// MIT License.
//
// For more information, see the file LICENSE

using UnityEngine;
using System.Collections.Generic;
using SanityEngine.Structure.Graph;
using SanityEngine.Structure.Graph.NavMesh;

[AddComponentMenu("Sanity Engine/Level Representation/Grid"),
	ExecuteInEditMode()]
public class Grid : UnityGraph
{
	private delegate bool Sample (Vector3 pos, Vector3 dir, out Vector3 result);

	public enum SamplerType
	{
		CENTER,
		CORNERS_AVG,
		CORNERS_MIN,
		CORNERS_MAX,
		MONTE_CARLO
	}

	public enum EdgeCostAlgorithm
	{
		ONE_PLUS_POS_SLOPE,
		ONE,
		EUCLIDEAN_DISTANCE
	}

	public enum GridType
	{
		EIGHT_GRID,
		FOUR_GRID
	}

	[System.Serializable]
	public class GridEdge
	{
		public int targetX;
		public int targetY;
		public float cost;

		public GridEdge ()
		{
		}

		public GridEdge (int tx, int ty, float cost)
		{
			this.targetX = tx;
			this.targetY = ty;
			this.cost = cost;
		}
	}

	[System.Serializable]
	public class GridCell
	{
		public List<GridEdge> edges;
		public float height;

		public GridCell ()
		{
		}

		public GridCell (float height)
		{
			this.edges = new List<GridEdge> ();
			this.height = height;
		}
	}
	
	[System.Serializable]
	public class GridParameters
	{
		public float xSize = 1.0f;
		public float ySize = 1.0f;
		public float scanHeight = 5.0f;
		public int xDimension = 10;
		public int yDimension = 10;
		
		public void CopyTo(GridParameters other)
		{
			other.xSize = xSize;
			other.ySize = ySize;
			other.scanHeight = scanHeight;
			other.xDimension = xDimension;
			other.yDimension = yDimension;
		}
	}

	public SamplerType samplingType = SamplerType.MONTE_CARLO;
	public LayerMask layerMask = -1;
	public float maxEdgeHeightChange = Mathf.Infinity;
	public bool edgeRaycast = false;
	public EdgeCostAlgorithm edgeCostAlgorithm;
	public GridType gridType;
	public bool alwaysDrawGrid = true;
	public int selectedX = -1;
	public int selectedY = -1;
	public GridParameters newParameters;

	[SerializeField]
	GridCell[] cells;
	SimpleNode[,] nodes;
	GraphChangeHelper<UnityNode, UnityEdge> helper;
	[SerializeField]
	GridParameters parameters;

	// Use this for initialization
	void Start ()
	{
		if(cells == null) {
			return;
		}
		
		helper = new GraphChangeHelper<UnityNode, UnityEdge> ();
		nodes = new SimpleNode[parameters.yDimension, parameters.xDimension];
		for (int y = 0; y < parameters.yDimension; y++) {
			for (int x = 0; x < parameters.xDimension; x++) {
				nodes[y, x] = new SimpleNode(transform.TransformPoint(
					GetLocalCellPosition(x, y) + cells[GetIdx(x, y)].height
					* Vector3.up), this);
			}
		}
		
		for (int y = 0; y < parameters.yDimension; y++) {
			for (int x = 0; x < parameters.xDimension; x++) {
				GridCell cell = cells[GetIdx(x, y)];
				foreach(GridEdge edge in cell.edges) {
					SimpleNode src = nodes[y, x];
					SimpleNode tgt = nodes[edge.targetY, edge.targetX];
					SimpleEdge e = new SimpleEdge(src, tgt, edge.cost); 
					src.AddOutEdge(e);
					tgt.AddInEdge(e);
				}
			}
		}
	}

	// Update is called once per frame
	void Update ()
	{
		
	}

	void OnDrawGizmos ()
	{
		if (alwaysDrawGrid) {
			DrawGizmos ();
		}
	}

	void OnDrawGizmosSelected ()
	{
		if (!alwaysDrawGrid) {
			DrawGizmos ();
		}
	}
	
	int GetDirection(int x1, int y1, int x2, int y2)
	{
		if(x1 == x2) {
			if(y2 < y1) return 0;
			return 1;
		}
		if(y1 == y2) {
			if(x2 < x1) return 2;
			return 3;
		}
		if(x2 < x1) {
			if(y2 < y1) return 4;
			return 5;
		}
		if(y2 < y1) return 6;
		return 7;
	}

	void DrawGizmos ()
	{
		Vector3[,] offsets = new Vector3[8, 2] {
			{ new Vector3(-0.1f, 0.0f, -0.4f), new Vector3(-0.1f, 0.0f, 0.4f) },
			{ new Vector3(0.1f, 0.0f, 0.4f), new Vector3(0.1f, 0.0f, -0.4f) },
			{ new Vector3(-0.4f, 0.0f, 0.1f), new Vector3(0.4f, 0.0f, 0.1f) },
			{ new Vector3(0.4f, 0.0f, -0.1f), new Vector3(-0.4f, 0.0f, -0.1f) },
			{ new Vector3(-0.4f, 0.0f, -0.3f), new Vector3(0.3f, 0.0f, 0.4f) },
			{ new Vector3(-0.3f, 0.0f, 0.4f), new Vector3(0.4f, 0.0f, -0.3f) },
			{ new Vector3(0.3f, 0.0f, -0.4f), new Vector3(-0.4f, 0.0f, 0.3f) },
			{ new Vector3(0.4f, 0.0f, 0.3f), new Vector3(-0.3f, 0.0f, -0.4f) }
		};
		Vector3 pos = transform.position;
		Gizmos.matrix = Matrix4x4.TRS (pos, transform.rotation, Vector3.one);
		Gizmos.color = new Color(1.0f, 1.0f, 0.0f, 0.5f);
		Gizmos.DrawWireCube (Vector3.zero, new Vector3(parameters.xSize * parameters.xDimension,
			parameters.scanHeight, parameters.ySize * parameters.yDimension));
		
		if (cells != null) {
			Vector3 cellSize = GetCellSize();
			float radius = Mathf.Min(cellSize.x, Mathf.Min(cellSize.y, cellSize.z)) * 0.05f;
			
			for (int y = 0; y < parameters.yDimension; y++) {
				for (int x = 0; x < parameters.xDimension; x++) {
					GridCell cell = cells[GetIdx (x, y)];
					Vector3 cellPos = GetLocalCellPosition(x, y) + (cell.height + 0.5f) * Vector3.up;
					Gizmos.color = new Color(1.0f, 1.0f, 1.0f, 0.2f);
					Gizmos.DrawWireCube (cellPos, cellSize);
					
					if(cell.edges != null) {
						if(selectedX >= 0 && (selectedX != x || selectedY != y)) {
							continue;
						}
						foreach(GridEdge edge in cell.edges) {
							if(edge.cost == Mathf.Infinity) {
								Gizmos.color = Color.red;
							} else {
								Gizmos.color = Color.green;
							}
							int tx = edge.targetX;
							int ty = edge.targetY;
							int dir = GetDirection(x, y, tx, ty);
							GridCell tcell = cells[GetIdx(tx, ty)];
							Vector3 tCellPos = GetLocalCellPosition(tx, ty) + (tcell.height + 0.5f) * Vector3.up;
							Vector3 start = cellPos + Vector3.Scale(cellSize, offsets[dir,0]);
							Vector3 end = tCellPos + Vector3.Scale(cellSize, offsets[dir,1]);
							Gizmos.DrawLine(start, end);
							Gizmos.DrawSphere(end, radius);
						}
					}
				}
			}
		}
	}
	
	public void FindCells ()
	{
		newParameters.CopyTo(parameters);
		
		// Choose a sampler function
		Sample sampler = null;
		switch (samplingType) {
		case SamplerType.CENTER:
			sampler = SampleCenter;
			break;
		case SamplerType.CORNERS_AVG:
			sampler = SampleCornersAvg;
			break;
		case SamplerType.CORNERS_MIN:
			sampler = SampleCornersMin;
			break;
		case SamplerType.CORNERS_MAX:
			sampler = SampleCornersMax;
			break;
		case SamplerType.MONTE_CARLO:
			sampler = SampleMonteCarlo;
			break;
		}
		
		cells = new GridCell[parameters.xDimension * parameters.yDimension];
		Vector3 down = -transform.up;
		for (int y = 0; y < parameters.yDimension; y++) {
			for (int x = 0; x < parameters.xDimension; x++) {
				Vector3 rayPos = transform.TransformPoint (GetLocalCellPosition(x, y)
					+ Vector3.up * parameters.scanHeight);
				Vector3 pos;
				float height = 0f;
				if (sampler (rayPos, down * parameters.scanHeight, out pos)) {
					Vector3 localPos = transform.InverseTransformPoint (pos);
					height = localPos.y + parameters.scanHeight * 0.5f;
				}
				
				cells[GetIdx (x, y)] = new GridCell (height);
			}
		}
		for (int y = 0; y < parameters.yDimension; y++) {
			for (int x = 0; x < parameters.xDimension; x++) {
				RegenerateEdges(x, y);
			}
		}
	}
	
	public void RegenerateEdges(int x, int y)
	{
		int minY = y <= 0 ? 0 : y - 1;
		int maxY = y >= parameters.yDimension - 1 ? parameters.yDimension - 1 : y + 1;
		int minX = x <= 0 ? 0 : x - 1;
		int maxX = x >= parameters.xDimension - 1 ? parameters.xDimension - 1 : x + 1;
		
		GridCell src = cells[GetIdx(x, y)];
		for (int ey = minY; ey <= maxY; ey++) {
			for (int ex = minX; ex <= maxX; ex++) {
				if (ex == x && ey == y) {
					continue;
				}
				if(gridType == GridType.FOUR_GRID && !(y == ey || x == ex)) {
					continue;
				}
				GridCell tgt = cells[GetIdx(ex, ey)];
				float cost = GetEdgeCost (x, y, ex, ey);
				float change = tgt.height - src.height;
				if(change > maxEdgeHeightChange) {
					cost = Mathf.Infinity;
				}
				GridEdge edge = new GridEdge (ex, ey, cost);
				src.edges.Add (edge);
			}
		}
	}

	bool SampleCenter (Vector3 pos, Vector3 dir, out Vector3 result)
	{
		if (RaycastNearest (pos, dir, out result)) {
			return true;
		}
		result = pos + dir;
		return false;
	}

	bool SampleCornersAvg (Vector3 pos, Vector3 dir, out Vector3 result)
	{
		Vector3 r = transform.right * parameters.xSize * 0.5f;
		Vector3 f = transform.forward * parameters.ySize * 0.5f;
		
		Vector3 sum = Vector3.zero;
		
		int hits = 0;
		Vector3 hit;
		if (RaycastNearest (pos - r - f, dir, out hit)) {
			sum += hit - pos;
			hits++;
		}
		if (RaycastNearest (pos + r - f, dir, out hit)) {
			sum += hit - pos;
			hits++;
		}
		if (RaycastNearest (pos - r + f, dir, out hit)) {
			sum += hit - pos;
			hits++;
		}
		if (RaycastNearest (pos + r + f, dir, out hit)) {
			sum += hit - pos;
			hits++;
		}
		sum += dir * (4 - hits);
		float dist = (sum / 4f).magnitude / dir.magnitude;
		result = pos + dir * dist;
		return hits > 0;
	}

	bool SampleCornersMin (Vector3 pos, Vector3 dir, out Vector3 result)
	{
		Vector3 r = transform.right * parameters.xSize * 0.5f;
		Vector3 f = transform.forward * parameters.ySize * 0.5f;
		
		float minDist = Mathf.Infinity;
		float max = dir.magnitude;
		
		Vector3 hit;
		bool didHit = false;
		if (RaycastNearest (pos - r - f, dir, out hit)) {
			minDist = Mathf.Min (minDist, (hit - pos).magnitude);
			didHit = true;
		}
		if (RaycastNearest (pos + r - f, dir, out hit)) {
			minDist = Mathf.Min (minDist, (hit - pos).magnitude);
			didHit = true;
		}
		if (RaycastNearest (pos - r + f, dir, out hit)) {
			minDist = Mathf.Min (minDist, (hit - pos).magnitude);
			didHit = true;
		}
		if (RaycastNearest (pos + r + f, dir, out hit)) {
			minDist = Mathf.Min (minDist, (hit - pos).magnitude);
			didHit = true;
		}
		minDist = Mathf.Min (minDist, max);
		result = pos + dir * minDist / max;
		return didHit;
	}

	bool SampleCornersMax (Vector3 pos, Vector3 dir, out Vector3 result)
	{
		Vector3 r = transform.right * parameters.xSize * 0.5f;
		Vector3 f = transform.forward * parameters.ySize * 0.5f;
		
		bool wasHit = false;
		float maxDist = 0f;
		
		Vector3 hit;
		if (RaycastNearest (pos - r - f, dir, out hit)) {
			maxDist = Mathf.Max (maxDist, (hit - pos).magnitude);
			wasHit = true;
		}
		if (RaycastNearest (pos + r - f, dir, out hit)) {
			maxDist = Mathf.Max (maxDist, (hit - pos).magnitude);
			wasHit = true;
		}
		if (RaycastNearest (pos - r + f, dir, out hit)) {
			maxDist = Mathf.Max (maxDist, (hit - pos).magnitude);
			wasHit = true;
		}
		if (RaycastNearest (pos + r + f, dir, out hit)) {
			maxDist = Mathf.Max (maxDist, (hit - pos).magnitude);
			wasHit = true;
		}
		if (!wasHit) {
			maxDist = dir.magnitude;
		}
		result = pos + dir * maxDist / dir.magnitude;
		return wasHit;
	}

	bool SampleMonteCarlo (Vector3 pos, Vector3 dir, out Vector3 result)
	{
		Vector3 r = transform.right * parameters.xSize * 0.5f;
		Vector3 f = transform.forward * parameters.ySize * 0.5f;
		
		const int numRays = 9;
		Vector3 sum = Vector3.zero;
		int hits = 0;
		for (int i = 0; i < numRays; i++) {
			Vector3 p = pos + r * Random.Range (-1f, 1f) + f * Random.Range (-1f, 1f);
			Vector3 hit;
			if (RaycastNearest (p, dir, out hit)) {
				sum += hit - pos;
				hits++;
			}
		}
		sum += dir * (numRays - hits);
		float dist = (sum / numRays).magnitude / dir.magnitude;
		result = pos + dir * dist;
		return hits > 0;
	}

	bool RaycastNearest (Vector3 pos, Vector3 dir, out Vector3 point)
	{
		RaycastHit[] hits = Physics.RaycastAll (pos, dir, dir.magnitude, layerMask.value);
		float minDist = Mathf.Infinity;
		point = Vector3.zero;
		foreach (RaycastHit hit in hits) {
			float dist = hit.distance;
			if (dist < minDist) {
				minDist = dist;
				point = hit.point;
			}
		}
		return hits.Length != 0;
	}

	Vector3 GetLocalCellPosition (int x, int y)
	{
		float cw = parameters.xSize;
		float ch = parameters.ySize;
		float w = parameters.xSize * parameters.xDimension;
		float h = parameters.ySize * parameters.yDimension;
		return new Vector3 (x * cw - (w - cw) * 0.5f, -parameters.scanHeight * 0.5f,
			y * ch - (h - ch) * 0.5f);
	}
	
	public bool IsInitialized
	{
		get { return cells != null; }
	}
	
	public Vector3 GetWorldCellPosition(int x, int y)
	{
		GridCell cell = cells[GetIdx(x, y)];
		return transform.TransformPoint(GetLocalCellPosition(x, y)
			+ (0.5f + cell.height) * Vector3.up);
	}
	
	public Vector3 GetCellSize()
	{
		return new Vector3 (parameters.xSize, 1.0f, parameters.ySize);
	}
	
	public void GetDimensions(out int x, out int y)
	{
		if(parameters == null) {
			x = 0;
			y = 0;
			return;
		}
		x = parameters.xDimension;
		y = parameters.yDimension;
	}
	
	public float GetCellHeight(int x, int y)
	{
		return cells[GetIdx(x, y)].height;
	}
	
	public void SetCellHeight(int x, int y, float height)
	{
		if(height < 0) {
			height = 0;
		} else if(height > parameters.scanHeight) {
			height = parameters.scanHeight;
		}
		cells[GetIdx(x, y)].height = height;
	}
	
	public bool IsSelected(int x, int y)
	{
		return selectedX == x && selectedY == y;	
	}
	
	public void SetSelected(int x, int y)
	{
		if(selectedX == x && selectedY == y) {
			selectedX = selectedY = -1;
			return;
		}
		selectedX = x;
		selectedY = y;
	}

	public GridCell GetSelectedCell(out int x, out int y)
	{
		x = selectedX;
		y = selectedY;
		if(selectedX < 0) {
			return null;
		}
		return cells[GetIdx(selectedX, selectedY)];
	}

	public override bool HasChanged
	{
		get { return helper.HasChanged; }
	}

	public override UnityEdge[] GetChangedEdges ()
	{
		return helper.GetChangedEdges ();
	}

	public override void ResetChanges ()
	{
		helper.Reset ();
	}

	public float GetEdgeCost (int x1, int y1, int x2, int y2)
	{
		Vector3 src = GetLocalCellPosition (x1, y1);
		Vector3 tgt = GetLocalCellPosition (x2, y2);
		
		switch (edgeCostAlgorithm) {
		case EdgeCostAlgorithm.EUCLIDEAN_DISTANCE:
			return Vector3.Distance (src, tgt);
		case EdgeCostAlgorithm.ONE_PLUS_POS_SLOPE:
			return 1.0f + Mathf.Max (0f, tgt.y - src.y);
		case EdgeCostAlgorithm.ONE:
			break;
		}
		return 1.0f;
	}

	public override UnityNode Quantize (Vector3 pos)
	{
		Vector3 p = transform.InverseTransformPoint (pos);
		float xSize = parameters.xSize;
		float ySize = parameters.ySize;
		float w = xSize * parameters.xDimension;
		float h = ySize * parameters.yDimension;
		int x = Mathf.FloorToInt ((p.x + w * 0.5f) / xSize);
		int y = Mathf.FloorToInt ((p.z + h * 0.5f) / ySize);
		if(x < 0) x = 0;
		else if(x >= parameters.xDimension) x = parameters.xDimension - 1;
		if(y < 0) y = 0;
		else if(y >= parameters.yDimension) y = parameters.yDimension - 1;
		return nodes[y, x];
	}

	/// <summary>
	/// Mark the edge between two grid squares as being changed
	/// </summary>
	/// <param name="x1">The x-coordinate of the source square.</param>
	/// <param name="y1">The y-coordinate of the source square.</param>
	/// <param name="x2">The x-coordinate of the destination square.</param>
	/// <param name="y2">The y-coordinate of the destination square.</param>
	/// <param name="cost">The new cost of the edge, set to Infinity to disable.</param>
	public void EdgeChanged (int x1, int y1, int x2, int y2, float cost)
	{
		SimpleNode src = nodes[y1, x1];
		SimpleNode target = nodes[y2, x2];

		for(int i = 0; i < src.OutEdgeCount; i ++) {
			SimpleEdge edge = (SimpleEdge)src.GetOutEdge(i);
			if(edge.Target == target) {
				edge.Cost = cost;
				helper.MarkChanged(edge);
			}
		}
	}

	int GetIdx (int x, int y)
	{
		return x + y * parameters.xDimension;
	}
}
