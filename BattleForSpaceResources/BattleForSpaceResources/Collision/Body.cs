using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleForSpaceResources.Collision
{
    public class Body
    {
        public Vector2 position;
        public Vector2 velocity;
        public float angle;

        public List<Poly> shapes = new List<Poly>();

        public struct PolygonCollisionResult
        {
            public bool WillIntersect;
            public bool Intersect;
        }
        public bool PolygonCollide(Body bodyA, Body bodyB)
        {
            foreach (Poly polyA in bodyA.shapes)
            {
                foreach (Poly polyB in bodyB.shapes)
                {
                    if (Broadphase.Collided(polyA.broadphase, polyB.broadphase))
                    {
                        //PolygonCollisionResult r = PolygonCollision(polyA, polyB);

                        //if (r.Intersect)
                        if (PolygonCollision(polyA, polyB))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        //private PolygonCollisionResult PolygonCollision(Poly polygonA, Poly polygonB)
        private bool PolygonCollision(Poly polygonA, Poly polygonB)
        {
            bool result = true;
            //PolygonCollisionResult result = new PolygonCollisionResult();
            //result.Intersect = true;
            //result.WillIntersect = true;

            int edgeCountA = polygonA.ed.Length;
            int edgeCountB = polygonB.ed.Length;
            Vector2 edge;

            for (int edgeIndex = 0; edgeIndex < edgeCountA + edgeCountB; edgeIndex++)
            {
                if (edgeIndex < edgeCountA)
                {
                    edge = polygonA.ed[edgeIndex].n;
                }
                else
                {
                    edge = polygonB.ed[edgeIndex - edgeCountA].n;
                }

                Vector2 axis = new Vector2(-edge.Y, edge.X);
                axis.Normalize();

                float minA = 0; float minB = 0; float maxA = 0; float maxB = 0;
                ProjectPolygon(axis, polygonA, ref minA, ref maxA);
                ProjectPolygon(axis, polygonB, ref minB, ref maxB);

                if (IntervalDistance(minA, maxA, minB, maxB) > 0) result = false;

                //float intervalDistance = IntervalDistance(minA, maxA, minB, maxB);
                //if (intervalDistance > 0) result.WillIntersect = false;

                if (!result) break;
            }

            return result;
        }
        private float IntervalDistance(float minA, float maxA, float minB, float maxB)
        {
            if (minA < minB)
            {
                return minB - maxA;
            }
            else
            {
                return minA - maxB;
            }
        }
        private void ProjectPolygon(Vector2 axis, Poly polygon, ref float min, ref float max)
        {
            float d = V2Extend.Dot(axis, polygon.v[0]);
            min = d;
            max = d;
            for (int i = 0; i < polygon.v.Length; i++)
            {
                d = V2Extend.Dot(axis, polygon.v[i]);
                if (d < min)
                {
                    min = d;
                }
                else
                {
                    if (d > max)
                    {
                        max = d;
                    }
                }
            }
        }
    }
}
