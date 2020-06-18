using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleForSpaceResources.Guis
{
    public class GuiStatistics : Gui
    {
        public GuiStatistics()
        {

        }

        public override void Escape()
        {
            core.currentGui = null;
        }
        public override void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Textures.guiFon, core.cam.screenCenter, null, new Color(255, 255, 255, 100), 0, new Vector2(Textures.guiFon.Width / 2, Textures.guiFon.Height / 2), Size, SpriteEffects.None, layer);
            DrawRectangle(new Color(GuiInGame.guiColor), new Vector2(-200, -200) + Position, new Vector2(200, 200) + Position, spriteBatch, 3);
            //base.Render(spriteBatch);
        }
    }
}
