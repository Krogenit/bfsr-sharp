using BattleForSpaceResources.Networking;
using BattleForSpaceResources.ShipComponents;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleForSpaceResources.Entitys
{
    public enum SpawnerType
    {
        Easy = 0, Medium = 1, Hard = 2
    }
    public class ShipsSpawner
    {
        private Vector2 Position;
        private int timer, maxShips, maxTimer;
        private SpawnerType spawnerType;
        private List<Ship> ships = new List<Ship>();
        private Faction faction;
        private Core core = Core.GetCore();
        private World world = Core.GetCore().GetWorld();
        public ShipsSpawner(Vector2 pos, SpawnerType st, Faction fact, int max, int time)
        {
            Position = pos;
            spawnerType = st;
            maxShips = max;
            maxTimer = time;
            faction = fact;
            timer = maxTimer;
            timer = 200;
        }
        public void Update()
        {
            if ( --timer <= 0)
            {
                for (int i = 0; i < ships.Count; i++)
                {
                    if (!ships[i].isLive || ships[i].hull <= 0)
                    {
                        ships.RemoveAt(i);
                        i--;
                    }
                }
                if(ships.Count == 0)
                SpawnShips();
                timer = maxTimer;
            }
        }
        private void SpawnShips()
        {
            World w = ServerCore.GetServerCore().GetWorld();
            if (faction == Faction.Enemy)
            {
                if (spawnerType == SpawnerType.Easy)
                {
                    for (int i = 0; i < maxShips; i++)
                    {
                        Vector2 rndVector = new Vector2(core.random.Next(-500, 500), core.random.Next(-500, 500)) + Position;
                        int[] comp = { 1, 1, 0, 0, 0, 0, 0 };
                        //r e s a a a a;
                        ShipNPC s = new ShipNPC(rndVector, rndVector, faction, ShipType.EnemySmall1, comp, w, AiSearchType.Around, AiAgressiveType.Attack);
                        GunSlot[] gs = { new GunSlot(GunType.LaserSmall, s), new GunSlot(GunType.LaserSmall, s) };
                        s.AddGuns(gs);
                        s.cargoSize = s.maxCargoSize;
                        s.crewSize = s.maxCrewSize;
                        s.shipName = "<BOT> Saimon";
                        s.id = w.GetNpcId();
                        ships.Add(s);
                        w.shipsNpc.Add(s);
                        ServerPacketSender.SendCreateNpcShip(s);
                    }
                }
            }
            else if (faction == Faction.Human)
            {
                if (spawnerType == SpawnerType.Easy)
                {
                    for (int i = 0; i < maxShips; i++)
                    {
                        Vector2 rndVector = new Vector2(core.random.Next(-500, 500), core.random.Next(-500, 500)) + Position;
                        int[] comp = { 1, 1, 0, 0, 0, 0, 0 };
                        //r e s a a a a;
                        ShipNPC s = new ShipNPC(rndVector, rndVector, faction, ShipType.HumanSmall1, comp, w, AiSearchType.Around, AiAgressiveType.Attack);
                        GunSlot[] gs = { new GunSlot(GunType.PlasmSmall, s), new GunSlot(GunType.PlasmSmall, s) };
                        s.AddGuns(gs);
                        s.cargoSize = s.maxCargoSize;
                        s.crewSize = s.maxCrewSize;
                        s.shipName = "<BOT> Human";
                        s.id = w.GetNpcId();
                        ships.Add(s);
                        w.shipsNpc.Add(s);
                        ServerPacketSender.SendCreateNpcShip(s);
                    }
                }
            }
            else if (faction == Faction.Civilian)
            {
                if (spawnerType == SpawnerType.Easy)
                {
                    for (int i = 0; i < maxShips; i++)
                    {
                        Vector2 rndVector = new Vector2(core.random.Next(-500, 500), core.random.Next(-500, 500)) + Position;
                        int[] comp = { 1, 1, 0, 0, 0, 0, 0 };
                        //r e s a a a a;
                        ShipNPC s = new ShipNPC(rndVector, rndVector, faction, ShipType.CivSmall1, comp, w, AiSearchType.Around, AiAgressiveType.Attack);
                        GunSlot[] gs = { new GunSlot(GunType.GausSmall, s), new GunSlot(GunType.GausSmall, s) };
                        s.AddGuns(gs);
                        s.cargoSize = s.maxCargoSize;
                        s.crewSize = s.maxCrewSize;
                        s.shipName = "<BOT> Engi";
                        s.id = w.GetNpcId();
                        ships.Add(s);
                        w.shipsNpc.Add(s);
                        ServerPacketSender.SendCreateNpcShip(s);
                    }
                }
            }
        }
    }
}