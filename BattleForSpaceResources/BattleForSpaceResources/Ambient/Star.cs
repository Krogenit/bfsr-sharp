using BattleForSpaceResources.Entitys;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleForSpaceResources.Ambient
{
    public class Star : GameObject
    {
        private Vector2 lightOriginal;
        private Vector2 addPosition;
        private float maxSize;
        private bool collide;
        private Core core;
        public Star(Texture2D texture, Vector2 pos, Vector4 color, float size)
            : base(texture, pos)
        {
            this.core = Core.GetCore();
            addPosition = Position = pos;
            this.color = color;
            lightOriginal = new Vector2(Textures.ambientStarLight.Width / 2, Textures.ambientStarLight.Height / 2);
            Size = size;
            maxSize = size;
            layer = 0.99F;
        }
        public override void Update()
        {
            Rotation += 0.001F;
            Position = core.cam.screenCenter + addPosition;
        }
        public void UpdateCollide()
        {
            collide = false;
            World w = core.GetWorld();
            for (int i = 0; i < w.planets.Count; i++)
            {
                CheckCollide(w.planets[i]);
            }
            if (!collide && maxSize < Size)
            {
                maxSize += 0.01f;
            }
        }
        private void CheckCollide(GameObject go)
        {
            float dis = Vector2.Distance(Position,go.Position);
            if(dis < go.Size*(go.Text.Width/2))
            {
                float newSize = (dis + 80 - go.Size * (go.Text.Width / 2)) / 100f;
                collide = true;
                if (maxSize > newSize)
                    maxSize -= 0.015f;
                else
                    maxSize += 0.015f;
            }
            if (maxSize < 0)
                maxSize = 0;
        }
        public override void Render(SpriteBatch spriteBatch)
        {
            base.Render(spriteBatch);
            spriteBatch.Draw(Textures.light, Position, null, new Color(new Vector4(color.X / 2, color.Y / 2, color.Z / 2, 0.5F)), Rotation, new Vector2(Textures.light.Width / 2, Textures.light.Height / 2), Size * 15, SpriteEffects.None, 0);
            spriteBatch.Draw(Textures.light, Position, null, new Color(new Vector4(color.X / 2, color.Y / 2, color.Z / 2, 0.5F)), Rotation, new Vector2(Textures.light.Width / 2, Textures.light.Height / 2), Size * 8, SpriteEffects.None, 0);
        }
        public void RenderLight(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Textures.ambientStarLight, Position, null, new Color(color), Rotation, lightOriginal, maxSize, SpriteEffects.None, 0);
            //spriteBatch.Draw(Textures.light, Position, null, new Color(new Vector4(color.X / 2, color.Y / 2, color.Z / 2, 0.5F)), Rotation, new Vector2(Textures.light.Width / 2, Textures.light.Height / 2), maxSize * 15, SpriteEffects.None, 0);
            spriteBatch.Draw(Textures.light, Position, null, new Color(new Vector4(color.X / 2, color.Y / 2, color.Z / 2, 0.5F)), Rotation, new Vector2(Textures.light.Width / 2, Textures.light.Height / 2), maxSize * 8, SpriteEffects.None, 0);
        }
    }
}
