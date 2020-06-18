using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleForSpaceResources.Networking
{
    public enum PacketType
    {
        Connect, Disconnect, Move, Deny, Register, MoveBullet, CreateBullet, LogRegMsg, Login, Refresh, OpenGui, ConnectNewPlayer, ConfrimSelect, SelectShip, GunShoot, ShipEngine,
        Respawn, DestroyShip, DestroyShield, Message, CreateNpcShip, MoveNpcs, CreateOldNpcShip
    }
}
