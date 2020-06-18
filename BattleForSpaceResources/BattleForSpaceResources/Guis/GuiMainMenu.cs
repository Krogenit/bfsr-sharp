using BattleForSpaceResources.Entitys;
using BattleForSpaceResources.Networking;
using BattleForSpaceResources.ShipComponents;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace BattleForSpaceResources.Guis
{
    public class GuiMainMenu : Gui
    {
        public GuiMainMenu() : base()
        {
            buttons = new Button[5];
            if(Settings.isDebug)
            {
                buttons = new Button[6];
            }
            buttons[0] = new Button(Textures.guiButtonBasic, new Vector2(0, -50), Fonts.basicFont, Language.GetString(StringName.Singleplayer));
            buttons[1] = new Button(Textures.guiButtonBasic, new Vector2(0, 0), Fonts.basicFont, Language.GetString(StringName.Multiplayer));
            buttons[2] = new Button(Textures.guiButtonBasic, new Vector2(0, 50), Fonts.basicFont, Language.GetString(StringName.BattleForRes));
            buttons[3] = new Button(Textures.guiButtonBasic, new Vector2(0, 100), Fonts.basicFont, Language.GetString(StringName.Settings));
            buttons[4] = new Button(Textures.guiButtonBasic, new Vector2(0, 150), Fonts.basicFont, Language.GetString(StringName.Quit));
            if(Settings.isDebug)
            {
                buttons[5] = new Button(Textures.guiButtonBasic, new Vector2(-300, 0), Fonts.basicFont, "Debug");
            }
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].color = GuiInGame.guiColor / 3f;
            }
        }
        public override bool LeftClick()
        {
            if (buttons[0].Rect.Contains(new Point((int)core.inputManager.cursor.Position.X, (int)core.inputManager.cursor.Position.Y)))
            {
                core.SetWorld(new World(true));
                core.currentGui = null;
                Thread t = new Thread(StartLocalServer);
                t.Start();
                Core.console.AddDebugString("Starting client world...");
                ClientNetwork.GetClientNetwork().Start();
                return true;
            }
            else if (buttons[1].Rect.Contains(new Point((int)core.inputManager.cursor.Position.X, (int)core.inputManager.cursor.Position.Y)))
            {
                core.currentGui = new GuiConnect(false);
                return true;
            }
            else if (buttons[2].Rect.Contains(new Point((int)core.inputManager.cursor.Position.X, (int)core.inputManager.cursor.Position.Y)))
            {
                core.currentGui = new GuiConnect(true);
                return true;
            }
            else if (buttons[3].Rect.Contains(new Point((int)core.inputManager.cursor.Position.X, (int)core.inputManager.cursor.Position.Y)))
            {
                core.currentGui = new GuiSettings(this);
                return true;
            }
            else if (buttons[4].Rect.Contains(new Point((int)core.inputManager.cursor.Position.X, (int)core.inputManager.cursor.Position.Y)))
            {
                core.isExit = true;
                if (ServerCore.GetServerCore() != null && ServerCore.GetServerCore().GetServer().Status == NetPeerStatus.Running)
                {
                    Core.console.AddDebugString("Closing server...");
                    Thread t = new Thread(() => ServerCore.GetServerCore().Stop());
                    t.Start();
                    //ServerCore.instance.Stop();
                }
                return true;
            }
            else if (Settings.isDebug && buttons[5].Rect.Contains(new Point((int)core.inputManager.cursor.Position.X, (int)core.inputManager.cursor.Position.Y)))
            {
                core.SetWorld(new World(true));
                int[] components = { 1, 1, 1, 1, 1, 1, 1 };
                Ship s = new Ship(Vector2.Zero, Vector2.Zero, Faction.Human, ShipType.HumanSmall1, components, world, 0, 0);
                s.shipName = "Debug";
                GunSlot[] gs = { new GunSlot(GunType.PlasmSmall, s), null };
                s.AddGuns(gs);
                world.ships.Add(s);
                core.currentGui = new GuiSelectFaction();
                return true;
            }
            else
            {
                return false;
            }
        }
        private void StartLocalServer()
        {
            ServerCore sc = new ServerCore(false);
            Thread.Sleep(300);
            Thread t = new Thread(() => ClientPacketSender.Login("Player", "PassClient"));
            t.Start();
        }
        public override void Update()
        {
            base.Update();
        }
        public override void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Textures.guiFon, core.cam.screenCenter, null, new Color(color), 0, new Vector2(Textures.guiFon.Width / 2, Textures.guiFon.Height / 2), Size, SpriteEffects.None, layer);
            spriteBatch.Draw(Textures.logoBFSR, core.cam.screenCenter + new Vector2(0,-200), null, new Color(color), 0, new Vector2(Textures.logoBFSR.Width / 2, Textures.logoBFSR.Height / 2), 0.5f, SpriteEffects.None, layer);
            spriteBatch.Draw(Textures.logoText2, core.cam.screenCenter + new Vector2(0, -140), null, new Color(color), 0, new Vector2(Textures.logoText2.Width / 2, Textures.logoText2.Height / 2), 0.5f, SpriteEffects.None, layer);
            base.Render(spriteBatch);
        }
    }
}
