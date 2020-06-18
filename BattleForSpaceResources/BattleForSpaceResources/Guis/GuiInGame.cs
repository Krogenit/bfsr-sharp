using BattleForSpaceResources.BFSRSystem.Helpers;
using BattleForSpaceResources.Entitys;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BattleForSpaceResources.Guis
{
    public class GuiInGame
    {
        private Vector2 armorPlateOriginal,leftCurrentGuiPos, rightCurrentGuiPos, maxGuiPos, position, fonVector, hudShip;
        public List<Ship> selShips = new List<Ship>();
        public GameObject[] selSlots = new GameObject[8];
        public int pageNum = 1;
        public CollisionObject selectedObject;
        private GameObject guiHudShip, guiSelectedShips, guiHudShipAdd, guiShield, guiMap, guiMap1, guiInfo;
        public Button controlButton, leftSelShips, rightSelShips;
        private Ship controlledShip;
        public Chat chat = new Chat();
        public Rectangle selectRect = new Rectangle();
        public static Vector4 guiColor = new Vector4(0.0f, 0.5f, 1, 1);
        private EnemyFoundingHelper efh;
        private Core core = Core.GetCore();
        private World world;
        public GuiInGame(World world)
        {
            this.world = world;
            leftCurrentGuiPos.X = -315;
            rightCurrentGuiPos.X = 315;
            armorPlateOriginal = new Vector2(Textures.guiAmorPlate.Width / 2, Textures.guiAmorPlate.Height / 2);
            controlButton = new Button(Textures.guiButtonControl, Vector2.Zero, Fonts.mediumFont, "Управлять кораблем");
            leftSelShips = new Button(Textures.guiButtonSmallStrelka, Vector2.Zero, Fonts.consoleFont, "");
            rightSelShips = new Button(Textures.guiButtonSmallStrelka, Vector2.Zero, Fonts.consoleFont, "");
            guiHudShip = new GameObject(Textures.guiHudShip,Vector2.Zero);
            guiHudShip.color = Vector4.Zero;
            guiSelectedShips = new GameObject(Textures.guiSelectedShips, Vector2.Zero);
            guiSelectedShips.color = Vector4.Zero;
            guiHudShipAdd = new GameObject(Textures.guiHudShipAdd[0], Vector2.Zero);
            guiHudShipAdd.color = Vector4.Zero;
            guiShield = new GameObject(Textures.guiShield, Vector2.Zero);
            guiShield.color = Vector4.Zero;
            guiMap = new GameObject(Textures.guiMap[1], Vector2.Zero);
            guiMap.color = Vector4.Zero;
            guiMap1 = new GameObject(Textures.guiMap[0], Vector2.Zero);
            guiMap1.color = Vector4.Zero;
            guiInfo = new GameObject(Textures.guiInfo, Vector2.Zero);
            guiInfo.color = Vector4.Zero;

            controlButton.color = GuiInGame.guiColor / 2f;
            leftSelShips.color = GuiInGame.guiColor / 2f;
            rightSelShips.color = GuiInGame.guiColor / 2f;
            efh = new EnemyFoundingHelper();
        }
        public void Update()
        {
            position = core.cam.screenCenter;
            fonVector = rightCurrentGuiPos + new Vector2(core.cam.screenCenter.X + core.screenWidth / 2F - 155, core.cam.screenCenter.Y + core.screenHeight / 2F - 125);
            if (core.inputManager.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.G))
            {
                if (core.currentGuiAdd == null && core.currentGui == null && !core.isChat)
                if (maxGuiPos == Vector2.Zero)
                {
                    maxGuiPos = new Vector2(-315, 0);
                }
                else
                {
                    maxGuiPos = Vector2.Zero;
                }
            }
            if (maxGuiPos != Vector2.Zero || leftCurrentGuiPos.X < 0)
            {
                if (leftCurrentGuiPos.X < maxGuiPos.X)
                {
                    leftCurrentGuiPos.X += 5F;
                    rightCurrentGuiPos.X -= 5F;
                    if (leftCurrentGuiPos.X > 0)
                    {
                        leftCurrentGuiPos.X = 0;
                        rightCurrentGuiPos.X = 0;
                    }
                }
                else
                {
                    leftCurrentGuiPos.X -= 5F;
                    rightCurrentGuiPos.X += 5F;
                    if (leftCurrentGuiPos.X < maxGuiPos.X)
                    {
                        leftCurrentGuiPos.X = maxGuiPos.X;
                        rightCurrentGuiPos.X = -maxGuiPos.X;
                    }
                }
            }
            if (world.selectedShips.Count > 1)
            {
                selShips.Clear();
                for (int i = 0; i < world.selectedShips.Count; i++)
                {
                    if (world.selectedShips[i].isLive)
                    {
                        if (pageNum == 1)
                        {
                            if (i < 8)
                                selShips.Add(world.selectedShips[i]);
                        }
                        else
                        {
                            if (i + 8 * (pageNum - 1) < world.selectedShips.Count && i + 8 * (pageNum - 1) < 8 * pageNum)
                                selShips.Add(world.selectedShips[i + 8 * (pageNum - 1)]);
                        }
                    }
                }
                for (int i = 0; i < 8; i++)
                {
                    selSlots[i].Update();
                    if (i < 4)
                    {
                        selSlots[i].Position = fonVector + new Vector2(-5 + 36 * i, -230);
                    }
                    else
                    {
                        selSlots[i].Position = fonVector + new Vector2(-5 + 36 * (i - 4), -190);
                    }
                }
            }
            guiHudShip.Position = fonVector;
            guiHudShip.color = GuiInGame.guiColor;
            guiHudShip.Size = 0.5f;
            guiSelectedShips.Position = fonVector + new Vector2(50, -210);
            guiSelectedShips.color = GuiInGame.guiColor;
            guiHudShipAdd.Position = rightCurrentGuiPos / 1.8F + new Vector2(fonVector.X - 235, fonVector.Y + 90);
            guiHudShipAdd.Size = 0.5f;
            guiHudShipAdd.color = GuiInGame.guiColor;
            hudShip = rightCurrentGuiPos + new Vector2(core.cam.screenCenter.X + (core.screenWidth / 2F) - 150, core.cam.screenCenter.Y + (core.screenHeight / 2F) - 125);
            controlledShip = null;
            for (int i = 0; i < world.selectedShips.Count; i++)
            {
                if (world.selectedShips[i].isLive && world.selectedShips[i].isSelected)
                {
                    controlledShip = world.selectedShips[i];
                    break;
                }
            }
            guiShield.Position = hudShip;
            guiShield.Size = 1.25f;
            if (controlledShip != null)
            {
                guiShield.color = ColorShield(controlledShip);
                controlButton.Update();
                if (controlledShip.isControled)
                {
                    controlButton.stringMain = "Отменить управление";
                    controlButton.Position = core.cam.screenCenter + rightCurrentGuiPos + new Vector2(core.screenWidth / 2F - 130, core.screenHeight / 2F - 265);
                }
                else
                {
                    controlButton.stringMain = "Управлять кораблем";
                    controlButton.Position = core.cam.screenCenter + rightCurrentGuiPos + new Vector2(core.screenWidth / 2F - 130, core.screenHeight / 2F - 265);
                }
                controlButton.Rect = new Rectangle((int)controlButton.Position.X - (int)(controlButton.Text.Width / 2 * controlButton.Size), (int)controlButton.Position.Y - (int)(controlButton.Text.Height / 2 * controlButton.Size), (int)(controlButton.Text.Width * controlButton.Size), (int)(controlButton.Text.Height * controlButton.Size));
            }
            guiMap.Position = leftCurrentGuiPos + new Vector2(core.cam.screenCenter.X - core.screenWidth / 2F + 160, core.cam.screenCenter.Y + core.screenHeight / 2F - 125);
            guiMap.color = GuiInGame.guiColor;
            guiMap.Size = 0.5f;
            guiMap1.Position = guiMap.Position;
            guiMap1.color = GuiInGame.guiColor;
            guiMap1.Size = 0.5f;
            guiInfo.Position = leftCurrentGuiPos + new Vector2(core.cam.screenCenter.X - core.screenWidth / 2 + 15, core.cam.screenCenter.Y - core.screenHeight / 2) + new Vector2(95, 64);
            guiInfo.color = GuiInGame.guiColor;
            guiInfo.Size = 0.6f;
            chat.Update(rightCurrentGuiPos*1.2f);
            efh.Update();
        }
        public void Render(SpriteBatch spriteBatch)
        {
            efh.Render(spriteBatch);
            RenderPlayerInfo(spriteBatch);
            RenderMap(spriteBatch);
            RenderObjectInfo(spriteBatch);
        }
        private void RenderPlayerInfo(SpriteBatch spriteBatch)
        {
            chat.Render(spriteBatch);
            Vector2 strPos = leftCurrentGuiPos + new Vector2(core.cam.screenCenter.X - core.screenWidth / 2 + 15, core.cam.screenCenter.Y - core.screenHeight / 2);
            guiInfo.Render(spriteBatch);
            spriteBatch.DrawString(Fonts.consoleFont, Language.GetString(StringName.Money) + ": " + core.money, strPos + new Vector2(0, 22), new Color(GuiInGame.guiColor));
            spriteBatch.DrawString(Fonts.consoleFont, Language.GetString(StringName.Metal) + ": " + core.metal, strPos + new Vector2(0, 40), new Color(GuiInGame.guiColor));
            spriteBatch.DrawString(Fonts.consoleFont, Language.GetString(StringName.Crystals) + ": " + core.crystals, strPos + new Vector2(0, 58), new Color(GuiInGame.guiColor));
            spriteBatch.DrawString(Fonts.consoleFont, Language.GetString(StringName.Experience) + ": " + core.experience, strPos + new Vector2(0, 76), new Color(GuiInGame.guiColor));
            spriteBatch.DrawString(Fonts.consoleFont, Language.GetString(StringName.SciencePoints) + ": " + core.science, strPos + new Vector2(0, 94), new Color(GuiInGame.guiColor));
        }
        private void RenderMap(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(Textures.guiMap[1], leftCurrentGuiPos + new Vector2(core.cam.screenCenter.X - Core.screenWidth / 2F + 160, core.cam.screenCenter.Y + Core.screenHeight / 2F - 125), null, new Color(GuiInGame.guiColor), 0, guiHudOrigin[0], 0.5F, SpriteEffects.None, 0);
            guiMap.Render(spriteBatch);
            Vector2 playerShipVector = leftCurrentGuiPos + new Vector2(core.cam.screenCenter.X - core.screenWidth / 2 + 160, core.cam.screenCenter.Y + core.screenHeight / 2 - 125);
            /*for (int i = 0; i < Core.stations.Count; i++)
            {
                if (Math.Abs((core.cam.screenCenter.X) - Core.stations[i].Position.X) <= 3250 &&
                    Math.Abs((core.cam.screenCenter.Y) - Core.stations[i].Position.Y) <= 2350)
                {
                    Vector4 colorShip = new Vector4(0.25F, 0.25F, 1F, 1F);
                    if (Core.stations[i].shipFaction == Faction.Human)
                    {
                        colorShip = new Vector4(0.5F, 0.5F, 1, 1);
                    }
                    else if (Core.stations[i].shipFaction == Faction.Enemy)
                    {
                        colorShip = new Vector4(1, 0.5F, 0.5F, 1);
                    }
                    else if (Core.stations[i].shipFaction == Faction.Civilian)
                    {
                        colorShip = new Vector4(0.5F, 1F, 0.5F, 1);
                    }
                    else
                    {
                        colorShip = new Vector4(1, 1, 0.25F, 1);
                    }
                    Vector2 renderShipVector = new Vector2(playerShipVector.X + ((Core.stations[i].Position.X - core.cam.screenCenter.X) / 25), playerShipVector.Y + ((Core.stations[i].Position.Y - core.cam.screenCenter.Y) / 25));
                    spriteBatch.Draw(Core.stations[i].Text, renderShipVector, null, new Color(colorShip), Core.stations[i].Rotation, Core.stations[i].Origin, Core.stations[i].shipInfo.mapIconSize, SpriteEffects.None, 0);
                }
            }*/
            /*for (int i = 0; i < Core.motherShips.Count; i++)
            {
                if (Math.Abs((core.cam.screenCenter.X) - Core.motherShips[i].Position.X) <= 3250 &&
                    Math.Abs((core.cam.screenCenter.Y) - Core.motherShips[i].Position.Y) <= 2350)
                {
                    Vector4 colorShip = new Vector4(0.25F, 0.25F, 1F, 1F);
                    if (Core.motherShips[i].shipFaction == Faction.Human)
                    {
                        colorShip = new Vector4(0.5F, 0.5F, 1, 1);
                    }
                    else if (Core.motherShips[i].shipFaction == Faction.Enemy)
                    {
                        colorShip = new Vector4(1, 0.5F, 0.5F, 1);
                    }
                    else if (Core.motherShips[i].shipFaction == Faction.Civilian)
                    {
                        colorShip = new Vector4(0.5F, 1F, 0.5F, 1);
                    }
                    else
                    {
                        colorShip = new Vector4(1, 1, 0.25F, 1);
                    }
                    Vector2 renderShipVector = new Vector2(playerShipVector.X + ((Core.motherShips[i].Position.X - core.cam.screenCenter.X) / 25), playerShipVector.Y + ((Core.motherShips[i].Position.Y - core.cam.screenCenter.Y) / 25));
                    spriteBatch.Draw(Core.motherShips[i].Text, renderShipVector, null, new Color(colorShip), Core.motherShips[i].Rotation, Core.motherShips[i].Origin, Core.motherShips[i].shipInfo.mapIconSize, SpriteEffects.None, 0);
                }
            }*/
            //float astSize = 0.2F;
            /*for (int i = 0; i < Core.asteroidsRenderMap.Count; i++)
            {
                if (Math.Abs((core.cam.screenCenter.X) - Core.asteroidsRenderMap[i].Position.X) <= 3250 &&
                    Math.Abs((core.cam.screenCenter.Y) - Core.asteroidsRenderMap[i].Position.Y) <= 2350)
                    if (Core.asteroidsRenderMap[i].asteroidType == 9 || Core.asteroidsRenderMap[i].asteroidType == 10)
                    {
                        Vector2 renderShipVector = new Vector2(playerShipVector.X + ((Core.asteroidsRenderMap[i].Position.X - core.cam.screenCenter.X) / 25), playerShipVector.Y + ((Core.asteroidsRenderMap[i].Position.Y - core.cam.screenCenter.Y) / 25));
                        spriteBatch.Draw(Core.asteroidsRenderMap[i].Text, renderShipVector, null, Color.Red, Core.asteroidsRenderMap[i].Rotation, Core.asteroidsRenderMap[i].Origin, astSize / 3F, SpriteEffects.None, 0);
                    }
            }
            for (int i = 0; i < Core.asteroidsRenderMap.Count; i++)
            {
                if (Math.Abs((core.cam.screenCenter.X) - Core.asteroidsRenderMap[i].Position.X) <= 3250 &&
                    Math.Abs((core.cam.screenCenter.Y) - Core.asteroidsRenderMap[i].Position.Y) <= 2350)
                    if (Core.asteroidsRenderMap[i].asteroidType == 0 || Core.asteroidsRenderMap[i].asteroidType == 1 || Core.asteroidsRenderMap[i].asteroidType == 2 || Core.asteroidsRenderMap[i].asteroidType == 3)
                    {
                        Vector2 renderShipVector = new Vector2(playerShipVector.X + ((Core.asteroidsRenderMap[i].Position.X - core.cam.screenCenter.X) / 25), playerShipVector.Y + ((Core.asteroidsRenderMap[i].Position.Y - core.cam.screenCenter.Y) / 25));
                        spriteBatch.Draw(Core.asteroidsRenderMap[i].Text, renderShipVector, null, Color.Yellow, Core.asteroidsRenderMap[i].Rotation, Core.asteroidsRenderMap[i].Origin, astSize / 2F, SpriteEffects.None, 0);
                    }
            }
            for (int i = 0; i < Core.asteroidsRenderMap.Count; i++)
            {
                if (Math.Abs((core.cam.screenCenter.X) - Core.asteroidsRenderMap[i].Position.X) <= 3250 &&
                    Math.Abs((core.cam.screenCenter.Y) - Core.asteroidsRenderMap[i].Position.Y) <= 2350)
                    if (Core.asteroidsRenderMap[i].asteroidType == 4 || Core.asteroidsRenderMap[i].asteroidType == 5 || Core.asteroidsRenderMap[i].asteroidType == 6 || Core.asteroidsRenderMap[i].asteroidType == 7 || Core.asteroidsRenderMap[i].asteroidType == 8)
                    {
                        Vector2 renderShipVector = new Vector2(playerShipVector.X + ((Core.asteroidsRenderMap[i].Position.X - core.cam.screenCenter.X) / 25), playerShipVector.Y + ((Core.asteroidsRenderMap[i].Position.Y - core.cam.screenCenter.Y) / 25));
                        spriteBatch.Draw(Core.asteroidsRenderMap[i].Text, renderShipVector, null, Color.Yellow, Core.asteroidsRenderMap[i].Rotation, Core.asteroidsRenderMap[i].Origin, astSize / 2F, SpriteEffects.None, 0);
                    }
            }*/
            if (world.shipsRenderMap != null)
            for (int i = 0; i < world.shipsRenderMap.Count; i++)
            {
                if (Math.Abs((core.cam.screenCenter.X) - world.shipsRenderMap[i].Position.X) <= 3250 &&
                    Math.Abs((core.cam.screenCenter.Y) - world.shipsRenderMap[i].Position.Y) <= 2350)
                {
                    Vector4 colorShip = new Vector4(0.25F, 0.25F, 1F, 1F);
                    if (world.shipsRenderMap[i].shipFaction == Faction.Human)
                    {
                        colorShip = new Vector4(0.5F, 0.5F, 1, 1);
                    }
                    else if (world.shipsRenderMap[i].shipFaction == Faction.Enemy)
                    {
                        colorShip = new Vector4(1, 0.5F, 0.5F, 1);
                    }
                    else if (world.shipsRenderMap[i].shipFaction == Faction.Civilian)
                    {
                        colorShip = new Vector4(0.5F, 1F, 0.5F, 1);
                    }
                    else
                    {
                        colorShip = new Vector4(1, 1, 0.25F, 1);
                    }
                    Vector2 renderShipVector = new Vector2(playerShipVector.X + ((world.shipsRenderMap[i].Position.X - core.cam.screenCenter.X) / 25), playerShipVector.Y + ((world.shipsRenderMap[i].Position.Y - core.cam.screenCenter.Y) / 25));
                    spriteBatch.Draw(world.shipsRenderMap[i].Text, renderShipVector, null, new Color(colorShip), world.shipsRenderMap[i].Rotation, world.shipsRenderMap[i].Origin, world.shipsRenderMap[i].shipInfo.mapIconSize, SpriteEffects.None, 0);
                }
            }
            guiMap1.Render(spriteBatch);
        }
        private void RenderObjectInfo(SpriteBatch spriteBatch)
        {
            if (world.selectedShips.Count > 1 && !core.cam.control)
            {
                guiSelectedShips.Render(spriteBatch);
                if (world.selectedShips.Count > 8)
                {
                    rightSelShips.Update();
                    rightSelShips.Position = core.cam.screenCenter + rightCurrentGuiPos + new Vector2(core.screenWidth / 2F - 20, core.screenHeight / 2F - 335);
                    leftSelShips.Update();
                    leftSelShips.Position = core.cam.screenCenter + rightCurrentGuiPos + new Vector2(core.screenWidth / 2F - 190, core.screenHeight / 2F - 335);
                    leftSelShips.Render(spriteBatch);
                    rightSelShips.Render(spriteBatch);
                }
                if (selShips.Count >= 1)
                {
                    for (int i = 0; i < selShips.Count; i++)
                    {
                        selSlots[i].Render(spriteBatch);
                        if (i < 4)
                        {
                            spriteBatch.Draw(selShips[i].Text, fonVector + new Vector2(-5 + 36 * i, -230), null, new Color(ColorHull(selShips[i])), MathHelper.ToRadians(90), selShips[i].Origin, selShips[i].shipInfo.mapIconSize, SpriteEffects.None, 0);
                        }
                        else
                        {
                            spriteBatch.Draw(selShips[i].Text, fonVector + new Vector2(-5 + 36 * (i - 4), -190), null, new Color(ColorHull(selShips[i])), MathHelper.ToRadians(90), selShips[i].Origin, selShips[i].shipInfo.mapIconSize, SpriteEffects.None, 0);
                        }
                    }
                    spriteBatch.DrawString(Fonts.consoleFont, Language.GetString(StringName.Page) +": " + pageNum, fonVector + new Vector2(-44, -177), new Color(GuiInGame.guiColor));
                }
            }
            if (controlledShip != null)
            {
                guiHudShip.Render(spriteBatch);

                float shipSize = controlledShip.shipInfo.iconSize;
                spriteBatch.Draw(controlledShip.Text, hudShip, null, new Color(ColorHull(controlledShip)), MathHelper.ToRadians(90), controlledShip.Origin, shipSize, SpriteEffects.None, 0);

                guiShield.Render(spriteBatch);

                Vector4 color;
                color = ColorArmorPlate(controlledShip, 2);
                spriteBatch.Draw(Textures.guiAmorPlate, new Vector2(hudShip.X, hudShip.Y + 60), null, new Color(color), MathHelper.ToRadians(90), armorPlateOriginal, 1F, SpriteEffects.None, 0);
                color = ColorArmorPlate(controlledShip, 0);
                spriteBatch.Draw(Textures.guiAmorPlate, new Vector2(hudShip.X, hudShip.Y - 60), null, new Color(color), MathHelper.ToRadians(90), armorPlateOriginal, 1F, SpriteEffects.FlipHorizontally, 0);
                color = ColorArmorPlate(controlledShip, 3);
                spriteBatch.Draw(Textures.guiAmorPlate, new Vector2(hudShip.X - 55, hudShip.Y + 0), null, new Color(color), MathHelper.ToRadians(180), armorPlateOriginal, 1F, SpriteEffects.None, 0);
                color = ColorArmorPlate(controlledShip, 1);
                spriteBatch.Draw(Textures.guiAmorPlate, new Vector2(hudShip.X + 55, hudShip.Y + 0), null, new Color(color), MathHelper.ToRadians(180), armorPlateOriginal, 1F, SpriteEffects.FlipHorizontally, 0);
                Vector2 reactorOriginal = new Vector2(Textures.guiReactor.Width / 2 - 100, Textures.guiReactor.Height / 2);
                float Rotout = 4.5F;
                for (int j = 1; j < 20; j++)
                {
                    spriteBatch.Draw(Textures.guiReactor, hudShip, null, new Color(ColorReactor(controlledShip) > 0.1F * j / 2 ? new Vector4(0.4F, 0.4F, 1F, 0.5F) : new Vector4(0.5F, 0.5F, 0.5F, 0.5F)), MathHelper.ToRadians(Rotout * j - 225), reactorOriginal, 1.25F, SpriteEffects.None, 0);
                }
                RenderGunSlot(controlledShip, spriteBatch, hudShip, shipSize);
                guiHudShipAdd.Render(spriteBatch);
                spriteBatch.DrawString(Fonts.consoleFont, Language.GetString(StringName.Cargo) + ": " + controlledShip.cargoSize + "/" + controlledShip.maxCargoSize, rightCurrentGuiPos / 1.8F + new Vector2(fonVector.X - 300, fonVector.Y + 62), new Color(GuiInGame.guiColor));
                spriteBatch.DrawString(Fonts.consoleFont, Language.GetString(StringName.Crew) + ": " + controlledShip.crewSize + "/" + controlledShip.maxCrewSize, rightCurrentGuiPos / 1.8F + new Vector2(fonVector.X - 300, fonVector.Y + 80), new Color(GuiInGame.guiColor));
                //spriteBatch.DrawString(Fonts.consoleFont, Language.GetString(StringName.Hull)+": " + string.Format("{0:0}", controlledShip.hull), rightCurrentGuiPos / 1.8F + new Vector2(fonVector.X - 300, fonVector.Y + 62), new Color(GuiInGame.guiColor));
                //spriteBatch.DrawString(Fonts.consoleFont, Language.GetString(StringName.Shield) + ": " + string.Format("{0:0}", controlledShip.shield), rightCurrentGuiPos / 1.8F + new Vector2(fonVector.X - 300, fonVector.Y + 80), new Color(GuiInGame.guiColor));
                //spriteBatch.DrawString(Fonts.consoleFont, Language.GetString(StringName.Energy)+": " + string.Format("{0:0}", controlledShip.energy), rightCurrentGuiPos / 1.8F + new Vector2(fonVector.X - 300, fonVector.Y + 98), new Color(GuiInGame.guiColor));
                spriteBatch.Draw(controlButton.Text, controlButton.Position, null, new Color(controlButton.color), 0, controlButton.Origin, controlButton.Size, SpriteEffects.FlipHorizontally, 0);
                spriteBatch.DrawString(Fonts.mediumFont, controlButton.stringMain, new Vector2(controlButton.Position.X - Fonts.mediumFont.MeasureString(controlButton.stringMain).X / 2, controlButton.Position.Y - 10), new Color(GuiInGame.guiColor));
            }
        }
        public void RenderGunSlot(Ship s, SpriteBatch spriteBatch, Vector2 pos, float shipSize)
        {
            if(s.gunSlots!=null)
            for(int i=0;i<s.gunSlots.Length;i++)
            {
                if(s.gunSlots[i] != null)
                {
                    float cos = (float)Math.Cos(MathHelper.ToRadians(90));
                    float sin = (float)Math.Sin(MathHelper.ToRadians(90));
                    float cos1 = (float)Math.Cos(MathHelper.ToRadians(180));
                    float sin1 = (float)Math.Sin(MathHelper.ToRadians(180));
                    Vector4 gunColor = new Vector4((float)s.gunSlots[i].shootTimer / (float)s.gunSlots[i].shootTimerMax, (float)s.gunSlots[i].shootTimer / (float)s.gunSlots[i].shootTimerMax, 1 - (float)s.gunSlots[i].shootTimer / (float)s.gunSlots[i].shootTimerMax, 1F);
                    Vector2 gunPos = pos + new Vector2((cos1 * s.gunPos[i].X - cos * s.gunPos[i].Y)*shipSize, (sin1 * s.gunPos[i].X - sin * s.gunPos[i].Y)*shipSize);
                    spriteBatch.Draw(s.gunSlots[i].Text, gunPos, null, new Color(gunColor), MathHelper.ToRadians(90), s.gunSlots[i].Origin, shipSize, SpriteEffects.None, 0);
                }
            }
            /*if (s.shipType == ShipType.HumanSmall1)
            {
                Vector4 gunColor = Vector4.Zero;
                if (es.gunSlots[0] != null)
                {
                    gunColor = new Vector4((float)es.gunSlots[0].shootTimer / (float)es.gunSlots[0].shootTimerMax, (float)es.gunSlots[0].shootTimer / (float)es.gunSlots[0].shootTimerMax, 1 - (float)es.gunSlots[0].shootTimer / (float)es.gunSlots[0].shootTimerMax, 1F);
                    spriteBatch.Draw(es.gunSlots[0].Text, new Vector2(hudShip.X - 28, hudShip.Y - 15), null, new Color(gunColor), MathHelper.ToRadians(90), es.gunSlots[0].Origin, shipSize, SpriteEffects.None, 0);
                }
                if (es.gunSlots[1] != null)
                {
                    gunColor = new Vector4((float)es.gunSlots[1].shootTimer / (float)es.gunSlots[1].shootTimerMax, (float)es.gunSlots[1].shootTimer / (float)es.gunSlots[1].shootTimerMax, 1 - (float)es.gunSlots[1].shootTimer / (float)es.gunSlots[1].shootTimerMax, 1F);
                    spriteBatch.Draw(es.gunSlots[1].Text, new Vector2(hudShip.X + 28, hudShip.Y - 15), null, new Color(gunColor), MathHelper.ToRadians(90), es.gunSlots[1].Origin, shipSize, SpriteEffects.None, 0);
                }
            }
            else if (es.shipType == ShipType.EnemySmall1)
            {
                Vector4 gunColor = Vector4.Zero;
                if (es.gunSlots[0] != null)
                {
                    gunColor = new Vector4((float)es.gunSlots[0].shootTimer / (float)es.gunSlots[0].shootTimerMax, (float)es.gunSlots[0].shootTimer / (float)es.gunSlots[0].shootTimerMax, 1 - (float)es.gunSlots[0].shootTimer / (float)es.gunSlots[0].shootTimerMax, 1F);
                    spriteBatch.Draw(es.gunSlots[0].Text, new Vector2(hudShip.X - 28, hudShip.Y - 20), null, new Color(gunColor), MathHelper.ToRadians(90), es.gunSlots[0].Origin, shipSize, SpriteEffects.None, 0);
                }
                if (es.gunSlots[1] != null)
                {
                    gunColor = new Vector4((float)es.gunSlots[1].shootTimer / (float)es.gunSlots[1].shootTimerMax, (float)es.gunSlots[1].shootTimer / (float)es.gunSlots[1].shootTimerMax, 1 - (float)es.gunSlots[1].shootTimer / (float)es.gunSlots[1].shootTimerMax, 1F);
                    spriteBatch.Draw(es.gunSlots[1].Text, new Vector2(hudShip.X + 28, hudShip.Y - 20), null, new Color(gunColor), MathHelper.ToRadians(90), es.gunSlots[1].Origin, shipSize, SpriteEffects.None, 0);
                }
            }
            else if (es.shipType == ShipType.HumanHuge1)
            {

            }*/
        }
        private Vector4 ColorArmorPlate(Ship ship, int i)
        {
            Vector4 color;
            if (ship.armorPlates[i] != null)
            {
                float procent = (ship.armorPlates[i].armor) / ship.armorPlates[i].maxArmor;
                return color = new Vector4(1 - procent, procent, 0.0F, 0.5F);
            }
            else
            {
                return color = new Vector4(0, 0, 0.0F, 0.0F);
            }

        }
        private Vector4 ColorShield(Ship ship)
        {
            Vector4 color;
            if (ship.shield <= 0)
            {
                if (ship.shieldTimer != ship.shieldMaxTimer)
                {
                    float procent = (ship.shieldMaxTimer) / ship.shieldMaxTimer;
                    return color = new Vector4(0, 0, 1 - procent, 0.1F);
                }
                else
                {
                    return color = new Vector4(0, 0, 0.0F, 0.1F);
                }
            }
            else
            {
                float procent = (ship.shield) / ship.maxShield;
                return color = new Vector4(1 - procent, procent, 0.0F, 0.1F);
            }
        }
        private Vector4 ColorHull(Ship go)
        {
            Vector4 color;
            if (go.hull <= 0)
            {
                return color = new Vector4(0, 0, 0.0F, 0.5F);
            }
            else
            {
                float procent = (go.hull) / go.maxHull;
                return color = new Vector4(1 - procent, procent, 0.0F, 0.5F);
            }
        }
        private float ColorReactor(Ship go)
        {
            return (go.energy) / go.maxEnergy;
        }
    }
}
