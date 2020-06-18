using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleForSpaceResources
{
    public static class Fonts
    {
        public static SpriteFont basicFont, consoleFont, mediumFont;
        public static void LoadFonts(ContentManager con)
        {
            basicFont = con.Load<SpriteFont>("font\\basicFont");
            consoleFont = con.Load<SpriteFont>("font\\consoleFont");
            mediumFont = con.Load<SpriteFont>("font\\mediumFont");
            basicFont.Spacing = -1f;
        }
    }
}
