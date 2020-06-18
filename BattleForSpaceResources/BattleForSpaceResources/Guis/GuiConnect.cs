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
    public class GuiConnect : Gui
    {
        private bool isWar;
        public GuiConnect(bool isWar)
            : base()
        {
            this.isWar = isWar;
            if (isWar)
            {
                buttons = new Button[3];
                tBox = new InputBox[2];
                buttons[0] = new Button(Textures.guiButtonBasic, new Vector2(200, 90), Fonts.basicFont, Language.GetString(StringName.Connect));
                buttons[1] = new Button(Textures.guiButtonBasic, new Vector2(-200, 90), Fonts.basicFont, Language.GetString(StringName.Register));
                buttons[2] = new Button(Textures.guiButtonBasic, new Vector2(0, 150), Fonts.basicFont, Language.GetString(StringName.Back));
                //tBox[0] = new TextBox(Textures.guiButtonBasic, new Vector2(200, -80), Fonts.basicFont, Language.GetString(StringName.Ip), 15);
                //tBox[1] = new TextBox(Textures.guiButtonBasic, new Vector2(200, -20), Fonts.basicFont, Language.GetString(StringName.Port), 6);
                tBox[0] = new InputBox(Textures.guiButtonBasic, new Vector2(-0, -30), Fonts.basicFont, Language.GetString(StringName.Login), 20, true);
                tBox[1] = new InputBox(Textures.guiButtonBasic, new Vector2(-0, 30), Fonts.basicFont, Language.GetString(StringName.Password), 20, true);
                string[] s = Settings.LoadLoginInfo();
                if(s!=null)
                {
                    tBox[0].stringInBox = s[0];
                    tBox[1].stringInBox = s[1];
                }
            }
            else
            {
                buttons = new Button[2];
                tBox = new InputBox[3];
                buttons[0] = new Button(Textures.guiButtonBasic, new Vector2(0, 90), Fonts.basicFont, Language.GetString(StringName.Connect));
                buttons[1] = new Button(Textures.guiButtonBasic, new Vector2(0, 150), Fonts.basicFont, Language.GetString(StringName.Back));
                tBox[1] = new InputBox(Textures.guiButtonBasic, new Vector2(-200, 30), Fonts.basicFont, Language.GetString(StringName.Ip), 15, false);
                tBox[2] = new InputBox(Textures.guiButtonBasic, new Vector2(200, 30), Fonts.basicFont, Language.GetString(StringName.Port), 6, true);
                tBox[0] = new InputBox(Textures.guiButtonBasic, new Vector2(-0, -30), Fonts.basicFont, Language.GetString(StringName.Login), 20, true);
                string[] s = Settings.LoadLoginInfo();
                if (s != null)
                {
                    tBox[0].stringInBox = s[0];
                    tBox[1].stringInBox = s[2];
                    tBox[2].stringInBox = s[3];
                }
            }
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].color = GuiInGame.guiColor / 3f;
            }
            for (int i = 0; i < tBox.Length; i++)
            {
                tBox[i].color = GuiInGame.guiColor / 3f;
            }
        }
        public override bool LeftClick()
        {
            ClientNetwork net = ClientNetwork.GetClientNetwork();
            foreach (InputBox b in tBox)
            {
                b.isSelected = false;
            }
            if (isWar)
            {
                if (buttons[0].Rect.Contains(new Point((int)core.inputManager.cursor.Position.X, (int)core.inputManager.cursor.Position.Y)))
                {
                    if (tBox[0].stringInBox != null && tBox[1].stringInBox != null
                        && tBox[0].stringInBox.Length > 0 && tBox[1].stringInBox.Length > 0)
                    {
                        if (!net.IsLogging())
                        {
                            core.SetWorld(new World(true));
                            ClientNetwork.GetClientNetwork().Start();
                            Core.console.AddDebugString("Connecting...");
                            Thread t = new Thread(() => ClientPacketSender.Login(tBox[0].stringInBox, tBox[1].stringInBox));
                            t.Start();
                            Settings.SaveLoginInfo(tBox[0].stringInBox, tBox[1].stringInBox);
                            return true;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
                else if (buttons[1].Rect.Contains(new Point((int)core.inputManager.cursor.Position.X, (int)core.inputManager.cursor.Position.Y)))
                {
                    if (tBox[0].stringInBox != null && tBox[1].stringInBox != null
                        && tBox[0].stringInBox.Length > 0 && tBox[1].stringInBox.Length > 0)
                    {
                        if (!net.IsLogging())
                        {
                            ClientNetwork.GetClientNetwork().Start();
                            Core.console.AddDebugString("Registering...");
                            Thread t = new Thread(() => ClientPacketSender.Register(tBox[0].stringInBox, tBox[1].stringInBox));
                            t.Start();
                            Settings.SaveLoginInfo(tBox[0].stringInBox, tBox[1].stringInBox);
                            return true;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
                else if (buttons[2].Rect.Contains(new Point((int)core.inputManager.cursor.Position.X, (int)core.inputManager.cursor.Position.Y)))
                {
                    core.currentGui = new GuiMainMenu();
                    Settings.SaveLoginInfo(tBox[0].stringInBox, tBox[1].stringInBox);
                    return true;
                }
                else
                {
                    foreach (InputBox b in tBox)
                    {
                        if (b.Rect.Contains(new Point((int)core.inputManager.cursor.Position.X, (int)core.inputManager.cursor.Position.Y)))
                        {
                            b.isSelected = true;
                            return true;
                        }
                    }
                }
                return false;
            }
            else
            {
                if (buttons[0].Rect.Contains(new Point((int)core.inputManager.cursor.Position.X, (int)core.inputManager.cursor.Position.Y)))
                {
                    if (tBox[0].stringInBox != null && tBox[1].stringInBox != null && tBox[2].stringInBox != null
                        && tBox[0].stringInBox.Length > 0 && tBox[1].stringInBox.Length > 0 && tBox[2].stringInBox.Length > 0)
                    {
                        if (!net.IsLogging())
                        {
                            Core.console.AddDebugString("Connecting...");
                            core.SetWorld(new World(true));
                            core.currentGui = null;
                            ClientNetwork.GetClientNetwork().Start();
                            Thread t = new Thread(() => ClientPacketSender.Connect(tBox[0].stringInBox, tBox[1].stringInBox, int.Parse(tBox[2].stringInBox)));
                            t.Start();
                            Settings.SaveLoginInfo(tBox[0].stringInBox, tBox[1].stringInBox, tBox[2].stringInBox);
                            return true;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
                else if (buttons[1].Rect.Contains(new Point((int)core.inputManager.cursor.Position.X, (int)core.inputManager.cursor.Position.Y)))
                {
                    core.currentGui = new GuiMainMenu();
                    Settings.SaveLoginInfo(tBox[0].stringInBox, tBox[1].stringInBox, tBox[2].stringInBox);
                    return true;
                }
                else
                {
                    foreach (InputBox b in tBox)
                    {
                        if (b.Rect.Contains(new Point((int)core.inputManager.cursor.Position.X, (int)core.inputManager.cursor.Position.Y)))
                        {
                            b.isSelected = true;
                            return true;
                        }
                    }
                }
                return false;
            }
        }
        public override void Escape()
        {
            core.currentGui = new GuiMainMenu();
        }
        public override void Update()
        {
            base.Update();
        }
        public override void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Textures.guiFon, core.cam.screenCenter, null, new Color(color), 0, new Vector2(Textures.guiFon.Width / 2, Textures.guiFon.Height / 2), Size, SpriteEffects.None, layer);
            spriteBatch.Draw(Textures.logoBFSR, core.cam.screenCenter + new Vector2(0, -200), null, new Color(color), 0, new Vector2(Textures.logoBFSR.Width / 2, Textures.logoBFSR.Height / 2), 0.5f, SpriteEffects.None, layer);
            spriteBatch.Draw(Textures.logoText2, core.cam.screenCenter + new Vector2(0, -140), null, new Color(color), 0, new Vector2(Textures.logoText2.Width / 2, Textures.logoText2.Height / 2), 0.5f, SpriteEffects.None, layer);
            base.Render(spriteBatch);
            if (core.inputManager.IsCapsLocked())
                spriteBatch.DrawString(Fonts.basicFont, "Caps Lock", Position + new Vector2(-52,10), new Color(1f, 0.3f, 0.3f, 1));
        }
    }
}
