using System;
using UnityEngine;

namespace SanityEngine.Structure.Graph.NavMesh
{
    /// <summary>
    /// A node object in 3D space.
    /// </summary>
    public interface NavMeshNode : Node
    {
		Vector3 Position
		{
			get;
		}
	}
}