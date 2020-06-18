using BattleForSpaceResources;
using BattleForSpaceResources.Entitys;
using BattleForSpaceResources.Guis;
using BattleForSpaceResources.Networking;
using BattleForSpaceResources.ShipComponents;
using Lidgren.Network;
using Microsoft.Xna.Framework;
public class ServerPacketHandler
{
    private ServerCore serverCore;
    public ServerPacketHandler(ServerCore serv)
    {
        this.serverCore = serv;
    }
    public void ReadPacket(PacketType pk, NetIncomingMessage incmsg)
    {
        switch (pk)
        {
            case PacketType.Message:
                Message(incmsg);
                break;
            case PacketType.Respawn:
                Respawn(incmsg);
                break;
            case PacketType.SelectShip:
                SelectShip(incmsg);
                break;
            case PacketType.ConfrimSelect:
                ConfrimSelect(incmsg);
                break;
            case PacketType.Register:
                Register(incmsg);
                break;
            case PacketType.Move:
                UpdateMove(incmsg); 
                break;
            case PacketType.Disconnect:
                DisconnetPlayer(incmsg);
                break;
            case PacketType.Login:
                Login(incmsg);
                break;
            case PacketType.Connect:
                if (!serverCore.isDedicated())
                    Connect(incmsg.ReadString(), incmsg);
                break;
            case PacketType.Refresh:
                NetOutgoingMessage outmsg = serverCore.CreateMessage();
                outmsg.Write((byte)PacketType.Refresh);
                ServerPacketSender.SendMessageTo(outmsg, incmsg.SenderConnection);
                break;
        }
    }
    private void UpdateMove(NetIncomingMessage incmsg)
    {
        string name = incmsg.ReadString();
        World w = serverCore.GetWorld();
        for (int i = 0; i < w.ships.Count; i++)
        {
            if (w.ships[i].shipName.Equals(name))
            {
                w.ships[i].timeOut = 0;
                w.ships[i].cursorPosition = new Vector2(incmsg.ReadSingle(), incmsg.ReadSingle());
                w.ships[i].isWPress = incmsg.ReadBoolean();
                w.ships[i].isSPress = incmsg.ReadBoolean();
                w.ships[i].isAPress = incmsg.ReadBoolean();
                w.ships[i].isDPress = incmsg.ReadBoolean();
                w.ships[i].isXPress = incmsg.ReadBoolean();
                w.ships[i].isLeftPress = incmsg.ReadBoolean();
                w.ships[i].isRightPress = incmsg.ReadBoolean();
                return;
            }
        }
        NetOutgoingMessage outmsg = serverCore.CreateMessage();
        outmsg.Write((byte)PacketType.DestroyShip);
        outmsg.Write(name);
        ServerPacketSender.SendMessageTo(outmsg, incmsg.SenderConnection);
    }
    private void Message(NetIncomingMessage incmsg)
    {
        NetOutgoingMessage outmsg = serverCore.GetServer().CreateMessage();
        outmsg.Write((byte)PacketType.Message);
        outmsg.Write(incmsg.ReadString());
        outmsg.Write(incmsg.ReadString());
        ServerPacketSender.SendMessageToAll(outmsg);
    }
    private void Register(NetIncomingMessage incmsg)
    {
        string name = incmsg.ReadString();
        string pass = incmsg.ReadString();
        string s = serverCore.GetDataBase().Register(name, pass);

        NetOutgoingMessage outmsg = serverCore.CreateMessage();
        outmsg.Write((byte)PacketType.LogRegMsg);
        outmsg.Write(s);
        ServerPacketSender.SendMessageTo(outmsg, incmsg.SenderConnection);
    }
    private void Login(NetIncomingMessage incmsg)
    {
        string playerName = incmsg.ReadString();
        string password = incmsg.ReadString();
        string s = serverCore.GetDataBase().Login(playerName, password, serverCore.isDedicated());

        if (s.Equals("Успешно"))
        {
            Connect(playerName, incmsg);
        }
        else
        {
            NetOutgoingMessage outmsg = serverCore.CreateMessage();
            outmsg.Write((byte)PacketType.LogRegMsg);
            outmsg.Write(s);
            ServerPacketSender.SendMessageTo(outmsg, incmsg.SenderConnection);
        }
    }
    private void Connect(string playerName, NetIncomingMessage incmsg)
    {
        World w = serverCore.GetWorld();
        DataBase db = serverCore.GetDataBase();
        if (!serverCore.isDedicated())
        {
            db.Login(playerName, "test", false);
        }
        bool playerRefhresh = true;
        for (int i = 0; i < w.ships.Count; i++)
        {
            if (w.ships[i].shipName.Equals(playerName))
            {
                NetOutgoingMessage outmsg = serverCore.CreateMessage();
                outmsg.Write((byte)PacketType.Deny);
                ServerPacketSender.SendMessageTo(outmsg, incmsg.SenderConnection);
                System.Threading.Thread.Sleep(100);
                incmsg.SenderConnection.Disconnect("bye");
                playerRefhresh = false;
                break;
            }
        }
        if (playerRefhresh == true)
        {
            //System.Threading.Thread.Sleep(100);
            int[] components = db.GetUserComponents(playerName);//{ 1, 1, 1, 1, 1, 1, 1 };
            if (components != null && components[0] != 0 && components[1] != 0)
            {
                byte[] b = db.GetUserHangar(playerName);
                ShipType st = (ShipType)b[0];
                Faction f = (Faction)b[1];
                Vector2 pos = f == Faction.Human ? new Vector2(0, 1500) : f == Faction.Enemy ? new Vector2(-1500, -500) : new Vector2(1500, -500);
                ShipPlayer s = new ShipPlayer(pos, pos, f, st, components, w, 0, 0);
                s.shipName = playerName;
                GunSlot[] gs = db.GetUserGuns(playerName, s);
                s.AddGuns(gs);
                ServerPacketSender.SendAllShips(w, incmsg.SenderConnection);
                //timersToSendNpcs.Add(0);
                //netConnections.Add(incmsg.SenderConnection);
                ServerPacketSender.SendAllNpcs(w, incmsg.SenderConnection);
                w.ships.Add(s);
                ServerPacketSender.SendNewPlayer(s, false);
                ServerCore.LogAdd(playerName + " connected.");
            }
            else
            {
                ServerPacketSender.SendOpenGui(GuiType.SelectFaction, incmsg.SenderConnection);
            }
        }
    }
    private void DisconnetPlayer(NetIncomingMessage incmsg)
    {
        string playerName = incmsg.ReadString();
        World w = serverCore.GetWorld();
        DataBase db = serverCore.GetDataBase();
        for (int i = 0; i < w.ships.Count; i++)
        {
            if (w.ships[i].shipName.Equals(playerName))
            {
                incmsg.SenderConnection.Disconnect("bye");
                //System.Threading.Thread.Sleep(100);
                ServerCore.LogAdd(playerName + " disconnected.");

                if (ServerCore.GetServerCore().GetServer().ConnectionsCount != 0)
                {
                    NetOutgoingMessage outmsg = serverCore.CreateMessage();
                    outmsg.Write((byte)PacketType.Disconnect);
                    outmsg.Write(playerName);
                    ServerPacketSender.SendMessageToAll(outmsg);
                }
                db.SaveUser(playerName, w.ships[i]);
                w.ships.RemoveAt(i);
                i--;
                break;
            }
        }
    }
    private void Respawn(NetIncomingMessage incmsg)
    {
        string playerName = incmsg.ReadString();
        DataBase db = serverCore.GetDataBase();
        World w = serverCore.GetWorld();
        int[] components = db.GetUserComponents(playerName);//{ 1, 1, 1, 1, 1, 1, 1 };
        if (components != null && components[0] != 0 && components[1] != 0)
        {
            byte[] b = db.GetUserHangar(playerName);
            ShipType st = (ShipType)b[0];
            Faction f = (Faction)b[1];
            Vector2 pos = f == Faction.Human ? new Vector2(0, 1500) : f == Faction.Enemy ? new Vector2(-1500, -500) : new Vector2(1500, -500);
            ShipPlayer s = new ShipPlayer(pos, pos, f, st, components, w, 0, 0);
            s.shipName = playerName;
            GunSlot[] gs = db.GetUserGuns(playerName, s);
            s.AddGuns(gs);
            int xpos = serverCore.random.Next(2) == 0 ? 750 : -750;
            int ypos = serverCore.random.Next(2) == 0 ? 750 : -750;
            s.jumpX = xpos;
            s.jumpY = ypos;
            w.ships.Add(s);
            ServerPacketSender.SendNewPlayer(s, true);
        }
    }
    private void SelectShip(NetIncomingMessage incmsg)
    {
        string playerName = incmsg.ReadString();
        World w = serverCore.GetWorld();
        for (int i = 0; i < w.ships.Count; i++)
        {
            if (w.ships[i].shipName.Equals(playerName))
            {
                w.ships[i].isControled = incmsg.ReadBoolean();
                break;
            }
        }
    }
    private void ConfrimSelect(NetIncomingMessage incmsg)
    {
        string playerName = incmsg.ReadString();
        World w = serverCore.GetWorld();
        DataBase db = serverCore.GetDataBase();
        ConfrimType ct = (ConfrimType)incmsg.ReadByte();
        int xpos = serverCore.random.Next(2) == 0 ? 750 : -750;
        int ypos = serverCore.random.Next(2) == 0 ? 750 : -750;
        if (ct == ConfrimType.SelectFaction)
        {
            Faction f = (Faction)incmsg.ReadByte();
            ShipType st = 0;
            Vector2 pos = Vector2.Zero;
            GunSlot[] gs = new GunSlot[2];
            int[] components = { 1, 1, 0, 0, 0, 0, 0 };
            if (f == Faction.Civilian)
            {
                st = ShipType.CivSmall1;
                pos = new Vector2(1500, -500);
            }
            else if (f == Faction.Enemy)
            {
                st = ShipType.EnemySmall1;
                pos = new Vector2(-1500, -500);
            }
            else if (f == Faction.Human)
            {
                st = ShipType.HumanSmall1;
                pos = new Vector2(0, 1500);
            }
            ShipPlayer s = new ShipPlayer(pos, pos, f, st, components, w, 0, 0);
            s.shipName = playerName;
            if (f == Faction.Civilian)
            {
                for (int i = 0; i < 2; i++)
                {
                    gs[i] = new GunSlot(GunType.GausSmall, s);
                }
            }
            else if (f == Faction.Enemy)
            {
                for (int i = 0; i < 2; i++)
                {
                    gs[i] = new GunSlot(GunType.LaserSmall, s);
                }
            }
            else if (f == Faction.Human)
            {
                for (int i = 0; i < 2; i++)
                {
                    gs[i] = new GunSlot(GunType.PlasmSmall, s);
                }
            }
            s.jumpX = xpos;
            s.jumpY = ypos;
            s.AddGuns(gs);
            //timersToSendNpcs.Add(0);
            //netConnections.Add(incmsg.SenderConnection);
            ServerPacketSender.SendAllNpcs(w, incmsg.SenderConnection);
            w.ships.Add(s);
            ServerPacketSender.SendNewPlayer(s, false);
            ServerCore.LogAdd(playerName + " connected.");
            db.SaveUser(playerName, s);
        }
    }
}