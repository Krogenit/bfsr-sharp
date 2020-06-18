using BattleForSpaceResources.Entitys;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleForSpaceResources.Guis
{
    public class Slider : GameObject
    {
        public Vector2 position;
        public Vector2 minPos, addPos;
        public Button button;
        private int factor;
        private float savePos;
        public Slider(Vector2 pos, String text, float savedPos, int f)
            : base(pos)
        {
            savePos = savedPos;
            addPos = pos;
            factor = f;
            minPos = pos + core.cam.screenCenter;
            button = new Button(Textures.guiButtonBasic, pos, Fonts.basicFont, text);
            position.X = (savedPos * 236/f + button.Position.X - 118);
            position.Y = pos.Y;
            Create(Textures.guiSlider);
            Position = position;
            color = GuiInGame.guiColor;
        }
        public override void Update()
        {
            minPos = addPos + core.cam.screenCenter;
            //Position = position;
            Position.X = position.X = (savePos * 236 / factor + button.Position.X - 118);
            button.Update();
            Position.Y = button.Position.Y;
            color = button.color;
            base.Update();
        }
        public float Move()
        {
            position.X = core.inputManager.cursor.Position.X;
            minPos = addPos + core.cam.screenCenter;
            if (position.X < minPos.X - Textures.guiButtonBasic.Width / 2F + 32)
                position.X = minPos.X - Textures.guiButtonBasic.Width / 2F + 32;
            else if (position.X > minPos.X + Textures.guiButtonBasic.Width / 2F - 32)
                position.X = minPos.X + Textures.guiButtonBasic.Width / 2F - 32;
            float f = (position.X - button.Position.X + 118) / 236;
            savePos = f * factor;
            Position.X = position.X = (savePos * 236 / factor + button.Position.X - 118);
            button.Update();
            Position.Y = button.Position.Y;
            return f < 0 ? 0 : f * factor;
        }
        public override void Render(SpriteBatch sb)
        {
            button.Render(sb);
            base.Render(sb);
        }
    }
}
