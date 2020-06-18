using BattleForSpaceResources.Ambient;
using BattleForSpaceResources.BFSRSystem.Helpers;
using BattleForSpaceResources.Entitys;
using BattleForSpaceResources.Entitys.BFSRSystem.Helpers;
using BattleForSpaceResources.Guis;
using BattleForSpaceResources.Networking;
using BattleForSpaceResources.ShipComponents;
using Krypton.Lights;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace BattleForSpaceResources
{
    public enum LocationType
    {
        Green = 2, Yellow = 0, Pink = 3, Blue = 1, Dark = 4
    }
    public class World
    {
        private Core core = Core.GetCore();
        public int grid = 2000;
        private int npcId = 0;
        private List<int> npcIds = new List<int>();
        public LocationType locType;

        public List<Ship>[,] shipsGrid;
        public List<Ship> ships = new List<Ship>();
        public List<Ship> shipsRender;
        public List<Ship> shipsRenderMap;
        public List<Ship> selectedShips = new List<Ship>();
        public List<Ship> shipsNpc = new List<Ship>();

        public List<Ship> destroingShips = new List<Ship>();

        public List<ShipsSpawner> spawners = new List<ShipsSpawner>();

        public List<Bullet> bullets = new List<Bullet>();

        private GameObject background;
        public List<Star> stars = new List<Star>();
        private List<Smoke> smokes = new List<Smoke>();
        public List<Planet> planets = new List<Planet>();

        public bool isRemote;
        public World(bool f)
        {
            isRemote = f;
            if(isRemote)
            {
                core.cam.screenCenter = Vector2.Zero;
                Vector4 ambientColor = Vector4.Zero;
                locType = LocationType.Yellow;
                if (locType != LocationType.Dark)
                {
                    background = new GameObject(Textures.background[(byte)locType], core.cam.screenCenter);
                    if (core.screenHeight == 1200)
                    {
                        background.Size = 1.06f;
                    }
                }
                if (locType == LocationType.Yellow)
                {
                    ambientColor = new Vector4(0.85f, 0.7f, 0.2f, 1);
                }
                else if (locType == LocationType.Blue)
                {
                    ambientColor = new Vector4(0.01f, 0.26f, 0.77f, 1);
                }
                else if (locType == LocationType.Green)
                {
                    ambientColor = new Vector4(0.3f, 0.75f, 0.35f, 1);
                }
                else if (locType == LocationType.Pink)
                {
                    ambientColor = new Vector4(0.85f, 0.3f, 0.7f, 1);
                }
                else
                {
                    ambientColor = Vector4.Zero;
                }
                if (locType != LocationType.Dark)
                {
                    stars.Add(new Star(Textures.ambientStars[0], new Vector2(-200, -400), ambientColor, 1));
                    Planet planet = new Planet(0, 1.5F, new Vector2(100, 100), stars.Count > 0 ? stars[0].color : Vector4.Zero);
                    planets.Add(planet);
                    Planet a = new Planet(1, 0, planets[0].positionAdd + new Vector2(300, 200));
                    planets.Add(a);
                }
                GenerateClouds();
                core.guiInGame = new GuiInGame(this);
            }
            else
            {
                ShipsSpawner ss = new ShipsSpawner(new Vector2(-1500, -500), SpawnerType.Easy, Faction.Enemy, 2, 1000);
                spawners.Add(ss);
                ss = new ShipsSpawner(new Vector2(0, 1500), SpawnerType.Easy, Faction.Human, 2, 1000);
                spawners.Add(ss);
                ss = new ShipsSpawner(new Vector2(1500, 500), SpawnerType.Easy, Faction.Civilian, 2, 1000);
                spawners.Add(ss);
            }
        }
        public void Update()
        {
            UpdateShips();
            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].Update();
                if (!bullets[i].isLive)
                {
                    bullets.RemoveAt(i);
                    i--;
                }
            }
            if(!isRemote)
            {
                //if (ServerCore.instance.GetServer().ConnectionsCount > 0 && ServerCore.instance.GetServer().timerUpdate == 200)
                {
                    //ServerPacketSender.UpdateBullet(bullets);
                }
                for (int i = 0; i < spawners.Count; i++)
                {
                    spawners[i].Update();
                }
            }
            else
            {
                if(background != null)
                background.Position = core.cam.screenCenter;
                for (int i = 0; i < stars.Count; i++)
                {
                    stars[i].Update();
                }
                for (int i = 0; i < planets.Count; i++)
                {
                    planets[i].Update();
                }
                for (int i = 0; i < stars.Count; i++)
                {
                    stars[i].UpdateCollide();
                }
                for (int i = 0; i < smokes.Count; i++)
                {
                    smokes[i].Update();
                }
            }
        }
        private void UpdateShips()
        {
            for (int i = 0; i < ships.Count; i++)
            {
                ships[i].Update();
            }
            for (int i = 0; i < shipsNpc.Count; i++)
            {
                shipsNpc[i].Update();
            }
                shipsGrid = new List<Ship>[80000 / grid, 80000 / grid];
                for (int i = 0; i < ships.Count; i++)
                {
                    if (ships[i].isLive)
                    {
                        int x = (int)((ships[i].Position.X + 40000) / grid);
                        int y = (int)((ships[i].Position.Y + 40000) / grid);
                        if (x < 0)
                            x = 0;
                        if (y < 0)
                            y = 0;
                        if (x + 1 > shipsGrid.GetLength(0))
                            x = shipsGrid.GetLength(0) - 1;
                        if (y + 1 > shipsGrid.GetLength(1))
                            y = shipsGrid.GetLength(1) - 1;
                        if (shipsGrid[x, y] == null)
                            shipsGrid[x, y] = new List<Ship>();
                        shipsGrid[x, y].Add(ships[i]);
                    }
                    else
                    {
                        if (ships[i].isControled)
                            core.isControl = false;
                        ships.RemoveAt(i);
                        i--;
                    }
                }
                for (int i = 0; i < shipsNpc.Count; i++)
                {
                    if (shipsNpc[i].isLive)
                    {
                        int x = (int)((shipsNpc[i].Position.X + 40000) / grid);
                        int y = (int)((shipsNpc[i].Position.Y + 40000) / grid);
                        if (x < 0)
                            x = 0;
                        if (y < 0)
                            y = 0;
                        if (x + 1 > shipsGrid.GetLength(0))
                            x = shipsGrid.GetLength(0) - 1;
                        if (y + 1 > shipsGrid.GetLength(1))
                            y = shipsGrid.GetLength(1) - 1;
                        if (shipsGrid[x, y] == null)
                            shipsGrid[x, y] = new List<Ship>();
                        shipsGrid[x, y].Add(shipsNpc[i]);
                    }
                    else
                    {
                        if (shipsNpc[i].isControled)
                            core.isControl = false;
                        shipsNpc.RemoveAt(i);
                        i--;
                    }
                }
                if (isRemote)
                {
                    int x1 = (int)((core.cam.screenCenter.X + 40000) / grid);
                    int y1 = (int)((core.cam.screenCenter.Y + 40000) / grid);
                    shipsRender = new List<Ship>();
                    shipsRenderMap = new List<Ship>();
                    for (int i = 0; i < 9; i++)
                    {
                        int[] xy = CollisionHelper.GetNearXY(i, x1, y1);
                        int x2 = xy[0];
                        int y2 = xy[1];
                        if (x2 < 0 || y2 < 0 || x2 + 1 > shipsGrid.GetLength(0) || y2 + 1 > shipsGrid.GetLength(1))
                        { }
                        else
                        {
                            if (shipsGrid[x2, y2] != null)
                            {
                                for (int j = 0; j < shipsGrid[x2, y2].Count; j++)
                                {
                                    shipsRender.Add(shipsGrid[x2, y2][j]);
                                    shipsRenderMap.Add(shipsGrid[x2, y2][j]);
                                }
                            }
                        }
                    }
                    for (int i = 1; i < 17; i++)
                    {
                        int[] xy = CollisionHelper.GetMapNearXY(i, x1, y1);
                        int x2 = xy[0];
                        int y2 = xy[1];
                        if (x2 < 0 || y2 < 0 || x2 + 1 > shipsGrid.GetLength(0) || y2 + 1 > shipsGrid.GetLength(1))
                        { }
                        else
                        {
                            if (shipsGrid[x2, y2] != null)
                            {
                                for (int j = 0; j < shipsGrid[x2, y2].Count; j++)
                                {
                                    shipsRenderMap.Add(shipsGrid[x2, y2][j]);
                                }
                            }
                        }
                    }
                }

        }
        public void Render(SpriteBatch spriteBatch)
        {
            RenderAmbients(spriteBatch);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, null, null, null, core.cam.defaultScreen);
            {
                if (shipsRender != null)
                    for (int i = 0; i < shipsRender.Count; i++)
                    {
                        shipsRender[i].Render(spriteBatch);
                    }
            }
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, null, null, null, core.cam.defaultScreen);
            {
                core.ps.BackRenderAlpha(spriteBatch);
                core.ps.RenderAlpha(spriteBatch);
            }
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.AnisotropicClamp, null, null, null, core.cam.defaultScreen);
            {
                core.ps.RederAdditive(spriteBatch);
                for (int i = 0; i < bullets.Count; i++)
                {
                    bullets[i].Render(spriteBatch);
                }
            }
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, null, null, null, core.cam.defaultScreen);
            {
                for (int i = 0; i < smokes.Count; i++)
                {
                    if (smokes[i].smokeScreen == SmokeScreen.Normal)
                        smokes[i].Render(spriteBatch);
                }
            }
            spriteBatch.End();
        }
        private void RenderAmbients(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, null, null, null, core.cam.guiScreen);
            {
                if (background != null)
                    background.Render(spriteBatch);
                else
                    spriteBatch.Draw(Textures.guiFon, core.cam.screenCenter, new Rectangle(0, 0, core.screenWidth, core.screenHeight), new Color(Vector4.One), 0, new Vector2(core.screenWidth/2, core.screenHeight/2), 1, SpriteEffects.None, 0);
                if (stars.Count > 0)
                    stars[0].Render(spriteBatch);
            }
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, null, null, null, core.cam.planetScreen);
            {
                for (int i = 0; i < planets.Count; i++)
                {
                    if (planets[i].isPlanet)
                        planets[i].Render(spriteBatch);
                }
            }
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, null, null, null, core.cam.moonScreen);
            {
                for (int i = 0; i < planets.Count; i++)
                {
                    if (!planets[i].isPlanet && planets[i].layer == 1)
                        planets[i].Render(spriteBatch);
                }
                for (int i = 0; i < planets.Count; i++)
                {
                    if (!planets[i].isPlanet && planets[i].layer == 0)
                        planets[i].Render(spriteBatch);
                }
            }
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, null, null, null, core.cam.smokeScreen1);
            {
                for (int i = 9; i < smokes.Count; i++)
                {
                    if (smokes[i].smokeScreen == SmokeScreen.Fon)
                        smokes[i].Render(spriteBatch);
                }
            }
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, null, null, null, core.cam.defaultScreen);
            {
                core.ps.RenderWrecks(spriteBatch);
            }
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.AnisotropicClamp, null, null, null, core.cam.defaultScreen);
            {
                core.ps.RenderBackAdditive(spriteBatch);
            }
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, null, null, null, core.cam.guiScreen);
            {
                for (int i = 0; i < stars.Count; i++)
                {
                    stars[i].RenderLight(spriteBatch);
                }
            }
            spriteBatch.End();
        }
        private void GenerateClouds()
        {
            int smokeType = 0;
            int smokesize = 2;
            Smoke smoke1 = new Smoke(SmokeScreen.Normal, core.cam.screenCenter, smokesize, smokeType);
            smokes.Add(smoke1);
            for (int i = 1; i < 2; i++)
            {
                smoke1 = new Smoke(SmokeScreen.Normal, new Vector2(core.cam.screenCenter.X + 2048 * i, core.cam.screenCenter.Y), smokesize, smokeType);
                smokes.Add(smoke1);
                smoke1 = new Smoke(SmokeScreen.Normal, new Vector2(core.cam.screenCenter.X - 2048 * i, core.cam.screenCenter.Y), smokesize, smokeType);
                smokes.Add(smoke1);
                smoke1 = new Smoke(SmokeScreen.Normal, new Vector2(core.cam.screenCenter.X, core.cam.screenCenter.Y + 2048 * i), smokesize, smokeType);
                smokes.Add(smoke1);
                smoke1 = new Smoke(SmokeScreen.Normal, new Vector2(core.cam.screenCenter.X, core.cam.screenCenter.Y - 2048 * i), smokesize, smokeType);
                smokes.Add(smoke1);
                for (int y = 1; y < 2; y++)
                {
                    smoke1 = new Smoke(SmokeScreen.Normal, new Vector2(core.cam.screenCenter.X - 2048 * smokesize * i, core.cam.screenCenter.Y + 2048 * y), smokesize, smokeType);
                    smokes.Add(smoke1);
                    smoke1 = new Smoke(SmokeScreen.Normal, new Vector2(core.cam.screenCenter.X + 2048 * smokesize * i, core.cam.screenCenter.Y - 2048 * y), smokesize, smokeType);
                    smokes.Add(smoke1);
                    smoke1 = new Smoke(SmokeScreen.Normal, new Vector2(core.cam.screenCenter.X - 2048 * smokesize * i, core.cam.screenCenter.Y - 2048 * y), smokesize, smokeType);
                    smokes.Add(smoke1);
                    smoke1 = new Smoke(SmokeScreen.Normal, new Vector2(core.cam.screenCenter.X + 2048 * smokesize * i, core.cam.screenCenter.Y + 2048 * y), smokesize, smokeType);
                    smokes.Add(smoke1);
                }
            }
            smoke1 = new Smoke(SmokeScreen.Fon, core.cam.screenCenter, smokesize, smokeType);
            smokes.Add(smoke1);
            for (int i = 1; i < 2; i++)
            {
                smoke1 = new Smoke(SmokeScreen.Fon, new Vector2(core.cam.screenCenter.X + 2048 * i, core.cam.screenCenter.Y), smokesize, smokeType);
                smokes.Add(smoke1);
                smoke1 = new Smoke(SmokeScreen.Fon, new Vector2(core.cam.screenCenter.X - 2048 * i, core.cam.screenCenter.Y), smokesize, smokeType);
                smokes.Add(smoke1);
                smoke1 = new Smoke(SmokeScreen.Fon, new Vector2(core.cam.screenCenter.X, core.cam.screenCenter.Y + 2048 * i), smokesize, smokeType);
                smokes.Add(smoke1);
                smoke1 = new Smoke(SmokeScreen.Fon, new Vector2(core.cam.screenCenter.X, core.cam.screenCenter.Y - 2048 * i), smokesize, smokeType);
                smokes.Add(smoke1);
                for (int y = 1; y < 2; y++)
                {
                    smoke1 = new Smoke(SmokeScreen.Fon, new Vector2(core.cam.screenCenter.X - 2048 * smokesize * i, core.cam.screenCenter.Y + 2048 * y), smokesize, smokeType);
                    smokes.Add(smoke1);
                    smoke1 = new Smoke(SmokeScreen.Fon, new Vector2(core.cam.screenCenter.X + 2048 * smokesize * i, core.cam.screenCenter.Y - 2048 * y), smokesize, smokeType);
                    smokes.Add(smoke1);
                    smoke1 = new Smoke(SmokeScreen.Fon, new Vector2(core.cam.screenCenter.X - 2048 * smokesize * i, core.cam.screenCenter.Y - 2048 * y), smokesize, smokeType);
                    smokes.Add(smoke1);
                    smoke1 = new Smoke(SmokeScreen.Fon, new Vector2(core.cam.screenCenter.X + 2048 * smokesize * i, core.cam.screenCenter.Y + 2048 * y), smokesize, smokeType);
                    smokes.Add(smoke1);
                }
            }
        }
        public int GetNpcId()
        {
            npcId++;
            bool isCan = true;
            for (int i = 0; i < npcIds.Count; i++)
            {
                if (npcId == npcIds[i])
                {
                    isCan = false;
                    break;
                }
            }
            if (isCan)
            {
                npcIds.Add(npcId);
                //ServerCore.LogAdd("Created bot with ID: " + npcId);
                return npcId;
            }
            else
            {
                return GetNpcId();
            }
        }
        public void MinusNpcId(int id)
        {
            npcIds.Remove(id);
            //ServerCore.LogAdd("Bot with ID: " + id + " destroyed");
            npcId--;
        }
    }
}
