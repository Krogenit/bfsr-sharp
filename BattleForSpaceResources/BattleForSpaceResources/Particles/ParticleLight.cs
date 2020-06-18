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

public class ParticleLight
{
    private float gravity = 0.002F;
    public Texture2D Texture { get; set; }
    public Vector2 Position { get; set; }
    public Vector2 Velocity;
    public float Angle { get; set; }
    public float AngularVelocity { get; set; }
    public Vector4 Color { get; set; }
    public float Size { get; set; }
    public float SizeVel { get; set; }
    public float AlphaVel { get; set; }
    public bool isLighting;
    private Vector2 origin;
    public ParticleLight(Texture2D texture, Vector2 position, Vector2 velocity, float angle, float angularVelocity, Vector4 color, float size, int ttl, float sizeVel, float alphaVel)
    {
        Texture = texture;
        Position = position;
        Velocity = velocity;
        Angle = angle;
        Color = color;
        AngularVelocity = angularVelocity;
        Size = size;
        SizeVel = sizeVel;
        AlphaVel = alphaVel;
        isLighting = false;
        origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
    }

    public void Update()
    {
        Position += Velocity;
        Angle += AngularVelocity;
        Size += SizeVel;
        float horiz = Velocity.X;
        float vertic = Velocity.Y;
        Velocity.X = horiz -= gravity * horiz;
        Velocity.Y = vertic -= gravity * vertic;
        if (!isLighting)
        {
            if (Color.W + AlphaVel < 1F)
            {
                Color = new Vector4(Color.X, Color.Y, Color.Z, Color.W + AlphaVel);
            }
            else
            {
                isLighting = true;
            }
        }
        else
        {
            Color = new Vector4(Color.X, Color.Y, Color.Z, Color.W - AlphaVel);
        }
    }


    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Position, null, new Color(Color), Angle, origin, Size, SpriteEffects.None, 0);
    }
}