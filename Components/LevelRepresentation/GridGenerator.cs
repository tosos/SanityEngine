using UnityEngine;
using System.Collections.Generic;
using SanityEngine.Structure.Graph;
using SanityEngine.Structure.Graph.Spatial;
using SanityEngine.LevelRepresentation.Grid;

[AddComponentMenu("Sanity/Level Representation/Grid Generator"),
ExecuteInEditMode()]
public class GridGenerator : UnityGraph
{
	private delegate bool Sample(Vector3 pos, Vector3 dir, out Vector3 result);
	
	public enum SamplerType {
		CENTER,
		CORNERS_AVG,
		CORNERS_MIN,
		CORNERS_MAX,
		MONTE_CARLO
	};
	
	public enum EdgeCostAlgorithm {
		ONE_PLUS_POS_SLOPE,
		ONE,
		EUCLIDEAN_DISTANCE
	};
	
	public enum GridType {
		EIGHT_GRID,
		FOUR_GRID
	};
	
	public SamplerType samplingType = SamplerType.MONTE_CARLO;
	public int xRes = 10;
	public int yRes = 10;
	public bool noHitNoCell = true;
	public float maxSlope = float.PositiveInfinity;
	public bool edgeRaycast = false;
	public EdgeCostAlgorithm edgeCostAlgorithm;
	public GridType gridType;
	
	GridCell[] cells;
	GraphChangeHelper<UnityNode, UnityEdge> helper;
	
