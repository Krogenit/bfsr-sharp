using BattleForSpaceResources.Entitys;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace BattleForSpaceResources.Networking
{
    public class ServerCore
    {
        private static ServerCore instance;
        private NetServer server;
        private ServerPacketHandler packetHandler;
        private NetPeerConfiguration config;
        private List<NetIncomingMessage> messages = new List<NetIncomingMessage>();
        private LoginHandler loginHandler;
        private DataBase db;
        private FixedStepThread networkThreaad, updateThread, loginThread;
        private bool isDedicatedServer;
        private World world;
        public Random random = new Random();

        #region Debug
        private TimeSpan netTime, updateTime;
        private Stopwatch stNet, stUpdate;
        private int timerMs;
        #endregion
        public ServerCore(bool isDedic)
        {
            LogAdd("Starting server...");
            if (instance == null)
            {
                isDedicatedServer = isDedic;
                db = new DataBase();
                stNet = new Stopwatch();
                stUpdate = new Stopwatch();
                config = new NetPeerConfiguration("BFSR");
                config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
                config.Port = 25565;
                //config.SimulatedMinimumLatency = 0.2f;
                server = new NetServer(config);
                loginHandler = new LoginHandler(this);
                packetHandler = new ServerPacketHandler(this);
                instance = this;
                this.Start();
            }
            else
            {
                instance.isDedicatedServer = isDedic;
                instance.Start();
            }
            LogAdd("Server started");
        }
        public void Start()
        {
            world = new World(false);
            server.Start();
            networkThreaad = new FixedStepThread(UpdateNetwork, 3);
            updateThread = new FixedStepThread(Update, 1);
            loginThread = new FixedStepThread(UpdateLogin, 4);
        }
        public static void LogAdd(string s)
        {
            Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " " + s);
        }
        private void UpdateLogin()
        {
            loginHandler.Update();
        }
        private void UpdateNetwork()
        {
            stNet.Restart();
            //UpdatePackets(true, null, null);
            lock (messages)
            {
                server.ReadMessages(messages);
                List<NetIncomingMessage> mss = messages;
                for (int i = 0; i < mss.Count; i++)
                {
                    NetIncomingMessage m = mss[i];
                    switch (m.MessageType)
                    {
                        case NetIncomingMessageType.Data:
                            PacketType pk = (PacketType)m.ReadByte();
                            packetHandler.ReadPacket(pk, m);
                            //UpdatePackets(false, m, messages);
                            mss.Remove(m);
                            server.Recycle(m);
                            break;
                    }
                }
            }
            //if (timerUpdate++ > 20)
            {
                //timerUpdate = 0;
            }
            //else if(timerUpdate == 20)
            {
                ServerPacketSender.UpdatePlayers(world, db);
                ServerPacketSender.UpdateNpcs(world);
            }
            stNet.Stop();
            netTime = stNet.Elapsed;
        }
        //public void UpdatePackets(bool isAdd, NetIncomingMessage m, List<NetIncomingMessage> mss)
        //{
        //    lock (locking)
        //    {
        //        if (isAdd)
        //        {
        //            server.ReadMessages(messages);
        //        }
        //        else
        //        {
        //            mss.Remove(m);
        //        }
        //    }
        //}
        private void Update()
        {
            stUpdate.Restart();
            world.Update();
            stUpdate.Stop();
            updateTime = stUpdate.Elapsed;

            if (timerMs++ > 400)
            {
                LogAdd("Update: " + string.Format("{0:00.0000}", updateTime.TotalMilliseconds) + " ms " +
                    "| Network: " + string.Format("{0:00.0000}", netTime.TotalMilliseconds) + " ms");
                timerMs = 0;
            }
        }
        public static ServerCore GetServerCore()
        {
            return instance;
        }
        public bool isDedicated()
        {
            return this.isDedicatedServer;
        }
        public NetOutgoingMessage CreateMessage()
        {
            return server.CreateMessage();
        }
        public NetServer GetServer()
        {
            return this.server;
        }
        public List<NetIncomingMessage> GetMessages()
        {
            return messages;
        }
        public DataBase GetDataBase()
        {
            return this.db;
        }
        public World GetWorld()
        {
            return this.world;
        }
        public void Stop()
        {
            for (int i = 0; i < world.ships.Count; i++)
            {
                db.SaveUser(world.ships[i].shipName, world.ships[i]);
            }
            networkThreaad.Stop();
            updateThread.Stop();
            loginThread.Stop();
            //Thread.Sleep(300);
            List<NetIncomingMessage> mss = messages;
            for (int i = 0; i < mss.Count; i++)
            {
                server.Recycle(mss[i]);
            }
            messages.Clear();
            server.Shutdown("Server closed");
            LogAdd("Server closed");
        }
    }
}
