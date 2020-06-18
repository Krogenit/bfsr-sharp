using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleForSpaceResources.Collision
{
    public static class V2Extend
    {
        public static Vector2 Perp(this Vector2 self)
        {
            return new Vector2(-self.Y, self.X);
        }

        public static float Dot(this Vector2 self, Vector2 vector)
        {
            return self.X * vector.X + self.Y * vector.Y;
        }

        public static Vector2 Rotate(this Vector2 self, Vector2 vector)
        {
            return new Vector2(self.X * vector.X - self.Y * vector.Y, self.X * vector.Y + self.Y * vector.X);
        }

        public static Vector2 Normalize2(this Vector2 self)
        {
            Vector2 vector = self;
            vector.Normalize();
            return vector;
        }
    }
}
