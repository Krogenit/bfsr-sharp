using BattleForSpaceResources;
using BattleForSpaceResources.Entitys;
using BattleForSpaceResources.Guis;
using BattleForSpaceResources.Networking;
using BattleForSpaceResources.ShipComponents;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using System;
class ClientPacketHandler
{
    private ClientNetwork net;
    private Core core;
    private World world;
    public ClientPacketHandler(ClientNetwork net)
    {
        this.net = net;
        this.core = Core.GetCore();
        this.world = core.GetWorld();
    }
    public void ReadPacket(NetIncomingMessage incmsg)
    {
        PacketType pk = (PacketType)incmsg.ReadByte();
        switch (pk)
        {
            case PacketType.CreateOldNpcShip:
                CreateNpcShip(incmsg, false);
                break;
            case PacketType.CreateNpcShip:
                CreateNpcShip(incmsg, true);
                break;
            case PacketType.Message:
                Message(incmsg);
                break;
            case PacketType.DestroyShield:
                DestroyShield(incmsg);
                break;
            case PacketType.DestroyShip:
                DestroyShip(incmsg);
                break;
            case PacketType.Respawn:
                Respawn(incmsg);
                break;
            case PacketType.ShipEngine:
                ShipEngine(incmsg);
                break;
            case PacketType.GunShoot:
                GunShoot(incmsg);
                break;
            case PacketType.OpenGui:
                OpenGui(incmsg);
                break;
            case PacketType.Connect:
                CreatePlayerShip(incmsg);
                break;
            case PacketType.ConnectNewPlayer:
                CreateNewPlayerShip(incmsg);
                break;
            case PacketType.LogRegMsg:
                LogRegError(incmsg);
                break;
            case PacketType.Move:
                UpdateMove(incmsg);
                break;
            case PacketType.MoveNpcs:
                UpdateNpcs(incmsg);
                break;
            case PacketType.MoveBullet:
                UpdateMoveBullet(incmsg);
                break;
            case PacketType.CreateBullet:
                CreateBullet(incmsg);
                break;
            case PacketType.Disconnect:
                PlayerDisconnect(incmsg);
                break;
            case PacketType.Deny:
                Core c = Core.GetCore();
                c.currentGuiAdd = new GuiAddDisconnectError("Ошибка", "Пользователь с таким именем уже играет");
                c.GetWorld().ships.Clear();
                break;
            case PacketType.Refresh:
                //timeOutTimer = 0;
                break;
        }
    }
    private void LogRegError(NetIncomingMessage incmsg)
    {
        string error = incmsg.ReadString();
        if (!error.Equals("Вы успешно зарегистрированы"))
            core.currentGuiAdd = new GuiAddDisconnectError("Ошибка", error);
        else
            core.currentGuiAdd = new GuiAddDisconnectError("Инфо", error);
        net.SetLogging(false);
        //isLogRegFalse = true;
        net.SetOnline(false);
        //net.timeOutTimer = 0;
    }
    private void CreateNpcShip(NetIncomingMessage incmsg, bool b)
    {
        if (b)
        {
            string name = null;
            int id = incmsg.ReadInt32();
            bool isRefreshing = false;
            for (int i1 = 0; i1 < world.ships.Count; i1++)
            {
                if (world.ships[i1].id == id)
                {
                    isRefreshing = true;
                }
            }
            if (!isRefreshing)
            {
                Vector2 pos = new Vector2(incmsg.ReadSingle(), incmsg.ReadSingle());
                int xpos = incmsg.ReadInt32(), ypos = incmsg.ReadInt32();
                ShipType st = (ShipType)incmsg.ReadByte();
                Faction f = (Faction)incmsg.ReadByte();
                switch (f)
                {
                    case Faction.Civilian:
                        name = "<Bot> Engi";
                        break;
                    case Faction.Enemy:
                        name = "<Bot> Saimon";
                        break;
                    case Faction.Human:
                        name = "<Bot> Human";
                        break;
                }

                int[] components = { incmsg.ReadByte(), incmsg.ReadByte(), incmsg.ReadByte(), incmsg.ReadByte(), incmsg.ReadByte(), incmsg.ReadByte(), incmsg.ReadByte() };
                ShipNPC s = new ShipNPC(pos, pos, f, st, components, world, 0, 0);
                s.shipName = name;
                s.id = id;
                s.jumpX = xpos;
                s.jumpY = ypos;
                int gunNum = incmsg.ReadByte();
                GunSlot[] gs = new GunSlot[gunNum];
                for (int i = 0; i < gunNum; i++)
                {
                    gs[i] = new GunSlot((GunType)incmsg.ReadByte(), s);
                }
                s.AddGuns(gs);
                world.shipsNpc.Add(s);
            }
        }
        else
        {
            string name = null;
            int id = incmsg.ReadInt32();
            bool isRefreshing = false;
            for (int i1 = 0; i1 < world.ships.Count; i1++)
            {
                if (world.ships[i1].id == id)
                {
                    isRefreshing = true;
                }
            }
            if (!isRefreshing)
            {
                Vector2 pos = new Vector2(incmsg.ReadSingle(), incmsg.ReadSingle());
                float rot = incmsg.ReadSingle();
                ShipType st = (ShipType)incmsg.ReadByte();
                Faction f = (Faction)incmsg.ReadByte();
                switch (f)
                {
                    case Faction.Civilian:
                        name = "<Bot> Engi";
                        break;
                    case Faction.Enemy:
                        name = "<Bot> Saimon";
                        break;
                    case Faction.Human:
                        name = "<Bot> Human";
                        break;
                }

                int[] components = { incmsg.ReadByte(), incmsg.ReadByte(), incmsg.ReadByte(), incmsg.ReadByte(), incmsg.ReadByte(), incmsg.ReadByte(), incmsg.ReadByte() };
                ShipNPC s = new ShipNPC(pos, pos, f, st, components, world, 0, 0);
                s.shipName = name;
                s.id = id;
                s.isJumping = false;
                int gunNum = incmsg.ReadByte();
                GunSlot[] gs = new GunSlot[gunNum];
                for (int i = 0; i < gunNum; i++)
                {
                    gs[i] = new GunSlot((GunType)incmsg.ReadByte(), s);
                }
                s.AddGuns(gs);
                world.shipsNpc.Add(s);
            }
        }
    }
    private void Message(NetIncomingMessage incmsg)
    {
        string playerName = incmsg.ReadString();
        string s = incmsg.ReadString();
        s = playerName + ": " + s;
        if (core.GetWorld() != null)
        {
            core.guiInGame.chat.chatStrings.Add(s);
            core.guiInGame.chat.UpdateStrings();
        }
    }
    private void DestroyShield(NetIncomingMessage incmsg)
    {
        string playerName = incmsg.ReadString();
        for (int i = 0; i < world.ships.Count; i++)
        {
            if (world.ships[i].shipName.Equals(playerName))
            {
                world.ships[i].DestroyShield();
                break;
            }
        }
    }
    private void DestroyShip(NetIncomingMessage incmsg)
    {
        int id = incmsg.ReadInt32();
        string playerName = id == 0 ? incmsg.ReadString() : "";
        if (id == 0)
        {
            for (int i = 0; i < world.ships.Count; i++)
            {
                if (world.ships[i].shipName.Equals(playerName))
                {
                    world.ships[i].hull = -10;
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < world.shipsNpc.Count; i++)
            {
                if (world.shipsNpc[i].id == id)
                {
                    world.shipsNpc[i].hull = -10;
                    break;
                }
            }
        }
    }
    private void Respawn(NetIncomingMessage incmsg)
    {
        string name = incmsg.ReadString();
        Vector2 pos = new Vector2(incmsg.ReadSingle(), incmsg.ReadSingle());
        int xpos = incmsg.ReadInt32(), ypos = incmsg.ReadInt32();
        bool isRefreshing = false;
        ShipType st = (ShipType)incmsg.ReadByte();
        Faction f = (Faction)incmsg.ReadByte();
        for (int i1 = 0; i1 < world.ships.Count; i1++)
        {
            if (world.ships[i1].shipName != null && world.ships[i1].shipName.Equals(name))
            {
                isRefreshing = true;
                break;
            }
        }
        if (!isRefreshing)
        {
            int[] components = { incmsg.ReadByte(), incmsg.ReadByte(), incmsg.ReadByte(), incmsg.ReadByte(), incmsg.ReadByte(), incmsg.ReadByte(), incmsg.ReadByte() };
            ShipPlayer s = new ShipPlayer(pos, pos, f, st, components, world, 0, 0);
            s.shipName = name;
            if (name.Equals(core.GetPlayerName()))
            {
                core.cam.camToNewPos = true;
                core.cam.newCamPos = s.Position;
            }
            s.jumpX = xpos;
            s.jumpY = ypos;
            int gunNum = incmsg.ReadByte();
            GunSlot[] gs = new GunSlot[gunNum];
            for (int i = 0; i < gunNum; i++)
            {
                gs[i] = new GunSlot((GunType)incmsg.ReadByte(), s);
            }
            s.AddGuns(gs);
            world.ships.Add(s);
        }
    }
    private void CreatePlayerShip(NetIncomingMessage incmsg)
    {
        string name = incmsg.ReadString();
        Vector2 pos = new Vector2(incmsg.ReadSingle(), incmsg.ReadSingle());
        float rot = incmsg.ReadSingle();
        bool isRefreshing = false;
        ShipType st = (ShipType)incmsg.ReadByte();
        Faction f = (Faction)incmsg.ReadByte();
        for (int i1 = 0; i1 < world.ships.Count; i1++)
        {
            if (world.ships[i1].shipName != null && world.ships[i1].shipName.Equals(name))
            {
                isRefreshing = true;
                break;
            }
        }
        if (!isRefreshing)
        {
            int[] components = { incmsg.ReadByte(), incmsg.ReadByte(), incmsg.ReadByte(), incmsg.ReadByte(), incmsg.ReadByte(), incmsg.ReadByte(), incmsg.ReadByte() };
            ShipPlayer s = new ShipPlayer(pos, pos, f, st, components, world, 0, 0);
            s.shipName = name;
            if (name.Equals(core.GetPlayerName()))
            {
                core.cam.camToNewPos = true;
                core.cam.newCamPos = s.Position;
            }
            s.Rotation = rot;
            s.isJumping = false;
            int gunNum = incmsg.ReadByte();
            GunSlot[] gs = new GunSlot[gunNum];
            for (int i = 0; i < gunNum; i++)
            {
                gs[i] = new GunSlot((GunType)incmsg.ReadByte(), s);
            }
            s.AddGuns(gs);
            world.ships.Add(s);
        }
    }
    private void CreateNewPlayerShip(NetIncomingMessage incmsg)
    {
        net.SetLogging(false);
        string name = incmsg.ReadString();
        Vector2 pos = new Vector2(incmsg.ReadSingle(), incmsg.ReadSingle());
        int xpos = incmsg.ReadInt32(), ypos = incmsg.ReadInt32();
        bool isRefreshing = false;
        ShipType st = (ShipType)incmsg.ReadByte();
        Faction f = (Faction)incmsg.ReadByte();
        for (int i1 = 0; i1 < world.ships.Count; i1++)
        {
            if (world.ships[i1].shipName != null && world.ships[i1].shipName.Equals(name))
            {
                isRefreshing = true;
            }
        }
        if (!isRefreshing)
        {
            int[] components = { incmsg.ReadByte(), incmsg.ReadByte(), incmsg.ReadByte(), incmsg.ReadByte(), incmsg.ReadByte(), incmsg.ReadByte(), incmsg.ReadByte() };
            ShipPlayer s = new ShipPlayer(pos, pos, f, st, components, world, 0, 0);
            s.shipName = name;
            if (name.Equals(core.GetPlayerName()))
            {
                core.SetCameraTarget(s.Position);
            }
            s.jumpX = xpos;
            s.jumpY = ypos;
            int gunNum = incmsg.ReadByte();
            GunSlot[] gs = new GunSlot[gunNum];
            for (int i = 0; i < gunNum; i++)
            {
                gs[i] = new GunSlot((GunType)incmsg.ReadByte(), s);
            }
            s.AddGuns(gs);
            world.ships.Add(s);
            Core.console.AddDebugString(name + " connected");
        }
    }
    private void ShipEngine(NetIncomingMessage incmsg)
    {
        string playerName = incmsg.ReadString();
        if (!playerName.Equals(core))
            for (int i = 0; i < world.ships.Count; i++)
            {
                if (world.ships[i].shipName.Equals(playerName))
                {
                    world.ships[i].ShipEngine(incmsg.ReadByte(), world.ships[i].cos, world.ships[i].cos1, world.ships[i].sin, world.ships[i].sin1);
                    break;
                }
            }
    }
    private void GunShoot(NetIncomingMessage incmsg)
    {
        int id = incmsg.ReadInt32();
        string playerName = id == 0 ? incmsg.ReadString() : "";
        if (!playerName.Equals(core.GetPlayerName()) && id == 0)
            for (int i = 0; i < world.ships.Count; i++)
            {
                if (world.ships[i].shipName.Equals(playerName))
                {
                    world.ships[i].gunSlots[incmsg.ReadByte()].Shoot();
                    break;
                }
            }
        else
            for (int i = 0; i < world.shipsNpc.Count; i++)
            {
                if (world.shipsNpc[i].id == id)
                {
                    world.shipsNpc[i].gunSlots[incmsg.ReadByte()].Shoot();
                    break;
                }
            }
    }
    private void OpenGui(NetIncomingMessage incmsg)
    {
        GuiType gt = (GuiType)incmsg.ReadByte();
        if (gt == GuiType.SelectFaction)
        {
            world = new World(true);
            core.currentGui = new GuiSelectFaction();
        }
    }
    private void UpdateMove(NetIncomingMessage incmsg)
    {
        string name = incmsg.ReadString();
        Vector2 pos = new Vector2(incmsg.ReadSingle(), incmsg.ReadSingle());
        float rot = incmsg.ReadSingle();
        Vector2 vel = new Vector2(incmsg.ReadSingle(), incmsg.ReadSingle());
        float hull = incmsg.ReadSingle();
        float shield = incmsg.ReadSingle();

        for (int i = 0; i < world.ships.Count; i++)
        {
            if (world.ships[i].shipName.Equals(name))
            {
                world.ships[i].Position = UpdatePosition(pos, world.ships[i].Position);
                world.ships[i].velocity = vel;
                world.ships[i].Rotation = UpdateRotation(rot, world.ships[i].Rotation);
                world.ships[i].hull = hull;
                world.ships[i].shield = shield;
                if (world.ships[i].shield > 0 && world.ships[i].shieldTimer != 0)
                    world.ships[i].shieldTimer = 0;
                break;
            }
        }
    }
    private void UpdateNpcs(NetIncomingMessage incmsg)
    {
        int full = incmsg.ReadInt32();
        if (full > world.shipsNpc.Count)
            full = world.shipsNpc.Count;
        for (int j = 0; j < full; j++)
        {
            int id = incmsg.ReadInt32();
            Vector2 pos = new Vector2(incmsg.ReadSingle(), incmsg.ReadSingle());
            float rot = incmsg.ReadSingle();
            Vector2 vel = new Vector2(incmsg.ReadSingle(), incmsg.ReadSingle());
            float hull = incmsg.ReadSingle();
            float shield = incmsg.ReadSingle();
            AiDirection aiDir = (AiDirection)incmsg.ReadByte();
            int drTimer = incmsg.ReadInt32();
            for (int i = 0; i < world.shipsNpc.Count; i++)
            {
                if (world.shipsNpc[i].id == id)
                {
                    world.shipsNpc[i].Position = UpdatePosition(pos, world.shipsNpc[i].Position);
                    world.shipsNpc[i].velocity = vel;
                    world.shipsNpc[i].Rotation = UpdateRotation(rot, world.shipsNpc[i].Rotation);
                    world.shipsNpc[i].hull = hull;
                    world.shipsNpc[i].shield = shield;
                    if (world.shipsNpc[i].shield > 0 && world.shipsNpc[i].shieldTimer != 0)
                        world.shipsNpc[i].shieldTimer = 0;
                    world.shipsNpc[i].aiDir = aiDir;
                    world.shipsNpc[i].directionTimer = drTimer;
                    break;
                }
            }
        }
    }
    private void UpdateMoveBullet(NetIncomingMessage incmsg)
    {
        int count = incmsg.ReadInt32();
        if (count > world.bullets.Count)
            count = world.bullets.Count;
        for (int i = 0; i < count; i++)
        {
            Vector2 pos = new Vector2(incmsg.ReadSingle(), incmsg.ReadSingle());
            world.bullets[i].Position = UpdatePosition(pos, world.bullets[i].Position);
        }
    }
    private Vector2 UpdatePosition(Vector2 remote, Vector2 local)
    {
        Vector2 difference = remote - local;
        //if (timerUpdate == 200)
        //return remote;
        //else
        {
            return local += (difference * 0.01f);
        }
    }
    private float UpdateRotation(float remoteRot, float locRot)
    {
        float maxRotDef = MathHelper.ToRadians(5);
        float difference = remoteRot - locRot;
        //if(timerUpdate==200)
        {
            //return remoteRot;
        }
        //else
        {
            if (difference > MathHelper.ToRadians(180))
            {
                difference = -0.04f;
            }
            else if (difference < MathHelper.ToRadians(-180))
            {
                difference = 0.04f;
            }
            return locRot += (difference);
        }
    }
    private void CreateBullet(NetIncomingMessage incmsg)
    {
        GunType gt = (GunType)incmsg.ReadByte();
        Vector2 pos = new Vector2(incmsg.ReadSingle(), incmsg.ReadSingle());
        float rot = incmsg.ReadSingle();
        float vel = incmsg.ReadSingle();
        Ship owner = new Ship(Vector2.Zero, Vector2.Zero, (Faction)incmsg.ReadByte(), ShipType.HumanSmall1, new int[] { 1, 1, 1, 1, 1, 1, 1 }, world, 0, 0);
        owner.shipName = incmsg.ReadString();
        Bullet b = new Bullet(gt, owner, pos, rot, vel, world);
        world.bullets.Add(b);
        Sounds.PlayGunSound(b.gunType, pos);
    }
    private void PlayerDisconnect(NetIncomingMessage incmsg)
    {
        string name = incmsg.ReadString();
        Core.console.AddDebugString(name + " left");
        if (world != null)
            for (int i = 0; i < world.ships.Count; i++)
            {
                if (world.ships[i].shipName.Equals(name))
                {
                    world.ships.RemoveAt(i);
                    i--;
                }
            }
    }
}