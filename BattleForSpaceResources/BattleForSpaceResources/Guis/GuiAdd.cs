using BattleForSpaceResources.Entitys;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleForSpaceResources.Guis
{
    public class GuiAdd : GameObject
    {
        public string textBase;
        public string textInfo;
        public Button[] buttons;
        public GuiAdd():base(Textures.pixel,Vector2.Zero)
        {
            Position = core.cam.screenCenter;
        }
        public virtual bool LeftClick()
        {
            return false;
        }
        public override void Update()
        {
            Position = core.cam.screenCenter;

            if (buttons != null)
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].Update();
                }
            core.isChat = false;
        }
        public override void Render(SpriteBatch spriteBatch)
        {
            Rectangle rect = new Rectangle(0, 0, Textures.guiAdd.Width, Textures.guiAdd.Height);
            spriteBatch.Draw(Textures.guiFon, Position, rect, new Color(new Vector4(0.75f,0.75f,0.75f,0.75f)), 0, new Vector2(Textures.guiAdd.Width / 2, Textures.guiAdd.Height / 2), 1, SpriteEffects.None, 0);
            spriteBatch.Draw(Textures.guiAdd, Position, null, new Color(GuiInGame.guiColor), 0, new Vector2(Textures.guiAdd.Width / 2, Textures.guiAdd.Height / 2), 1, SpriteEffects.None, 0);
            if (textBase != null)
                spriteBatch.DrawString(Fonts.basicFont, textBase, Position + new Vector2(-Textures.guiAdd.Width / 2 + 20, -Textures.guiAdd.Height / 2 + 15), new Color(GuiInGame.guiColor));
            if (textInfo != null)
                        spriteBatch.DrawString(Fonts.basicFont, textInfo, Position + new Vector2(-Textures.guiAdd.Width / 2 + 20, -Textures.guiAdd.Height / 2 + 60), new Color(GuiInGame.guiColor));
            if (buttons != null)
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].Render(spriteBatch);
                }
            //base.Render(spriteBatch);
        }
    }
}
