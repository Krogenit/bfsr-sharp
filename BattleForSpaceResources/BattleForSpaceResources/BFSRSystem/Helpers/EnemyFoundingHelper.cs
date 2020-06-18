using BattleForSpaceResources.Entitys;
using BattleForSpaceResources.Entitys.BFSRSystem.Helpers;
using BattleForSpaceResources.Guis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleForSpaceResources.BFSRSystem.Helpers
{
    public class EnemyFoundingHelper
    {
        public Ship enemy;
        private List<Ship> selectedShips = new List<Ship>();
        private int timer;
        private float pos, speed = 0.5f;
        private bool change;
        private Core core;
        private World w;
        public EnemyFoundingHelper()
        {
            this.core = Core.GetCore();
            this.w = core.GetWorld();
        }
        public void Update()
        {
            if (core.inputManager.IsKeyReleased(Keys.F))
            {
                if (core.currentGui == null && core.currentGuiAdd == null && !core.isChat)
                {
                    if (selectedShips.Count == w.ships.Count)
                        selectedShips.Clear();
                    if (enemy == null || timer > 0)
                    {
                        int xGrid = (int)((core.cam.screenCenter.X + 40000) / w.grid);
                        int yGrid = (int)((core.cam.screenCenter.Y + 40000) / w.grid);
                        for (int j = 0; j < 9; j++)
                        {
                            int[] xy = CollisionHelper.GetNearXY(j, xGrid, yGrid);
                            int x2 = xy[0];
                            int y2 = xy[1];
                            if (x2 < 0 || y2 < 0 || x2 + 1 > w.shipsGrid.GetLength(0) || y2 + 1 > w.shipsGrid.GetLength(1))
                            { }
                            else
                            {
                                if (w.shipsGrid[x2, y2] != null)
                                {
                                    float lastDis = 0;
                                    for (int i = 0; i < w.shipsGrid[x2, y2].Count; i++)
                                    {
                                        if (!w.shipsGrid[x2, y2][i].shipName.Equals(core.GetPlayerName()))
                                            if (lastDis == 0 && selectedShips.Count == 0)
                                            {
                                                enemy = w.shipsGrid[x2, y2][i];
                                                lastDis = Vector2.Distance(w.shipsGrid[x2, y2][i].Position, core.cam.screenCenter);
                                            }
                                            else
                                            {
                                                float newDis = Vector2.Distance(w.shipsGrid[x2, y2][i].Position, core.cam.screenCenter);
                                                if (selectedShips.Count == 0)
                                                {
                                                    if (newDis < lastDis)
                                                    {
                                                        enemy = w.shipsGrid[x2, y2][i];
                                                        lastDis = newDis;
                                                    }
                                                }
                                                else
                                                {
                                                    bool isRefresh = false;
                                                    for (int k = 0; k < selectedShips.Count; k++)
                                                    {
                                                        if (newDis < lastDis)
                                                        {
                                                            if (w.shipsGrid[x2, y2][i] == selectedShips[k])
                                                            {
                                                                isRefresh = true;
                                                                break;
                                                            }
                                                        }
                                                    }
                                                    if (!isRefresh)
                                                    {
                                                        enemy = w.shipsGrid[x2, y2][i];
                                                        lastDis = newDis;
                                                    }
                                                }
                                            }
                                    }
                                }
                                selectedShips.Add(enemy);
                                timer = 240;
                            }
                        }
                    }
                    else
                    {
                        enemy = null;
                    }
                }
            }
            if (timer > 0)
                timer--;
            else
                selectedShips.Clear();
            if(enemy != null)
            {
                if(change)
                {
                    if(pos < 20)
                    {
                        pos += speed;
                    }
                    else
                    {
                        change = !change;
                    }
                }
                else
                {
                    if (pos > 0)
                    {
                        pos -= speed;
                    }
                    else
                    {
                        change = !change;
                    }
                }
                if (!enemy.isLive)
                    enemy = null;
            }
        }
        public void Render(SpriteBatch spriteBatch)
        {
            if(enemy != null)
            {
                Vector2 origin = new Vector2(Textures.guiSelect.Width/2, Textures.guiSelect.Height/2);
                float width = enemy.Text.Width/2 + pos;
                float height = enemy.Text.Height / 2 + pos;
                spriteBatch.Draw(Textures.guiSelect, enemy.Position + new Vector2(enemy.cos * -width - enemy.cos1 * height, enemy.sin * -width - enemy.sin1 * height), null, new Color(GuiInGame.guiColor), enemy.Rotation, origin, 1, SpriteEffects.None, 0);
                spriteBatch.Draw(Textures.guiSelect, enemy.Position + new Vector2(enemy.cos * width - enemy.cos1 * height, enemy.sin * width - enemy.sin1 * height), null, new Color(GuiInGame.guiColor), enemy.Rotation + MathHelper.ToRadians(90), origin, 1, SpriteEffects.None, 0);
                spriteBatch.Draw(Textures.guiSelect, enemy.Position + new Vector2(enemy.cos * width - enemy.cos1 * -height, enemy.sin * width - enemy.sin1 * -height), null, new Color(GuiInGame.guiColor), enemy.Rotation + MathHelper.ToRadians(180), origin, 1, SpriteEffects.None, 0);
                spriteBatch.Draw(Textures.guiSelect, enemy.Position + new Vector2(enemy.cos * -width - enemy.cos1 * -height, enemy.sin * -width - enemy.sin1 * -height), null, new Color(GuiInGame.guiColor), enemy.Rotation + MathHelper.ToRadians(270), origin, 1, SpriteEffects.None, 0);
            }
        }
    }
}
