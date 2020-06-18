using BattleForSpaceResources.Entitys;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleForSpaceResources.ShipComponents
{
    public class ShipDamage : GameObject
    {
        private Ship owner;
        private int damageType;
        private Texture2D textureLight;
        private Texture2D textureFire;
        private Texture2D textureFix;
        private bool changeFire, changeLight;
        public Vector4 colorFix, colorFire, colorLight;
        public float damage;
        private int repairTimer;
        private int smokeTimer;
        private int lightingTimer, lightingTimer1;
        private bool isCreate;
        private float damageRot;
        private int ionTimer;
        public ShipDamage(Ship s, int type, float dmg, Vector2 pos)
            : base(pos)
        {
            owner = s;
            isCreate = false;
            damage = dmg;
            damageType = type;
            if (damageType == 0)
            {
                Create(Textures.damage[damageType]);
                textureFire = Textures.damageFire[damageType];
                textureFix = Textures.damageFix[damageType];
            }
            if (damageType == 1)
            {
                Create(Textures.damage[damageType]);
                textureFire = Textures.damageFire[damageType];
                textureFix = Textures.damageFix[damageType];
            }
            if (damageType == 2)
            {
                Create(Textures.damage[damageType]);
                textureFire = Textures.damageFire[damageType];
                textureFix = Textures.damageFix[damageType];
                textureLight = Textures.damageLight[0];
            }
            if (damageType == 3)
            {
                Create(Textures.damage[damageType]);
                textureFire = Textures.damageFire[damageType];
                textureFix = Textures.damageFix[damageType];
                textureLight = Textures.damageLight[1];
            }
            colorFire = new Vector4(0.5F, 0.5F, 0.5F, 0.5F);
            colorLight = new Vector4(0, 0, 0, 0);
            colorFix = new Vector4(0, 0, 0, 0);
        }
        public void Update(float x, float x1, float y, float y1)
        {
            Rotation = owner.Rotation;
            Position = new Vector2(owner.Position.X + x - x1, owner.Position.Y + y - y1);

            if (owner.hull / owner.maxHull <= damage)
            {
                if (!isCreate)
                {
                    damageRot = MathHelper.ToRadians(core.random.Next(360));
                    core.ps.ShipDamageDerbis(core.random.Next(2), owner.velocity, Position);
                    core.ps.Explosion(Position, 0.1F);
                    core.ps.Explosion(Position, 0.1F);
                    core.ps.DestroyShipEffect(Position, 0.5F, .04f, new Vector4(1.0f, 0.5f, 0.0f, 0.5f));
                    core.ps.Light(Position, 4F, new Vector4(1.0f, 0.5f, 0.5f, 0.7f), .04f);
                    isCreate = true;
                }
                if (--smokeTimer <= 0)
                {
                    core.ps.DamageSmoke(Position, 0.1F);
                    core.ps.ShipDamage(1, Position, owner, 0.1F);
                    smokeTimer = 15 + core.random.Next(1, 5);
                }
                if (--ionTimer <= 0 && (damageType == 2 || damageType == 3))
                {
                    core.ps.LightingIon(Position, Size);
                    ionTimer = 200 + core.random.Next(0, 120);
                }
                colorFix = new Vector4(1, 1, 1, 1);
            }
            else if (colorFix.W > 0)
            {
                if (colorFix.W == 1 && repairTimer != 1000)
                {
                    repairTimer = 1000;
                    colorFix.W -= 0.0005F;
                    colorFix.X -= 0.0005F;
                    colorFix.Y -= 0.0005F;
                    colorFix.Z -= 0.0005F;
                }
                if (--repairTimer <= 0)
                {
                    isCreate = false;
                    colorFix.W -= 0.0005F;
                    colorFix.X -= 0.0005F;
                    colorFix.Y -= 0.0005F;
                    colorFix.Z -= 0.0005F;
                }
            }
            if (owner.hull / owner.maxHull <= damage || repairTimer > 0)
            {
                if (!changeFire)
                {
                    if (colorFire.W < 1)
                    {
                        colorFire.W += 0.005F;
                        colorFire.X += 0.005F;
                        colorFire.Y += 0.005F;
                        colorFire.Z += 0.005F;
                    }
                    else
                    {
                        changeFire = true;
                    }
                }
                else
                {
                    if (colorFire.W > 0.5F)
                    {
                        colorFire.W -= 0.005F;
                        colorFire.X -= 0.005F;
                        colorFire.Y -= 0.005F;
                        colorFire.Z -= 0.005F;
                    }
                    else
                    {
                        changeFire = false;
                    }
                }
                if (--lightingTimer1 <= 0 && (damageType == 2 || damageType == 3))
                {
                    if (!changeLight)
                    {
                        if (colorLight.W < 1F)
                        {
                            colorLight.W += 0.2F;
                            colorLight.X += 0.2F;
                            colorLight.Y += 0.2F;
                            colorLight.Z += 0.2F;
                        }
                        else
                        {
                            changeLight = true;
                            lightingTimer += 25;
                        }
                    }
                    else
                    {
                        if (colorLight.W > 0.0F)
                        {
                            colorLight.W -= 0.2F;
                            colorLight.X -= 0.2F;
                            colorLight.Y -= 0.2F;
                            colorLight.Z -= 0.2F;
                        }
                        else
                        {
                            changeLight = false;
                            lightingTimer += 25;
                        }
                    }
                }
                if (lightingTimer >= 100)
                {
                    lightingTimer1 = 200 + core.random.Next(-75, 75);
                    lightingTimer = 0;
                }
            }
        }
        public override void Render(SpriteBatch spriteBatch)
        {
            if ((owner.hull / owner.maxHull <= damage || repairTimer > 0))
            {
                spriteBatch.Draw(Text, Position, null, Color.White, Rotation + damageRot, Origin, Size, SpriteEffects.None, 0);
                spriteBatch.Draw(textureFire, Position, null, new Color(colorFire), Rotation + damageRot, Origin, Size, SpriteEffects.None, 0);
                if (textureLight != null)
                    spriteBatch.Draw(textureLight, Position, null, new Color(colorLight), Rotation + damageRot, Origin, Size, SpriteEffects.None, 0);
            }
            else if (repairTimer <= 0)
            {
                spriteBatch.Draw(textureFix, Position, null, new Color(colorFix), Rotation + damageRot, Origin, Size, SpriteEffects.None, 0);
            }
        }
    }
}
