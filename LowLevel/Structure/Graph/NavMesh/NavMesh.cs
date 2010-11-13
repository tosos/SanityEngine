using UnityEngine;

namespace SanityEngine.Structure.Graph.NavMesh
{
    /// <summary>
    /// Specialized version of the graph interface for graphs with nodes
    /// representing points in space.
    /// </summary>
    public interface NavMesh : Graph
    {
    	/// <summary>
    	/// Find the node containing the point.
    	/// </summary>
    	NavMeshNode Quantize(Vector3 pos);
    }
}
