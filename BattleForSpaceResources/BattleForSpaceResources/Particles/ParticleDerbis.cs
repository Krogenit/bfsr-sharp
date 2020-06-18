using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;
using BattleForSpaceResources.Entitys;
using BattleForSpaceResources;

public class ParticleDerbis : GameObject
{
    public float AngularVelocity { get; set; }
    public Vector4 Color { get; set; }
    public float SizeVel { get; set; }
    public float AlphaVel { get; set; }
    public int TTL { get; set; }
    private Vector4 colorFire;
    private Vector4 colorLight;
    private Vector2 origin;
    private int lightingTimer, lightingTimer1;
    private bool changeFire, changeLight;
    private bool isFire, isLight, isFireExplosion;
    private Texture2D textureDerbis;
    private Texture2D textureDerbisFire;
    private Texture2D textureDerbisLight;
    private int genFireTimer;
    private bool isShipWrecks;
    private Vector2 velocity;
    public ParticleDerbis(int type, bool isWrecks, bool isFire, bool isfireexplosion, Vector2 position, Vector2 newvelocity, float angle, float angularVelocity, Vector4 color, float size, int ttl, float sizeVel, float alphaVel)
        : base(position)
    {
        isFireExplosion = isfireexplosion;
        this.isFire = isFire;
        if (!isWrecks)
        {
            isLight = false;
            textureDerbis = Textures.particleDerbis[type];
            if (isFire)
                textureDerbisFire = Textures.particleDerbisFire[type];
        }
        else
        {
            isLight = true;
            textureDerbis = Textures.particleWrecks[type];
            textureDerbisFire = Textures.particleWrecksFire[type];
            textureDerbisLight = Textures.particleWrecksLight[type];
        }
        float colorR = (float)core.random.NextDouble();
        colorFire = new Vector4(colorR, colorR, colorR, colorR);
        velocity = newvelocity;
        Rotation = angle;
        Color = color;
        AngularVelocity = angularVelocity;
        Size = size;
        SizeVel = sizeVel;
        AlphaVel = alphaVel;
        TTL = ttl;
        origin = new Vector2(textureDerbis.Width / 2, textureDerbis.Height / 2);
    }

    public ParticleDerbis(ShipType shiptype, int texturenum, Vector2 position, Vector2 velocity, float angle, float angularVelocity, Vector4 color, float size, int ttl, float sizeVel, float alphaVel)
        : base(position)
    {
        isFire = true;
        isLight = true;
        if (shiptype == ShipType.HumanHuge1)
        {
            textureDerbis = Textures.wercksHuge1[texturenum];
            textureDerbisFire = Textures.wercksHuge1[texturenum + 3];
            textureDerbisLight = Textures.wercksHuge1[texturenum + 6];
        }
        else if (shiptype == ShipType.EnemyMotherShip)
        {
            textureDerbis = Textures.wercksEnemyMotherShip[texturenum];
            textureDerbisFire = Textures.wercksEnemyMotherShip[texturenum + 3];
            textureDerbisLight = Textures.wercksEnemyMotherShip[texturenum + 6];
        }
        else if (shiptype == ShipType.HumanLargeStation1)
        {
            textureDerbis = Textures.wercksStation1[texturenum];
            textureDerbisFire = Textures.wercksStation1[texturenum + 2];
            textureDerbisLight = Textures.wercksStation1[texturenum + 4];
        }
        else if (shiptype == ShipType.HumanMotherShip1)
        {
            textureDerbis = Textures.wercksHumanMotherShip1[texturenum];
            textureDerbisFire = Textures.wercksHumanMotherShip1[texturenum + 2];
            textureDerbisLight = Textures.wercksHumanMotherShip1[texturenum + 4];
        }
        else if (shiptype == ShipType.CivMotherShip1)
        {
            textureDerbis = Textures.wercksCivilianMotherShip[texturenum];
            textureDerbisFire = Textures.wercksCivilianMotherShip[texturenum + 3];
            textureDerbisLight = Textures.wercksCivilianMotherShip[texturenum + 6];
        }
        float colorR = (float)core.random.NextDouble();
        colorFire = new Vector4(colorR, colorR, colorR, colorR);
        Position = position;
        this.velocity = velocity;
        Rotation = angle;
        Color = color;
        AngularVelocity = angularVelocity;
        Size = size;
        SizeVel = sizeVel;
        AlphaVel = alphaVel;
        TTL = ttl;
        origin = new Vector2(textureDerbis.Width / 2, textureDerbis.Height / 2);
        isShipWrecks = true;
    }

