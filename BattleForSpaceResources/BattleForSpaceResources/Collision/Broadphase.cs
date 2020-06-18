using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleForSpaceResources.Collision
{
    public struct Broadphase
    {
        public Vector2[] vertexs;
        public float X1
        {
            get { return vertexs[0].X; }
        }
        public float Y1
        {
            get { return vertexs[0].Y; }
        }
        public float X2
        {
            get { return vertexs[2].X; }
        }
        public float Y2
        {
            get { return vertexs[2].Y; }
        }
        public Broadphase(Vector2[] broadphase)
        {
            this.vertexs = broadphase;
        }
        public static bool Collided(Broadphase b1, Broadphase b2)
        {
            if (b1.X1 < b2.X2 && b1.X2 > b2.X1 && b1.Y1 < b2.Y2 && b1.Y2 > b2.Y1)
                return true;
            else
                return false;
        }
    }
}
