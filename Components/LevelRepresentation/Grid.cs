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
		EIGHT_GRID = 8,
		FOUR_GRID = 4
	}
		
	public enum EdgeDir
	{
		North = 0,
		South = 1,
		West = 2,
		East = 3,
		NorthWest = 4,
		NorthEast = 5,
		SouthWest = 6,
		SouthEast = 7,
	}
	
	[System.Flags]
	public enum CellFlags : byte
	{
		Invalid =	0x01,
		Blocked =	0x02
	}

	[System.Serializable]
	public class GridCell : NavMeshNode
	{
		public float[] edges;
		public Vector3 Position
		{
			get { return position; }
			set { position = value; }
		}
		
		[SerializeField]
		Vector3 position;

		[SerializeField]
		byte flags;
		
		[SerializeField]
		int index;

		public GridCell (int index, GridType gridType, Vector3 pos)
		{
			this.index = index;
			this.edges = new float[(int)gridType];
			this.position = pos;
		}
		
		public void SetFlags(CellFlags flags)
		{
			this.flags |= (byte)flags;
		}
		
		public void ClearFlags(CellFlags flags)
		{
			this.flags &= (byte)~((byte)flags);
		}
		
		public bool TestFlags(CellFlags flags)
		{
			return (this.flags & (byte)flags) != 0;
		}
		
		public bool Equals (Node other)
		{
			return other == this;
		}
				
		public override int GetHashCode ()
		{
			return index;
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
		public GridType gridType = GridType.EIGHT_GRID;
		
		public void CopyTo(GridParameters other)
		{
			other.xSize = xSize;
			other.ySize = ySize;
			other.scanHeight = scanHeight;
			other.xDimension = xDimension;
			other.yDimension = yDimension;
			other.gridType = gridType;
		}
	}
	
	struct GridEdge : Edge
	{
        public Node Source
        {
            get { return source; }
        }

        public Node Target
        {
            get { return target; }
        }

        public float Cost
        {
            get { return cost; }
        }
		
		GridCell source;
		GridCell target;
		float cost;
		
		public GridEdge(GridCell source, GridCell target, float cost)
		{
			this.source = source;
			this.target = target;
			this.cost = cost;
		}
	}

	public SamplerType samplingType = SamplerType.MONTE_CARLO;
	public LayerMask layerMask = -1;
	public float maxEdgeHeightChange = Mathf.Infinity;
	public bool edgeRaycast = false;
	public EdgeCostAlgorithm edgeCostAlgorithm;
	public int selectedX = -1;
	public int selectedY = -1;
	public GridParameters newParameters;
	public bool noHitNoCell = true;
	public bool alwaysDrawGrid = true;
	public bool drawEdges = true;
	
	public static readonly int[,] edgeGridOffsets = {
		{ 0, -1 },
		{ 0, 1 },
		{ -1, 0 },
		{ 1, 0 },
		{ -1, -1 },
		{ 1, -1 },
		{ -1, 1 },
		{ 1, 1 },
	}; 
		
	[SerializeField]
	GridCell[] cells;
	[SerializeField]
	GridParameters parameters;

	GraphChangeHelper helper;

	// Use this for initialization
	void Start ()
	{
		if(cells == null) {
			return;
		}
		
		helper = new GraphChangeHelper ();
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
			{ new Vector3(0.3f, 0.0f, -0.4f), new Vector3(-0.4f, 0.0f, 0.3f) },
			{ new Vector3(-0.3f, 0.0f, 0.4f), new Vector3(0.4f, 0.0f, -0.3f) },
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
					if(cell.TestFlags(CellFlags.Invalid)) {
						continue;
					}
					Vector3 cellPos = transform.InverseTransformPoint(cell.Position);
					Gizmos.color = new Color(1.0f, 1.0f, 1.0f, 0.2f);
					Gizmos.DrawWireCube (cellPos, cellSize);
					
					if(drawEdges) {
						if(selectedX >= 0 && (selectedX != x || selectedY != y)) {
							continue;
						}
						for(int i = 0; i < (int)parameters.gridType; i ++) {
							if(cell.edges[i] < 0f) {
								continue;
							}
							if(cell.edges[i] == Mathf.Infinity) {
								Gizmos.color = Color.red;
							} else {
								Gizmos.color = Color.green;
							}
							int tx = x + edgeGridOffsets[i,0];
							int ty = y + edgeGridOffsets[i,1];
							if(!Valid(tx, ty)) {
								continue;
							}
							GridCell tcell = cells[GetIdx(tx, ty)];
							if(tcell.TestFlags(CellFlags.Invalid)) {
								continue;
							}
							Vector3 tCellPos = transform.InverseTransformPoint(tcell.Position);
							Vector3 start = cellPos + Vector3.Scale(cellSize, offsets[i,0]);
							Vector3 end = tCellPos + Vector3.Scale(cellSize, offsets[i,1]);
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
				Vector3 rayPos = transform.TransformPoint (GetLocalCellBase(x, y)
					+ Vector3.up * parameters.scanHeight);
				Vector3 pos;
				float height = noHitNoCell ? -1f : 0f;
				if (sampler (rayPos, down * parameters.scanHeight, out pos)) {
					Vector3 localPos = transform.InverseTransformPoint (pos);
					height = localPos.y + parameters.scanHeight * 0.5f;
				}
				
				cells[GetIdx (x, y)] = new GridCell (GetIdx(x, y),
					parameters.gridType, transform.TransformPoint(
					GetLocalCellBase(x, y) + Vector3.up * height));
				if(height < 0f) {
					cells[GetIdx (x, y)].SetFlags(CellFlags.Invalid);
				}
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
		GridCell src = cells[GetIdx(x, y)];
		for(int i = 0; i < (int)parameters.gridType; i ++) {
			int tx = x + edgeGridOffsets[i, 0];
			int ty = y + edgeGridOffsets[i, 1];
			float cost = Mathf.NegativeInfinity;
			if(Valid(tx, ty)) {
				GridCell tgt = cells[GetIdx(tx, ty)];
				float change = Vector3.Dot(tgt.Position - src.Position, transform.up);
				if(change > maxEdgeHeightChange || src.TestFlags(CellFlags.Invalid)
					|| tgt.TestFlags(CellFlags.Invalid))
				{
					cost = Mathf.Infinity;
				} else {
					cost = GetEdgeCost (x, y, tx, ty);
				}
			}
			src.edges[i] = cost;
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

	Vector3 GetLocalCellBase (int x, int y)
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
		return cells[GetIdx(x, y)].Position;
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
		Vector3 pos = transform.InverseTransformPoint(
			cells[GetIdx(x, y)].Position) - GetLocalCellBase(x, y);
		return pos.y;
	}
	
	public bool IsCellShown(int x, int y)
	{
		return !cells[GetIdx(x, y)].TestFlags(CellFlags.Invalid);
	}
	
	/// <summary>
	/// Sets the height of the cell.
	/// </summary>
	/// <param name='x'>the cell's x-coordinate</param>
	/// <param name='y'>the cell's y-coordinate</param>
	/// <param name='height'>the cell's new height</param>
	/// <remarks>Note that this does NOT change the edge costs</remarks>
	public void SetCellHeight(int x, int y, float height)
	{
		if(height < 0) {
			height = 0;
		} else if(height > parameters.scanHeight) {
			height = parameters.scanHeight;
		}
		cells[GetIdx(x, y)].Position = transform.TransformPoint(
			GetLocalCellBase(x, y) + Vector3.up * height);
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

	public override Edge[] GetChangedEdges ()
	{
		return helper.GetChangedEdges ();
	}

	public override void ResetChanges ()
	{
		helper.Reset ();
	}
	
	public float GetEdgeCost (int x1, int y1, int x2, int y2)
	{
		Vector3 src = transform.InverseTransformPoint(cells[GetIdx(x1, y1)].Position);
		Vector3 tgt = transform.InverseTransformPoint(cells[GetIdx(x2, y2)].Position);
		
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

    public override int GetOutEdgeCount(Node node)
	{
		return (int)parameters.gridType;
	}

    public override int GetInEdgeCount(Node node)
	{
		return GetOutEdgeCount(node);
	}

    public override Edge GetOutEdge(Node node, int edgeIndex)
	{
		int index = node.GetHashCode();
		int x, y;
		SplitIdx(index, out x, out y);

		GridCell source = cells[index];

		int tx = x + edgeGridOffsets[edgeIndex,0];
		int ty = y + edgeGridOffsets[edgeIndex,1];
		if(!Valid(tx, ty)) {
			return new GridEdge(source, null, Mathf.Infinity);
		}
		
		GridCell target = cells[GetIdx(tx, ty)];

		return new GridEdge(source, target, source.edges[edgeIndex]);
	}

    public override Edge GetInEdge(Node node, int edgeIndex)
	{
		int index = node.GetHashCode();
		int x, y;
		SplitIdx(index, out x, out y);

		GridCell target = cells[index];

		int sx = x + edgeGridOffsets[edgeIndex,0];
		int sy = y + edgeGridOffsets[edgeIndex,1];
		if(!Valid(sx, sy)) {
			return new GridEdge(null, target, Mathf.Infinity);
		}
		
		GridCell source = cells[GetIdx(sx, sy)];

		return new GridEdge(source, target, source.edges[edgeIndex]);
	}

	public override NavMeshNode Quantize (Vector3 pos)
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
		GridCell cell = cells[GetIdx(x, y)];
		if(cell.TestFlags(CellFlags.Invalid)) {
			return null;
		}
		return cell;
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
		GridCell src = cells[GetIdx(x1, y1)];
		GridCell target = cells[GetIdx(x2, y2)];
		int dir = GetDirection(x1, y2, x2, y2);
		
		src.edges[dir] = cost;
		
		helper.MarkChanged(new GridEdge(src, target, cost));
	}

	int GetIdx (int x, int y)
	{
		return x + y * parameters.xDimension;
	}
	
	void SplitIdx(int index, out int x, out int y)
	{
		x = index % parameters.xDimension;
		y = index / parameters.xDimension;
	}
	
	bool Valid(int x, int y)
	{
		return x >= 0 && y >= 0
			&& x < parameters.xDimension && y < parameters.yDimension;
	}
}
