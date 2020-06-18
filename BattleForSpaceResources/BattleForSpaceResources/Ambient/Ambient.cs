using BattleForSpaceResources.Entitys;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleForSpaceResources.Ambient
{
    public class Ambients : GameObject
    {
        public Vector2 addPosition;
        public float range;
        public float size;
        public int screen;
        private float rotationSpeed;
        public Ambients(int type, int numScreen, Vector2 pos)
            : base(pos)
        {
            screen = numScreen;
            if (numScreen == 0)
            {
                range = 1.2F;
                size = 1F;
            }
            else if (numScreen == 1)
            {
                range = 1.1F;
                size = 1F;
            }
            else if (numScreen == 2)
            {
                range = 1.075F;
                size = 1F;
            }
            addPosition = pos;
            //Text = Textures.textureAmbient[type];
            Rotation = MathHelper.ToRadians(core.random.Next(360));
            rotationSpeed = core.random.Next(-10, 10) / 50000F;
            //base.Create();
        }
        public override void Update()
        {
            Position = new Vector2(core.cam.screenCenter.X / range, core.cam.screenCenter.Y / range) + addPosition;
            Rotation += rotationSpeed;
        }
        public override void Render(SpriteBatch spriteBatch)
        {
            base.Render(spriteBatch);
        }
    }
}
