using System;
using System.Collections.Generic;
using System.Text;
using AIEngine.Utility.Math;

namespace AIEngine.Actors
{
    class VelocityWrapperActor : Actor
    {
        Actor actor;

        public AIVector3 Velocity
        {
            get { return actor.Velocity; }
        }

        public AIVector3 Position
        {
            get { return actor.Position + actor.Velocity; }
        }

        public AIVector3 Facing
        {
            get { return actor.Facing; }
        }

        public float MaxSpeed
        {
            get { return actor.MaxSpeed; }
        }

        public float MaxAngSpeed
        {
            get { return actor.MaxAngSpeed; }
        }

        public VelocityWrapperActor(Actor actor)
        {
            this.actor = actor;
        }

        public void SetProperty(string name, string value)
        {
            actor.SetProperty(name, value);
        }

        public void SetBoolProperty(string name)
        {
            actor.SetBoolProperty(name);
        }

        public void ClearBoolProperty(string name)
        {
            actor.ClearBoolProperty(name);
        }

        public string GetProperty(string name)
        {
            return actor.GetProperty(name);
        }

        public bool HasBoolProperty(string name)
        {
            return actor.HasBoolProperty(name);
        }
    }
}
