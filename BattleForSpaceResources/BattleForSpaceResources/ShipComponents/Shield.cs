using BattleForSpaceResources.Collision;
using BattleForSpaceResources.Entitys;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleForSpaceResources.ShipComponents
{
    public class Shield : CollisionObject
    {
        private Ship ship;
        private float shieldMaxSize;
        public Shield(Ship ship, World w)
            : base(ship.Position, w)
        {
            Size = 0;
            this.ship = ship;
            color = new Vector4(0.1F, 0.5F, 1F, 0.5F);
            Vector2[] v = new Vector2[2];
            body = new Body();
            body.shapes = new List<Poly>();
            #region small
            if (ship.shipType == ShipType.HumanSmall1)
            {
                if(world.isRemote)
                Create(Textures.shieldSmall[0]);
                shieldMaxSize = 1f;

                v = new Vector2[8];

                v[0] = new Vector2(-38f, -0f);
                v[1] = new Vector2(-26f, -26f);
                v[2] = new Vector2(0f, -38f);
                v[3] = new Vector2(26f, -26f);
                v[4] = new Vector2(38f, 0f);
                v[5] = new Vector2(26f, 26f);
                v[6] = new Vector2(0f, 38f);
                v[7] = new Vector2(-26f, 26f);

                Poly p = new Poly(body, v);
            }
            else if (ship.shipType == ShipType.EnemySmall1)
            {
                if (world.isRemote)
                Create(Textures.shieldSmall[0]);
                shieldMaxSize = 1.5f;
                float shieldRange = 1.4F;
                color = new Vector4(1F, 0.1F, 0.1F, 0.5F);
                v = new Vector2[8];

                v[0] = new Vector2(-38f * shieldRange, -0f);
                v[1] = new Vector2(-26f * shieldRange, -26f * shieldRange);
                v[2] = new Vector2(0f, -38f * shieldRange);
                v[3] = new Vector2(26f * shieldRange, -26f * shieldRange);
                v[4] = new Vector2(38f * shieldRange, 0f);
                v[5] = new Vector2(26f * shieldRange, 26f * shieldRange);
                v[6] = new Vector2(0f, 38f * shieldRange);
                v[7] = new Vector2(-26f * shieldRange, 26f * shieldRange);

                Poly p = new Poly(body, v);
            }
            else if(ship.shipType == ShipType.CivSmall1)
            {
                if (world.isRemote)
                Create(Textures.shieldSmall[0]);
                shieldMaxSize = 1.1f;
                float shieldRange = 1.0F;
                color = new Vector4(0.5f, 1f, 0.6f, 0.5f);
                v = new Vector2[8];

                v[0] = new Vector2(-38f * shieldRange, -0f);
                v[1] = new Vector2(-26f * shieldRange, -26f * shieldRange);
                v[2] = new Vector2(0f, -38f * shieldRange);
                v[3] = new Vector2(26f * shieldRange, -26f * shieldRange);
                v[4] = new Vector2(38f * shieldRange, 0f);
                v[5] = new Vector2(26f * shieldRange, 26f * shieldRange);
                v[6] = new Vector2(0f, 38f * shieldRange);
                v[7] = new Vector2(-26f * shieldRange, 26f * shieldRange);

                Poly p = new Poly(body, v);
            }
            #endregion
            #region medium
            if (ship.shipType == ShipType.CivMedium1)
            {
                if (world.isRemote)
                Create(Textures.shieldMedium[0]);
                color = new Vector4(0.2F, 1F, 0.4F, 0.5F);
                shieldMaxSize = 1f;
                v = new Vector2[8];
                float shieldRange = 120f;
                float[] shieldRange1 = new float[2];
                shieldRange1[0] = 85f;
                shieldRange1[1] = 65f;
                float shieldRange2 = 90f;
                v[0] = new Vector2(-shieldRange * shieldMaxSize, -0f);
                v[1] = new Vector2(-shieldRange1[0] * shieldMaxSize, -shieldRange1[1] * shieldMaxSize);
                v[2] = new Vector2(0f, -shieldRange2 * shieldMaxSize);
                v[3] = new Vector2(shieldRange1[0] * shieldMaxSize, -shieldRange1[1] * shieldMaxSize);
                v[4] = new Vector2(shieldRange * shieldMaxSize, 0f);
                v[5] = new Vector2(shieldRange1[0] * shieldMaxSize, shieldRange1[1] * shieldMaxSize);
                v[6] = new Vector2(0f, shieldRange2 * shieldMaxSize);
                v[7] = new Vector2(-shieldRange1[0] * shieldMaxSize, shieldRange1[1] * shieldMaxSize);

                Poly p = new Poly(body, v);
            }
            #endregion
            #region huge
            else if (ship.shipType == ShipType.HumanHuge1)
            {
                if (world.isRemote)
                Create(Textures.shieldHuge[0]);
                shieldMaxSize = 1.1F;

                v = new Vector2[8];

                v[0] = new Vector2(-380f, -0f);
                v[1] = new Vector2(-245f, -180f);
                v[2] = new Vector2(0f, -235f);
                v[3] = new Vector2(245f, -180f);
                v[4] = new Vector2(380f, 0f);
                v[5] = new Vector2(245f, 180f);
                v[6] = new Vector2(0f, 235f);
                v[7] = new Vector2(-245f, 180f);

                Poly p = new Poly(body, v);
            }
            #endregion
            #region mother
            else if (ship.shipType == ShipType.HumanMotherShip1)
            {
                if (world.isRemote)
                Create(Textures.shieldHuge[0]);
                shieldMaxSize = 1.25F;

                v = new Vector2[8];
                float shieldRange = 350f;
                float[] shieldRange1 = new float[2];
                shieldRange1[0] = 252f;
                shieldRange1[1] = 152f;
                float shieldRange2 = 220f;
                v[0] = new Vector2(-shieldRange * shieldMaxSize, -0f);
                v[1] = new Vector2(-shieldRange1[0] * shieldMaxSize, -shieldRange1[1] * shieldMaxSize);
                v[2] = new Vector2(0f, -shieldRange2 * shieldMaxSize);
                v[3] = new Vector2(shieldRange1[0] * shieldMaxSize, -shieldRange1[1] * shieldMaxSize);
                v[4] = new Vector2(shieldRange * shieldMaxSize, 0f);
                v[5] = new Vector2(shieldRange1[0] * shieldMaxSize, shieldRange1[1] * shieldMaxSize);
                v[6] = new Vector2(0f, shieldRange2 * shieldMaxSize);
                v[7] = new Vector2(-shieldRange1[0] * shieldMaxSize, shieldRange1[1] * shieldMaxSize);

                Poly p = new Poly(body, v);
            }
            else if (ship.shipType == ShipType.HumanMotherShip2)
            {
                if (world.isRemote)
                Create(Textures.shieldHuge[0]);
                shieldMaxSize = 2F;

                v = new Vector2[8];
                float shieldRange = 350f;
                float[] shieldRange1 = new float[2];
                shieldRange1[0] = 252f;
                shieldRange1[1] = 152f;
                float shieldRange2 = 220f;
                v[0] = new Vector2(-shieldRange * shieldMaxSize, -0f);
                v[1] = new Vector2(-shieldRange1[0] * shieldMaxSize, -shieldRange1[1] * shieldMaxSize);
                v[2] = new Vector2(0f, -shieldRange2 * shieldMaxSize);
                v[3] = new Vector2(shieldRange1[0] * shieldMaxSize, -shieldRange1[1] * shieldMaxSize);
                v[4] = new Vector2(shieldRange * shieldMaxSize, 0f);
                v[5] = new Vector2(shieldRange1[0] * shieldMaxSize, shieldRange1[1] * shieldMaxSize);
                v[6] = new Vector2(0f, shieldRange2 * shieldMaxSize);
                v[7] = new Vector2(-shieldRange1[0] * shieldMaxSize, shieldRange1[1] * shieldMaxSize);

                Poly p = new Poly(body, v);
            }
            else if (ship.shipType == ShipType.HumanMotherShip3)
            {
                if (world.isRemote)
                Create(Textures.shieldHuge[0]);
                shieldMaxSize = 2F;

                v = new Vector2[8];
                float shieldRange = 350f;
                float[] shieldRange1 = new float[2];
                shieldRange1[0] = 252f;
                shieldRange1[1] = 152f;
                float shieldRange2 = 220f;
                v[0] = new Vector2(-shieldRange * shieldMaxSize, -0f);
                v[1] = new Vector2(-shieldRange1[0] * shieldMaxSize, -shieldRange1[1] * shieldMaxSize);
                v[2] = new Vector2(0f, -shieldRange2 * shieldMaxSize);
                v[3] = new Vector2(shieldRange1[0] * shieldMaxSize, -shieldRange1[1] * shieldMaxSize);
                v[4] = new Vector2(shieldRange * shieldMaxSize, 0f);
                v[5] = new Vector2(shieldRange1[0] * shieldMaxSize, shieldRange1[1] * shieldMaxSize);
                v[6] = new Vector2(0f, shieldRange2 * shieldMaxSize);
                v[7] = new Vector2(-shieldRange1[0] * shieldMaxSize, shieldRange1[1] * shieldMaxSize);

                Poly p = new Poly(body, v);
            }
            else if (ship.shipType == ShipType.EnemyMotherShip)
            {
                if (world.isRemote)
                Create(Textures.shieldHuge[0]);
                shieldMaxSize = 1.6F;
                color = new Vector4(1F, 0.1F, 0.1F, 0.5F);
                v = new Vector2[8];
                float shieldRange = 360f;
                float[] shieldRange1 = new float[2];
                shieldRange1[0] = 256f;
                shieldRange1[1] = 156f;
                float shieldRange2 = 220f;
                v[0] = new Vector2(-shieldRange * shieldMaxSize, -0f);
                v[1] = new Vector2(-shieldRange1[0] * shieldMaxSize, -shieldRange1[1] * shieldMaxSize);
                v[2] = new Vector2(0f, -shieldRange2 * shieldMaxSize);
                v[3] = new Vector2(shieldRange1[0] * shieldMaxSize, -shieldRange1[1] * shieldMaxSize);
                v[4] = new Vector2(shieldRange * shieldMaxSize, 0f);
                v[5] = new Vector2(shieldRange1[0] * shieldMaxSize, shieldRange1[1] * shieldMaxSize);
                v[6] = new Vector2(0f, shieldRange2 * shieldMaxSize);
                v[7] = new Vector2(-shieldRange1[0] * shieldMaxSize, shieldRange1[1] * shieldMaxSize);

                Poly p = new Poly(body, v);
            }
            else if (ship.shipType == ShipType.CivMotherShip1)
            {
                if (world.isRemote)
                Create(Textures.shieldHuge[0]);
                shieldMaxSize = 1.8F;
                color = new Vector4(0.2F, 1F, 0.4F, 0.5F);
                v = new Vector2[8];
                float shieldRange = 350f;
                float[] shieldRange1 = new float[2];
                shieldRange1[0] = 246f;
                shieldRange1[1] = 146f;
                float shieldRange2 = 215f;
                v[0] = new Vector2(-shieldRange * shieldMaxSize, -0f);
                v[1] = new Vector2(-shieldRange1[0] * shieldMaxSize, -shieldRange1[1] * shieldMaxSize);
                v[2] = new Vector2(0f, -shieldRange2 * shieldMaxSize);
                v[3] = new Vector2(shieldRange1[0] * shieldMaxSize, -shieldRange1[1] * shieldMaxSize);
                v[4] = new Vector2(shieldRange * shieldMaxSize, 0f);
                v[5] = new Vector2(shieldRange1[0] * shieldMaxSize, shieldRange1[1] * shieldMaxSize);
                v[6] = new Vector2(0f, shieldRange2 * shieldMaxSize);
                v[7] = new Vector2(-shieldRange1[0] * shieldMaxSize, shieldRange1[1] * shieldMaxSize);

                Poly p = new Poly(body, v);
            }
            #endregion
            #region station
            else if (ship.shipType == ShipType.HumanLargeStation1)
            {
                if (world.isRemote)
                Create(Textures.shieldStation[0]);
                shieldMaxSize = 2.5F;

                v = new Vector2[8];
                float shieldRange = 285f;
                float shieldRange1 = 200f;
                v[0] = new Vector2(-shieldRange * shieldMaxSize, -0f);
                v[1] = new Vector2(-shieldRange1 * shieldMaxSize, -shieldRange1 * shieldMaxSize);
                v[2] = new Vector2(0f, -shieldRange * shieldMaxSize);
                v[3] = new Vector2(shieldRange1 * shieldMaxSize, -shieldRange1 * shieldMaxSize);
                v[4] = new Vector2(shieldRange * shieldMaxSize, 0f);
                v[5] = new Vector2(shieldRange1 * shieldMaxSize, shieldRange1 * shieldMaxSize);
                v[6] = new Vector2(0f, shieldRange * shieldMaxSize);
                v[7] = new Vector2(-shieldRange1 * shieldMaxSize, shieldRange1 * shieldMaxSize);

                Poly p = new Poly(body, v);
            }
            #endregion
        }
        public override void Update()
        {
            Rotation = ship.Rotation;
            Position = ship.Position;
            if (Size < shieldMaxSize)
            {
                Size += 0.05F;
            }
            base.Update();
        }
        public override void Render(SpriteBatch spriteBatch)
        {
            base.Render(spriteBatch);
        }
    }
}
