using System;
using UnityEngine;

namespace SanityEngine.Structure.Graph.Spatial
{
    /// <summary>
    /// A node object in 3D space.
    /// </summary>
    /// <typeparam name="TNode">The node type</typeparam>
    /// <typeparam name="TEdge">The edge type</typeparam>
    public interface SpatialNode<TNode, TEdge> : Node<TNode, TEdge>
        where TNode : SpatialNode<TNode, TEdge>
        where TEdge : Edge<TNode, TEdge>
    {
		Vector3 Position
		{
			get;
		}
	}
}