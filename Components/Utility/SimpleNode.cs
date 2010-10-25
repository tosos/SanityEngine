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

public class SimpleNode : UnityNode
{
	public Graph<UnityNode, UnityEdge> Graph
	{
		get { return graph; }
	}

	public int OutEdgeCount
	{
		get { return outEdges.Count; }
	}

	public int InEdgeCount
	{
		get { return inEdges.Count; }
	}

	public UnityEdge GetOutEdge (int index)
	{
		return outEdges[index];
	}

	public UnityEdge GetInEdge (int index)
	{
		return inEdges[index];
	}

	public Vector3 Position
	{
		get { return position; }
	}

	public NavMesh<UnityNode, UnityEdge> NavMesh
	{
		get { return graph; }
	}
	
	Vector3 position;
	UnityGraph graph;
	List<SimpleEdge> outEdges = new List<SimpleEdge> ();
	List<SimpleEdge> inEdges = new List<SimpleEdge> ();

	public SimpleNode (Vector3 position, Grid graph)
	{
		this.position = position;
		this.graph = graph;
	}

	public void AddOutEdge (SimpleEdge edge)
	{
		outEdges.Add (edge);
	}

	public void AddInEdge (SimpleEdge edge)
	{
		inEdges.Add (edge);
	}

	public void RemoveOutEdge (int index)
	{
		outEdges.RemoveAt (index);
	}

	public void RemoveInEdge (int index)
	{
		inEdges.RemoveAt (index);
	}
}
