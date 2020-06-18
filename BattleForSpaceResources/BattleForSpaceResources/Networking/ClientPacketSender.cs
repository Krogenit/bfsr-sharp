using BattleForSpaceResources;
using BattleForSpaceResources.Entitys;
using BattleForSpaceResources.Guis;
using BattleForSpaceResources.Networking;
using Lidgren.Network;
using Microsoft.Xna.Framework.Input;
class ClientPacketSender
{
    private static NetClient client = ClientNetwork.GetClientNetwork().GetNetClient();
    private static ClientNetwork net = ClientNetwork.GetClientNetwork();
    private static Core core = Core.GetCore();
    public static void Connect(string name, string ip, int port)
    {
        net.SetLogging(true);
        NetOutgoingMessage outmsg = client.CreateMessage();
        outmsg.Write((byte)20);
        client.Connect(ip, port, outmsg);
        System.Threading.Thread.Sleep(1500);
        outmsg = client.CreateMessage();
        outmsg.Write((byte)PacketType.Connect);
        outmsg.Write(name);
        client.SendMessage(outmsg, NetDeliveryMethod.ReliableOrdered);

        //System.Threading.Thread.Sleep(300);
        net.SetOnline(true);
        Core c = Core.GetCore();
        c.SetPlayerName(name);
        //isWar = false;
    }
    public static void Login(string name, string password)
    {
        net.SetLogging(true);
        NetOutgoingMessage outmsg = client.CreateMessage();
        outmsg.Write((byte)20);
        client.Connect("127.0.0.1", 25565, outmsg);
        System.Threading.Thread.Sleep(1500);
        outmsg = client.CreateMessage();
        outmsg.Write((byte)PacketType.Login);
        outmsg.Write(name);
        outmsg.Write(password);
        client.SendMessage(outmsg, NetDeliveryMethod.ReliableOrdered);

        //System.Threading.Thread.Sleep(300);

        if (password.Equals("PassClient"))
        {
            Core.console.AddDebugString("Client world started");
           // isWar = false;
        }
        //else
        //{
        //    isWar = true;
        //}
        net.SetOnline(true);
        Core c = Core.GetCore();
        c.SetPlayerName(name);
    }
    public static void Register(string name, string password)
    {
        net.SetLogging(true);
        client.Start();
        NetOutgoingMessage outmsg = client.CreateMessage();
        outmsg.Write((byte)20);
        client.Connect("127.0.0.1", 25565, outmsg);

        System.Threading.Thread.Sleep(1500);

        outmsg = client.CreateMessage();
        outmsg.Write((byte)PacketType.Register);
        outmsg.Write(name);
        outmsg.Write(password);
        client.SendMessage(outmsg, NetDeliveryMethod.ReliableOrdered);
        //isWar = true;
        net.SetOnline(true);
    }
    public static void Disconnect(string name)
    {
        NetOutgoingMessage outmsg = client.CreateMessage();
        outmsg.Write((byte)PacketType.Disconnect);
        outmsg.Write(name);
        client.SendMessage(outmsg, NetDeliveryMethod.ReliableOrdered);
    }
    public static void SendMessage(string s)
    {
        NetOutgoingMessage outmsg = client.CreateMessage();
        outmsg.Write((byte)PacketType.Message);
        outmsg.Write(core.GetPlayerName());
        outmsg.Write(s);
        client.SendMessage(outmsg, NetDeliveryMethod.Unreliable);
    }
    public static void SendRespawn()
    {
        NetOutgoingMessage outmsg = client.CreateMessage();
        outmsg.Write((byte)PacketType.Respawn);
        outmsg.Write(core.GetPlayerName());
        client.SendMessage(outmsg, NetDeliveryMethod.Unreliable);
    }
    public static void SendSelectShip(bool select)
    {
        NetOutgoingMessage outmsg = client.CreateMessage();
        outmsg.Write((byte)PacketType.SelectShip);
        outmsg.Write(core.GetPlayerName());
        outmsg.Write(select);
        client.SendMessage(outmsg, NetDeliveryMethod.Unreliable);
    }
    public static void SendConfrimSelect(ConfrimType ct, byte m)
    {
        NetOutgoingMessage outmsg = client.CreateMessage();
        outmsg.Write((byte)PacketType.ConfrimSelect);
        outmsg.Write(core.GetPlayerName());
        outmsg.Write((byte)ct);
        if (ct == ConfrimType.SelectFaction)
        {
            outmsg.Write((byte)m);
        }
        client.SendMessage(outmsg, NetDeliveryMethod.Unreliable);
    }
    public static void SendShip(Ship s)
    {
        NetOutgoingMessage outmsg = client.CreateMessage();
        outmsg.Write((byte)PacketType.Move);
        outmsg.Write(s.shipName);
        if (core.currentGui == null && core.currentGuiAdd == null && !core.isChat)
        {
            outmsg.Write(core.inputManager.cursorAdvancedPosition.X);
            outmsg.Write(core.inputManager.cursorAdvancedPosition.Y);
            outmsg.Write(core.inputManager.IsKeyDown(Keys.W));
            outmsg.Write(core.inputManager.IsKeyDown(Keys.S));
            outmsg.Write(core.inputManager.IsKeyDown(Keys.A));
            outmsg.Write(core.inputManager.IsKeyDown(Keys.D));
            outmsg.Write(core.inputManager.IsKeyDown(Keys.X));
            outmsg.Write(net.IsLeftDown());
            outmsg.Write(net.IsRightDown());
        }
        else
        {
            outmsg.Write(s.Position.X - 10);
            outmsg.Write(s.Position.Y);
            outmsg.Write(false);
            outmsg.Write(false);
            outmsg.Write(false);
            outmsg.Write(false);
            outmsg.Write(false);
            outmsg.Write(false);
            outmsg.Write(false);
        }
        client.SendMessage(outmsg, NetDeliveryMethod.Unreliable);
        net.SetLeftButton(false); net.SetRightButton(false);
    }
}