using BattleForSpaceResources.Entitys;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleForSpaceResources.Particles
{
    public class ParticleSystem
    {
        public static List<Particle> backparticles;
        public static List<Particle> particles;
        public static List<Particle> colorParticles;
        public static List<Particle> backcolorParticles;
        public static List<ParticleLight> lightParticles;
        public static List<ParticleLight> colorlightParticles;
        public static List<ParticleDerbis> colorDerbisParticle;
        public static List<ParticleDerbis> fonWrecksParticle;
        private Core core = Core.GetCore();
        private World world = Core.GetCore().GetWorld();
        public ParticleSystem()
        {
            particles = new List<Particle>();
            backcolorParticles = new List<Particle>();
            colorParticles = new List<Particle>();
            backparticles = new List<Particle>();
            lightParticles = new List<ParticleLight>();
            colorlightParticles = new List<ParticleLight>();
            colorDerbisParticle = new List<ParticleDerbis>();
            fonWrecksParticle = new List<ParticleDerbis>();
        }
        public void ShipDamageDerbis(int num, Vector2 shipvelocity, Vector2 position)
        {
            for (int a = 0; a < num; a++)
            {
                Vector2 velocity = shipvelocity / 2 + AngleToV2((float)(Math.PI * 2d * core.random.NextDouble()), 0.2f + (float)core.random.NextDouble() / 2);
                float angle = MathHelper.ToRadians(core.random.Next(360));
                float angleVel = -0.005F + (float)core.random.NextDouble() / 100;
                Vector4 color = new Vector4(0.5F, 0.5F, 0.5F, 1);
                int ttl = 3000;
                float size = 1F - (float)core.random.NextDouble() / 3F;
                float sizeVel = 0.0F;
                int rnd = core.random.Next(4);
                float alphaVel = rnd == 0 ? 0.0005F : rnd == 1 ? 0.0004F : rnd == 2 ? 0.0003F : 0.0006F;
                GenerateNewColorDerbisParticle(core.random.Next(6), false, core.random.Next(3) == 0 ? true : false, core.random.Next(5) == 0 ? true : false, position, velocity, angle, angleVel, color, size, ttl, sizeVel, alphaVel);
            }
        }
        public void ShipWrecks(ShipType shipType, Vector2 shipvelocity, Vector2 position, float angle)
        {
            float x = (float)Math.Cos(angle + 1.57F);
            float y = (float)Math.Sin(angle + 1.57F);
            float x1 = (float)Math.Cos(angle);
            float y1 = (float)Math.Sin(angle);
            Vector2[] velocity2 = new Vector2[4];
            velocity2[0] = new Vector2(x1 / 4F, y1 / 4F);//Down
            velocity2[1] = new Vector2(-x1 / 4F, -y1 / 4F);//Up
            velocity2[2] = new Vector2(x / 4F, y / 4F);//Left
            velocity2[3] = new Vector2(-x / 4F, -y / 4F);//Right
            if (shipType == ShipType.HumanHuge1)
            {
                for (int a = 0; a < 3; a++)
                {
                    Vector2 velocity = (a == 0 ? velocity2[a] : a == 1 ? velocity2[3] : velocity2[1]) + shipvelocity / 2F;
                    float angleVel = (float)core.random.Next(-100, 100) / 500000F;
                    Vector4 color = new Vector4(0.5F, 0.5F, 0.5F, 1);
                    int ttl = 7000;
                    float size = 1F;
                    float sizeVel = 0.0F;
                    float alphaVel = 0.001F;
                    ParticleDerbis particle = new ParticleDerbis(shipType, a, position, velocity, angle, angleVel, color, size, ttl, sizeVel, alphaVel);
                    fonWrecksParticle.Add(particle);
                }
            }
            else if (shipType == ShipType.EnemyMotherShip)
            {
                for (int a = 0; a < 3; a++)
                {
                    Vector2 velocity = (a == 0 ? velocity2[a] : a == 1 ? velocity2[1] : velocity2[3]) + shipvelocity / 2F;
                    float angleVel = (float)core.random.Next(-100, 100) / 500000F;
                    Vector4 color = new Vector4(0.5F, 0.5F, 0.5F, 1);
                    int ttl = 7000;
                    float size = 1F;
                    float sizeVel = 0.0F;
                    float alphaVel = 0.001F;
                    ParticleDerbis particle = new ParticleDerbis(shipType, a, position, velocity, angle, angleVel, color, size, ttl, sizeVel, alphaVel);
                    fonWrecksParticle.Add(particle);
                }
            }
            else if (shipType == ShipType.CivMotherShip1)
            {
                for (int a = 0; a < 3; a++)
                {
                    Vector2 velocity = (a == 0 ? velocity2[0] : a == 1 ? velocity2[1] : velocity2[2]) + shipvelocity / 2F;
                    float angleVel = (float)core.random.Next(-100, 100) / 500000F;
                    Vector4 color = new Vector4(0.5F, 0.5F, 0.5F, 1);
                    int ttl = 7000;
                    float size = 1F;
                    float sizeVel = 0.0F;
                    float alphaVel = 0.001F;
                    ParticleDerbis particle = new ParticleDerbis(shipType, a, position, velocity, angle, angleVel, color, size, ttl, sizeVel, alphaVel);
                    fonWrecksParticle.Add(particle);
                }
            }
            else if (shipType == ShipType.HumanMotherShip1)
            {
                for (int a = 0; a < 2; a++)
                {
                    Vector2 velocity = velocity2[a] + shipvelocity / 2F;
                    float angleVel = (float)core.random.Next(-100, 100) / 500000F;
                    Vector4 color = new Vector4(0.5F, 0.5F, 0.5F, 1);
                    int ttl = 7000;
                    float size = 1F;
                    float sizeVel = 0.0F;
                    float alphaVel = 0.001F;
                    ParticleDerbis particle = new ParticleDerbis(shipType, a, position, velocity, angle, angleVel, color, size, ttl, sizeVel, alphaVel);
                    fonWrecksParticle.Add(particle);
                }
            }
            else if (shipType == ShipType.HumanLargeStation1)
            {
                for (int a = 0; a < 2; a++)
                {
                    Vector2 velocity = (a == 0 ? velocity2[3] : velocity2[2]) + shipvelocity / 2F;
                    float angleVel = (float)core.random.Next(-100, 100) / 500000F;
                    Vector4 color = new Vector4(0.5F, 0.5F, 0.5F, 1);
                    int ttl = 7000;
                    float size = 1F;
                    float sizeVel = 0.0F;
                    float alphaVel = 0.001F;
                    ParticleDerbis particle = new ParticleDerbis(shipType, a, position, velocity, angle, angleVel, color, size, ttl, sizeVel, alphaVel);
                    fonWrecksParticle.Add(particle);
                }
            }
        }
        public void ShipDamageWrecks(int num, Vector2 shipvelocity, Vector2 position)
        {
            for (int a = 0; a < num; a++)
            {
                Vector2 velocity = shipvelocity / 2 + AngleToV2((float)(Math.PI * 2d * core.random.NextDouble()), 0.2f + (float)core.random.NextDouble() / 2);
                float angle = MathHelper.ToRadians(core.random.Next(360));
                float angleVel = -0.005F + (float)core.random.NextDouble() / 100;
                Vector4 color = new Vector4(0.5F, 0.5F, 0.5F, 1);
                int ttl = 3000;
                float size = 1F - (float)core.random.NextDouble() / 3F;
                float sizeVel = 0.0F;
                int rnd = core.random.Next(4);
                float alphaVel = rnd == 0 ? 0.0005F : rnd == 1 ? 0.0004F : rnd == 2 ? 0.0003F : 0.0006F;
                GenerateNewColorDerbisParticle(core.random.Next(3), true, true, core.random.Next(4) == 0 ? true : false, position, velocity, angle, angleVel, color, size, ttl, sizeVel, alphaVel);
            }
        }
        public void DamageSmoke(Vector2 position, float size)
        {
            for (int a = 0; a < 1; a++)
            {
                Vector2 velocity = AngleToV2((float)(Math.PI * 2d * core.random.NextDouble()), 0.2f + (float)core.random.NextDouble());
                float angle = MathHelper.ToRadians(core.random.Next(360));
                float angleVel = 0.0F;
                Vector4 color = new Vector4(0.75F, 0.75F, 0.75F, 1);
                int ttl = 200;
                float sizeVel = 0.008F;
                float alphaVel = 0.012F;
                GenerateNewBackColorParticle(Textures.effectSmokeRing, position, velocity, angle, angleVel, color, size, ttl, sizeVel, alphaVel);
            }
        }
        public void Explosion(Vector2 position, float size)
        {
            for (int a = 0; a < 2; a++)
            {
                Vector2 velocity = AngleToV2((float)(Math.PI * 2d * core.random.NextDouble()), 0.2f + (float)core.random.NextDouble());
                float angle = MathHelper.ToRadians(core.random.Next(360));
                float angleVel = 0.01F;
                Vector4 color = new Vector4(1F, 1F, 1F, 1);
                int ttl = 200;
                float sizeVel = 0.004F;
                float alphaVel = 0.014F;
                GenerateNewParticle(Textures.effectExplosion, position, velocity, angle, angleVel, color, size, ttl, sizeVel, alphaVel);
            }
        }
        public void LightingIon(Vector2 position, float size)
        {
            for (int a = 0; a < 3; a++)
            {
                Vector2 velocity = Vector2.Zero;
                float angle = MathHelper.ToRadians(core.random.Next(360));
                float angleVel = 0.0F;
                Vector4 color = new Vector4(0.75F, 0.75F, 1, 1);
                int ttl = 200;
                float sizeVel = 0.02F;
                float alphaVel = 0.08F;
                GenerateNewParticle(Textures.effectLighting, new Vector2(position.X + core.random.Next(-20, 20), position.Y + core.random.Next(-20, 20)), velocity, angle, angleVel, color, size, ttl, sizeVel, alphaVel);
            }
        }
        public void Directed(CollisionObject go, int type, Vector2 position, float rotation, Vector4 bulletColor, float size)
        {
            for (int a = 0; a < 1; a++)
            {
                Vector2 velocity = go.velocity / 3;
                float angle = rotation;
                float angleVel = 0.0F;
                int ttl = 300;
                float sizeVel = -0.01F;
                float alphaVel = 0.06F;
                if (bulletColor.W < 0.5F) bulletColor.W = 0.5F;
                GenerateNewBackParticle(type == 0 ? Textures.effectDirectedSpark : Textures.effectDirectedSplat, position, velocity, angle, angleVel, bulletColor, size, ttl, sizeVel, alphaVel);
            }
        }
        public float Jumping(Vector2 position, float size, int xpos, int ypos)
        {
            float angle = 0;
            for (int a = 0; a < 1; a++)
            {
                Vector2 spawnposition = new Vector2(position.X + xpos, position.Y + ypos);
                var mDx = (spawnposition.X) - position.X;
                var mDy = (spawnposition.Y) - position.Y;
                angle = (float)Math.Atan2(mDy, mDx);
                Vector2 velocity = new Vector2(-(float)Math.Cos(angle) * 35, -(float)Math.Sin(angle) * 35);
                float angleVel = 0.0F;
                Vector4 color = new Vector4(1f, 1f, 1f, 0f);
                int ttl = 35;
                float sizeVel = 0.0F;
                float alphaVel = 0.065F;
                GenerateNewLightParticle(Textures.effectJump, spawnposition, velocity, angle, angleVel, color, size, ttl, sizeVel, alphaVel);
            }
            if (MathHelper.ToDegrees(angle) == -45)
                angle = MathHelper.ToRadians(315);
            else if (MathHelper.ToDegrees(angle) == -135)
                angle = MathHelper.ToRadians(225);
            return angle;
        }
        public float JumpingOut(Vector2 position, float size, float angle)
        {
            for (int a = 0; a < 1; a++)
            {
                Vector2 velocity = new Vector2(-(float)Math.Cos(angle) * 35, -(float)Math.Sin(angle) * 35);
                float angleVel = 0.0F;
                Vector4 color = new Vector4(1f, 1f, 1f, 1f);
                int ttl = 35;
                float sizeVel = 0.0F;
                float alphaVel = 0.025F;
                GenerateNewParticle(Textures.effectJump, position, velocity, angle, angleVel, color, size, ttl, sizeVel, alphaVel);
            }
            return angle;
        }
        public void Jump(Vector2 position, float size)
        {
            for (int a = 0; a < 2; a++)
            {
                Vector2 velocity = new Vector2(0, 0);
                float angle = MathHelper.ToRadians(core.random.Next(360));
                float angleVel = 0.0F;
                Vector4 color = new Vector4(0.9f, 0.9f, 1f, 1f);
                int ttl = 200;
                float newsize = size * 4;
                float sizeVel = -0.02F;
                float alphaVel = 0.05F;
                GenerateNewLightParticle(Textures.light, position, velocity, angle, angleVel, color, newsize, ttl, sizeVel, alphaVel);
            }
            for (int a = 0; a < 2; a++)
            {
                Vector2 velocity = new Vector2(0, 0);
                float angle = MathHelper.ToRadians(core.random.Next(360));
                float angleVel = 0.01F;
                Vector4 color = new Vector4(1f, 1f, 1f, 1f);
                int ttl = 300;
                float sizeVel = -0.02F;
                float alphaVel = 0.05F;
                GenerateNewParticle(Textures.effectShieldDown, position, velocity, angle, angleVel, color, size, ttl, sizeVel, alphaVel);
            }
        }
        public void PrepareToJump(Vector2 pos, float size)
        {
            Vector2 velocity = new Vector2(0, 0);
            float angle = MathHelper.ToRadians(core.random.Next(360));
            float angleVel = 0.0F;
            Vector4 color = new Vector4(1f, 1f, 1f, 0.2f);
            int ttl = 300;
            float sizeVel = -0.02F;
            float alphaVel = -0.07F;
            GenerateNewParticle(Textures.effectShieldDown, pos, velocity, angle, angleVel, color, size, ttl, sizeVel, alphaVel);
        }
        public void ShieldDown(Vector2 position, float size)
        {
            for (int a = 0; a < 1; a++)
            {
                Vector2 velocity = new Vector2(0, 0);
                float angle = MathHelper.ToRadians(core.random.Next(360));
                float angleVel = 0.0F;
                Vector4 color = new Vector4(1f, 1f, 1f, 1f);
                int ttl = 300;
                float sizeVel = -0.01F;
                float alphaVel = 0.03F;
                GenerateNewParticle(Textures.effectShockWave[2], position, velocity, angle, angleVel, color, size, ttl, sizeVel, alphaVel);
            }
            for (int a = 0; a < 1; a++)
            {
                Vector2 velocity = new Vector2(0, 0);
                float angle = MathHelper.ToRadians(core.random.Next(360));
                float angleVel = 0.05F;
                Vector4 color = new Vector4(1f, 1f, 1f, 1f);
                int ttl = 300;
                float sizeVel = -0.01F;
                float alphaVel = 0.03F;
                GenerateNewParticle(Textures.effectShieldDown, position, velocity, angle, angleVel, color, size, ttl, sizeVel, alphaVel);
            }
        }
        public void Skockwave(int type, Vector2 position, float size)
        {
            for (int a = 0; a < 1; a++)
            {
                Vector2 velocity = new Vector2(0, 0);
                float angle = MathHelper.ToRadians(core.random.Next(360));
                float angleVel = 0.0F;
                Vector4 color = new Vector4(0.5F, 0.5F, 0.5F, 1);
                int ttl = 300;
                float sizeVel = type == 1 ? 0.008F : type == 2 ? 0.015F : 0.02F;
                float alphaVel = type == 1 ? 0.004F : type == 2 ? 0.006F : 0.004F;
                GenerateNewParticle(Textures.effectShockWave[type], position, velocity, angle, angleVel, color, size, ttl, sizeVel, alphaVel);
            }
        }
        public void RocketShoot(Vector2 position, float size)
        {
            for (int a = 0; a < 2; a++)
            {
                Vector2 velocity = new Vector2(0, 0);
                float angle = MathHelper.ToRadians(core.random.Next(360));
                float angleVel = 0.0F;
                Vector4 color = new Vector4(1f, 1f, 0.5f, 1f);
                float size1 = size * 1.5f;
                int ttl = 200;
                float sizeVel = 0.035F;
                float alphaVel = 0.02F;
                GenerateNewBackParticle(Textures.effectRocketEffect, position, velocity, angle, angleVel, color, size1, ttl, sizeVel, alphaVel);
            }
            for (int a = 0; a < 1; a++)
            {
                Vector2 velocity = new Vector2(0, 0);
                float angle = MathHelper.ToRadians(core.random.Next(360));
                float angleVel = 0.0F;
                Vector4 color = new Vector4(0.3f, 0.3f, 0.3f, 1f);
                int ttl = 200;
                float sizeVel = .01F;
                float alphaVel = 0.015F;
                GenerateNewColorParticle(Textures.effectRocketSmoke, position, velocity, angle, angleVel, color, size, ttl, sizeVel, alphaVel);
            }
        }
        public void ShipDamage(int num, Vector2 position, CollisionObject go, float size)
        {
            for (int a = 0; a < num; a++)
            {
                Vector2 velocity = new Vector2(go.velocity.X / 3, go.velocity.Y / 3) + AngleToV2((float)(Math.PI * 2d * core.random.NextDouble()), 0.2f + (float)core.random.NextDouble() / 3);
                float angle = MathHelper.ToRadians(core.random.Next(360));
                int rnd = core.random.Next(2);
                float angleVel = rnd == 0 ? 0.001F - (float)core.random.NextDouble() / 200 : -0.001F + (float)core.random.NextDouble() / 200;
                Vector4 color = new Vector4(0.5F, 0.5F, 0.5F, 1);
                int ttl = 1000;
                float sizeVel = (float)core.random.NextDouble() / 600F;
                float alphaVel = 0.002F;
                GenerateNewBackColorParticle(Textures.effectShipDamage, position, velocity, angle, angleVel, color, size, ttl, sizeVel, alphaVel);
            }
        }
        public void ShipDerbis(int num, int type, float size, Vector2 position, CollisionObject go)
        {
            for (int a = 0; a < num; a++)
            {
                Vector2 velocity = new Vector2(go.velocity.X / 3, go.velocity.Y / 3) + AngleToV2((float)(Math.PI * 2d * core.random.NextDouble()), 0.25f + (float)core.random.NextDouble());
                float angle = MathHelper.ToRadians(core.random.Next(360));
                float angleVel = 0.0005F - (float)core.random.NextDouble() / 2000;
                Vector4 color = new Vector4(0.5F, 0.5F, 0.5F, 1);
                int ttl = 1000;
                int rnd = core.random.Next(2);
                float sizeVel = -0.0001F;
                float alphaVel = 0.001F;
                GenerateNewBackColorParticle(type == 0 ? Textures.effectShipDerbiSmall : Textures.effectShipDerbi, position, velocity, angle, angleVel, color, size, ttl, sizeVel, alphaVel);
            }
        }
        public void ShipOst(Vector2 position, CollisionObject go)
        {
            int num = core.random.Next(2);
            for (int a = 0; a < num; a++)
            {
                Vector2 velocity = new Vector2(go.velocity.X / 3, go.velocity.Y / 3) + AngleToV2((float)(Math.PI * 2d * core.random.NextDouble()), 0.2f + (float)core.random.NextDouble() / 3);
                float angle = MathHelper.ToRadians(core.random.Next(360));
                int rnd = core.random.Next(2);
                float angleVel = rnd == 0 ? 0.01F - (float)core.random.NextDouble() / 10 : -0.01F + (float)core.random.NextDouble() / 10;
                Vector4 color = new Vector4(0.5F, 0.5F, 0.5F, 1);
                float size = 0.75f + (float)core.random.NextDouble() / 4F;
                int ttl = 1000;
                float sizeVel = 0;
                float alphaVel = 0.001F;
                rnd = core.random.Next(2);
                GenerateNewBackColorParticle(rnd == 0 ? Textures.particleShipOst1 : Textures.particleShipOst2, position, velocity, angle, angleVel, color, size, ttl, sizeVel, alphaVel);
            }
        }
        public void BeamDamage(Vector2 position, float rot, float size, Vector4 color)
        {
            for (int a = 0; a < 1; a++)
            {
                Vector2 velocity = Vector2.Zero;
                float angle = rot;
                float angleVel = 0;
                int ttl = 100;
                float sizeVel = 0.075F + (float)core.random.NextDouble() / 40;
                float alphaVel = .03f;

                GenerateNewParticle(Textures.effectBeamDamage, position, velocity, angle, angleVel, color, size, ttl, sizeVel, alphaVel);
            }
        }
        public void Lighting(Vector2 position, float size, Vector4 color, float alphaVel)
        {
            for (int a = 0; a < 1; a++)
            {
                Vector2 velocity = Vector2.Zero;
                float angle = 0;
                float angleVel = 0;
                int ttl = 200;
                float sizeVel = 0;

                GenerateNewLightParticle(Textures.light, position, velocity, angle, angleVel, color, size, ttl, sizeVel, alphaVel);
            }
        }
        public void BackLight(Vector2 position, float size, Vector4 color, float alphaVel)
        {
            Vector2 velocity = Vector2.Zero;
            float angle = 0;
            float angleVel = 0;
            int ttl = 200;
            float sizeVel = 0;

            GenerateNewBackParticle(Textures.light, position, velocity, angle, angleVel, color, size, ttl, sizeVel, alphaVel);
        }
        public void Light(Vector2 position, float size, Vector4 color, float alphaVel)
        {
            for (int a = 0; a < 1; a++)
            {
                Vector2 velocity = Vector2.Zero;
                float angle = 0;
                float angleVel = 0;
                int ttl = 200;
                float sizeVel = 0;

                GenerateNewParticle(Textures.light, position, velocity, angle, angleVel, color, size, ttl, sizeVel, alphaVel);
            }
        }
        public void SpawnLight(Vector2 position, float size)
        {
            for (int a = 0; a < 1; a++)
            {
                Vector2 velocity = Vector2.Zero;
                float angle = 0;
                float angleVel = 0;
                Vector4 color = new Vector4(0.8f, 0.8f, 1f, 0.4f);
                int ttl = 100;
                float sizeVel = 0;
                float alphaVel = .05f;

                GenerateNewLightParticle(Textures.light, position, velocity, angle, angleVel, color, size, ttl, sizeVel, alphaVel);
            }
        }
        public void EngineAnother(Vector2 position)
        {
            for (int a = 0; a < 2; a++)
            {
                Vector2 velocity = AngleToV2((float)(Math.PI * 2d * core.random.NextDouble()), 0.6f);
                float angle = 0;
                float angleVel = 0;
                Vector4 color = new Vector4(1f, 1f, 1f, 1f);
                float size = 0.2f;
                int ttl = 30;
                float sizeVel = 0;
                float alphaVel = 0.05F;

                GenerateNewParticle(Textures.smoke[1], position, velocity, angle, angleVel, color, size, ttl, sizeVel, alphaVel);
            }
        }
        public void EngineRocket(Vector2 position, CollisionObject go, Vector4 color)
        {
            if (core.random.Next(3) == 0)
                for (int a = 0; a < 1; a++)
                {
                    Vector2 velocity = AngleToV2((float)(Math.PI * 2d * core.random.NextDouble()), .2f);
                    float angle = MathHelper.ToRadians(core.random.Next(360));
                    float angleVel = 0;
                    float size = 0.2F;
                    color = new Vector4(color.X, color.Y, color.Z, color.W / 2);
                    float sizeVel = .005F;
                    int ttl = 60;
                    float alphaVel = .03F;
                    GenerateNewBackParticle(Textures.smoke[2], position, velocity, angle, angleVel, color, size, ttl, sizeVel, alphaVel);
                }

            for (int a = 0; a < 1; a++)
            {
                Vector2 velocity = Vector2.Zero;
                float angle = go.Rotation;
                float angleVel = 0;
                float size = 1F;
                int ttl = 15;
                float sizeVel = -.08f;
                float alphaVel = .02F;
                GenerateNewBackParticle(Textures.blue[2], position, velocity, angle, angleVel, color, size, ttl, sizeVel, alphaVel);
            }
        }
        public void EngineEmpuls(Vector2 position, CollisionObject go, float size, float alphaVel)
        {
            for (int a = 0; a < 2; a++)
            {
                Vector2 velocity = AngleToV2((float)(Math.PI * 2d * core.random.NextDouble()), 0.6f);
                float angle = 0;
                float angleVel = 0;
                Vector4 color = new Vector4(1f, 1f, 1f, 1f);
                int ttl = 100;
                float sizeVel = 0;
                float alphaVel1 = alphaVel * 2;

                GenerateNewParticle(Textures.smoke[1], position, velocity, angle, angleVel, color, size, ttl, sizeVel, alphaVel1);
            }

            for (int a = 0; a < 1; a++)
            {
                Vector2 velocity = AngleToV2((float)(Math.PI * 2d * core.random.NextDouble()), .2f);
                float angle = 0;
                float angleVel = 0;
                Vector4 color = new Vector4(0.5f, 0.5f, 1f, 0.5f);
                int ttl = 80;
                float sizeVel = 0;

                GenerateNewParticle(Textures.smoke[2], position, velocity, angle, angleVel, color, size, ttl, sizeVel, alphaVel);
            }

            for (int a = 0; a < 2; a++)
            {
                Vector2 velocity = Vector2.Zero;
                float angle = go.Rotation;
                float angleVel = 0;
                Vector4 color = new Vector4(0.5f, 0.5f, 1f, 1f);
                float size1 = size + size * 2 * (float)core.random.NextDouble();
                int ttl = 40;
                float sizeVel = -.05f;
                float alphaVel1 = alphaVel * 2;

                GenerateNewParticle(Textures.blue[2], position, velocity, angle, angleVel, color, size1, ttl, sizeVel, alphaVel1);
            }
        }
        public void HugeEngineEmpuls(Vector2 position, CollisionObject go, float size, float alphaVel)
        {
            for (int a = 0; a < 1; a++)
            {
                Vector2 velocity = AngleToV2((float)(Math.PI * 2d * core.random.NextDouble()), .2f);
                float angle = MathHelper.ToRadians(core.random.Next(360));
                float angleVel = 0;
                Vector4 color = new Vector4(0.5f, 0.5f, 1f, 0.5f);
                int ttl = 150;
                float sizeVel = .005F;
                if (core.random.Next(2) == 0)
                    GenerateNewParticle(Textures.smoke[2], position, velocity, angle, angleVel, color, 0.2F, ttl, sizeVel, alphaVel);
            }

            for (int a = 0; a < 2; a++)
            {
                Vector2 velocity = Vector2.Zero;
                float angle = go.Rotation;
                float angleVel = 0;
                Vector4 color = new Vector4(0.5f, 0.5f, 1f, 1f);
                float size1 = size + size * 3 * (float)core.random.NextDouble();
                int ttl = 150;
                float sizeVel = -.05f;
                float alphaVel1 = alphaVel * 2;

                GenerateNewParticle(Textures.blue[2], position, velocity, angle, angleVel, color, size1, ttl, sizeVel, alphaVel1);
            }
        }
        public void DestroyShipEffect(Vector2 position, float size, float alphaVel1, Vector4 color1)
        {
            for (int a = 0; a < 3; a++)
            {
                Vector2 velocity = AngleToV2((float)(Math.PI * 2d * core.random.NextDouble()), .2f);
                float angle = MathHelper.ToRadians(core.random.Next(360));
                float angleVel = 0;
                Vector4 color = new Vector4(1.0f, 0.5f, 0.0f, 0.0f);
                int ttl = 200;
                float sizeVel = 0;
                float alphaVel = .05f;

                GenerateNewLightParticle(Textures.effectExplosion, position, velocity, angle, angleVel, color, size / 4, ttl, sizeVel, alphaVel);
            }

            for (int a = 0; a < 2; a++)
            {
                Vector2 velocity = AngleToV2((float)(Math.PI * 2d * core.random.NextDouble()), .2f);
                float angle = MathHelper.ToRadians(core.random.Next(360));
                float angleVel = 0;
                //Vector4 color = new Vector4(1.0f, 0.5f, 0.0f, 0.0f);
                int ttl = 200;
                float sizeVel = 0;
                //float alphaVel = .04f;

                GenerateNewLightParticle(Textures.spark[core.random.Next(4)], position, velocity, angle, angleVel, color1, size, ttl, sizeVel, alphaVel1);
            }
        }
        /*public void PickupEffect(Vector2 position, Vector2 velocity, PickupType pt, PickupSize ps)
        {
            if (pt == PickupType.Crystals)
            {
                for (int a = 0; a < 1; a++)
                {
                    float angle = 0;
                    float angleVel = 0;
                    int ttl = 200;
                    float sizeVel = 0;
                    float alphaVel = 0.05F;
                    float size = ps == PickupSize.Small ? 1f : 1.5f;
                    Vector4 color = new Vector4(0.5F, 0.5F, 0.7F, 1f);
                    GenerateNewBackParticle(Textures.light, position, velocity, angle, angleVel, color, size, ttl, sizeVel, alphaVel);
                }
            }
            else
            {
                for (int a = 0; a < 1; a++)
                {
                    float angle = 0;
                    float angleVel = 0;
                    int ttl = 200;
                    float sizeVel = 0;
                    float alphaVel = 0.05F;
                    float size = 1f;
                    Vector4 color = new Vector4(0.5F, 0.5F, 0.5F, 1f);
                    GenerateNewBackParticle(Textures.light, position, velocity, angle, angleVel, color, size, ttl, sizeVel, alphaVel);
                }
            }
        }*/
        private ParticleDerbis GenerateNewColorDerbisParticle(int type, bool isWrecks, bool isFire, bool isFireExplosion, Vector2 position, Vector2 velocity,
            float angle, float angularVelocity, Vector4 color, float size, int ttl, float sizeVel, float alphaVel)
        {
            ParticleDerbis particle = new ParticleDerbis(type, isWrecks, isFire, isFireExplosion, position, velocity, angle, angularVelocity, color, size, ttl, sizeVel, alphaVel);
            colorDerbisParticle.Add(particle);
            return particle;
        }
        private Particle GenerateNewColorParticle(Texture2D texture, Vector2 position, Vector2 velocity,
            float angle, float angularVelocity, Vector4 color, float size, int ttl, float sizeVel, float alphaVel)
        {
            Particle particle = new Particle(texture, position, velocity, angle, angularVelocity, color, size, sizeVel, alphaVel);
            colorParticles.Add(particle);
            return particle;
        }
        private Particle GenerateNewParticle(Texture2D texture, Vector2 position, Vector2 velocity,
            float angle, float angularVelocity, Vector4 color, float size, int ttl, float sizeVel, float alphaVel)
        {
            Particle particle = new Particle(texture, position, velocity, angle, angularVelocity, color, size, sizeVel, alphaVel);
            particles.Add(particle);
            return particle;
        }
        private ParticleLight GenerateNewLightParticle(Texture2D texture, Vector2 position, Vector2 velocity,
            float angle, float angularVelocity, Vector4 color, float size, int ttl, float sizeVel, float alphaVel)
        {
            ParticleLight particleLight = new ParticleLight(texture, position, velocity, angle, angularVelocity, color, size, ttl, sizeVel, alphaVel);
            lightParticles.Add(particleLight);
            return particleLight;
        }
        private Particle GenerateNewBackColorParticle(Texture2D texture, Vector2 position, Vector2 velocity,
            float angle, float angularVelocity, Vector4 color, float size, int ttl, float sizeVel, float alphaVel)
        {
            Particle particleLight = new Particle(texture, position, velocity, angle, angularVelocity, color, size, sizeVel, alphaVel);
            backcolorParticles.Add(particleLight);
            return particleLight;
        }
        private Particle GenerateNewBackParticle(Texture2D texture, Vector2 position, Vector2 velocity,
            float angle, float angularVelocity, Vector4 color, float size, int ttl, float sizeVel, float alphaVel)
        {
            Particle particle = new Particle(texture, position, velocity, angle, angularVelocity, color, size, sizeVel, alphaVel);
            backparticles.Add(particle);
            return particle;
        }
        private ParticleLight GenerateNewColorLightParticle(Texture2D texture, Vector2 position, Vector2 velocity,
            float angle, float angularVelocity, Vector4 color, float size, int ttl, float sizeVel, float alphaVel)
        {
            ParticleLight particleLight = new ParticleLight(texture, position, velocity, angle, angularVelocity, color, size, ttl, sizeVel, alphaVel);
            colorlightParticles.Add(particleLight);
            return particleLight;
        }
        public void Update()
        {
            for (int particle = 0; particle < particles.Count; particle++)
            {
                particles[particle].Update();
                if (particles[particle].Size <= 0 || (particles[particle].alphaVelocity >= 0 && particles[particle].color.W <= 0.0001F))
                {
                    particles.RemoveAt(particle);
                }
            }
            for (int particle = 0; particle < lightParticles.Count; particle++)
            {
                lightParticles[particle].Update();
                if (lightParticles[particle].Size <= 0 || (lightParticles[particle].isLighting && lightParticles[particle].Color.W <= 0.0001F))
                {
                    lightParticles.RemoveAt(particle);
                }
            }
            for (int particle = 0; particle < colorParticles.Count; particle++)
            {
                colorParticles[particle].Update();
                if (colorParticles[particle].Size <= 0 || (colorParticles[particle].alphaVelocity >= 0 && colorParticles[particle].color.W <= 0.0001F))
                {
                    colorParticles.RemoveAt(particle);
                }
            }
            for (int particle = 0; particle < colorlightParticles.Count; particle++)
            {
                colorlightParticles[particle].Update();
                if (colorlightParticles[particle].Size <= 0 || (colorlightParticles[particle].isLighting && colorlightParticles[particle].Color.W <= 0.0001F))
                {
                    colorlightParticles.RemoveAt(particle);
                }
            }
            for (int particle = 0; particle < backcolorParticles.Count; particle++)
            {
                backcolorParticles[particle].Update();
                if (backcolorParticles[particle].Size <= 0 || (backcolorParticles[particle].alphaVelocity >= 0 && backcolorParticles[particle].color.W <= 0.0001F))
                {
                    backcolorParticles.RemoveAt(particle);
                }
            }
            for (int particle = 0; particle < colorDerbisParticle.Count; particle++)
            {
                colorDerbisParticle[particle].Update();
                if (colorDerbisParticle[particle].Size <= 0 || colorDerbisParticle[particle].TTL <= 0 || (colorDerbisParticle[particle].AlphaVel >= 0 && colorDerbisParticle[particle].Color.W <= 0.0001F))
                {
                    colorDerbisParticle.RemoveAt(particle);
                }
            }
            for (int i = 0; i < fonWrecksParticle.Count; i++)
            {
                fonWrecksParticle[i].Update();
                if (fonWrecksParticle[i].Size <= 0 || fonWrecksParticle[i].TTL <= 0 || (fonWrecksParticle[i].AlphaVel >= 0 && fonWrecksParticle[i].Color.W <= 0.0001F))
                {
                    fonWrecksParticle.RemoveAt(i);
                }
            }
            for (int i = 0; i < backparticles.Count; i++)
            {
                backparticles[i].Update();
                if (backparticles[i].Size <= 0 || (backparticles[i].alphaVelocity >= 0 && backparticles[i].color.W <= 0.0001F))
                {
                    backparticles.RemoveAt(i);
                }
            }
        }
        public void RederAdditive(SpriteBatch spriteBatch)
        {
            for (int index = 0; index < particles.Count; index++)
            {
                if (Vector2.Distance(particles[index].Position, core.cam.screenCenter) <= Settings.renderDistance)
                    particles[index].Render(spriteBatch);
            }
            for (int index = 0; index < lightParticles.Count; index++)
            {
                if (Vector2.Distance(lightParticles[index].Position, core.cam.screenCenter) <= Settings.renderDistance)
                    lightParticles[index].Draw(spriteBatch);
            }
        }
        public void RenderBackAdditive(SpriteBatch spriteBatch)
        {
            for (int index = 0; index < backparticles.Count; index++)
            {
                if (Vector2.Distance(backparticles[index].Position, core.cam.screenCenter) <= Settings.renderDistance)
                    backparticles[index].Render(spriteBatch);
            }
        }
        public void RenderAlpha(SpriteBatch spriteBatch)
        {
            for (int index = 0; index < colorParticles.Count; index++)
            {
                if (Vector2.Distance(colorParticles[index].Position, core.cam.screenCenter) <= Settings.renderDistance)
                    colorParticles[index].Render(spriteBatch);
            }
            for (int index = 0; index < colorlightParticles.Count; index++)
            {
                if (Vector2.Distance(colorlightParticles[index].Position, core.cam.screenCenter) <= Settings.renderDistance)
                    colorlightParticles[index].Draw(spriteBatch);
            }
        }
        public void BackRenderAlpha(SpriteBatch spriteBatch)
        {
            for (int index = 0; index < colorDerbisParticle.Count; index++)
            {
                if (Vector2.Distance(colorDerbisParticle[index].Position, core.cam.screenCenter) <= Settings.renderDistance)
                    colorDerbisParticle[index].Draw(spriteBatch);
            }
            for (int index = 0; index < backcolorParticles.Count; index++)
            {
                if (Vector2.Distance(backcolorParticles[index].Position, core.cam.screenCenter) <= Settings.renderDistance)
                    backcolorParticles[index].Render(spriteBatch);
            }
        }
        public void RenderWrecks(SpriteBatch spriteBatch)
        {
            for (int index = 0; index < fonWrecksParticle.Count; index++)
            {
                if (Vector2.Distance(fonWrecksParticle[index].Position, core.cam.screenCenter) <= Settings.renderDistance)
                {
                    fonWrecksParticle[index].Draw(spriteBatch);
                }
            }
        }
        public static Vector2 AngleToV2(float angle, float length)
        {
            Vector2 direction = Vector2.Zero;
            direction.X = (float)Math.Cos(angle) * length;
            direction.Y = -(float)Math.Sin(angle) * length;
            return direction;
        }
    }
}
