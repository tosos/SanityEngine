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

public class SimpleNode : NavMeshNode
{
	public Vector3 Position
	{
		get { return position; }
	}
	
	Vector3 position;

	public SimpleNode (Vector3 position)
	{
		this.position = position;
	}
	
	public bool Equals (Node other)
	{
		return this == other;
	}
}
