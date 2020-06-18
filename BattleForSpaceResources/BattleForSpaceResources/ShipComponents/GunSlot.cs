using BattleForSpaceResources.Entitys;
using BattleForSpaceResources.Networking;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleForSpaceResources.ShipComponents
{
    public enum GunType
    {
        PlasmSmall = 1, LaserSmall = 2, GausSmall = 3, IonSmall
    }
    public class GunSlot : GameObject
    {
        private Ship ownerShip;
        public GunType gunType;
        public float bulletSpeed, maxDis, energyCost;
        private float randomSize;
        public int shootTimer, shootTimerMax;
        private byte randomEffect;
        private Vector4 effectColor;
        private Vector2[] gunPos = new Vector2[2];
        private GameObject light, blue;
        public GunSlot(GunType type, Ship owner):base(owner.Position)
        {
            gunPos = owner.gunPos;
            ownerShip = owner;
            gunType = type;
            if(type == GunType.PlasmSmall)
            {
                if (owner.world.isRemote)
                Create(Textures.gunPlasm1);
                shootTimerMax = 30;
                bulletSpeed = 15;
                maxDis = 800;
                energyCost = 5;
            }
            else if (type == GunType.LaserSmall)
            {
                if (owner.world.isRemote)
                    Create(Textures.gunLaser1);
                shootTimerMax = 25;
                bulletSpeed = 15;
                maxDis = 800;
                energyCost = 4;
            }
            else if (type == GunType.GausSmall)
            {
                if (owner.world.isRemote)
                    Create(Textures.gunGaus1);
                shootTimerMax = 27;
                bulletSpeed = 14;
                maxDis = 750;
                energyCost = 6;
            }
            if (owner.world.isRemote)
            {
                light = new GameObject(Textures.light, owner.Position);
                blue = new GameObject(Textures.blue[3], owner.Position);
                light.color = Vector4.Zero;
                blue.color = Vector4.Zero;
            }
        }
        public void Update(float x, float x1, float y, float y1)
        {
            Rotation = ownerShip.Rotation;
            Position = new Vector2(ownerShip.Position.X + x - x1, ownerShip.Position.Y + y - y1);
            if (shootTimer > 0)
                --shootTimer;
            if (effectColor.W > 0)
                effectColor.W -= 0.2F;
            if (ownerShip.world.isRemote)
            {
                x = (float)Math.Cos(Rotation + 1.57F);
                y = (float)Math.Sin(Rotation + 1.57F);
                x1 = (float)Math.Cos(Rotation);
                y1 = (float)Math.Sin(Rotation);
                light.Position = blue.Position = new Vector2(Position.X + x - x1 * 10, Position.Y + y - y1 * 10);
                light.color = new Vector4(effectColor.X / 2F, effectColor.Y / 2F, effectColor.Z / 2F, effectColor.W / 2F);
                light.Size = 1.5f;
            }
        }
        public bool Shoot()
        {
            if (shootTimer <= 0 && ownerShip.energy >= energyCost)
            {
                if (!ownerShip.world.isRemote)
                {
                    Bullet bp = new Bullet(gunType, ownerShip, Position, Rotation, bulletSpeed, ownerShip.world);
                    ownerShip.energy -= energyCost;
                    effectColor = bp.color;
                    shootTimer = shootTimerMax;
                    ownerShip.world.bullets.Add(bp);
                    ServerPacketSender.CreateBullet(bp);
                }
                else
                {
                    Bullet bp = new Bullet(gunType, ownerShip, Position, Rotation, bulletSpeed, ownerShip.world);
                    effectColor = bp.color;
                    shootTimer = shootTimerMax;
                    ownerShip.energy -= energyCost;
                    randomSize = 1 + (float)core.random.NextDouble() / 4F;
                    randomEffect = (byte)core.random.Next(2);
                }
                return true;
            }
            return false;
        }
        public void RenderEffects(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Textures.blue[3], blue.Position, null, new Color(effectColor), Rotation, blue.Origin, randomSize, randomEffect == 0 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0);
            /*if (beam != null)
            {
                if (beam.beamTime > 0 && (gunType == 13 || gunType == 14 || gunType == 15 || gunType == 25))
                {
                    spriteBatch.Draw(Textures.textureLight, Position, null, new Color(new Vector4(beam.beamColor.X / 2F, beam.beamColor.Y / 2F, beam.beamColor.Z / 2F, beam.beamColor.W / 2F)), 0, new Vector2(Textures.textureLight.Width / 2, Textures.textureLight.Height / 2), 1.5F, SpriteEffects.None, 0);
                }
            }*/
            light.Render(spriteBatch);
        }
    }
}
