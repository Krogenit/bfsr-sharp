using BattleForSpaceResources.Networking;
using BattleForSpaceResources.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleForSpaceResources.BFSRSystem
{
    public class BFSRConsole
    {
        private List<AdvancedString> infoStrings = new List<AdvancedString>();
        private List<string> noTimeStrings = new List<string>();
        private Vector2 Position;
        private int timer;
        private Core core = Core.GetCore();
        private World world = Core.GetCore().GetWorld();
        private ClientNetwork net = ClientNetwork.GetClientNetwork();
        public BFSRConsole()
        {
            if (Settings.isDebug)
            {
                AddNoTimeString("BFSR Client dev_0.0.3");
                AddNoTimeString("Author: Krogenit");
                AddNoTimeString("");
                AddNoTimeString("Fps: " + Core.fps);
                AddNoTimeString("");
                AddNoTimeString("Update: " + string.Format("{0:00.0000}", Core.updateTime.TotalMilliseconds) + " ms");
                AddNoTimeString("Render: " + string.Format("{0:00.0000}", Core.renderTime.TotalMilliseconds) + " ms");
                AddNoTimeString("Network: " + string.Format("{0:00.0000}", net.netTS.TotalMilliseconds) + " ms");
                AddNoTimeString("");
                //if (world != null)
                {
                    AddNoTimeString("Game ojects: 0");// + (world.ships.Count + world.bullets.Count));
                    //int numpart = 0;
                    //for (int i = 0; i < world.ships.Count; i++)
                    {
                        //numpart += world.ships[i].particles.Count;
                    }
                    AddNoTimeString("Particles: 0");// + (numpart + ParticleSystem.particles.Count + ParticleSystem.lightParticles.Count + ParticleSystem.backcolorParticles.Count + ParticleSystem.colorDerbisParticle.Count + ParticleSystem.colorParticles.Count + ParticleSystem.colorlightParticles.Count));
                    AddNoTimeString("");
                }
                AddNoTimeString("Cursor position: X " + string.Format("{0:0.00}", core.inputManager.cursorAdvancedPosition.X) + " Y " + string.Format("{0:0.00}", core.inputManager.cursorAdvancedPosition.Y));
                AddNoTimeString("Cam position: X " + string.Format("{0:0.00}", core.cam.screenCenter.X) + " Y " + string.Format("{0:0.00}", core.cam.screenCenter.Y));
            }
        }
        public int GetStrings()
        {
            return infoStrings.Count;
        }
        public void AddDebugStringWithTimer(string s)
        {
            if(timer == 0)
            {
                AddDebugString(s);
                timer = 500;
            }
        }
        public void AddDebugString(string s)
        {
            AdvancedString aString = new AdvancedString(DateTime.Now.ToString("HH:mm:ss") + " " + s);
            infoStrings.Add(aString);
        }
        public void AddString(string s)
        {
            AdvancedString aString = new AdvancedString(s);
            aString.position = new Vector2(0,-core.screenHeight/2 + 100);
            infoStrings.Add(aString);
        }
        public void AddNoTimeString(string s)
        {
            noTimeStrings.Add(s);
        }
        public void UpdateNoTimeString(int i, string s)
        {
            noTimeStrings[i] = s;
        }
        public void Update()
        {
            Position = core.cam.screenCenter;
            if (infoStrings.Count >= 10)
            {
                infoStrings.RemoveAt(0);
            }
            for(int i=0;i<infoStrings.Count;i++)
            {
                if (infoStrings[i].position.Y < ((10 - i) * 12))
                {
                    float speed = (infoStrings[i].position.Y - ((10 - i) * 12)) / 50f;
                    if (speed > -0.8f)
                        speed = -0.8f;
                    infoStrings[i].position.Y -= speed;
                }
            }
            for (int i = 0; i < infoStrings.Count; i++)
            if (infoStrings[i].timer++ >= 400)
            {
                if (infoStrings[i].colorW > 0)
                {
                    infoStrings[i].colorW -= 0.01f;
                }
                else
                {
                    infoStrings.RemoveAt(i);
                }
            }
            if (Core.fpsCounter == 0)
            {
                if (Settings.isDebug)
                {
                    UpdateNoTimeString(3, "Fps: " + Core.fps);

                    UpdateNoTimeString(5, "Update: " + string.Format("{0:00.0000}", Core.updateTime.TotalMilliseconds) + " ms");
                    UpdateNoTimeString(6, "Render: " + string.Format("{0:00.0000}", Core.renderTime.TotalMilliseconds) + " ms");
                    UpdateNoTimeString(7, "Network: " + string.Format("{0:00.0000}", net.netTS.TotalMilliseconds) + " ms");
                }
            }
            if (Settings.isDebug)
            {
                if (world != null)
                {
                    UpdateNoTimeString(9, "Game ojects: " + (world.ships.Count + world.bullets.Count + world.shipsNpc.Count));
                    int numpart = 0;
                    for (int i = 0; i < world.ships.Count; i++)
                    {
                        numpart += world.ships[i].particles.Count;
                    }
                    UpdateNoTimeString(10, "Particles: " + (numpart + ParticleSystem.particles.Count + ParticleSystem.lightParticles.Count + ParticleSystem.backcolorParticles.Count + ParticleSystem.colorDerbisParticle.Count + ParticleSystem.colorParticles.Count + ParticleSystem.colorlightParticles.Count));
                    UpdateNoTimeString(12, "Cursor position: X " + string.Format("{0:0.00}", core.inputManager.cursorAdvancedPosition.X) + " Y " + string.Format("{0:0.00}", core.inputManager.cursorAdvancedPosition.Y));
                    UpdateNoTimeString(13, "Cam position: X " + string.Format("{0:0.00}", core.cam.screenCenter.X) + " Y " + string.Format("{0:0.00}", core.cam.screenCenter.Y));
                }
                else
                {
                    UpdateNoTimeString(12, "Cursor position: X " + string.Format("{0:0.00}", core.inputManager.cursorAdvancedPosition.X) + " Y " + string.Format("{0:0.00}", core.inputManager.cursorAdvancedPosition.Y));
                    UpdateNoTimeString(13, "Cam position: X " + string.Format("{0:0.00}", core.cam.screenCenter.X) + " Y " + string.Format("{0:0.00}", core.cam.screenCenter.Y));
                }
            }
            if (timer > 0)
                timer--;
        }

        public void Render(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < noTimeStrings.Count; i++)
            {
                spriteBatch.DrawString(Fonts.consoleFont, noTimeStrings[i], core.cam.screenCenter + new Vector2(-core.screenWidth / 2, -core.screenHeight / 2 + 124 + i * 12), Color.White);
            }
            for (int i = 0; i < infoStrings.Count; i++)
            {
                spriteBatch.DrawString(Fonts.consoleFont, infoStrings[i].GetString(), core.cam.screenCenter + new Vector2(-core.screenWidth / 2 + 200, -core.screenHeight / 2 + 120 - infoStrings[i].position.Y), new Color(new Vector4(infoStrings[i].colorW, infoStrings[i].colorW, infoStrings[i].colorW, infoStrings[i].colorW)));
            }
        }
    }

    public class AdvancedString
    {
        private string stringMain;
        public int timer;
        public Vector2 position;
        public float colorW = 1f;
        public AdvancedString(string s)
        {
            stringMain = s;
        }
        public string GetString()
        {
            return stringMain;
        }
    }
}
