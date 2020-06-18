using BattleForSpaceResources.Networking;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleForSpaceResources.Entitys
{
    public class ShipPlayer : Ship
    {
        public ShipPlayer(Vector2 pos, Vector2 bp, Faction faction, ShipType type, int[] components, World w, AiSearchType ast, AiAgressiveType agr)
            : base(pos, bp, faction, type, components, w, ast, agr)
        {

        }
        public override void Ai()
        {
            if (isControled)
            {
                Control();
            }
            else
            {
                cos = (float)Math.Cos(Rotation);
                sin = (float)Math.Sin(Rotation);
                cos1 = (float)Math.Cos(Rotation + 1.57F);
                sin1 = (float)Math.Sin(Rotation + 1.57F);
            }
        }
        private void Control()
        {
            Vector2 ShipAcceleration = Vector2.Zero;
            Vector2 Forcevelocity = Vector2.Zero;
            String direction = "";
            if (world.isRemote && shipName.Equals(core.GetPlayerName()) && core.currentGui == null && core.currentGuiAdd == null && !core.isChat)
            {
                RotateToVector(core.inputManager.cursorAdvancedPosition, Vector2.Zero);
                if (core.inputManager.IsLeftButtonDown() && !core.guiInGame.controlButton.Rect.Contains(new Point((int)core.inputManager.cursor.Position.X, (int)core.inputManager.cursor.Position.Y)))
                {
                    if (gunSlots != null)
                    {
                        for (int i = 0; i < gunSlots.Length; i++)
                        {
                            if (gunSlots[i] != null)
                            {
                                if (gunSlots[i].Shoot())
                                {
                                    break;
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
                                if (turretSlots[i].Shoot(false))
                                    break;
                            }
                        }
                    }*/
                }
                /*if (core.inputManager.IsRightButtonDown())
                {
                    /*if (rocketSlots != null)
                    {
                        for (int i = 0; i < rocketSlots.Length; i++)
                        {
                            if (rocketSlots[i] != null)
                            {
                                if (rocketSlots[i].Shoot())
                                    break;
                            }
                        }
                    }
                }*/
                if (core.inputManager.IsKeyDown(Keys.W))
                {
                    ShipAcceleration = new Vector2(-(float)Math.Cos(Rotation), -(float)Math.Sin(Rotation)) / (mass + armorMass);
                    direction = "Up";
                    ShipEngine(0, cos, cos1, sin, sin1);
                }
                else if (core.inputManager.IsKeyDown(Keys.S) && shipInfo.manevering)
                {
                    ShipAcceleration = new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation)) / (mass + armorMass);
                    direction = "Down";
                    ShipEngine(3, cos, cos1, sin, sin1);
                }
                else
                {
                    direction = "";
                    if (core.inputManager.IsKeyDown(Keys.X))
                    {
                        float horiz = velocity.X;
                        float vertic = velocity.Y;
                        velocity.X = horiz -= Settings.gravity * 20 * horiz;
                        velocity.Y = vertic -= Settings.gravity * 20 * vertic;
                        if (shipInfo.manevering)
                        {
                            ShipEngine(1, cos, cos1, sin, sin1);
                            ShipEngine(2, cos, cos1, sin, sin1);
                            ShipEngine(3, cos, cos1, sin, sin1);
                            ShipEngine(0, cos, cos1, sin, sin1);
                        }
                    }
                    else
                    {
                        float horiz = velocity.X;
                        float vertic = velocity.Y;
                        velocity.X = horiz -= Settings.gravity * horiz;
                        velocity.Y = vertic -= Settings.gravity * vertic;
                    }
                }
                if (core.inputManager.IsKeyDown(Keys.A) && shipInfo.manevering)
                {
                    Forcevelocity = new Vector2((float)Math.Cos(Rotation + 1.57F), (float)Math.Sin(Rotation + 1.57F)) / (mass + armorMass);
                    ShipEngine(1, cos, cos1, sin, sin1);
                }
                else if (core.inputManager.IsKeyDown(Keys.D) && shipInfo.manevering)
                {
                    Forcevelocity = new Vector2((float)Math.Cos(Rotation - 1.57F), (float)Math.Sin(Rotation - 1.57F)) / (mass + armorMass);
                    ShipEngine(2, cos, cos1, sin, sin1);
                }
            }
            else
            {
                RotateToVector(cursorPosition, Vector2.Zero);
                if (isLeftPress)
                {
                    if (gunSlots != null)
                    {
                        for (int i = 0; i < gunSlots.Length; i++)
                        {
                            if (gunSlots[i] != null)
                            {
                                if (gunSlots[i].Shoot())
                                {
                                    ServerPacketSender.SendGunShoot(i, shipName);
                                    break;
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
                                if (turretSlots[i].Shoot(false))
                                    break;
                            }
                        }
                    }*/
                }
                /*if (core.inputManager.IsRightButtonDown())
                {
                    /*if (rocketSlots != null)
                    {
                        for (int i = 0; i < rocketSlots.Length; i++)
                        {
                            if (rocketSlots[i] != null)
                            {
                                if (rocketSlots[i].Shoot())
                                    break;
                            }
                        }
                    }
                }*/
                if (isWPress)
                {
                    ShipAcceleration = new Vector2(-(float)Math.Cos(Rotation), -(float)Math.Sin(Rotation)) / (mass + armorMass);
                    direction = "Up";
                    //ServerPacketSender.SendShipEngine(this, 0);
                    ShipEngine(0, cos, cos1, sin, sin1);
                }
                else if (isSPress && shipInfo.manevering)
                {
                    ShipAcceleration = new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation)) / (mass + armorMass);
                    direction = "Down";
                    ServerPacketSender.SendShipEngine(this, 3);
                }
                else
                {
                    direction = "";
                    if (isXPress)
                    {
                        float horiz = velocity.X;
                        float vertic = velocity.Y;
                        velocity.X = horiz -= Settings.gravity * 20 * horiz;
                        velocity.Y = vertic -= Settings.gravity * 20 * vertic;
                        if (shipInfo.manevering)
                        {
                            for (byte i = 0; i < 4; i++)
                            {
                                ServerPacketSender.SendShipEngine(this, i);
                            }
                        }
                    }
                    else
                    {
                        float horiz = velocity.X;
                        float vertic = velocity.Y;
                        velocity.X = horiz -= Settings.gravity * horiz;
                        velocity.Y = vertic -= Settings.gravity * vertic;
                    }
                }
                if (isAPress && shipInfo.manevering)
                {
                    Forcevelocity = new Vector2((float)Math.Cos(Rotation + 1.57F), (float)Math.Sin(Rotation + 1.57F)) / (mass + armorMass);
                    ServerPacketSender.SendShipEngine(this, 1);
                }
                else if (isDPress && shipInfo.manevering)
                {
                    Forcevelocity = new Vector2((float)Math.Cos(Rotation - 1.57F), (float)Math.Sin(Rotation - 1.57F)) / (mass + armorMass);
                    ServerPacketSender.SendShipEngine(this, 2);
                }
            }
            if (ShipAcceleration != Vector2.Zero)
            {
                Vector2 newvelocity = velocity + ShipAcceleration;

                if (newvelocity.Length() > velocity.Length())
                {
                    double b;
                    if (direction == "Up")
                        b = 1 - velocity.LengthSquared() / (maxSpeedFace * maxSpeedFace);
                    else
                        b = 1 - velocity.LengthSquared() / (maxSpeedBack * maxSpeedBack);
                    if (b <= 0)
                    {
                        b = 0;
                    }

                    double lorentz_factor = 1 / Math.Sqrt(b);
                    ShipAcceleration.X /= (float)lorentz_factor;
                    ShipAcceleration.Y /= (float)lorentz_factor;
                }
                velocity += ShipAcceleration;

                if (velocity.Length() > 0)
                {
                    newvelocity.Normalize();
                    velocity = newvelocity * velocity.Length();
                }
            }
            if (Forcevelocity != Vector2.Zero)
            {
                Vector2 newvelocity = velocity + Forcevelocity;

                if (newvelocity.Length() > velocity.Length())
                {
                    double b = 1 - velocity.LengthSquared() / (maxSpeedForce * maxSpeedForce);
                    if (b <= 0)
                    {
                        b = 0;
                    }

                    double lorentz_factor = 1 / Math.Sqrt(b);
                    Forcevelocity.X /= (float)lorentz_factor;
                    Forcevelocity.Y /= (float)lorentz_factor;
                }
                velocity += Forcevelocity;

                if (velocity.Length() > 0)
                {
                    newvelocity.Normalize();
                    velocity = newvelocity * velocity.Length();
                }
            }
        }
    }
}