	void Awake()
	{
		helper = new GraphChangeHelper<UnityNode, UnityEdge>();
		cells = GetComponentsInChildren<GridCell>();
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnDrawGizmos()
	{
		Vector3 pos = transform.position;
		Gizmos.matrix = Matrix4x4.TRS(pos, transform.rotation, Vector3.one);
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(Vector3.zero, Vector3.up * transform.localScale.y * 0.5f);
		Gizmos.color = Color.white;
		Gizmos.DrawWireCube(Vector3.zero, transform.localScale);
	}

	public void FindCells()
	{
		List<GameObject> del = new List<GameObject>();
		foreach(Transform child in transform) {
			del.Add(child.gameObject);
		}
		
		foreach(GameObject obj in del) {
			DestroyImmediate(obj);
		}
		
		Sample sampler = null;
		switch(samplingType) {
			case SamplerType.CENTER: sampler = SampleCenter; break;
			case SamplerType.CORNERS_AVG: sampler = SampleCornersAvg; break;
			case SamplerType.CORNERS_MIN: sampler = SampleCornersMin; break;
			case SamplerType.CORNERS_MAX: sampler = SampleCornersMax; break;
			case SamplerType.MONTE_CARLO: sampler = SampleMonteCarlo; break;
		}
		float xSize = transform.localScale.x / xRes;
		float ySize = transform.localScale.z / yRes;
		float thickness = transform.localScale.y * 0.1f;
		float xOff = xSize * 0.5f;
		float yOff = ySize * 0.5f;
		Vector3 down = transform.up * -transform.localScale.y;
		Vector3 top = Vector3.up * transform.localScale.y * 0.5f;
		Vector3 scale = new Vector3(xSize, thickness, ySize);
		Vector3 start = new Vector3(-xOff * xRes + xOff, 0.0f, -yOff * yRes + yOff);
		
		GridCell[,] cells = new GridCell[yRes, xRes];
		// Node creation pass
		for(int y = 0; y < yRes ; y ++) {
			for(int x = 0; x < xRes ; x ++) {
				Vector3 rayPos = transform.position + transform.rotation * 
					(new Vector3(xSize * x, 0f, ySize * y) + start + top);
				Vector3 pos = Vector3.zero;
				if(!sampler(rayPos, down, out pos) && noHitNoCell) {
					continue;
				}
				GameObject cell = new GameObject("Cell (" + x + "," + y + ")");
				cell.transform.position = pos;
				cell.transform.rotation = transform.rotation;
				cell.transform.parent = transform;
				cell.transform.localScale = scale;
				BoxCollider box = cell.AddComponent<BoxCollider>();
				box.isTrigger = true;
				Vector3 s = transform.localScale;
				box.size = new Vector3(1.0f/s.x, 1.0f/s.y, 1.0f/s.z);
				cells[y, x] = cell.AddComponent<GridCell>();
			}
		}
		
		// Edge creation pass
		List<UnityEdge> edges = new List<UnityEdge>();
		for(int y = 0; y < yRes ; y ++) {
			for(int x = 0; x < xRes ; x ++) {
				edges.Clear();
				GridCell src = cells[y, x];
				if(src == null) {
					continue;
				}
				for(int dy = -1; dy <= 1; dy ++) {
					for(int dx = -1; dx <= 1; dx ++) {
						if(gridType == GridType.FOUR_GRID
							&& Mathf.Abs(dx) + Mathf.Abs(dy) >= 2)
						{
							continue;
						}
						int x2 = x + dx, y2 = y + dy;
						// Skip the center
						if((dx == 0 && dy == 0) || x2 < 0 || x2 >= cells.GetLength(1)
							|| y2 < 0 || y2 >= cells.GetLength(0))
						{
							// TODO add 4-/8- criteria
							continue;
						}
						GridCell dest = cells[y2, x2];
						if(dest == null) {
							continue;
						}
						Vector3 diff = dest.transform.position - src.transform.position;
						float slope = Mathf.Abs(Vector3.Dot(diff, transform.up));
						if(slope < maxSlope) {
							edges.Add(new UnityEdge(dest,
								CalculateEdgeCost(src, dest)));
						}
					}
				}
				src.edges = edges.ToArray();
			}
		}
	}
	
	bool SampleCenter(Vector3 pos, Vector3 dir, out Vector3 result) {
		if(RaycastNearest(pos, dir, out result)) {
			return true;
		}
		result = pos + dir;
		return false;
	}

	bool SampleCornersAvg(Vector3 pos, Vector3 dir, out Vector3 result) {
		Vector3 r = transform.right * (transform.localScale.x / xRes) * 0.5f;
		Vector3 f = transform.forward * (transform.localScale.z / yRes) * 0.5f;

		Vector3 sum = Vector3.zero;
		
		int hits = 0;
		Vector3 hit;
		if(RaycastNearest(pos - r - f, dir, out hit))
		{
			sum += hit - pos;
			hits ++;
		}
		if(RaycastNearest(pos + r - f, dir, out hit))
		{
			sum += hit - pos;
			hits ++;
		}
		if(RaycastNearest(pos - r + f, dir, out hit))
		{
			sum += hit - pos;
			hits ++;
		}
		if(RaycastNearest(pos + r + f, dir, out hit))
		{
			sum += hit - pos;
			hits ++;
		}
		sum += dir * (4 - hits);
		float dist = (sum / 4f).magnitude / dir.magnitude;
		result = pos + dir * dist;
		return hits > 0;
	}

	bool SampleCornersMin(Vector3 pos, Vector3 dir, out Vector3 result) {
		Vector3 r = transform.right * (transform.localScale.x / xRes) * 0.5f;
		Vector3 f = transform.forward * (transform.localScale.z / yRes) * 0.5f;

		float minDist = float.PositiveInfinity;
		float max = dir.magnitude;
		
		Vector3 hit;
		bool didHit = false;
		if(RaycastNearest(pos - r - f, dir, out hit))
		{
			minDist = Mathf.Min(minDist, (hit - pos).magnitude);
			didHit = true;
		}
		if(RaycastNearest(pos + r - f, dir, out hit))
		{
			minDist = Mathf.Min(minDist, (hit - pos).magnitude);
			didHit = true;
		}
		if(RaycastNearest(pos - r + f, dir, out hit))
		{
			minDist = Mathf.Min(minDist, (hit - pos).magnitude);
			didHit = true;
		}
		if(RaycastNearest(pos + r + f, dir, out hit))
		{
			minDist = Mathf.Min(minDist, (hit - pos).magnitude);
			didHit = true;
		}
		minDist = Mathf.Min(minDist, max);
		result = pos + dir * minDist / max;
		return didHit;
	}

	bool SampleCornersMax(Vector3 pos, Vector3 dir, out Vector3 result) {
		Vector3 r = transform.right * (transform.localScale.x / xRes) * 0.5f;
		Vector3 f = transform.forward * (transform.localScale.z / yRes) * 0.5f;

		bool wasHit = false;
		float maxDist = 0f;
		
		Vector3 hit;
		if(RaycastNearest(pos - r - f, dir, out hit))
		{
			maxDist = Mathf.Max(maxDist, (hit - pos).magnitude);
			wasHit = true;
		}
		if(RaycastNearest(pos + r - f, dir, out hit))
		{
			maxDist = Mathf.Max(maxDist, (hit - pos).magnitude);
			wasHit = true;
		}
		if(RaycastNearest(pos - r + f, dir, out hit))
		{
			maxDist = Mathf.Max(maxDist, (hit - pos).magnitude);
			wasHit = true;
		}
		if(RaycastNearest(pos + r + f, dir, out hit))
		{
			maxDist = Mathf.Max(maxDist, (hit - pos).magnitude);
			wasHit = true;
		}
		if(!wasHit) {
			maxDist = dir.magnitude;
		}
		result = pos + dir * maxDist / dir.magnitude;
		return wasHit;
	}
	
	bool SampleMonteCarlo(Vector3 pos, Vector3 dir, out Vector3 result)
	{
		Vector3 r = transform.right * (transform.localScale.x / xRes) * 0.5f;
		Vector3 f = transform.forward * (transform.localScale.z / yRes) * 0.5f;
		
		const int numRays = 9;
		Vector3 sum = Vector3.zero;
		int hits = 0;
		for(int i = 0; i < numRays; i ++) {
			Vector3 p = pos + r * Random.Range(-1f, 1f)
				+ f * Random.Range(-1f, 1f);
			Vector3 hit;
			if(RaycastNearest(p, dir, out hit)) {
				sum += hit - pos;
				hits ++;
			}
		}
		sum += dir * (numRays - hits);
		float dist = (sum / numRays).magnitude / dir.magnitude;
		result = pos + dir * dist;
		return hits > 0;
	}
	
	bool RaycastNearest(Vector3 pos, Vector3 dir, out Vector3 point)
	{
		RaycastHit[] hits = Physics.RaycastAll(pos, dir, dir.magnitude, ~0);
		float minDist = float.PositiveInfinity;
		point = Vector3.zero;
		foreach(RaycastHit hit in hits) {
			float dist = hit.distance;
			if(dist < minDist) {
				minDist = dist;
				point = hit.point;
			}
		}
		return hits.Length != 0;
	}
	
	float CalculateEdgeCost(GridCell src, GridCell tgt)
	{
		switch(edgeCostAlgorithm) {
			case EdgeCostAlgorithm.EUCLIDEAN_DISTANCE:
				return Vector3.Distance(tgt.transform.position,
					src.transform.position);
			case EdgeCostAlgorithm.ONE_PLUS_POS_SLOPE:
				return 1.0f + Mathf.Max(0f,
					Vector3.Dot((tgt.transform.position - src.transform.position),
						transform.up));
			case EdgeCostAlgorithm.ONE:
				break;
		}
		return 1.0f;
	}
	
    public override bool HasChanged
    {
        get { return helper.HasChanged; }
    }

    public override UnityEdge[] GetChangedEdges()
    {
    	return helper.GetChangedEdges();
    }

    public override void ResetChanges()
    {
    	helper.Reset();
    }
        
   	public override UnityNode Quantize(Vector3 pos)
   	{
   		// FIXME this is slow
   		float minDist = float.PositiveInfinity;
   		GridCell nearest = null;
   		foreach(GridCell cell in cells) {
   			float dist = (pos - cell.transform.position).sqrMagnitude;
   			if(dist < minDist) {
   				nearest = cell;
   				minDist = dist;
   			}
   		}
   		return nearest;
   	}
}
