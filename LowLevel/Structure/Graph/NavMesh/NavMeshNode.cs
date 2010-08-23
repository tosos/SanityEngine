using System;
using UnityEngine;

namespace SanityEngine.Structure.Graph.NavMesh
{
    /// <summary>
    /// A node object in 3D space.
    /// </summary>
    /// <typeparam name="TNode">The node type</typeparam>
    /// <typeparam name="TEdge">The edge type</typeparam>
    public interface NavMeshNode<TNode, TEdge> : Node<TNode, TEdge>
        where TNode : NavMeshNode<TNode, TEdge>
        where TEdge : Edge<TNode, TEdge>
    {
		Vector3 Position
		{
			get;
		}
	}
}