using UnityEngine;

namespace SanityEngine.Structure.Graph.Spatial
{
    /// <summary>
    /// Specialized version of the graph interface for graphs with nodes
    /// representing points in space.
    /// </summary>
    public interface SpatialGraph<TNode, TEdge> : Graph<TNode, TEdge>
        where TNode : SpatialNode<TNode, TEdge>
        where TEdge : Edge<TNode, TEdge>
    {
    	/// <summary>
    	/// Find the node containing the point.
    	/// </summary>
    	TNode Quantize(Vector3 pos);
    }
}
