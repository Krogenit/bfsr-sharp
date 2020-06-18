using BattleForSpaceResources.Collision;
using BattleForSpaceResources.Entitys;
using BattleForSpaceResources.Entitys.BFSRSystem.Helpers;
using BattleForSpaceResources.ShipComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleForSpaceResources.Entitys
{
    public class Bullet : CollisionObject
    {
        public float bulletDamageArmor, bulletDamageHull, bulletDamageShield, bulletSpeed, alphaVelocity;
        public GunType gunType;
        public Ship owner;
        public Bullet(GunType type,Ship s, Vector2 pos, float rot, float vel, World w)
            : base(pos, w)
        {
            gunType = type;
            Rotation = rot;
            owner = s;
            bulletSpeed = vel;
            isLive = true;
            Vector2[] v = new Vector2[4];
            if (type == GunType.PlasmSmall)
            {
                if(w.isRemote)
                    Create(Textures.bulletPlasm);
                bulletDamageArmor = 5F;
                bulletDamageHull = bulletDamageArmor / 2F;
                bulletDamageShield = bulletDamageArmor / 2F;
                alphaVelocity = .028F;
                color = new Vector4(0.5f, 0.5f, 1f, 1.5f);
                Size = 1;
                v[0] = new Vector2(-6f, -2f);
                v[1] = new Vector2(-6f, 2f);
                v[2] = new Vector2(6f, 2f);
                v[3] = new Vector2(6f, -2f);
                Poly p = new Poly(body, v);
            }
            else if (type == GunType.LaserSmall)
            {
                if (w.isRemote)
                    Create(Textures.bulletLaser);
                bulletDamageShield = 4F;
                bulletDamageArmor = bulletDamageShield / 2F;
                bulletDamageHull = bulletDamageShield / 2F;
                alphaVelocity = .028F;
                color = new Vector4(1f, 0.5f, 0.5f, 1.5f);
                Size = 1;
                v[0] = new Vector2(-6f, -2f);
                v[1] = new Vector2(-6f, 2f);
                v[2] = new Vector2(6f, 2f);
                v[3] = new Vector2(6f, -2f);
                Poly p = new Poly(body, v);
            }
            else if (type == GunType.GausSmall)
            {
                if (w.isRemote)
                    Create(Textures.bulletGaus);
                bulletDamageHull = 5F;
                bulletDamageArmor = bulletDamageHull / 2F;
                bulletDamageShield = bulletDamageHull / 2F;
                alphaVelocity = .026F;
                color = new Vector4(1f, 1f, 0.5f, 1.5f);
                Size = 1;
                v[0] = new Vector2(-6f, -2f);
                v[1] = new Vector2(-6f, 2f);
                v[2] = new Vector2(6f, 2f);
                v[3] = new Vector2(6f, -2f);
                Poly p = new Poly(body, v);
            }
            float x1 = (float)Math.Cos(Rotation);
            float y1 = (float)Math.Sin(Rotation);
            velocity.X = -x1 * bulletSpeed;
            velocity.Y = -y1 * bulletSpeed;
        }
        public override void Update()
        {
            if (color.W <= 0)
            {
                isLive = false;
            }
            Position += velocity;
            color = new Vector4(color.X, color.Y, color.Z, color.W - alphaVelocity);
            base.Update();
            CheckInteracts();
        }
        public void CheckInteracts()
        {
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
                        for (int i = 0; i < world.shipsGrid[x2, y2].Count; i++)
                        {
                            if (world.shipsGrid[x2, y2][i].shipFaction != owner.shipFaction && world.shipsGrid[x2, y2][i] != owner && !world.shipsGrid[x2, y2][i].isJumping)
                            {
                                if (((world.shipsGrid[x2, y2][i].shield > 0 && body.PolygonCollide(body, world.shipsGrid[x2, y2][i].shieldBase.body)) || body.PolygonCollide(body, world.shipsGrid[x2, y2][i].body)))
                                {
                                    world.shipsGrid[x2, y2][i].DamageShip(this);
                                    isLive = false;
                                    if (world.isRemote)
                                    {
                                        var x = Math.Cos(Rotation + 1.57F);
                                        var y = Math.Sin(Rotation + 1.57F);
                                        var x1 = Math.Cos(Rotation);
                                        var y1 = Math.Sin(Rotation);
                                        if (world.shipsGrid[x2, y2][i].shield > 0)
                                        {

                                        }
                                        else
                                        {
                                            if (core.random.Next(2) == 0)
                                                core.ps.ShipOst(Position, world.shipsGrid[x2, y2][i]);
                                            core.ps.ShipDamage(core.random.Next(4), Position, world.shipsGrid[x2, y2][i], 0.25F);
                                            if (core.random.Next(2) == 0)
                                                core.ps.ShipDamageDerbis(core.random.Next(2), world.shipsGrid[x2, y2][i].velocity, Position);
                                        }
                                        if (gunType == GunType.IonSmall)
                                        {
                                            //Core.ps.LightingIon(Position, gunType == 10 ? 1 : gunType == 11 ? 1.5F : 2F);
                                        }
                                        else
                                        {
                                            core.ps.Directed(world.shipsGrid[x2, y2][i], 1, Position, Rotation, color, Size);
                                            core.ps.Directed(world.shipsGrid[x2, y2][i], 0, Position, Rotation, color, Size);
                                        }
                                        core.ps.BackLight(Position, Size + 2F, new Vector4(color.X / 2F, color.Y / 2F, color.Z / 2F, color.W / 2F), .1F);
                                    }
                                    return;
                                }
                            }
                        }
                    }
                }
                /*if (Core.dronsGrid[x2, y2] != null)
                {
                    for (int i = 0; i < Core.dronsGrid[x2, y2].Count; i++)
                    {
                        if (Core.dronsGrid[x2, y2][i].faction != shooterObject.shipFaction && Core.dronsGrid[x2, y2][i].ownerShip != shooterObject)
                        {
                            if (body.PolygonCollide(Core.dronsGrid[x2, y2][i].body))
                            {
                                Core.dronsGrid[x2, y2][i].hull -= bulletDamageHull;
                                isLive = false;
                                var x = Math.Cos(Rotation + 1.57F);
                                var y = Math.Sin(Rotation + 1.57F);
                                var x1 = Math.Cos(Rotation);
                                var y1 = Math.Sin(Rotation);

                                if (Core.random.Next(2) == 0)
                                    Core.ps.ShipOst(Position, Core.dronsGrid[x2, y2][i]);
                                Core.ps.ShipDamage(Core.random.Next(2), Position, Core.dronsGrid[x2, y2][i], 0.25F);
                                if (Core.random.Next(3) == 0)
                                    Core.ps.ShipDamageDerbis(Core.random.Next(2), Core.dronsGrid[x2, y2][i].velocity, Position);
                                if (gunType == 10 || gunType == 11 || gunType == 12)
                                {
                                    Core.ps.LightingIon(Position, gunType == 10 ? 1 : gunType == 11 ? 1.5F : 2F);
                                }
                                else
                                {
                                    Core.ps.Directed(Core.dronsGrid[x2, y2][i], 1, Position, Rotation, color, Size);
                                    Core.ps.Directed(Core.dronsGrid[x2, y2][i], 0, Position, Rotation, color, Size);
                                }
                                Core.ps.BackLight(Position, Size + 2F, new Vector4(color.X / 2F, color.Y / 2F, color.Z / 2F, color.W / 2F), .1F);
                                return;
                            }
                        }
                    }
                }
                if (Core.asteroidsGrid[x2, y2] != null)
                {
                    for (int i = 0; i < Core.asteroidsGrid[x2, y2].Count; i++)
                    {
                        if (Vector2.Distance(Core.asteroidsGrid[x2, y2][i].Position, Position) <= 500)
                        {
                            if (body.PolygonCollide(Core.asteroidsGrid[x2, y2][i].body))
                            {
                                Core.asteroidsGrid[x2, y2][i].hull -= 1;
                                if (Core.asteroidsGrid[x2, y2][i].hull <= 0 && Core.asteroidsGrid[x2, y2][i].isLive)
                                {
                                    Core.asteroidsGrid[x2, y2][i].DestroyAsteroid(false, shooterObject);
                                    Core.asteroidsGrid[x2, y2].RemoveAt(i);
                                    --i;
                                    --Core.asteroidsCount;
                                }
                                if (gunType == 10 || gunType == 11 || gunType == 12)
                                {
                                    Core.ps.LightingIon(Position, gunType == 10 ? 1 : gunType == 11 ? 1.5F : 2F);
                                }
                                else
                                {
                                    if (Core.asteroidsGrid[x2, y2].Count > i && i > -1)
                                    {
                                        Core.ps.Directed(Core.asteroidsGrid[x2, y2][i], 1, Position, Rotation, color, Size);
                                        Core.ps.Directed(Core.asteroidsGrid[x2, y2][i], 0, Position, Rotation, color, Size);
                                    }
                                }
                                Core.ps.BackLight(Position, Size + 2F, new Vector4(color.X / 2F, color.Y / 2F, color.Z / 2F, color.W / 2F), .1F);
                                if (Core.asteroidsGrid[x2, y2].Count > i && i > -1)
                                    Core.ps.ShipDamage(Core.random.Next(4), Position, Core.asteroidsGrid[x2, y2][i], 0.3F);
                                isLive = false;
                                return;
                            }
                        }
                    }
                }
            }*/
            }
            /*for (int i = 0; i < Core.motherShips.Count; i++)
            {
                if (Core.motherShips[i].shipFaction != shooterObject.shipFaction && Core.motherShips[i] != shooterObject && !Core.motherShips[i].isJumping)
                {
                    if (Vector2.Distance(Core.motherShips[i].Position, Position) <= 2000)
                    {
                        if (((Core.motherShips[i].shield > 0 && body.PolygonCollide(Core.motherShips[i].shieldBase.body)) || body.PolygonCollide(Core.motherShips[i].body)))
                        {
                            Core.motherShips[i].DamageShip(this);
                            isLive = false;
                            var x = Math.Cos(Rotation + 1.57F);
                            var y = Math.Sin(Rotation + 1.57F);
                            var x1 = Math.Cos(Rotation);
                            var y1 = Math.Sin(Rotation);
                            if (Core.motherShips[i].shield > 0)
                            {

                            }
                            else
                            {
                                if (Core.random.Next(2) == 0)
                                    Core.ps.ShipOst(Position, Core.motherShips[i]);
                                Core.ps.ShipDamage(Core.random.Next(4), Position, Core.motherShips[i], 0.25F);
                                if (Core.random.Next(2) == 0)
                                    Core.ps.ShipDamageDerbis(Core.random.Next(2), Core.motherShips[i].velocity, Position);
                            }
                            if (gunType == 10 || gunType == 11 || gunType == 12)
                            {
                                Core.ps.LightingIon(Position, gunType == 10 ? 1 : gunType == 11 ? 1.5F : 2F);
                            }
                            else
                            {
                                Core.ps.Directed(Core.motherShips[i], 1, Position, Rotation, color, Size);
                                Core.ps.Directed(Core.motherShips[i], 0, Position, Rotation, color, Size);
                            }
                            Core.ps.BackLight(Position, Size + 2F, new Vector4(color.X / 2F, color.Y / 2F, color.Z / 2F, color.W / 2F), .1F);
                            return;
                        }
                    }
                }
            }
            /*for (int i = 0; i < Core.stations.Count; i++)
            {
                if (Core.stations[i].shipFaction != shooterObject.shipFaction && Core.stations[i] != shooterObject && !Core.stations[i].isJumping)
                {
                    if (Vector2.Distance(Core.stations[i].Position, Position) <= 2000)
                    {
                        if (((Core.stations[i].shield > 0 && body.PolygonCollide(Core.stations[i].shieldBase.body)) || body.PolygonCollide(Core.stations[i].body)))
                        {
                            Core.stations[i].DamageShip(this);
                            isLive = false;
                            var x = Math.Cos(Rotation + 1.57F);
                            var y = Math.Sin(Rotation + 1.57F);
                            var x1 = Math.Cos(Rotation);
                            var y1 = Math.Sin(Rotation);
                            if (Core.stations[i].shield > 0)
                            {

                            }
                            else
                            {
                                if (Core.random.Next(2) == 0)
                                    Core.ps.ShipOst(Position, Core.stations[i]);
                                Core.ps.ShipDamage(Core.random.Next(4), Position, Core.stations[i], 0.25F);
                                if (Core.random.Next(2) == 0)
                                    Core.ps.ShipDamageDerbis(Core.random.Next(2), Core.stations[i].velocity, Position);
                            }
                            if (gunType == 10 || gunType == 11 || gunType == 12)
                            {
                                Core.ps.LightingIon(Position, gunType == 10 ? 1 : gunType == 11 ? 1.5F : 2F);
                            }
                            else
                            {
                                Core.ps.Directed(Core.stations[i], 1, Position, Rotation, color, Size);
                                Core.ps.Directed(Core.stations[i], 0, Position, Rotation, color, Size);
                            }
                            Core.ps.BackLight(Position, Size + 2F, new Vector4(color.X / 2F, color.Y / 2F, color.Z / 2F, color.W / 2F), .1F);
                            return;
                        }
                    }
                }
            }*/
        }
        public override void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Textures.light, Position, null, new Color(new Vector4(color.X / 2F, color.Y / 2F, color.Z / 2F, color.W / 5F)), Rotation, new Vector2(Textures.light.Width / 2, Textures.light.Height / 2), Size, SpriteEffects.None, layer);
            base.Render(spriteBatch);
        }
    }
}
