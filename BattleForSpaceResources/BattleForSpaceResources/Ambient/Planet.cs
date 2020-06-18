using BattleForSpaceResources.Entitys;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleForSpaceResources.Ambient
{
    public class Planet : GameObject
    {
        public Vector2 positionAdd;
        private Vector4 lightColor;
        private float lightRot;
        public float range;
        private int screenNum;
        public bool isPlanet = true;
        public Planet(int type, float sizze, Vector2 newPlanetPos, Vector4 color)
            : base(newPlanetPos)
        {
            Create(Textures.ambientPlanets[type]);
            positionAdd = newPlanetPos;
            lightColor = color;
            Size = sizze;
            range = 1.02f;
            layer = 0.94F;
        }

        public Planet(int type, int screen, Vector2 newPlanetPos)
            : base(newPlanetPos)
        {
            screenNum = screen;
            Create(Textures.ambientMoons[type]);
            positionAdd = newPlanetPos;
            Size = 1F;
            if (screen == 0)
            {
                range = 1.06f;
                layer = 0;
            }
            else if (screen == 1)
            {
                range = 1.04F;
                layer = 1;
            }
            isPlanet = false;
        }
        public override void Update()
        {
            Core core = Core.GetCore();
            World world = core.GetWorld();
            Position = new Vector2(core.cam.screenCenter.X / range + positionAdd.X, core.cam.screenCenter.Y / range + positionAdd.Y);
            if (world.stars.Count > 0)
            {
                var mDx = (world.stars[0].Position.X) - Position.X;
                var mDy = (world.stars[0].Position.Y) - Position.Y;
                lightRot = (float)Math.Atan2(mDy, mDx);
            }
            base.UpdateColor();
        }
        public override void Render(SpriteBatch spriteBatch)
        {
            base.Render(spriteBatch);
            if (isPlanet)
            {
                spriteBatch.Draw(Textures.ambientPlanetLight, Position, null, new Color(lightColor), lightRot, Origin, Size, SpriteEffects.None, 0);
            }
        }
    }
}
