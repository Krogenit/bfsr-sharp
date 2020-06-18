using BattleForSpaceResources;
using BattleForSpaceResources.Entitys;
using BattleForSpaceResources.Guis;
using BattleForSpaceResources.Networking;
using Lidgren.Network;
using System;
using System.Collections.Generic;
public class ServerPacketSender
{
    private static NetServer server = ServerCore.GetServerCore().GetServer();
    public static void SendCreateNpcShip(Ship s)
    {
        if (server.Connections.Count > 0)
        {
            NetOutgoingMessage outmsg = server.CreateMessage();
            outmsg.Write((byte)PacketType.CreateNpcShip);
            outmsg.Write(s.id);
            outmsg.Write(s.Position.X);
            outmsg.Write(s.Position.Y);
            //outmsg.Write(s.Rotation);
            outmsg.Write(s.jumpX);
            outmsg.Write(s.jumpY);
            outmsg.Write((byte)s.shipType);
            outmsg.Write((byte)s.shipFaction);
            outmsg.Write((byte)s.engineType);
            outmsg.Write((byte)s.reactorType);
            outmsg.Write((byte)s.shieldType);
            for (int j = 0; j < 4; j++)
            {
                outmsg.Write((byte)s.armorPlates[j].armorType);
            }
            byte gunsNum = 0;
            for (int j = 0; j < s.gunSlots.Length; j++)
            {
                if (s.gunSlots[j] != null)
                    gunsNum++;
            }
            outmsg.Write(gunsNum);
            for (int j = 0; j < s.gunSlots.Length; j++)
            {
                if (s.gunSlots[j] != null)
                    outmsg.Write((byte)s.gunSlots[j].gunType);
            }
            SendMessageToAll(outmsg);
        }
    }
    public static void UpdatePlayers(World w, DataBase db)
    {
        for (int i = 0; i < w.ships.Count; i++)
        {
            w.ships[i].timeOut++;
            NetOutgoingMessage outmsg = server.CreateMessage();
            outmsg.Write((byte)PacketType.Move);
            outmsg.Write(w.ships[i].shipName);
            outmsg.Write(w.ships[i].Position.X);
            outmsg.Write(w.ships[i].Position.Y);
            outmsg.Write(w.ships[i].Rotation);
            outmsg.Write(w.ships[i].velocity.X);
            outmsg.Write(w.ships[i].velocity.Y);
            outmsg.Write(w.ships[i].hull);
            outmsg.Write(w.ships[i].shield);
            ServerPacketSender.SendMessageToAll(outmsg);
            if (w.ships[i].timeOut > 180)
            {
                ServerCore.LogAdd(w.ships[i].shipName + " is timed out.");
                //System.Threading.Thread.Sleep(100);

                if (server.ConnectionsCount != 0)
                {
                    outmsg = server.CreateMessage();

                    outmsg.Write((byte)PacketType.Disconnect);
                    outmsg.Write(w.ships[i].shipName);

                    SendMessageToAll(outmsg);
                }
                db.SaveUser(w.ships[i].shipName, w.ships[i]);
                w.ships.RemoveAt(i);
                i--;
                break;
            }
        }
    }
    public static void UpdateNpcs(World w)
    {
        if (server.ConnectionsCount > 0)
        {
            NetOutgoingMessage outmsg = server.CreateMessage();
            outmsg.Write((byte)PacketType.MoveNpcs);
            outmsg.Write(w.shipsNpc.Count);
            for (int i = 0; i < w.shipsNpc.Count; i++)
            {
                //outmsg.Write(w.shipsNpc.Count);
                outmsg.Write(w.shipsNpc[i].id);
                outmsg.Write(w.shipsNpc[i].Position.X);
                outmsg.Write(w.shipsNpc[i].Position.Y);
                outmsg.Write(w.shipsNpc[i].Rotation);
                outmsg.Write(w.shipsNpc[i].velocity.X);
                outmsg.Write(w.shipsNpc[i].velocity.Y);
                outmsg.Write(w.shipsNpc[i].hull);
                outmsg.Write(w.shipsNpc[i].shield);
                outmsg.Write((byte)w.shipsNpc[i].aiDir);
                outmsg.Write(w.shipsNpc[i].directionTimer);
            }
            server.SendMessage(outmsg, server.Connections, NetDeliveryMethod.Unreliable, 0);
        }
    }
    public static void SendDestroyShield(Ship s)
    {
        NetOutgoingMessage outmsg = server.CreateMessage();
        outmsg.Write((byte)PacketType.DestroyShield);
        outmsg.Write(s.shipName);
        SendMessageToAll(outmsg);
    }
    public static void SendDestroyShip(Ship s)
    {
        NetOutgoingMessage outmsg = server.CreateMessage();
        outmsg.Write((byte)PacketType.DestroyShip);
        outmsg.Write(s.id);
        if (s.id == 0)
            outmsg.Write(s.shipName);
        SendMessageToAll(outmsg);
    }
    public static void SendShipEngine(Ship s, byte eng)
    {
        NetOutgoingMessage outmsg = server.CreateMessage();
        outmsg.Write((byte)PacketType.ShipEngine);
        outmsg.Write(s.shipName);
        outmsg.Write(eng);
        SendMessageToAll(outmsg);
    }
    public static void SendGunShoot(int i, int id)
    {
        NetOutgoingMessage outmsg = server.CreateMessage();
        outmsg.Write((byte)PacketType.GunShoot);
        outmsg.Write(id);
        outmsg.Write((byte)i);
        SendMessageToAll(outmsg);
    }
    public static void SendGunShoot(int i, string name)
    {
        NetOutgoingMessage outmsg = server.CreateMessage();
        outmsg.Write((byte)PacketType.GunShoot);
        outmsg.Write(0);
        outmsg.Write(name);
        outmsg.Write((byte)i);
        SendMessageToAll(outmsg);
    }
    public static void SendOpenGui(GuiType gt, NetConnection con)
    {
        NetOutgoingMessage outmsg = server.CreateMessage();
        outmsg.Write((byte)PacketType.OpenGui);
        outmsg.Write((byte)gt);
        SendMessageTo(outmsg, con);
    }
    public static void SendAllShips(World w, NetConnection con)
    {
        for (int i = 0; i < w.ships.Count; i++)
        {
            NetOutgoingMessage outmsg = server.CreateMessage();
            outmsg.Write((byte)PacketType.Connect);
            outmsg.Write(w.ships[i].shipName);
            outmsg.Write(w.ships[i].Position.X);
            outmsg.Write(w.ships[i].Position.Y);
            outmsg.Write(w.ships[i].Rotation);
            outmsg.Write((byte)w.ships[i].shipType);
            outmsg.Write((byte)w.ships[i].shipFaction);
            outmsg.Write((byte)w.ships[i].engineType);
            outmsg.Write((byte)w.ships[i].reactorType);
            outmsg.Write((byte)w.ships[i].shieldType);
            for (int j = 0; j < 4; j++)
            {
                outmsg.Write((byte)w.ships[i].armorPlates[j].armorType);
            }
            byte gunsNum = 0;
            for (int j = 0; j < w.ships[i].gunSlots.Length; j++)
            {
                if (w.ships[i].gunSlots[j] != null)
                    gunsNum++;
            }
            outmsg.Write(gunsNum);
            for (int j = 0; j < w.ships[i].gunSlots.Length; j++)
            {
                if (w.ships[i].gunSlots[j] != null)
                    outmsg.Write((byte)w.ships[i].gunSlots[j].gunType);
            }
            SendMessageTo(outmsg, con);
        }
    }
    public static void SendAllNpcs(World w, NetConnection con)
    {
        for (int i = 0; i < w.shipsNpc.Count; i++)
        {
            NetOutgoingMessage outmsg = server.CreateMessage();
            outmsg.Write((byte)PacketType.CreateOldNpcShip);
            outmsg.Write(w.shipsNpc[i].id);
            outmsg.Write(w.shipsNpc[i].Position.X);
            outmsg.Write(w.shipsNpc[i].Position.Y);
            outmsg.Write(w.shipsNpc[i].Rotation);
            outmsg.Write((byte)w.shipsNpc[i].shipType);
            outmsg.Write((byte)w.shipsNpc[i].shipFaction);
            outmsg.Write((byte)w.shipsNpc[i].engineType);
            outmsg.Write((byte)w.shipsNpc[i].reactorType);
            outmsg.Write((byte)w.shipsNpc[i].shieldType);
            for (int j = 0; j < 4; j++)
            {
                outmsg.Write((byte)w.shipsNpc[i].armorPlates[j].armorType);
            }
            byte gunsNum = 0;
            for (int j = 0; j < w.shipsNpc[i].gunSlots.Length; j++)
            {
                if (w.shipsNpc[i].gunSlots[j] != null)
                    gunsNum++;
            }
            outmsg.Write(gunsNum);
            for (int j = 0; j < w.shipsNpc[i].gunSlots.Length; j++)
            {
                if (w.shipsNpc[i].gunSlots[j] != null)
                    outmsg.Write((byte)w.shipsNpc[i].gunSlots[j].gunType);
            }
            SendMessageTo(outmsg, con);
        }
    }
    public static void SendNewPlayer(Ship s, bool isRepsawn)
    {
        NetOutgoingMessage outmsg = server.CreateMessage();
        if (isRepsawn)
            outmsg.Write((byte)PacketType.Respawn);
        else
            outmsg.Write((byte)PacketType.ConnectNewPlayer);
        outmsg.Write(s.shipName);
        outmsg.Write(s.Position.X);
        outmsg.Write(s.Position.Y);
        //outmsg.Write(s.Rotation);
        outmsg.Write(s.jumpX);
        outmsg.Write(s.jumpY);
        outmsg.Write((byte)s.shipType);
        outmsg.Write((byte)s.shipFaction);
        outmsg.Write((byte)s.engineType);
        outmsg.Write((byte)s.reactorType);
        outmsg.Write((byte)s.shieldType);
        for (int j = 0; j < 4; j++)
        {
            outmsg.Write((byte)s.armorPlates[j].armorType);
        }
        byte gunsNum = 0;
        for (int j = 0; j < s.gunSlots.Length; j++)
        {
            if (s.gunSlots[j] != null)
                gunsNum++;
        }
        outmsg.Write(gunsNum);
        for (int j = 0; j < s.gunSlots.Length; j++)
        {
            if (s.gunSlots[j] != null)
                outmsg.Write((byte)s.gunSlots[j].gunType);
        }
        SendMessageToAll(outmsg);
    }
    public static void CreateBullet(Bullet b)
    {
        NetOutgoingMessage outmsg = server.CreateMessage();
        outmsg.Write((byte)PacketType.CreateBullet);
        outmsg.Write((byte)b.gunType);
        outmsg.Write(b.Position.X);
        outmsg.Write(b.Position.Y);
        outmsg.Write(b.Rotation);
        outmsg.Write(b.bulletSpeed);
        outmsg.Write((byte)b.owner.shipFaction);
        outmsg.Write(b.owner.shipName);
        SendMessageToAll(outmsg);
    }
    public static void UpdateBullet(List<Bullet> bullets)
    {
        NetOutgoingMessage outmsg = server.CreateMessage();
        outmsg.Write((byte)PacketType.MoveBullet);
        outmsg.Write(bullets.Count);
        for (int i = 0; i < bullets.Count; i++)
        {
            outmsg.Write(bullets[i].Position.X);
            outmsg.Write(bullets[i].Position.Y);
        }
        SendMessageToAll(outmsg);
    }
    public static void SendMessageTo(NetOutgoingMessage msg, NetConnection con)
    {
        server.SendMessage(msg, con, NetDeliveryMethod.ReliableOrdered, 0);
    }
    public static void SendMessageToAll(NetOutgoingMessage msg)
    {
        if (server.ConnectionsCount > 0)
            server.SendMessage(msg, server.Connections, NetDeliveryMethod.ReliableOrdered, 0);
    }
}