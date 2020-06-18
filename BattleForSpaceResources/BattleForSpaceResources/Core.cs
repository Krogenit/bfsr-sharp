using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Lidgren.Network;
using System.Threading;
using System.Timers;
using System.Xml;
using BattleForSpaceResources.BFSRSystem;
using BattleForSpaceResources.Networking;
using BattleForSpaceResources.Guis;
using BattleForSpaceResources.Entitys;
using Krypton;
using Krypton.Lights;
using System.IO;
using System.Diagnostics;
using BattleForSpaceResources.Particles;
using BattleForSpaceResources.BFSRSystem.Helpers;

namespace BattleForSpaceResources
{
    public class Core : Game
    {
        public static Core instance;
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        //public Random random = new Random();
        public bool isExit = false, isPlayingVideo = true, isControl, isChat;
        public int screenWidth, screenHeight, timerVideo, timerMaxVideo = 700, money, crystals, metal, experience, science;
        public Matrixs cam;
        public Gui currentGui;
        public GuiAdd currentGuiAdd;
        private World world;
        private string playerName;
        private ClientNetwork net;
        public KryptonEngine krypton;
        public ParticleSystem ps;
        public GuiInGame guiInGame;
        public InputManager inputManager;
        public Random random = new Random();
        #region Video
        private Video intro;
        private VideoPlayer videoPlayer = new VideoPlayer();
        #endregion
        #region Debug
        public static TimeSpan renderTime, updateTime;
        private Stopwatch stUpdate, stRender;
        public static BFSRConsole console;
        public static int fps, fpsCounter;
        private DateTime fpsTime;
        #endregion
        public Core(int width, int height, bool fullScreen, bool isBorder)
        {
            Settings.LoadSettings(this);
            graphics = new GraphicsDeviceManager(this);
            graphics.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = true;
            Content.RootDirectory = "Content";
            if (width == 0 || height == 0)
            {
                graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                if (graphics.PreferredBackBufferWidth > 1920)
                    graphics.PreferredBackBufferWidth = 1920;
                if (graphics.PreferredBackBufferHeight > 1080)
                    graphics.PreferredBackBufferHeight = 1080;
            }
            else
            {
                graphics.PreferredBackBufferWidth = width;
                graphics.PreferredBackBufferHeight = height;
            }
            screenWidth = graphics.PreferredBackBufferWidth;
            screenHeight = graphics.PreferredBackBufferHeight;
            graphics.IsFullScreen = fullScreen;
            if (!fullScreen && !isBorder)
            {
                System.Windows.Forms.Form gameForm = (System.Windows.Forms.Form)System.Windows.Forms.Form.FromHandle(Window.Handle);
                gameForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            }
            Window.Title = "BFSR Client";
            krypton = new KryptonEngine(this, "shader\\KryptonEffect");
            instance = this;
        }
        protected override void Initialize()
        {
            ps = new ParticleSystem();
            net = new ClientNetwork();
            cam = new Matrixs(GraphicsDevice.Viewport);
            stUpdate = new Stopwatch();
            stRender = new Stopwatch();
            krypton.Initialize();
            krypton.SpriteBatchCompatablityEnabled = true;
            krypton.CullMode = CullMode.None;
            krypton.Bluriness = 3;
            krypton.AmbientColor = new Color(5, 5, 5);
            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Textures.LoadTextures(Content);
            Fonts.LoadFonts(Content);
            Sounds.LoadSounds(Content);
            LoadIntro();
            inputManager = new InputManager();
            console = new BFSRConsole();
            currentGui = new GuiMainMenu();
        }
        private void LoadIntro()
        {
            Random r = new Random();
            if (r.Next(2) == 0)
                intro = Content.Load<Video>("Video\\intro");
            else
            {
                intro = Content.Load<Video>("Video\\intro1");
                timerMaxVideo = 750;
            }
            if (Settings.soundVolume < 0)
                Settings.soundVolume = 0;
            else if (Settings.soundVolume > 1)
                Settings.soundVolume = 1;
            videoPlayer.Volume = Settings.soundVolume;
        }
        protected override void Update(GameTime gameTime)
        {
            if (isPlayingVideo)
            {
                videoPlayer.Play(intro);
                inputManager.Update();
                if (timerVideo++ > timerMaxVideo || inputManager.IsKeyReleased(Keys.Escape) || inputManager.IsKeyReleased(Keys.Enter) || inputManager.IsLeftButtonClicked() || inputManager.IsKeyReleased(Keys.Space))
                {
                    videoPlayer.Stop();
                    isPlayingVideo = false;
                    videoPlayer = null;
                    intro = null;
                }
            }
            else
            {
                stUpdate.Restart();
                UpdateBase();
                stUpdate.Stop();
                updateTime = stUpdate.Elapsed;
            }
            base.Update(gameTime);
        }
        private void UpdateBase()
        {
            console.Update();
            cam.Update();
            if (world != null)
            {
                world.Update();
                ps.Update();
                if (world.locType == LocationType.Dark)
                {
                    krypton.Lights.Clear();
                    krypton.Hulls.Clear();
                }
                guiInGame.Update();
            }
            UpdateGui();
            if (IsActive)
            {
                inputManager.Update();
            }
            UpdateInput();
            Sounds.Update();
            //if (net.isOnline)
            //net.Update();
        }
        private void UpdateInput()
        {
            if (inputManager.IsLeftButtonClicking())
            {
                inputManager.mousePressedVector = inputManager.cursor.Position;
            }
            else if (inputManager.IsLeftButtonClicked())
            {
                if (currentGuiAdd != null)
                {
                    if (currentGuiAdd.LeftClick())
                        Sounds.Play(Sounds.buttonClick, Settings.soundVolume / 2f, random.Next(-2, 2) / 100f);
                }
                else if (currentGui != null)
                {
                    if (currentGui.LeftClick())
                    {
                        if (currentGui != null)
                            currentGui.RestartButtons();
                        Sounds.Play(Sounds.buttonClick, Settings.soundVolume / 2f, random.Next(-2, 2) / 100f);
                        if (isExit)
                        {
                            Settings.SaveSettings();
                            Exit();
                        }
                        if (IsMouseVisible == !Settings.isUseSystemCursor)
                            IsMouseVisible = !IsMouseVisible;
                    }
                }
                else
                    LeftClick();
            }
            else if (inputManager.IsRightButtonClicked())
            {
                if (currentGuiAdd != null)
                {
                    //currentGuiAdd.RightClick();
                }
                else if (currentGui != null)
                {
                    currentGui.RightClick();
                }
                else
                    RightClick();
            }
            if (inputManager.IsLeftButtonDown())
            {
                if (currentGui == null && currentGuiAdd == null)
                    LeftDown();
            }
            else if (inputManager.IsRightButtonDown())
            {
                if (currentGui == null && currentGuiAdd == null)
                    RightDown();
            }
            if (inputManager.IsKeyReleased(Keys.Escape))
            {
                if (isChat)
                    isChat = false;
                else if (currentGui != null)
                {
                    currentGui.Escape();
                }
                else
                {
                    currentGui = new GuiInGameMenu();
                }
            }
            else if (inputManager.IsKeyReleased(Keys.T))
            {
                if (!isChat && currentGui == null && currentGuiAdd == null)
                {
                    isChat = true;
                    Sounds.Play(Sounds.buttonClick, Settings.soundVolume / 2f, random.Next(-2, 2) / 100f);
                }
            }
        }
        private void UpdateGui()
        {
            if (currentGuiAdd != null)
                currentGuiAdd.Update();
            else if (currentGui != null)
                currentGui.Update();
        }
        private void LeftDown()
        {
            if (world != null && !guiInGame.controlButton.Rect.Contains(new Point((int)inputManager.cursor.Position.X, (int)inputManager.cursor.Position.Y)))
                net.SetLeftButton(true);
        }
        private void RightDown()
        {
            if (world != null)
                net.SetRightButton(true);
        }
        private void LeftClick()
        {
            bool isMouseClick = false;
            bool check = false;
            for (int i = 0; i < world.selectedShips.Count; i++)
            {
                if (world.selectedShips[i].isLive && world.selectedShips[i].isSelected
                    && guiInGame.controlButton.Rect.Contains(new Point((int)inputManager.cursor.Position.X, (int)inputManager.cursor.Position.Y)))
                {
                    if (!world.selectedShips[i].isControled)
                    {
                        world.selectedShips[i].isControled = true;
                        world.selectedShips[i].runnigOutVector = Vector2.Zero;
                        check = true;
                        cam.control = true;
                        isMouseClick = true;
                        ClientPacketSender.SendSelectShip(true);
                    }
                    else
                    {
                        cam.control = false;
                        check = true;
                        world.selectedShips[i].isControled = false;
                        world.selectedShips[i].moveToVector = Vector2.Zero;
                        isMouseClick = true;
                        ClientPacketSender.SendSelectShip(false);
                    }
                    Sounds.Play(Sounds.buttonClick, Settings.soundVolume / 2f, random.Next(-2, 2) / 100f);
                    break;
                }
            }
            if (!isMouseClick)
            {
                check = false;
                for (int i = 0; i < guiInGame.selShips.Count; i++)
                {
                    if (guiInGame.selSlots[i].Rect.Contains((int)inputManager.cursor.Position.X, (int)inputManager.cursor.Position.Y) && world.selectedShips[i + 8 * (guiInGame.pageNum - 1)].isLive)
                    {
                        for (int j = 0; j < world.selectedShips.Count; j++)
                        {
                            world.selectedShips[j].isSelected = false;
                        }
                        world.selectedShips[i + 8 * (guiInGame.pageNum - 1)].isSelected = true;
                        check = true;
                        isMouseClick = true;
                    }
                }
                if (guiInGame.leftSelShips.Rect.Contains((int)inputManager.cursor.Position.X, (int)inputManager.cursor.Position.Y))
                {
                    if (guiInGame.pageNum - 1 > 0)
                        guiInGame.pageNum--;
                    check = true;
                    isMouseClick = true;
                    Sounds.Play(Sounds.buttonClick, Settings.soundVolume / 2f, random.Next(-2, 2) / 100f);
                }
                else if (guiInGame.rightSelShips.Rect.Contains((int)inputManager.cursor.Position.X, (int)inputManager.cursor.Position.Y))
                {
                    if ((guiInGame.pageNum + 1) * 8 <= (world.selectedShips.Count + 8))
                        guiInGame.pageNum++;
                    check = true;
                    isMouseClick = true;
                    Sounds.Play(Sounds.buttonClick, Settings.soundVolume / 2f, random.Next(-2, 2) / 100f);
                }
                if (!check && !cam.control)
                {
                    for (int i = 0; i < world.selectedShips.Count; i++)
                    {
                        world.selectedShips[i].isSelected = false;
                    }
                    world.selectedShips.Clear();
                }
            }
            bool selectedFromRectangle = false;
            bool selectedFromClick = false;
            guiInGame.selectRect = new Rectangle();
            Vector2 pressedPos = Vector2.Transform(new Vector2(inputManager.mousePressedVector.X - cam.screenCenter.X + screenWidth / 2, inputManager.mousePressedVector.Y - cam.screenCenter.Y + screenHeight / 2), Matrix.Invert(cam.defaultScreen));
            if (!isMouseClick && !isChat)
            {
                if (pressedPos.X - inputManager.cursorAdvancedPosition.X >= 5)
                {
                    if (pressedPos.Y - inputManager.cursorAdvancedPosition.Y >= 5)
                    {
                        guiInGame.selectRect = new Rectangle((int)inputManager.cursorAdvancedPosition.X, (int)inputManager.cursorAdvancedPosition.Y, (int)(pressedPos.X - inputManager.cursorAdvancedPosition.X), (int)(pressedPos.Y - inputManager.cursorAdvancedPosition.Y));
                        selectedFromRectangle = true;
                    }
                    else if (inputManager.cursorAdvancedPosition.Y - pressedPos.Y >= 5)
                    {
                        guiInGame.selectRect = new Rectangle((int)inputManager.cursorAdvancedPosition.X, (int)pressedPos.Y, (int)(pressedPos.X - inputManager.cursorAdvancedPosition.X), (int)(inputManager.cursorAdvancedPosition.Y - pressedPos.Y));
                        selectedFromRectangle = true;
                    }
                }
                else if (inputManager.cursorAdvancedPosition.X - pressedPos.X >= 5)
                {
                    if (pressedPos.Y - inputManager.cursorAdvancedPosition.Y >= 5)
                    {
                        guiInGame.selectRect = new Rectangle((int)pressedPos.X, (int)inputManager.cursorAdvancedPosition.Y, (int)(inputManager.cursorAdvancedPosition.X - pressedPos.X), (int)(pressedPos.Y - inputManager.cursorAdvancedPosition.Y));
                        selectedFromRectangle = true;
                    }
                    else if (inputManager.cursorAdvancedPosition.Y - pressedPos.Y >= 5)
                    {
                        guiInGame.selectRect = new Rectangle((int)pressedPos.X, (int)pressedPos.Y, (int)(inputManager.cursorAdvancedPosition.X - pressedPos.X), (int)(inputManager.cursorAdvancedPosition.Y - pressedPos.Y));
                        selectedFromRectangle = true;
                    }
                }
                else if (!selectedFromRectangle && !cam.control)
                {
                    for (int i = 0; i < world.ships.Count; i++)
                    {
                        Rectangle rect = new Rectangle((int)world.ships[i].Position.X - world.ships[i].Text.Width / 2, (int)world.ships[i].Position.Y - world.ships[i].Text.Height / 2, world.ships[i].Text.Width, world.ships[i].Text.Height);
                        if (rect.Contains((int)inputManager.cursorAdvancedPosition.X, (int)inputManager.cursorAdvancedPosition.Y) && world.ships[i].shipName.Equals(playerName))
                        {
                            for (int j = 0; j < world.selectedShips.Count; j++)
                            {
                                world.selectedShips[j].isSelected = false;
                            }
                            world.selectedShips.Clear();
                            selectedFromClick = true;
                            world.ships[i].isSelected = true;
                            world.selectedShips.Add(world.ships[i]);
                            break;
                        }
                    }
                    /*if (!selectedFromClick)
                        for (int i = 0; i < motherShips.Count; i++)
                        {
                            Rectangle rect = new Rectangle((int)motherShips[i].Position.X - motherShips[i].Text.Width / 2, (int)motherShips[i].Position.Y - motherShips[i].Text.Height / 2, motherShips[i].Text.Width, motherShips[i].Text.Height);
                            if (rect.Contains((int)InputManager.cursorAdvancedPosition.X, (int)InputManager.cursorAdvancedPosition.Y) && motherShips[i].shipFaction == Faction.Human)
                            {
                                for (int j = 0; j < world.selectedShips.Count; j++)
                                {
                                    world.selectedShips[j].isSelected = false;
                                }
                                world.selectedShips.Clear();
                                selectedFromClick = true;
                                motherShips[i].isSelected = true;
                                world.selectedShips.Add(motherShips[i]);
                                break;
                            }
                        }*/
                    /*if (!selectedFromClick)
                        for (int i = 0; i < stations.Count; i++)
                        {
                            Rectangle rect = new Rectangle((int)stations[i].Position.X - stations[i].Text.Width / 2, (int)stations[i].Position.Y - stations[i].Text.Height / 2, stations[i].Text.Width, stations[i].Text.Height);
                            if (rect.Contains((int)InputManager.cursorAdvancedPosition.X, (int)InputManager.cursorAdvancedPosition.Y)) //&& stations[i].shipFaction == Faction.Human)
                            {
                                for (int j = 0; j < world.selectedShips.Count; j++)
                                {
                                    world.selectedShips[j].isSelected = false;
                                }
                                world.selectedShips.Clear();
                                selectedFromClick = true;
                                stations[i].isSelected = true;
                                world.selectedShips.Add(stations[i]);
                                break;
                            }
                        }*/
                }
            }
            if (selectedFromRectangle && !cam.control)
            {
                for (int i = 0; i < world.selectedShips.Count; i++)
                {
                    world.selectedShips[i].isSelected = false;
                }
                world.selectedShips.Clear();
                for (int i = 0; i < world.ships.Count; i++)
                {
                    if (guiInGame.selectRect.Contains((int)world.ships[i].Position.X, (int)world.ships[i].Position.Y) && world.ships[i].shipName.Equals(playerName))
                    {
                        world.selectedShips.Add(world.ships[i]);
                    }
                }
                /*for (int i = 0; i < motherShips.Count; i++)
                {
                    if (selectRect.Contains((int)motherShips[i].Position.X, (int)motherShips[i].Position.Y) && motherShips[i].shipFaction == Faction.Human)
                    {
                        world.selectedShips.Add(motherShips[i]);
                    }
                }*/
                /*for (int i = 0; i < stations.Count; i++)
                {
                    if (selectRect.Contains((int)stations[i].Position.X, (int)stations[i].Position.Y))// && stations[i].shipFaction == Faction.Human)
                    {
                        world.selectedShips.Add(stations[i]);
                    }
                }*/
                if (world.selectedShips.Count == 1)
                {
                    world.selectedShips[0].isSelected = true;
                }
            }
        }
        private void RightClick()
        {

        }
        public void SetPlayerName(string name)
        {
            this.playerName = name;
        }
        public string GetPlayerName()
        {
            return this.playerName;
        }
        public static Core GetCore()
        {
            return instance;
        }
        public World GetWorld()
        {
            return world;
        }
        public void SetWorld(World w)
        {
            this.world = w;
        }
        public void SetCameraTarget(Vector2 pos)
        {
            cam.camToNewPos = true;
            cam.newCamPos = pos;
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            if (isPlayingVideo)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(videoPlayer.GetTexture(), new Rectangle(0, 0, screenWidth, screenHeight), Color.White);
                spriteBatch.End();
            }
            else
            {
                DateTime start = DateTime.Now;
                stRender.Restart();
                TimeSpan fpsT = start - fpsTime;
                if (fpsT.TotalSeconds >= 1)
                {
                    fps = fpsCounter;
                    fpsCounter = 0;
                    fpsTime = start;
                }
                else
                {
                    fpsCounter++;
                }
                Render(spriteBatch, gameTime);
                renderTime = stRender.Elapsed;
            }
            base.Draw(gameTime);
        }
        private void Render(SpriteBatch spriteBatch, GameTime gt)
        {
            if (world != null)
            {
                if (world.locType == LocationType.Dark)
                {
                    krypton.Matrix = cam.defaultScreen;
                    krypton.LightMapPrepare();
                    world.Render(spriteBatch);
                    krypton.Draw(gt);
                }
                else
                {
                    world.Render(spriteBatch);
                }
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, null, null, null, cam.guiScreen);
                guiInGame.Render(spriteBatch);
                drawGuiElements(spriteBatch);
                spriteBatch.End();
            }
            else
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, null, null, null, cam.guiScreen);
                drawGuiElements(spriteBatch);
                spriteBatch.End();
            }
        }
        private void drawGuiElements(SpriteBatch spriteBatch)
        {
            if (currentGui != null)
                currentGui.Render(spriteBatch);
            if (currentGuiAdd != null)
                currentGuiAdd.Render(spriteBatch);
            else if (currentGui == null && !isChat)
            {
                if (inputManager.mouseState.LeftButton == ButtonState.Pressed && (Math.Abs(inputManager.mousePressedVector.X - inputManager.cursor.Position.X) >= 5 || Math.Abs(inputManager.mousePressedVector.Y - inputManager.cursor.Position.Y) >= 5))
                {
                    if (!cam.control)
                    {
                        DrawHelper.DrawLine(new Color(GuiInGame.guiColor), inputManager.mousePressedVector, new Vector2(inputManager.cursor.Position.X, inputManager.mousePressedVector.Y), spriteBatch, 2);
                        DrawHelper.DrawLine(new Color(GuiInGame.guiColor), new Vector2(inputManager.cursor.Position.X, inputManager.mousePressedVector.Y), inputManager.cursor.Position, spriteBatch, 2);
                        DrawHelper.DrawLine(new Color(GuiInGame.guiColor), new Vector2(inputManager.mousePressedVector.X, inputManager.cursor.Position.Y), inputManager.cursor.Position, spriteBatch, 2);
                        DrawHelper.DrawLine(new Color(GuiInGame.guiColor), inputManager.mousePressedVector, new Vector2(inputManager.mousePressedVector.X, inputManager.cursor.Position.Y), spriteBatch, 2);
                    }
                }
            }
            console.Render(spriteBatch);
            if (!Settings.isUseSystemCursor)
            {
                inputManager.cursor.color = GuiInGame.guiColor;
                inputManager.cursor.Render(spriteBatch);
            }
        }
    }
}