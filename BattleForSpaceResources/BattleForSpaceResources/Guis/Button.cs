using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleForSpaceResources.Guis
{
    public class Button : GuiObject
    {
        private Vector4 textColor = GuiInGame.guiColor;
        private Vector2 addPos;
        public Button(Texture2D text, Vector2 pos, SpriteFont newfont, string newMainString) : base(text,pos,newfont,newMainString)
        {
            addPos = pos;
        }
        public override void Update()
        {
            Position = core.cam.screenCenter + addPos;
            base.Update();
        }
        public override void Render(SpriteBatch spriteBatch)
        {
            base.Render(spriteBatch);
            spriteBatch.DrawString(font, stringMain, new Vector2(Position.X -font.MeasureString(stringMain).X / 2, Position.Y - 10), new Color(textColor));
        }
    }
}
