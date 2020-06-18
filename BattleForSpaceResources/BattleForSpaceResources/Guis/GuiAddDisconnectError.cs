using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BattleForSpaceResources.Guis
{
    public class GuiAddDisconnectError : GuiAdd
    {
        public GuiAddDisconnectError(string sBase, string text)
        {
            textBase = sBase;
            textInfo = text;
            buttons = new Button[1];
            buttons[0] = new Button(Textures.guiButtonSmall, new Vector2(0, 100), Fonts.basicFont, Language.GetString(StringName.Ok));
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].color = GuiInGame.guiColor / 3f;
            }
        }
        public override bool LeftClick()
        {
            if (buttons[0].Rect.Contains(new Point((int)core.inputManager.cursor.Position.X, (int)core.inputManager.cursor.Position.Y)))
            {
                core.currentGuiAdd = null;
                return true;
            }
            return base.LeftClick();
        }
    }
}
