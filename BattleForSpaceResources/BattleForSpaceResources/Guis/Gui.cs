using BattleForSpaceResources.Entitys;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleForSpaceResources.Guis
{
    public class Gui : GameObject
    {
        public Button[] buttons;
        public InputBox[] tBox;
        public Slider[] sliders;
        public bool[] isMoveSlider;
        public Gui previousGui;
        protected Core core = Core.GetCore();
        protected World world = Core.GetCore().GetWorld();
        public Gui() : base(Textures.pixel,Vector2.Zero)
        {
            Position = core.cam.screenCenter;
        }
        public void RestartButtons()
        {
            if (buttons != null)
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].color = GuiInGame.guiColor / 3f;
            }
            if (tBox != null)
            for (int i = 0; i < tBox.Length; i++)
            {
                tBox[i].color = GuiInGame.guiColor / 3f;
            }
            if (sliders != null)
            for (int i = 0; i < sliders.Length; i++)
            {
                sliders[i].button.color = GuiInGame.guiColor / 3f;
            }
        }
        public override void Update()
        {
            Position = core.cam.screenCenter;
            if (buttons != null)
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].Update();
                }
            if (tBox != null)
                for (int i = 0; i < tBox.Length; i++)
                {
                    tBox[i].Update();
                }
            if (sliders != null)
            {
                foreach (Slider s in sliders)
                {
                    if (s != null)
                        s.Update();
                }
                if (core.inputManager.IsLeftButtonDown())
                {
                    for (int i = 0; i < sliders.Length; i++)
                    {
                        if (sliders[i].Rect.Contains(new Point((int)core.inputManager.cursor.Position.X, (int)core.inputManager.cursor.Position.Y)))
                        {
                            bool isNoraml = true;
                            for (int j = 0; j < sliders.Length; j++)
                                if (isMoveSlider[j] == true)
                                    isNoraml = false;
                            if (isNoraml)
                                isMoveSlider[i] = true;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < sliders.Length; i++)
                    {
                        isMoveSlider[i] = false;
                    }
                }
            }
            core.isChat = false;
        }
        public virtual bool LeftClick()
        {
            return false;
        }
        public virtual bool RightClick()
        {
            return false;
        }
        public virtual void Escape()
        {

        }
        public override void Render(SpriteBatch spriteBatch)
        {
            if (buttons != null)
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].Render(spriteBatch);
            }
            if (tBox != null)
            for (int i = 0; i < tBox.Length; i++)
            {
                tBox[i].Render(spriteBatch);
            }
            if(sliders != null)
                foreach (Slider s in sliders)
                {
                    if (s != null)
                        s.Render(spriteBatch);
                }
            //base.Render(spriteBatch);
        }
    }
}
