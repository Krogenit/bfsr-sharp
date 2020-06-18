using BattleForSpaceResources.Entitys.BFSRSystem.Helpers;
using BattleForSpaceResources.Networking;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleForSpaceResources.Entitys
{
    public class ShipNPC : Ship
    {
        public ShipNPC(Vector2 pos, Vector2 bp, Faction faction, ShipType type, int[] components, World w, AiSearchType ast, AiAgressiveType agr)
            : base(pos, bp, faction, type, components, w, ast, agr)
        {

        }
        public override void Ai()
        {
            if (!world.isRemote)
            {
                if (isStation)
                {
                    Rotation += rotationSpeed;
                    //RotateTurrets(true, false);
                }
                if (runnigOutVector == Vector2.Zero && shipInfo.exType != ShipInfo.ExplosionType.Mother && !isStation && shipInfo.manevering)
                {
                    for (int j = 0; j < world.destroingShips.Count; j++)
                    {
                        if (Vector2.Distance(world.destroingShips[j].Position, Position) <= 1000)
                        {
                            var mDx = (world.destroingShips[j].Position.X) - Position.X;
                            var mDy = (world.destroingShips[j].Position.Y) - Position.Y;
                            float rot = (float)Math.Atan2(mDy, mDx);
                            runnigOutVector = new Vector2((float)Math.Cos(rot) * -2500, (float)Math.Sin(rot) * -2500);
                            target = null;
                            break;
                        }
                    }
                }
                if (runnigOutVector != Vector2.Zero)
                {
                    MoveToVector(runnigOutVector, Vector2.Zero);
                    if (world.destroingShips.Count <= 0 && (runningOutTimer <= 0 || hull / maxHull > minHullRunOut + 5))
                        runnigOutVector = Vector2.Zero;
                    if (runningOutTimer > 0)
                        runningOutTimer--;
                }
                else if (moveToVector != Vector2.Zero && !isStation)
                {
                    MoveToVector(moveToVector, Vector2.Zero);
                }
                else if (target == null)
                {
                    if (aiAgrType == AiAgressiveType.Mining)
                    {
                        if (hull / maxHull <= minHullRunOut)
                        {
                            int b = core.random.Next(4);
                            switch (b)
                            {
                                case 0:
                                    runnigOutVector = new Vector2(-19000, -19000);
                                    break;
                                case 1:
                                    runnigOutVector = new Vector2(-19000, 19000);
                                    break;
                                case 2:
                                    runnigOutVector = new Vector2(19000, -19000);
                                    break;
                                case 3:
                                    runnigOutVector = new Vector2(19000, 19000);
                                    break;
                            }
                            runningOutTimer = 1000;
                            target = null;
                        }
                        if (cargoSize < maxCargoSize + 50)
                        {
                            //SeachAsteroids();
                        }
                        else
                        {

                        }
                    }
                    else if (aiAgrType == AiAgressiveType.Attack)
                    {
                        SearchShips();
                    }
                }
                if (target != null)
                {
                    if (!isStation)
                        MoveToVector(target.Position, target.velocity);
                    GunsShoot();
                    if (!target.isLive || target.isSetDestroingTimer || (aiAgrType == AiAgressiveType.Defend && Vector2.Distance(target.Position, Position) >= 2500)
                        || (aiAgrType == AiAgressiveType.Attack && Vector2.Distance(target.Position, Position) >= 5000))
                    {
                        target = null;
                        aiDir = 0;
                    }
                }
                else if (id != 0 && moveToVector == Vector2.Zero && runnigOutVector == Vector2.Zero && basePosition != Vector2.Zero)
                {
                    if (!isStation)
                        MoveToVector(basePosition + addBasePosition, Vector2.Zero);
                    if (--changeBasePosTimer <= 0)
                    {
                        addBasePosition = new Vector2(core.random.Next(-750, 750), core.random.Next(-750, 750));
                        changeBasePosTimer = 100 + core.random.Next(200);
                    }
                }
                else if (moveToVector == Vector2.Zero && runnigOutVector == Vector2.Zero &&
                    (velocity.X > 0.01F || velocity.X < -0.01f || velocity.Y > 0.01f || velocity.Y < -0.01f))
                {
                    float horiz = velocity.X;
                    float vertic = velocity.Y;
                    velocity.X = horiz -= Settings.gravity * 20 * horiz;
                    velocity.Y = vertic -= Settings.gravity * 20 * vertic;
                }
            }
            else
            {
                cos = (float)Math.Cos(Rotation);
                sin = (float)Math.Sin(Rotation);
                cos1 = (float)Math.Cos(Rotation + 1.57F);
                sin1 = (float)Math.Sin(Rotation + 1.57F);
                switch (aiDir)
                {
                    case AiDirection.Back:
                        ShipEngine(3, cos, cos1, sin, sin1);
                        break;
                    case AiDirection.Face:
                        ShipEngine(0, cos, cos1, sin, sin1);
                        break;
                    case AiDirection.Left:
                        ShipEngine(1, cos, cos1, sin, sin1);
                        break;
                    case AiDirection.Right:
                        ShipEngine(2, cos, cos1, sin, sin1);
                        break;
                    case AiDirection.Stop:
                        ShipEngine(0, cos, cos1, sin, sin1);
                        ShipEngine(1, cos, cos1, sin, sin1);
                        ShipEngine(2, cos, cos1, sin, sin1);
                        ShipEngine(3, cos, cos1, sin, sin1);
                        break;
                }
            }
        }
        private void SearchShips()
        {
            switch (aiSeachType)
            {
                case AiSearchType.Around:
                    int xGrid = (int)((Position.X + 40000) / world.grid);
                    int yGrid = (int)((Position.Y + 40000) / world.grid);
                    for (int j = 0; j < 9; j++)
                    {
                        int[] xy = CollisionHelper.GetNearXY(j, xGrid, yGrid);
                        int x2 = xy[0];
                        int y2 = xy[1];
                        if (x2 < 0 || y2 < 0 || x2 + 1 > world.shipsGrid.GetLength(0) || y2 + 1 > world.shipsGrid.GetLength(1))
                        { }
                        else
                        {
                            if (world.shipsGrid[x2, y2] != null)
                            {
                                float newDis = 0;
                                for (int i = 0; i < world.shipsGrid[x2, y2].Count; i++)
                                {
                                    float curDis = Vector2.Distance(world.shipsGrid[x2, y2][i].Position, Position);
                                    if (world.shipsGrid[x2, y2][i].shipFaction != shipFaction && (newDis == 0 || curDis < newDis) && !world.shipsGrid[x2, y2][i].isSetDestroingTimer)
                                    {
                                        target = world.shipsGrid[x2, y2][i];
                                        newDis = Vector2.Distance(world.shipsGrid[x2, y2][i].Position, Position);
                                    }
                                }
                            }
                        }
                    }
                    break;
            }
            /*for (int i = 0; i < world.motherShips.Count; i++)
            {
                float curDis = Vector2.Distance(world.motherShips[i].Position, Position);
                if (world.motherShips[i].shipFaction != shipFaction && curDis <= searchDis && (newDis == 0 || curDis < newDis) && !world.motherShips[i].isSetDestroingTimer)
                {
                    target = world.motherShips[i];
                    newDis = Vector2.Distance(world.motherShips[i].Position, Position);
                }
            }
            for (int i = 0; i < world.stations.Count; i++)
            {
                float curDis = Vector2.Distance(world.stations[i].Position, Position);
                if (world.stations[i].shipFaction != shipFaction && curDis <= searchDis && (newDis == 0 || curDis < newDis) && !world.stations[i].isSetDestroingTimer)
                {
                    target = world.stations[i];
                    newDis = Vector2.Distance(world.stations[i].Position, Position);
                }
            }*/
        }
        private void GunsShoot()
        {
            GetGunsMinMaxDis();
            FixShipMinMaxDis();
            if (gunSlots != null && rotateDifference >= 175 && rotateDifference < 185)
            {
                for (int i = 0; i < gunSlots.Length; i++)
                {
                    if (gunSlots[i] != null)
                    {
                        if (Vector2.Distance(target.Position, Position) <= gunSlots[i].maxDis)
                        {
                            gunSlots[i].Shoot();
                            ServerPacketSender.SendGunShoot(i, id);
                        }
                    }
                }
            }
            /*if (turretSlots != null)
            {
                for (int i = 0; i < turretSlots.Length; i++)
                {
                    if (turretSlots[i] != null)
                    {
                        turretSlots[i].Shoot(true);
                    }
                }
            }
            if (rocketSlots != null && target.GetType() != typeof(Asteroid))
            {
                for (int i = 0; i < rocketSlots.Length; i++)
                {
                    if (rocketSlots[i] != null)
                        rocketSlots[i].Shoot();
                }
            }*/
        }
        private void GetGunsMinMaxDis()
        {
            int reloadTimer = 0;
            bulletSpeed = 1;
            maxDis = 0;
            if (gunSlots != null)
            {
                for (int i = 0; i < gunSlots.Length; i++)
                {
                    if (gunSlots[i] != null)
                    {
                        if ((reloadTimer == 0) || gunSlots[i].shootTimer < reloadTimer)
                        {
                            //if (gunSlots[i].gunType != 13 && gunSlots[i].gunType != 14 && gunSlots[i].gunType != 15 && gunSlots[i].gunType != 25)
                            {
                                bulletSpeed = gunSlots[i].bulletSpeed;
                                reloadTimer = gunSlots[i].shootTimer;
                            }
                            /*else
                            {
                                bulletSpeed = 1;
                                reloadTimer = gunSlots[i].shootTimer;
                            }*/
                            if (maxDis == 0 || gunSlots[i].maxDis < maxDis)
                            {
                                maxDis = gunSlots[i].maxDis;
                            }
                        }
                    }
                }
            }
            else
            {
                bulletSpeed = 1;
                maxDis = 0;
            }
            /*if (turretSlots != null)
            {
                for (int i = 0; i < turretSlots.Length; i++)
                {
                    if (turretSlots[i] != null)
                    {
                        if (turretSlots[i].gunSlots != null)
                        {
                            for (int j = 0; j < turretSlots[i].gunSlots.Length; j++)
                            {
                                if ((reloadTimer == 0) || turretSlots[i].gunSlots[j].shootTimer < reloadTimer)
                                {
                                    if (turretSlots[i].gunSlots[j].gunType != 13 && turretSlots[i].gunSlots[j].gunType != 14 &&
                                        turretSlots[i].gunSlots[j].gunType != 15 && turretSlots[i].gunSlots[j].gunType != 25)
                                    {
                                        bulletSpeed = turretSlots[i].gunSlots[j].bulletSpeed;
                                        reloadTimer = turretSlots[i].gunSlots[j].shootTimer;
                                    }
                                    else
                                    {
                                        bulletSpeed = 1;
                                        reloadTimer = turretSlots[i].gunSlots[j].shootTimer;
                                    }
                                    if (maxDis == 0 || turretSlots[i].gunSlots[j].maxDis < maxDis)
                                    {
                                        maxDis = turretSlots[i].gunSlots[j].maxDis;
                                    }
                                }
                            }
                        }
                    }
                }
            }*/
        }
        private void FixShipMinMaxDis()
        {
            if (maxDis < shipInfo.maxDis)
                maxDis = shipInfo.maxDis;
            minDis = shipInfo.minDis;
        }
    }
}
