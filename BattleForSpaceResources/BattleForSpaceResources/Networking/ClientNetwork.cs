using BattleForSpaceResources.Entitys;
using BattleForSpaceResources.Guis;
using BattleForSpaceResources.ShipComponents;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace BattleForSpaceResources.Networking
{
    public class ClientNetwork
    {
        private static ClientNetwork instance;
        private Core core = Core.GetCore();
        private NetClient client;
        private ClientPacketHandler packetHandler;
        private NetPeerConfiguration config;
        private FixedStepThread networkThreaad;
        private int timeOutTimer;
        //public bool isWar;
        private bool isOnline, isLeftDown, isRightDown, isLogging;
        private Stopwatch netST;
        public TimeSpan netTS;
        public ClientNetwork()
        {
            netST = new Stopwatch();
            this.config = new NetPeerConfiguration("BFSR");
            this.client = new NetClient(config);
            this.client.Start();
            instance = this;
        }
        public void Start()
        {
            this.packetHandler = new ClientPacketHandler(this);
            this.networkThreaad = new FixedStepThread(Update, 3);
        }
        public bool IsLogging()
        {
            return this.isLogging;
        }
        public bool IsLeftDown()
        {
            return isLeftDown;
        }
        public bool IsRightDown()
        {
            return isRightDown;
        }
        public void SetLeftButton(bool isD)
        {
            isLeftDown = isD;
        }
        public void SetRightButton(bool isD)
        {
            isRightDown = isD;
        }
        public void SetLogging(bool isLog)
        {
            this.isLogging = isLog;
        }
        public void SetOnline(bool isOnl)
        {
            this.isOnline = isOnl;
        }
        public bool IsOnline()
        {
            return this.isOnline;
        }
        public NetClient GetNetClient()
        {
            return this.client;
        }
        public static ClientNetwork GetClientNetwork()
        {
            return instance;
        }
        public void Update()
        {
            netST.Restart();
            NetIncomingMessage incmsg;
            while ((incmsg = client.ReadMessage()) != null)
            {
                switch (incmsg.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        {
                            packetHandler.ReadPacket(incmsg);
                            timeOutTimer = 0;
                        }
                        break;
                }
                client.Recycle(incmsg);
            }
            //if (timeOutTimer >= 60)
            //{
            //    outmsg = Core.net.client.CreateMessage();
            //    outmsg.Write((byte)PacketType.Refresh);
            //    client.SendMessage(outmsg, NetDeliveryMethod.Unreliable);
            //}
            timeOutTimer++;
            //if (timerUpdate++ > 200)
            {
                //timerUpdate = 0;
            }
            if (timeOutTimer >= 200)
            {
                timeOutTimer = 0;
                core.SetWorld(null);
                core.currentGui = new GuiMainMenu();
                core.currentGuiAdd = new GuiAddDisconnectError("Ошибка соединения", "Время ожидания истекло");
                isOnline = false;
                core.cam.control = false;
                //isLogRegFalse = false;
                isLogging = false;
            }
            netST.Stop();
            netTS = netST.Elapsed;
        }

        public void Stop()
        {
            //if (Core.net.isOnline)
            {
                networkThreaad.Stop();
                ClientPacketSender.Disconnect(core.GetPlayerName());
                core.currentGui = new GuiMainMenu();
                core.SetWorld(null);
                SetOnline(false);
                if (core.GetPlayerName().Equals("Player"))
                {
                    //Thread t = new Thread(StopLocalServer);
                    // t.Start();
                    StopLocalServer();
                }
            }
            //else if (core.world != null)
            //{
            //    core.currentgui = new guimainmenu();
            //    core.world = null;
            //    core.console.adddebugstring("closing server...");
            //    //thread t = new thread(() => servercore.instance.stop());
            //    //t.start();
            //    servercore.instance.stop();
            //    core.console.adddebugstring("server closed");
            //    core.net.isonline = false;
            //}
            core.cam.control = false;
        }
        private void StopLocalServer()
        {
            Core.console.AddDebugString("Closing server...");
            ServerCore.GetServerCore().Stop();
            Core.console.AddDebugString("Server closed");
        }
    }
}
