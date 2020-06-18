using BattleForSpaceResources.Entitys;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleForSpaceResources.Ambient
{
    public enum SmokeScreen
    {
        Normal = 1, Fon = 2
    }
    public class Smoke : GameObject
    {
        private int smokeType;
        public SmokeScreen smokeScreen;
        private float SmokeX, SmokeY;
        public Smoke(SmokeScreen smokeScreenType, Vector2 position, float size, int type)
            : base(position)
        {
            smokeType = type;
            Size = size;
            smokeScreen = smokeScreenType;
            if (smokeType == 0)
            {
                Create(Textures.ambientClouds[0]);
                color = new Vector4(0.25F, 0.25f, 0.25f, 1);
            }
            if (smokeScreenType == SmokeScreen.Fon)
            {
                SmokeX = position.X;
                SmokeY = position.Y;
                Rotation = MathHelper.ToRadians(270);
            }
        }

        public override void Update()
        {
            Core core = Core.GetCore();
            if (smokeScreen == SmokeScreen.Fon)
            {
                Position = new Vector2(core.cam.screenCenter.X / 1.5F + SmokeX, core.cam.screenCenter.Y / 1.5F + SmokeY);
                if (core.cam.screenCenter.X - Position.X >= 2048 * 1.5F)
                {
                    SmokeX = SmokeX + 2048 * 3;
                }
                else if (core.cam.screenCenter.X - Position.X <= -2048 * 1.5F)
                {
                    SmokeX = SmokeX - 2048 * 3;
                }
                if (core.cam.screenCenter.Y - Position.Y >= 2048 * 1.5F)
                {
                    SmokeY = SmokeY + 2048 * 3;
                }
                else if (core.cam.screenCenter.Y - Position.Y <= -2048 * 1.5F)
                {
                    SmokeY = SmokeY - 2048 * 3;
                }
            }
            else
            {
                if (core.cam.screenCenter.X - Position.X >= 2048 * 1.5F)
                {
                    Position.X = Position.X + 2048 * 3;
                }
                else if (core.cam.screenCenter.X - Position.X <= -2048 * 1.5F)
                {
                    Position.X = Position.X - 2048 * 3;
                }
                if (core.cam.screenCenter.Y - Position.Y >= 2048 * 1.5F)
                {
                    Position.Y = Position.Y + 2048 * 3;
                }
                else if (core.cam.screenCenter.Y - Position.Y <= -2048 * 1.5F)
                {
                    Position.Y = Position.Y - 2048 * 3;
                }
            }
            UpdateColor();
        }
        public override void UpdateColor()
        {
            World world = Core.GetCore().GetWorld();
            if (world.stars.Count > 0)
            {
                float del = 1.5f;
                float baseColor = 0.0f;
                color = new Vector4(baseColor + world.stars[0].color.X / del, baseColor + world.stars[0].color.Y / del, baseColor + world.stars[0].color.Z / del, 1);
            }
        }
    }
}
