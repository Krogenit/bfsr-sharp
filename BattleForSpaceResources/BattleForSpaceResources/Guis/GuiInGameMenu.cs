using BattleForSpaceResources.Networking;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace BattleForSpaceResources.Guis
{
    public class GuiInGameMenu : Gui
    {
        public GuiInGameMenu()
            : base()
        {
            buttons = new Button[4];
            buttons[0] = new Button(Textures.guiButtonBasic, new Vector2(0, -30), Fonts.basicFont, Language.GetString(StringName.BackToGame));
            buttons[1] = new Button(Textures.guiButtonBasic, new Vector2(0, 30), Fonts.basicFont, Language.GetString(StringName.Settings));
            buttons[2] = new Button(Textures.guiButtonBasic, new Vector2(0, 180), Fonts.basicFont, Language.GetString(StringName.InMainMenu));
            buttons[3] = new Button(Textures.guiButtonBasic, new Vector2(0, 90), Fonts.basicFont, Language.GetString(StringName.Statistics));
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].color = GuiInGame.guiColor / 3f;
            }
        }
        public override bool LeftClick()
        {
            if (buttons[0].Rect.Contains(new Point((int)core.inputManager.cursor.Position.X, (int)core.inputManager.cursor.Position.Y)))
            {
                core.currentGui = null;
                return true;
            }
            else if (buttons[1].Rect.Contains(new Point((int)core.inputManager.cursor.Position.X, (int)core.inputManager.cursor.Position.Y)))
            {
                core.currentGui = new GuiSettings(this);
                return true;
            }
            else if (buttons[2].Rect.Contains(new Point((int)core.inputManager.cursor.Position.X, (int)core.inputManager.cursor.Position.Y)))
            {
                ClientNetwork net = ClientNetwork.GetClientNetwork();
                net.Stop();
                return true;
            }
            else if (buttons[3].Rect.Contains(new Point((int)core.inputManager.cursor.Position.X, (int)core.inputManager.cursor.Position.Y)))
            {
                core.currentGui = new GuiStatistics();
                return true;
            }
            else
            {
                return false;
            }
        }
        public override void Escape()
        {
            core.currentGui = null;
        }
        public override void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Textures.guiFon, core.cam.screenCenter, null, new Color(255, 255, 255, 100), 0, new Vector2(Textures.guiFon.Width / 2, Textures.guiFon.Height / 2), Size, SpriteEffects.None, layer);
            spriteBatch.Draw(Textures.logoBFSR, core.cam.screenCenter + new Vector2(0, -200), null, new Color(color), 0, new Vector2(Textures.logoBFSR.Width / 2, Textures.logoBFSR.Height / 2), 0.5f, SpriteEffects.None, layer);
            base.Render(spriteBatch);
        }
    }
}
