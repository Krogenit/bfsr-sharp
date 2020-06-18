using BattleForSpaceResources.Entitys;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleForSpaceResources.Particles
{
    public class Particle : GameObject
    {
        private Vector2 velocity;
        public float angleVelocity, sizeVelocity, alphaVelocity;
        public Particle(Texture2D text, Vector2 pos, Vector2 vel, float angle, float angleVel, Vector4 col, float newSize, float sizeVel, float alphaVel)
            : base(text, pos)
        {
            velocity = vel;
            angleVelocity = angleVel;
            sizeVelocity = sizeVel;
            alphaVelocity = alphaVel;
            color = col;
            Size = newSize;
            Rotation = angle;
        }
        public override void Update()
        {
            Position += velocity;
            Rotation += angleVelocity;
            Size += sizeVelocity;
            float horiz = velocity.X;
            float vertic = velocity.Y;
            velocity.X = horiz -= Settings.gravity * horiz;
            velocity.Y = vertic -= Settings.gravity * vertic;
            color = new Vector4(color.X, color.Y, color.Z, color.W - alphaVelocity);
        }
    }
}
