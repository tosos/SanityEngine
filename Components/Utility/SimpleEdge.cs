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

public class SimpleEdge : UnityEdge
{
	public UnityNode Source
	{
		get { return source; }
	}

	public UnityNode Target
	{
		get { return target; }
	}

	public float Cost
	{
		get { return cost; }
		set { cost = value; }
	}

	SimpleNode source;
	SimpleNode target;
	float cost;

	public SimpleEdge (SimpleNode source, SimpleNode target, float cost)
	{
		this.source = source;
		this.target = target;
		this.cost = cost;
	}
}