    public override void Update()
    {
        TTL--;
        Position += velocity;
        Rotation += AngularVelocity;
        Size += SizeVel;
        float horiz = velocity.X;
        float vertic = velocity.Y;
        velocity.X = horiz -= Settings.gravity * horiz;
        velocity.Y = vertic -= Settings.gravity * vertic;
        if (isFire)
        {
            if (isFireExplosion)
            {
                if (--genFireTimer <= 0 && Color.W > 0.6F)
                {
                    core.ps.Explosion(Position, 0.05F);
                    core.ps.DamageSmoke(Position, 0.05F);
                    genFireTimer = 10 + core.random.Next(1, 10);
                }
            }
            if (!changeFire)
            {
                if (colorFire.W < 1F)
                {
                    if (isShipWrecks)
                    {
                        colorFire.W += 0.0045F + (float)(core.random.NextDouble() / 4000F);
                        colorFire.X += 0.0045F;
                        colorFire.Y += 0.0045F;
                        colorFire.Z += 0.0045F;
                    }
                    else
                    {
                        colorFire.W += 0.0075F + (float)(core.random.NextDouble() / 4000F);
                        colorFire.X += 0.0075F;
                        colorFire.Y += 0.0075F;
                        colorFire.Z += 0.0075F;
                    }
                }
                else
                {
                    changeFire = true;
                }
            }
            else
            {
                if (colorFire.W > (isShipWrecks ? 0.7F : 0.6F))
                {
                    if (isShipWrecks)
                    {
                        colorFire.W -= 0.0045F - (float)(core.random.NextDouble() / 4000F);
                        colorFire.X -= 0.0045F;
                        colorFire.Y -= 0.0045F;
                        colorFire.Z -= 0.0045F;
                    }
                    else
                    {
                        colorFire.W -= 0.0075F - (float)(core.random.NextDouble() / 4000F);
                        colorFire.X -= 0.0075F;
                        colorFire.Y -= 0.0075F;
                        colorFire.Z -= 0.0075F;
                    }
                }
                else
                {
                    changeFire = false;
                }
            }
        }
        if (isLight)
        {
            if (--lightingTimer1 <= 0)
            {
                if (!changeLight)
                {
                    if (colorLight.W < Color.W)
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
                lightingTimer1 = 200 + core.random.Next(-100, 100);
                lightingTimer = 0;
            }
        }
        if (Color.W <= 0.5F && !isShipWrecks)
        {
            isFire = false;
            if (colorFire.W > 0.0F)
            {
                colorFire.W -= 0.005F;
                colorFire.X -= 0.005F;
                colorFire.Y -= 0.005F;
                colorFire.Z -= 0.005F;
            }
        }
        if (Color.W <= 0.3F && !isShipWrecks)
        {
            isLight = false;
            if (colorLight.W > 0.0F)
            {
                colorLight.W -= 0.2F;
                colorLight.X -= 0.2F;
                colorLight.Y -= 0.2F;
                colorLight.Z -= 0.2F;
            }
        }
        if (isShipWrecks)
        {
            if (TTL <= 3000)
            {
                isFire = false;
                if (colorFire.W > 0.0F)
                {
                    colorFire.W -= 0.005F;
                    colorFire.X -= 0.005F;
                    colorFire.Y -= 0.005F;
                    colorFire.Z -= 0.005F;
                }
            }
            if (TTL <= 1000)
            {
                isLight = false;
                if (colorLight.W > 0.0F)
                {
                    colorLight.W -= 0.2F;
                    colorLight.X -= 0.2F;
                    colorLight.Y -= 0.2F;
                    colorLight.Z -= 0.2F;
                }
                Color = new Vector4(Color.X, Color.Y, Color.Z, Color.W - AlphaVel);
            }
        }
        if (Color.W <= 0.2F && !isShipWrecks)
        {
            Color = new Vector4(Color.X, Color.Y, Color.Z, Color.W - AlphaVel * 3);
        }
        else if (!isShipWrecks)
            Color = new Vector4(Color.X, Color.Y, Color.Z, Color.W - AlphaVel);
    }
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(textureDerbis, Position, null, new Color(Color), Rotation, origin, Size, SpriteEffects.None, 0);
        if ((isFire || colorFire.W > 0) && textureDerbisFire != null)
        {
            spriteBatch.Draw(textureDerbisFire, Position, null, new Color(colorFire), Rotation, origin, Size, SpriteEffects.None, 0);
        }
        if (isLight || colorLight.W > 0)
        {
            spriteBatch.Draw(textureDerbisLight, Position, null, new Color(colorLight), Rotation, origin, Size, SpriteEffects.None, 0);
        }
    }
}