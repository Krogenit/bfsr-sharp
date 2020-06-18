using BattleForSpaceResources.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleForSpaceResources.Entitys
{
    public class CollisionObject : GameObject
    {
        public Body body = new Body();
        public bool isLive, isSetDestroingTimer;
        public Vector2 velocity;
        public float rotationSpeed;
        public World world;
        public CollisionObject(Texture2D text, Vector2 pos, World w)
            : base(text, pos)
        {
            world = w;
            isLive = true;
            body.position = pos;
        }
        public CollisionObject(Vector2 pos, World w)
            : base(pos)
        {
            world = w;
            isLive = true;
            body.position = pos;
        }
        public override void Update()
        {
            FixRotation();
            UpdatePolygons();
            //base.Update();
        }
        private void UpdatePolygons()
        {
            body.angle = Rotation;
            body.position = Position;
            for (int j = 0; j < body.shapes.Count; j++)
            {
                body.shapes[j].rot = new Vector2((float)Math.Cos(body.angle), (float)Math.Sin(body.angle));
                for (int i = 0; i < body.shapes[j].VertexsCount; i++)
                {
                    body.shapes[j].v[i] = body.shapes[j].body.position + V2Extend.Rotate(body.shapes[j].v_base[i], body.shapes[j].rot);

                    body.shapes[j].ed[i].n = V2Extend.Rotate(body.shapes[j].ed_base[i].n, body.shapes[j].rot);
                    body.shapes[j].ed[i].d = V2Extend.Dot(body.shapes[j].body.position, body.shapes[j].ed[i].n) + body.shapes[j].ed_base[i].d;
                }
                body.shapes[j].broadphase = Poly.GetBroadphase(body.shapes[j]);
            }
        }
        public void FixRotation()
        {
            if (MathHelper.ToDegrees(Rotation) > 360)
            {
                Rotation = 0;
            }
            if (MathHelper.ToDegrees(Rotation) < 0)
            {
                Rotation = MathHelper.ToRadians(360);
            }
        }
        public override void Render(SpriteBatch spriteBatch)
        {
            base.Render(spriteBatch);
            if (Settings.isRenderBodys)
                DrawDebugLines(spriteBatch);
        }
        public IEnumerable<DebugLine> getDebugLines()
        {
            for (int j = 0; j < body.shapes.Count; j++)
            {
                for (int i = 0; i < body.shapes[j].VertexsCount; i++)
                {
                    Vector2 v1 = body.shapes[j].v[i];
                    Vector2 v2 = body.shapes[j].v[((i + 1) % body.shapes[j].VertexsCount)];
                    Color color = Color.White;

                    yield return new DebugLine(v1, v2, color);
                }
            }

            yield return new DebugLine(body.position - new Vector2(3f, 0f), body.position + new Vector2(3f, 0f), Color.White);
            yield return new DebugLine(body.position - new Vector2(0, 3f), body.position + new Vector2(0f, 3f), Color.White);

            for (int j = 0; j < body.shapes.Count; j++)
            {
                Vector2 dp = Vector2.Zero;

                for (int i = 0; i < body.shapes[j].VertexsCount; i++)
                {
                    dp.X += body.shapes[j].v[i].X;
                    dp.Y += body.shapes[j].v[i].Y;

                }

                dp /= body.shapes[j].VertexsCount;

                yield return new DebugLine(dp - new Vector2(2f, 0f), dp + new Vector2(2f, 0f), Color.Red);
                yield return new DebugLine(dp - new Vector2(0, 2f), dp + new Vector2(0f, 2f), Color.Red);

                Vector2[] broadphase = body.shapes[j].broadphase.vertexs;

                for (int i = 0; i < 4; i++)
                {
                    Vector2 v1 = broadphase[i];
                    Vector2 v2 = broadphase[((i + 1) % 4)];
                    Color color = new Color(0.5f, 0.5f, 0.5f, 0.5f);


                    yield return new DebugLine(v1, v2, color);
                }
            }

            yield break;
        }
        public void DrawLine(Color color, Vector2 position1, Vector2 position2, float thickness, SpriteBatch spriteBatch)
        {
            double distance = Math.Sqrt(((position2.X - position1.X) * (position2.X - position1.X)) + ((position2.Y - position1.Y) * (position2.Y - position1.Y)));
            float angle = -(float)Math.Atan2(position1.X - position2.X, position1.Y - position2.Y) - (float)Math.PI / 2f;

            spriteBatch.Draw(Textures.pixel, position1, new Rectangle(0, 0, (int)distance, (int)thickness), color, angle, new Vector2(0f, thickness / 2f), 1f, SpriteEffects.None, layer);
        }
        public void DrawDebugLines(SpriteBatch spriteBatch)
        {
            for (int j = 0; j < body.shapes.Count; j++)
            {
                for (int i = 0; i < body.shapes[j].v.Length; i++)
                {
                    spriteBatch.Draw(Textures.pixel, body.shapes[j].v[i], null, Color.Red, Rotation, new Vector2(Textures.pixel.Width / 2, Textures.pixel.Height / 2), 0.5F, SpriteEffects.None, layer);
                }
            }
            IEnumerable<DebugLine> lines = getDebugLines();
            foreach (DebugLine line in lines)
                DrawLine(line.color, line.vector1, line.vector2, 1f, spriteBatch);
        }
    }
    public struct DebugLine
    {
        public Vector2 vector1;
        public Vector2 vector2;
        public Color color;

        public DebugLine(Vector2 vector1, Vector2 vector2, Color color)
        {
            this.vector1 = vector1;
            this.vector2 = vector2;
            this.color = color;
        }
    }
}
