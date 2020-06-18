using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleForSpaceResources.BFSRSystem.Helpers
{
    public class DrawHelper
    {
        public static void DrawLine(Color color, Vector2 position1, Vector2 position2, SpriteBatch spriteBatch, int linesize)
        {
            double distance = Math.Sqrt(((position2.X - position1.X) * (position2.X - position1.X)) + ((position2.Y - position1.Y) * (position2.Y - position1.Y)));
            float angle = -(float)Math.Atan2(position1.X - position2.X, position1.Y - position2.Y) - (float)Math.PI / 2f;
            spriteBatch.Draw(Textures.pixel, position1, new Rectangle(0, 0, (int)distance, linesize), color, angle, new Vector2(0f, 2 / 2f), 1f, SpriteEffects.None, 0);
        }
    }
}
