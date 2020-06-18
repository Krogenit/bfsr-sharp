using BattleForSpaceResources.Networking;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace BattleForSpaceResources.Guis
{
    public class GuiAddDestroyed : GuiAdd
    {
        public GuiAddDestroyed(string text, string text2)
        {
            textBase = text;
            textInfo = text2;
            buttons = new Button[2];
            buttons[0] = new Button(Textures.guiButtonSmall, new Vector2(-185, 100), Fonts.basicFont, Language.GetString(StringName.Respawn));
            buttons[1] = new Button(Textures.guiButtonSmall, new Vector2(185, 100), Fonts.basicFont, Language.GetString(StringName.Quit));
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].color = GuiInGame.guiColor / 3f;
            }
        }
        public override bool LeftClick()
        {
            if (buttons[0].Rect.Contains(new Point((int)core.inputManager.cursor.Position.X, (int)core.inputManager.cursor.Position.Y)))
            {
                ClientPacketSender.SendRespawn();
                core.currentGuiAdd = null;
                core.cam.control = false;
                return true;
            }
            else if (buttons[1].Rect.Contains(new Point((int)core.inputManager.cursor.Position.X, (int)core.inputManager.cursor.Position.Y)))
            {
                ClientNetwork net = ClientNetwork.GetClientNetwork();
                net.Stop();
                core.currentGuiAdd = null;
                return true;
            }
            return false;
        }
    }
}