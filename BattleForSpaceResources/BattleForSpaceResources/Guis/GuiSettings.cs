using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleForSpaceResources.Guis
{
    public class GuiSettings : Gui
    {
        private string[] veriables = new string[8], strings = new string[8];
        public GuiSettings(Gui prevGui)
            : base()
        {
            previousGui = prevGui;
            sliders = new Slider[2];
            isMoveSlider = new bool[2];
            sliders[0] = new Slider(new Vector2(200, -120), Language.GetString(StringName.SoundVolume), Settings.soundVolume,1);
            sliders[1] = new Slider(new Vector2(-200, -60), Language.GetString(StringName.MouseSpeed), Settings.mouseSpeed,2);
            buttons = new Button[5];
            buttons[0] = new Button(Textures.guiButtonBasic, new Vector2(-200, 180), Fonts.basicFont, Language.GetString(StringName.Language));
            buttons[1] = new Button(Textures.guiButtonBasic, new Vector2(-200, -120), Fonts.basicFont, Language.GetString(StringName.UseSystemCursor));
            buttons[2] = new Button(Textures.guiButtonBasic, new Vector2(200, 120), Fonts.basicFont, Language.GetString(StringName.RenderBodys));
            buttons[3] = new Button(Textures.guiButtonBasic, new Vector2(200, 180), Fonts.basicFont, Language.GetString(StringName.Debug));
            buttons[4] = new Button(Textures.guiButtonBasic, new Vector2(0, 240), Fonts.basicFont, Language.GetString(StringName.Save));
            for (int i = 0; i < buttons.Length; i++)
            {
                strings[i] = buttons[i].stringMain;
                buttons[i].color = GuiInGame.guiColor / 3f;
            }
            for (int i = 0; i < sliders.Length; i++)
            {
                sliders[i].button.color = GuiInGame.guiColor / 3f;
            }
            strings[6] = sliders[0].button.stringMain;
            strings[7] = sliders[1].button.stringMain;
            Update();
        }
        public override bool LeftClick()
        {
            if (buttons[0].Rect.Contains(new Point((int)core.inputManager.cursor.Position.X, (int)core.inputManager.cursor.Position.Y)))
            {
                if(Settings.language.Equals("ru"))
                {
                    Settings.language = "en";
                }
                else
                {
                    Settings.language = "ru";
                }
                core.currentGui = new GuiSettings((previousGui != null && previousGui.GetType() == typeof(GuiMainMenu)) ? new GuiMainMenu() : null);
                return true;
            }
            else if (buttons[1].Rect.Contains(new Point((int)core.inputManager.cursor.Position.X, (int)core.inputManager.cursor.Position.Y)))
            {
                Settings.isUseSystemCursor = !Settings.isUseSystemCursor;
                return true;
            }
            else if (buttons[2].Rect.Contains(new Point((int)core.inputManager.cursor.Position.X, (int)core.inputManager.cursor.Position.Y)))
            {
                Settings.isRenderBodys = !Settings.isRenderBodys;
                return true;
            }
            else if (buttons[3].Rect.Contains(new Point((int)core.inputManager.cursor.Position.X, (int)core.inputManager.cursor.Position.Y)))
            {
                Settings.isDebug = !Settings.isDebug;
                Core.console = new BFSRSystem.BFSRConsole();
                return true;
            }
            else if (buttons[4].Rect.Contains(new Point((int)core.inputManager.cursor.Position.X, (int)core.inputManager.cursor.Position.Y)))
            {
                Settings.SaveSettings();
                core.currentGui = previousGui;
                return true;
            }
            else if (sliders[0].button.Rect.Contains(new Point((int)core.inputManager.cursor.Position.X, (int)core.inputManager.cursor.Position.Y)))
            {
                Settings.soundVolume = sliders[0].Move();
                return true;
            }
            else if (sliders[1].button.Rect.Contains(new Point((int)core.inputManager.cursor.Position.X, (int)core.inputManager.cursor.Position.Y)))
            {
                Settings.mouseSpeed = sliders[1].Move();
                return true;
            }
            return false;
        }
        public override void Escape()
        {
            Settings.SaveSettings();
            core.currentGui = previousGui;
        }
        public override void Update()
        {
            veriables[0] = ": " + Settings.language;
            veriables[1] = ": " + (Settings.isUseSystemCursor ? Language.GetString(StringName.Yes) : Language.GetString(StringName.No));
            veriables[2] = ": " + (Settings.isRenderBodys ? Language.GetString(StringName.Yes) : Language.GetString(StringName.No));
            veriables[3] = ": " + (Settings.isDebug ? Language.GetString(StringName.Yes) : Language.GetString(StringName.No));
            veriables[6] = ": " + string.Format("{0:0.0}", Settings.soundVolume);
            veriables[7] = ": " + string.Format("{0:0.0}", Settings.mouseSpeed);
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].stringMain = strings[i] + veriables[i];
            }
            sliders[0].button.stringMain = strings[6] + veriables[6];
            sliders[1].button.stringMain = strings[7] + veriables[7];
            if (isMoveSlider[0])
                Settings.soundVolume = sliders[0].Move();
            else if (isMoveSlider[1])
                Settings.mouseSpeed = sliders[1].Move();
            base.Update();
        }
        public override void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Textures.guiFon, core.cam.screenCenter, null, new Color(255, 255, 255, 100), 0, new Vector2(Textures.guiFon.Width / 2, Textures.guiFon.Height / 2), Size, SpriteEffects.None, layer);
            spriteBatch.DrawString(Fonts.basicFont, Language.GetString(StringName.Settings), core.cam.screenCenter + new Vector2(0 - Fonts.basicFont.MeasureString(Language.GetString(StringName.Settings)).X / 2, -240), new Color(GuiInGame.guiColor));
            spriteBatch.DrawString(Fonts.consoleFont, Language.GetString(StringName.Mouse), core.cam.screenCenter + new Vector2(-200 - Fonts.consoleFont.MeasureString(Language.GetString(StringName.Mouse)).X / 2, -180), new Color(GuiInGame.guiColor));
            spriteBatch.DrawString(Fonts.consoleFont, Language.GetString(StringName.Sound), core.cam.screenCenter + new Vector2(200 - Fonts.consoleFont.MeasureString(Language.GetString(StringName.Sound)).X / 2, -180), new Color(GuiInGame.guiColor));
            spriteBatch.DrawString(Fonts.consoleFont, Language.GetString(StringName.Debug), core.cam.screenCenter + new Vector2(200 - Fonts.consoleFont.MeasureString(Language.GetString(StringName.Debug)).X / 2, 60), new Color(GuiInGame.guiColor));
            base.Render(spriteBatch);
        }
    }
}
