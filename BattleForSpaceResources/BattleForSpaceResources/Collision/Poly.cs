using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleForSpaceResources.Collision
{
    public class Poly
    {
        public Vector2[] v;
        public Vector2[] v_base;

        public Edge[] ed;
        public Edge[] ed_base;
        public Broadphase broadphase;

        public int VertexsCount
        {
            get { return v_base.Length; }
        }

        public Vector2 rot;
        public Body body;

        public Poly(Body body, Vector2[] vertexs)
        {
            Vector2 a, b;
            this.body = body;
            this.v_base = vertexs;
            this.v = new Vector2[VertexsCount];
            this.ed = new Edge[VertexsCount];
            this.ed_base = new Edge[VertexsCount];
            for (int i = 0; i < this.VertexsCount; i++)
            {
                a = this.v_base[i];
                b = this.v_base[((i + 1) % VertexsCount)];

                Vector2 someRENAME = ((Vector2)(b - a)).Perp();

                this.ed_base[i].n = someRENAME.Normalize2();
                this.ed_base[i].d = this.ed_base[i].n.Dot(a);
            }
            body.shapes.Add(this);
            rot = new Vector2((float)Math.Cos(body.angle), (float)Math.Sin(body.angle));
            for (int i = 0; i < VertexsCount; i++)
            {
                v[i] = body.position + v_base[i].Rotate(rot);
                ed[i].n = ed_base[i].n.Rotate(rot);
                ed[i].d = body.position.Dot(ed[i].n) + ed_base[i].d;
            }
            broadphase = Poly.GetBroadphase(this);
        }
        public static Broadphase GetBroadphase(Poly poly)
        {
            Vector2[] broadphase = new Vector2[4];

            float minX = poly.v[0].X;
            float minY = poly.v[0].Y;
            float maxX = poly.v[0].X;
            float maxY = poly.v[0].Y;

            for (int i = 0; i < poly.v.Length; i++)
            {
                if (poly.v[i].X < minX) minX = poly.v[i].X;
                if (poly.v[i].Y < minY) minY = poly.v[i].Y;
                if (poly.v[i].X > maxX) maxX = poly.v[i].X;
                if (poly.v[i].Y > maxY) maxY = poly.v[i].Y;
            }

            broadphase[0] = new Vector2(minX, minY);
            broadphase[1] = new Vector2(maxX, minY);
            broadphase[2] = new Vector2(maxX, maxY);
            broadphase[3] = new Vector2(minX, maxY);

            return new Broadphase(broadphase);
        }
    }
}
