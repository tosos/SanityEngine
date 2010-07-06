using System;
using System.Collections.Generic;
using System.Text;
using AIEngine.Utility.Math;

namespace AIEngine.Actors
{
    class AIPointActor : Actor
    {
        AIVector3 point;

        public AIVector3 Velocity
        {
            get { return AIVector3.zero; }
        }

        public AIVector3 Position
        {
            get { return point; }
        }

        public AIVector3 Facing
        {
            get { return AIVector3.zero; }
        }

        public float MaxSpeed
        {
            get { return 0.0f; }
        }

        public float MaxAngSpeed
        {
            get { return 0.0f; }
        }

        public AIVector3 Point
        {
            get { return point; }
            set { point = value; }
        }

        public AIPointActor(AIVector3 point)
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
