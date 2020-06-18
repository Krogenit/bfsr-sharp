using BattleForSpaceResources.Guis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleForSpaceResources.Guis
{
    public class GuiSelectFaction : Gui
    {
        private float[] shipRot = new float[3];
        public GuiSelectFaction()
            : base()
        {
            buttons = new Button[3];
            buttons[0] = new Button(Textures.guiButtonBasic, new Vector2(-309, 230), Fonts.basicFont, Language.GetString(StringName.Human));
            buttons[1] = new Button(Textures.guiButtonBasic, new Vector2(-1, 230), Fonts.basicFont, Language.GetString(StringName.Engi));
            buttons[2] = new Button(Textures.guiButtonBasic, new Vector2(308, 230), Fonts.basicFont, Language.GetString(StringName.Saimon));
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].color = GuiInGame.guiColor / 3f;
            }
        }
        public override bool LeftClick()
        {
            if (buttons[0].Rect.Contains(new Point((int)core.inputManager.cursor.Position.X, (int)core.inputManager.cursor.Position.Y)))
            {
                ClientPacketSender.SendConfrimSelect(ConfrimType.SelectFaction, 1);
                core.currentGui = null;
                return true;
            }
            else if (buttons[1].Rect.Contains(new Point((int)core.inputManager.cursor.Position.X, (int)core.inputManager.cursor.Position.Y)))
            {
                ClientPacketSender.SendConfrimSelect(ConfrimType.SelectFaction, 3);
                core.currentGui = null;
                return true;
            }
            else if (buttons[2].Rect.Contains(new Point((int)core.inputManager.cursor.Position.X, (int)core.inputManager.cursor.Position.Y)))
            {
                ClientPacketSender.SendConfrimSelect(ConfrimType.SelectFaction, 2);
                core.currentGui = null;
                return true;
            }
            else
            {
                return false;
            }
        }
        public override void Update()
        {
            if (buttons[0].Rect.Contains(new Point((int)core.inputManager.cursor.Position.X, (int)core.inputManager.cursor.Position.Y)))
            {
                shipRot[0] += 0.03f;
                for (int i = 0; i < 3; i++)
                {
                    if (shipRot[i] != shipRot[0] && shipRot[i] > 0)
                    {
                        shipRot[i] -= 0.03f;
                    }
                }
                if(shipRot[0] >= MathHelper.ToRadians(360))
                {
                    shipRot[0] = 0;
                }
            }
            else if (buttons[1].Rect.Contains(new Point((int)core.inputManager.cursor.Position.X, (int)core.inputManager.cursor.Position.Y)))
            {
                shipRot[1] += 0.03f;
                for (int i = 0; i < 3; i++)
                {
                    if (shipRot[i] != shipRot[1] && shipRot[i] > 0)
                    {
                        shipRot[i] -= 0.03f;
                    }
                }
                if (shipRot[1] >= MathHelper.ToRadians(360))
                {
                    shipRot[1] = 0;
                }
            }
            else if (buttons[2].Rect.Contains(new Point((int)core.inputManager.cursor.Position.X, (int)core.inputManager.cursor.Position.Y)))
            {
                shipRot[2] += 0.03f;
                for (int i = 0; i < 3; i++)
                {
                    if (shipRot[i] != shipRot[2] && shipRot[i] > 0)
                    {
                        shipRot[i] -= 0.03f;
                    }
                }
                if (shipRot[2] >= MathHelper.ToRadians(360))
                {
                    shipRot[2] = 0;
                }
            }
            else
            {
                for(int i=0;i<3;i++)
                {
                    if(shipRot[i] > 0)
                    {
                        shipRot[i] -= 0.03f;
                    }
                    if (shipRot[i] < 0)
                    {
                        shipRot[i] = 0;
                    }
                }

            }
            base.Update();
        }
        public override void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Textures.guiFon, core.cam.screenCenter, null, new Color(255, 255, 255, 100), 0, new Vector2(Textures.guiFon.Width / 2, Textures.guiFon.Height / 2), Size, SpriteEffects.None, layer);
            spriteBatch.Draw(Textures.guiSelectFaction, core.cam.screenCenter, null, new Color(GuiInGame.guiColor), 0, new Vector2(Textures.guiSelectFaction.Width / 2, Textures.guiSelectFaction.Height / 2), 1, SpriteEffects.None, layer);
            spriteBatch.Draw(Textures.logoBFSR, core.cam.screenCenter + new Vector2(0, -200), null, new Color(color), 0, new Vector2(Textures.logoBFSR.Width / 2, Textures.logoBFSR.Height / 2), 0.5f, SpriteEffects.None, layer);
            spriteBatch.Draw(Textures.logoText2, core.cam.screenCenter + new Vector2(0, -140), null, new Color(color), 0, new Vector2(Textures.logoText2.Width / 2, Textures.logoText2.Height / 2), 0.5f, SpriteEffects.None, layer);
            spriteBatch.Draw(Textures.shipHumanSmall1, core.cam.screenCenter + new Vector2(-309, 150), null, new Color(color), shipRot[0] + MathHelper.ToRadians(90), new Vector2(Textures.shipHumanSmall1.Width / 2, Textures.shipHumanSmall1.Height / 2), 1f, SpriteEffects.None, layer);
            spriteBatch.Draw(Textures.shipCivSmall1, core.cam.screenCenter + new Vector2(-1, 150), null, new Color(color), shipRot[1] + MathHelper.ToRadians(90), new Vector2(Textures.shipCivSmall1.Width / 2, Textures.shipCivSmall1.Height / 2), 1f, SpriteEffects.None, layer);
            spriteBatch.Draw(Textures.shipEnemySmall1, core.cam.screenCenter + new Vector2(308, 150), null, new Color(color), shipRot[2] + MathHelper.ToRadians(90), new Vector2(Textures.shipEnemySmall1.Width / 2, Textures.shipEnemySmall1.Height / 2), 1f, SpriteEffects.None, layer);
            spriteBatch.DrawString(Fonts.basicFont, Language.GetString(StringName.SelectFaction), core.cam.screenCenter + new Vector2(-100, -120), new Color(GuiInGame.guiColor));
            spriteBatch.DrawString(Fonts.consoleFont, Language.GetString(StringName.HumanDescr), core.cam.screenCenter + new Vector2(-450, -80), new Color(GuiInGame.guiColor));
            spriteBatch.DrawString(Fonts.consoleFont, Language.GetString(StringName.CivDescr), core.cam.screenCenter + new Vector2(-140, -80), new Color(GuiInGame.guiColor));
            spriteBatch.DrawString(Fonts.consoleFont, Language.GetString(StringName.EnemyDescr), core.cam.screenCenter + new Vector2(170, -80), new Color(GuiInGame.guiColor));
            base.Render(spriteBatch);
        }
    }
}
