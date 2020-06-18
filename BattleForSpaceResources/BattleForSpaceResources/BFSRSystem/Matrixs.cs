using BattleForSpaceResources.Guis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleForSpaceResources
{
    public class Matrixs
    {
        private Core core = Core.GetCore();
        public Vector2 screenCenter;
        public Vector2 screenControlCenter, newCamPos;
        public Matrix defaultScreen;
        public Matrix guiScreen;
        public Matrix smokeScreen1, ambientScreen1, ambientScreen2, ambientScreen3, planetScreen, moonScreen;
        private Viewport viewport;
        public float zoom = 1;
        public float smokeZoom = 0.9F;
        public float planetZoom = 1;
        public float ambientZoom1 = 0.8F, ambientZoom2 = 0.7F, ambientZoom3 = 0.6F, moonZoom = 0.95F;
        public bool control = false, camToNewPos;
        private float screenMovingSpeed = 25F;
        public Matrixs(Viewport newViewport)
        {
            viewport = newViewport;
            screenCenter = new Vector2(screenControlCenter.X, screenControlCenter.Y);
        }
        public void Update()
        {
            if (core.currentGui == null && (core.currentGuiAdd == null || core.currentGuiAdd.GetType() == typeof(GuiAddDestroyed)) && !core.isChat)
            {
                if (!control)
                {
                    if (core.inputManager.cursor.Position.X <= screenCenter.X - core.screenWidth / 2 + 3)
                    {
                        screenCenter.X -= screenMovingSpeed;
                        camToNewPos = false;
                    }
                    if (core.inputManager.IsKeyDown(Keys.A))
                    {
                        screenCenter.X -= screenMovingSpeed * 2;
                        camToNewPos = false;
                    }
                    if (core.inputManager.cursor.Position.X >= screenCenter.X + core.screenWidth / 2 - 3)
                    {
                        screenCenter.X += screenMovingSpeed;
                        camToNewPos = false;
                    }
                    if (core.inputManager.IsKeyDown(Keys.D))
                    {
                        screenCenter.X += screenMovingSpeed * 2;
                        camToNewPos = false;
                    }
                    if (core.inputManager.cursor.Position.Y <= screenCenter.Y - core.screenHeight / 2 + 3)
                    {
                        screenCenter.Y -= screenMovingSpeed;
                        camToNewPos = false;
                    }
                    if (core.inputManager.IsKeyDown(Keys.W))
                    {
                        screenCenter.Y -= screenMovingSpeed * 2;
                        camToNewPos = false;
                    }
                    if (core.inputManager.cursor.Position.Y >= screenCenter.Y + core.screenHeight / 2 - 3)
                    {
                        screenCenter.Y += screenMovingSpeed;
                        camToNewPos = false;
                    }
                    if (core.inputManager.IsKeyDown(Keys.S))
                    {
                        screenCenter.Y += screenMovingSpeed * 2;
                        camToNewPos = false;
                    }
                }
                else
                {
                    screenCenter += new Vector2(core.inputManager.cursor.Position.X - screenCenter.X, core.inputManager.cursor.Position.Y - screenCenter.Y) / 30f;
                    camToNewPos = false;
                }
                float scroll = core.inputManager.GetMouseScroll();
                zoom += scroll;
                if (zoom > 1.5F)
                {
                    zoom -= scroll;
                }
                else if (zoom < 0.4F)
                {
                    zoom -= scroll;
                }
                else
                {
                    smokeZoom += scroll / 3F;
                    planetZoom += scroll / 16F;
                    ambientZoom3 += scroll / 5F;
                    ambientZoom2 += scroll / 9F;
                    ambientZoom1 += scroll / 12F;
                    moonZoom += scroll / 14F;
                }
            }
            World world = core.GetWorld();
            if (world != null)
            {
                if (control && world.selectedShips.Count > 0)
                {
                    //for (int i = 0; i < world.ships.Count; i++)
                    {
                        //if (world.ships[i].playerName.Equals(Core.playerName))
                        {
                            int j = 0;
                            for (int i = 0; i < world.selectedShips.Count; i++)
                            {
                                if (world.selectedShips[i].isLive && world.selectedShips[i].isControled)
                                {
                                    j = i;
                                    break;
                                }
                            }
                            //float dis = Vector2.Distance(world.ships[i].Position, screenCenter);
                            float dis = Vector2.Distance(world.selectedShips[j].Position, screenCenter);
                            if (dis > 2)
                            {
                                var mDx = (world.selectedShips[j].Position.X) - screenCenter.X;
                                var mDy = (world.selectedShips[j].Position.Y) - screenCenter.Y;
                                float rot = (float)Math.Atan2(mDy, mDx);
                                Vector2 velocity = new Vector2(-(float)Math.Cos(rot) * (dis / 20), -(float)Math.Sin(rot) * (dis / 20));
                                screenCenter -= velocity;
                                //break;
                            }
                        }
                    }
                }
                if (camToNewPos)
                {
                    float dis = Vector2.Distance(newCamPos, screenCenter);
                    if (dis > 2)
                    {
                        var mDx = (newCamPos.X) - screenCenter.X;
                        var mDy = (newCamPos.Y) - screenCenter.Y;
                        float rot = (float)Math.Atan2(mDy, mDx);
                        Vector2 velocity = new Vector2(-(float)Math.Cos(rot) * (dis / 20), -(float)Math.Sin(rot) * (dis / 20));
                        screenCenter -= velocity;
                        //break;
                    }
                    else
                    {
                        camToNewPos = false;
                    }
                }
            }
            defaultScreen = Matrix.CreateTranslation(new Vector3(-screenCenter.X, -screenCenter.Y, 0)) *
                            Matrix.CreateScale(new Vector3(zoom, zoom, 1)) *
                            Matrix.CreateTranslation(new Vector3(viewport.Width / 2, viewport.Height / 2, 0));
            smokeScreen1 = Matrix.CreateTranslation(new Vector3(-screenCenter.X, -screenCenter.Y, 0)) *
                            Matrix.CreateScale(new Vector3(smokeZoom, smokeZoom, 1)) *
                            Matrix.CreateTranslation(new Vector3(viewport.Width / 2, viewport.Height / 2, 0));
            planetScreen = Matrix.CreateTranslation(new Vector3(-screenCenter.X, -screenCenter.Y, 0)) *
                            Matrix.CreateScale(new Vector3(planetZoom, planetZoom, 1)) *
                            Matrix.CreateTranslation(new Vector3(viewport.Width / 2, viewport.Height / 2, 0));
            moonScreen = Matrix.CreateTranslation(new Vector3(-screenCenter.X, -screenCenter.Y, 0)) *
                            Matrix.CreateScale(new Vector3(moonZoom, moonZoom, 1)) *
                            Matrix.CreateTranslation(new Vector3(viewport.Width / 2, viewport.Height / 2, 0));
            ambientScreen1 = Matrix.CreateTranslation(new Vector3(-screenCenter.X, -screenCenter.Y, 0)) *
                            Matrix.CreateScale(new Vector3(ambientZoom1, ambientZoom1, 1)) *
                            Matrix.CreateTranslation(new Vector3(viewport.Width / 2, viewport.Height / 2, 0));
            ambientScreen2 = Matrix.CreateTranslation(new Vector3(-screenCenter.X, -screenCenter.Y, 0)) *
                            Matrix.CreateScale(new Vector3(ambientZoom2, ambientZoom2, 1)) *
                            Matrix.CreateTranslation(new Vector3(viewport.Width / 2, viewport.Height / 2, 0));
            ambientScreen3 = Matrix.CreateTranslation(new Vector3(-screenCenter.X, -screenCenter.Y, 0)) *
                            Matrix.CreateScale(new Vector3(ambientZoom3, ambientZoom3, 1)) *
                            Matrix.CreateTranslation(new Vector3(viewport.Width / 2, viewport.Height / 2, 0));
            guiScreen = Matrix.CreateTranslation(new Vector3(-screenCenter.X, -screenCenter.Y, 0)) *
                            Matrix.CreateTranslation(new Vector3(viewport.Width / 2, viewport.Height / 2, 0));
        }
        private Matrix getCamCenter()
        {
            return Matrix.CreateTranslation(new Vector3(-screenCenter.X, -screenCenter.Y, 0));
        }
        private Matrix getScreenCenter()
        {
            return Matrix.CreateTranslation(new Vector3(viewport.Width / 2, viewport.Height / 2, 0));
        }
        private Matrix getZoom(float zoom)
        {
            return Matrix.CreateScale(new Vector3(zoom, zoom, 1));
        }
    }
}
