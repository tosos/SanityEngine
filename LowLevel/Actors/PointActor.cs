//
// Copyright (C) 2010 The Sanity Engine Development Team
//
// This source code is licensed under the terms of the
// MIT License.
//
// For more information, see the file LICENSE

using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;

namespace SanityEngine.Actors
{
    class PointActor : Actor
    {
        Vector3 point;

        public Vector3 Velocity
        {
            get { return Vector3.zero; }
        }

        public Vector3 Position
        {
            get { return point; }
        }

        public Vector3 Facing
        {
            get { return Vector3.zero; }
        }

        public float MaxForce
        {
            get { return 0.0f; }
        }

        public float MaxAngSpeed
        {
            get { return 0.0f; }
        }

        public Vector3 Point
        {
            get { return point; }
            set { point = value; }
        }

        public PointActor(Vector3 point)
        {
            this.point = point;
        }

        public void SetProperty(string name, string value)
        {
            throw new NotImplementedException();
        }

        public void SetBoolProperty(string name)
        {
            throw new NotImplementedException();
        }

        public void ClearBoolProperty(string name)
        {
            throw new NotImplementedException();
        }

        public string GetProperty(string name)
        {
            throw new NotImplementedException();
        }

        public bool HasBoolProperty(string name)
        {
            throw new NotImplementedException();
        }
    }
}
