using BattleForSpaceResources.Entitys;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleForSpaceResources.Guis
{
    public class GuiObject : GameObject
    {
        public string stringMain;
        public SpriteFont font;
        private float speedColor = 20f;
        public bool isSelected, isPlayed;
        public GuiObject(Texture2D text, Vector2 pos, SpriteFont newFont, string newMainString) : base(text,pos)
        {
            stringMain = newMainString;
            font = newFont;
            Position = core.cam.screenCenter + pos;
        }
        public override void Update()
        {
            if (Rect.Contains(new Point((int)core.inputManager.cursor.Position.X, (int)core.inputManager.cursor.Position.Y)))
            {
                Collide(true);
                if(!isPlayed)
                {
                    Sounds.Play(Sounds.buttonCollide, Settings.soundVolume / 5f, core.random.Next(-2, 2) / 100f);
                    isPlayed = true;
                }
            }
            else
            {
                isPlayed = false;
                Collide(false);
            }

            base.Update();
        }
        public void Collide(bool a)
        {
            float minColor = 3f;
            if (a)
            {
                if (color.W < GuiInGame.guiColor.W)
                {
                    color.W += GuiInGame.guiColor.W / speedColor;
                }
                if (color.X < GuiInGame.guiColor.X)
                {
                    color.X += GuiInGame.guiColor.X / speedColor;
                }
                if (color.Y < GuiInGame.guiColor.Y)
                {
                    color.Y += GuiInGame.guiColor.Y / speedColor;
                }
                if (color.Z < GuiInGame.guiColor.Z)
                {
                    color.Z += GuiInGame.guiColor.Z / speedColor;
                }
                if (color.W > 1)
                {
                    color.W = 1;
                }
                if (color.X > 1)
                {
                    color.X = 1;
                }
                if (color.Y > 1)
                {
                    color.Y = 1;
                }
                if (color.Z > 1)
                {
                    color.Z = 1;
                }
            }
            else
            {
                if (color.W > GuiInGame.guiColor.W / minColor)
                {
                    color.W -= GuiInGame.guiColor.W / speedColor;
                }
                if (color.X > GuiInGame.guiColor.X / minColor)
                {
                    color.X -= GuiInGame.guiColor.X / speedColor;
                }
                if (color.Y > GuiInGame.guiColor.Y / minColor)
                {
                    color.Y -= GuiInGame.guiColor.Y / speedColor;
                }
                if (color.Z > GuiInGame.guiColor.Z / minColor)
                {
                    color.Z -= GuiInGame.guiColor.Z / speedColor;
                }
                if (color.W < 0)
                {
                    color.W = 0;
                }
                if (color.X < 0)
                {
                    color.X = 0;
                }
                if (color.Y < 0)
                {
                    color.Y = 0;
                }
                if (color.Z < 0)
                {
                    color.Z = 0;
                }
            }
        }
    }
}
