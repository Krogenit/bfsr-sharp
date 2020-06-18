using BattleForSpaceResources.Collision;
using BattleForSpaceResources.Entitys.BFSRSystem.Helpers;
using BattleForSpaceResources.Guis;
using BattleForSpaceResources.Networking;
using BattleForSpaceResources.Particles;
using BattleForSpaceResources.ShipComponents;
using Krypton;
using Krypton.Lights;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleForSpaceResources.Entitys
{
    public enum Faction
    {
        Human = 1, Enemy = 2, Civilian = 3
    }
    public enum ShipType
    {
        HumanSmall1 = 0,
        HumanHuge1 = 1,
        HumanLargeStation1 = 2,
        HumanMotherShip1 = 3,
        HumanMotherShip2 = 4,
        HumanMotherShip3 = 5,
        EnemyMotherShip = 6,
        EnemySmall1 = 7,
        CivMedium1 = 8,
        CivMotherShip1 = 9,
        CivSmall1 = 10
    }
    public struct ShipInfo
    {
        public float mapIconSize;
        public float jumpSize;
        public float removeShieldSize;
        public int destroyTime;
        public bool moreRotForMove;
        public int constructionTime;
        public int minDis, maxDis;
        public bool manevering;
        public bool bigExplosion;
        public ExplosionType exType;
        public int waveSize;
        public float iconSize;
        public float selectedSize;
        public enum ExplosionType
        {
            Small = 0, Medium = 1, Large = 2, Huge = 3, Mother = 4, Station = 5
        }
        /// <summary>
        /// 1. Размер иконки на миникарте.
        /// 2. Размер эффекта гиперпрыжка.
        /// 3. Размер эффекта отключения щита.
        /// 4. Время уничтожения.
        /// 5. Больше вращения перед стрельбой.
        /// 6. Время строительства.
        /// 7. Мнимальная дистанция для стрельбы.
        /// 8. Максимальная дистанция стрельбы.
        /// 9. Маневренный корабль?
        /// 10. Больше взрывов!
        /// 11. Тип взрыва.
        /// 12. Размер волны.
        /// 13. Размер иконки справа.
        /// 14. Размер иконки в списке выбранных.
        /// </summary>
        public ShipInfo(float mapIconSize, float jumpSize, float removeShieldSize, int destroyTime,
            bool moreRotForMove, int constructionTime, int minDis, int maxDis, bool manevering,
            bool bigExplosion, ExplosionType exType, int waveSize, float iconSize, float selectedSize)
        {
            this.mapIconSize = mapIconSize;
            this.jumpSize = jumpSize;
            this.removeShieldSize = removeShieldSize;
            this.destroyTime = destroyTime;
            this.moreRotForMove = moreRotForMove;
            this.constructionTime = constructionTime;
            this.maxDis = maxDis;
            this.minDis = minDis;
            this.manevering = manevering;
            this.bigExplosion = bigExplosion;
            this.exType = exType;
            this.waveSize = waveSize;
            this.iconSize = iconSize;
            this.selectedSize = selectedSize;
        }
    }
    public enum AiDirection
    {
        Face = 1, Back = 2, Left = 3, Right = 4, Stop = 5, Gravity = 6
    }
    public enum AiAgressiveType
    {
        Nothing = 0, Defend = 1, Attack = 2, Mining = 3
    }
    public enum AiSearchType
    {
        Around = 0, AllMap = 1
    }
    public class Ship : CollisionObject
    {
        public Faction shipFaction;
        public ShipType shipType;
        public AiSearchType aiSeachType;
        public AiAgressiveType aiAgrType;
        public Vector2 mass, armorMass, randomJumpAngle, moveToVector, runnigOutVector, basePosition, addBasePosition, cursorPosition;
        public Vector2[] gunPos, dmgPos;
        public AiDirection aiDir;
        public ShipInfo shipInfo;
        public Vector4 damageColor;
        public float maxSpeedFace, maxSpeedBack, maxSpeedForce, rotationToObject,
            shield, maxShield, shieldRegen, defaultSpeed, defaultRotSpeed, defaultEnergy,
            defaultEnergyRegen, defaultShield, defaultShieldRegen, defaultArmor, defaultArmorRegen,
            hull, maxHull, hullRegen, energy, maxEnergy, energyRegen, minDis, maxDis, bulletSpeed,
            minHullRunOut = 0.25F, sin, cos, sin1, cos1;
        public int rotateDifference, shieldMaxTimer, shieldTimer, jumpTimer = 25, stopTimer, cargoSize, maxCargoSize,
            crewSize, maxCrewSize, directionTimer, destroingTimer, hangarNum, timeToDestroy, runningOutTimer,
            reactorType, engineType, shieldType, changeBasePosTimer, movingTime, timeOut, jumpX, jumpY, id;
        public bool isJumping = true, isControled, isSelected, isStation, isWPress, isSPress, isAPress, isDPress, isXPress,
            isLeftPress, isRightPress;
        public List<Particle> particles = new List<Particle>();
        public ShipDamage[] shipDamages;
        public Shield shieldBase;
        public GunSlot[] gunSlots;
        //public RocketSlot[] rocketSlots;
        public ArmorPlate[] armorPlates = new ArmorPlate[4];
        //public TurretSlot[] turretSlots;
        //public DronsSlot[] dronSlots;
        public CollisionObject target;
        private Ship lastAttackShip;
        private bool isDebugMode = false;
        public string shipName;
        public Ship(Vector2 pos, Vector2 bp, Faction faction, ShipType type, int[] components, World w, AiSearchType ast, AiAgressiveType agr)
            : base(pos, w)
        {
            shipType = type;
            shipFaction = faction;
            aiSeachType = ast;
            aiAgrType = agr;
            basePosition = bp;
            jumpX = core.random.Next(2) == 0 ? 750 : -750;
            jumpY = core.random.Next(2) == 0 ? 750 : -750;
            CreateShipBase(components);
        }
        private void CreateShipBase(int[] components)
        {
            #region smallShips
            if (shipType == ShipType.HumanSmall1)
            {
                if (world.isRemote)
                    Create(Textures.shipHumanSmall1);
                mass = new Vector2(5, 5);
                maxHull = hull = 25;
                maxCargoSize = 2;
                maxCrewSize = 4;
                hullRegen = 0.0025F;
                defaultSpeed = 4;
                defaultRotSpeed = 0.02F;
                defaultEnergy = 30;
                defaultEnergyRegen = 0.15F;
                defaultShield = 15;
                defaultShieldRegen = 0.01F;
                defaultArmor = 20;
                defaultArmorRegen = 0.005F;
                gunSlots = new GunSlot[2];
                gunPos = new Vector2[] {new Vector2(26,10), new Vector2(-26,10)};
                if (world.isRemote)
                {
                    dmgPos = new Vector2[]{new Vector2(20,-5), new Vector2(0,0), new Vector2(0,20)};
                    shipDamages = new ShipDamage[3];
                    shipDamages[0] = new ShipDamage(this,0, 0.75F, Position);
                    shipDamages[1] = new ShipDamage(this, 1, 0.25F, Position);
                    shipDamages[2] = new ShipDamage(this, 0, 0.5F, Position);
                }
                shipInfo = new ShipInfo(0.25f, 1.2F, 0.5F, 10, false, 250, 400, 500, true, false, ShipInfo.ExplosionType.Small, 150, 1F, 1F);
            }
            else if (shipType == ShipType.EnemySmall1)
            {
                if (world.isRemote)
                    Create(Textures.shipEnemySmall1);
                mass = new Vector2(6, 6);
                maxHull = hull = 25;
                maxCargoSize = 3;
                maxCrewSize = 3;
                hullRegen = 0.0025F;
                defaultSpeed = 4;
                defaultRotSpeed = 0.02F;
                defaultEnergy = 30;
                defaultEnergyRegen = 0.15F;
                defaultShield = 15;
                defaultShieldRegen = 0.01F;
                defaultArmor = 20;
                defaultArmorRegen = 0.005F;
                gunSlots = new GunSlot[2];
                gunPos = new Vector2[] { new Vector2(26, 15), new Vector2(-26, 15) };
                if (world.isRemote)
                {
                    dmgPos = new Vector2[] { new Vector2(25, 0), new Vector2(0, 5), new Vector2(0, -25) };
                    shipDamages = new ShipDamage[3];
                    shipDamages[0] = new ShipDamage(this,0, 0.75F, Position);
                    shipDamages[1] = new ShipDamage(this,1, 0.25F, Position);
                    shipDamages[2] = new ShipDamage(this,0, 0.5F, Position);
                }
                shipInfo = new ShipInfo(0.2f, 1.2F, 0.5F, 10, false, 250, 400, 500, true, false, ShipInfo.ExplosionType.Small, 150, 1F, 1F);
            }
            else if (shipType == ShipType.CivSmall1)
            {
                if (world.isRemote)
                    Create(Textures.shipCivSmall1);
                mass = new Vector2(4, 4);
                maxHull = hull = 25;
                maxCargoSize = 4;
                maxCrewSize = 2;
                hullRegen = 0.0025F;
                defaultSpeed = 4;
                defaultRotSpeed = 0.02F;
                defaultEnergy = 30;
                defaultEnergyRegen = 0.15F;
                defaultShield = 15;
                defaultShieldRegen = 0.01F;
                defaultArmor = 20;
                defaultArmorRegen = 0.005F;
                gunSlots = new GunSlot[2];
                gunPos = new Vector2[] { new Vector2(15, 10), new Vector2(-15, 10) };
                if (world.isRemote)
                {
                    dmgPos = new Vector2[]{new Vector2(-7,20), new Vector2(0,0), new Vector2(12,-20)};
                    shipDamages = new ShipDamage[3];
                    shipDamages[0] = new ShipDamage(this,0, 0.75F, Position);
                    shipDamages[1] = new ShipDamage(this, 1, 0.25F, Position);
                    shipDamages[2] = new ShipDamage(this, 0, 0.5F, Position);
                }
                shipInfo = new ShipInfo(0.25f, 1.2F, 0.5F, 10, false, 250, 400, 500, true, false, ShipInfo.ExplosionType.Small, 150, 1F, 1F);
            }
            #endregion
            float[] f = CreateEngine(components[1], this);
            engineType = components[1];
            maxSpeedFace = f[0];
            maxSpeedBack = f[1];
            maxSpeedForce = f[2];
            rotationSpeed = f[3];
            f = CreateReactor(components[0], this);
            reactorType = components[0];
            maxEnergy = energy = f[0];
            energyRegen = f[1];
            f = CreateShield(components[2], this);
            shieldMaxTimer = (int)f[2];
            shieldType = components[2];
            shield = maxShield = f[0];
            shieldRegen = f[1];
            for (int i = 0; i < 4; i++)
            {
                f = CreateArmor(components[i + 3], this);
                armorPlates[i] = new ArmorPlate(components[i + 3], f[0], f[1]);
                armorMass += new Vector2(f[2], f[2]);
            }
            BuildPolygons();
            shieldBase = new Shield(this, world);
        }
        private void BuildPolygons()
        {
            Vector2[] v = new Vector2[4];
            #region small
            if (shipType == ShipType.HumanSmall1)
            {
                v = new Vector2[4];
                v[0] = new Vector2(-25f, -0f);
                v[1] = new Vector2(0f, -27f);
                v[2] = new Vector2(25f, 0f);
                v[3] = new Vector2(0f, 27f);
                Poly p = new Poly(body, v);
            }
            else if (shipType == ShipType.EnemySmall1)
            {
                v = new Vector2[4];
                v[0] = new Vector2(-50f, -10f);
                v[1] = new Vector2(-10f, -10f);
                v[2] = new Vector2(-10f, 10f);
                v[3] = new Vector2(-50f, 10f);
                Poly p = new Poly(body, v);

                v = new Vector2[4];
                v[0] = new Vector2(-10f, -30f);
                v[1] = new Vector2(10f, -30f);
                v[2] = new Vector2(10f, 30f);
                v[3] = new Vector2(-10f, 30f);
                p = new Poly(body, v);

                v = new Vector2[4];
                v[0] = new Vector2(10f, -17f);
                v[1] = new Vector2(40f, -17f);
                v[2] = new Vector2(40f, 17f);
                v[3] = new Vector2(10f, 17f);
                p = new Poly(body, v);
            }
            else if (shipType == ShipType.CivSmall1)
            {
                v = new Vector2[6];
                v[0] = new Vector2(-32f, -0f);
                v[1] = new Vector2(-9f, -20f);
                v[2] = new Vector2(20f, -17f);
                v[3] = new Vector2(32f, 0f);
                v[4] = new Vector2(20f, 17f);
                v[5] = new Vector2(-9f, 20f);
                Poly p = new Poly(body, v);
            }
            #endregion
            #region civmed1
            else if (shipType == ShipType.CivMedium1)
            {
                v = new Vector2[4];
                v[0] = new Vector2(-90f, -18f);
                v[1] = new Vector2(7f, -27f);
                v[2] = new Vector2(7f, 27f);
                v[3] = new Vector2(-90f, 18f);
                Poly p = new Poly(body, v);

                v = new Vector2[5];
                v[0] = new Vector2(21f, -77f);
                v[1] = new Vector2(43f, -80f);
                v[2] = new Vector2(43f, 80f);
                v[3] = new Vector2(21f, 77f);
                v[4] = new Vector2(0f, 0f);
                p = new Poly(body, v);

                v = new Vector2[4];
                v[0] = new Vector2(43f, -58f);
                v[1] = new Vector2(92f, -12f);
                v[2] = new Vector2(92f, 12f);
                v[3] = new Vector2(43f, 58f);
                p = new Poly(body, v);
            }
            #endregion
            #region humhuge1
            else if (shipType == ShipType.HumanHuge1)
            {
                v = new Vector2[4];
                v[0] = new Vector2(-312f, -51f);
                v[1] = new Vector2(-134f, -51f);
                v[2] = new Vector2(-134f, 51f);
                v[3] = new Vector2(-312f, 51f);
                Poly p = new Poly(body, v);
                v = new Vector2[4];
                v[0] = new Vector2(-287f, -110f);
                v[1] = new Vector2(-252f, -110f);
                v[2] = new Vector2(-252f, -51f);
                v[3] = new Vector2(-312f, -51f);
                p = new Poly(body, v);

                v = new Vector2[4];
                v[0] = new Vector2(-287f, 110f);
                v[1] = new Vector2(-252f, 110f);
                v[2] = new Vector2(-252f, 51f);
                v[3] = new Vector2(-312f, 51f);
                p = new Poly(body, v);

                v = new Vector2[6];
                v[0] = new Vector2(-134f, -165f);
                v[1] = new Vector2(-66f, -210f);
                v[2] = new Vector2(80f, -210f);
                v[3] = new Vector2(80f, 210f);
                v[4] = new Vector2(-66f, 210f);
                v[5] = new Vector2(-134f, 165f);

                p = new Poly(body, v);
                v = new Vector2[4];
                v[0] = new Vector2(80f, -140f);
                v[1] = new Vector2(210f, -140f);
                v[2] = new Vector2(210f, 140f);
                v[3] = new Vector2(80f, 140f);
                p = new Poly(body, v);
                v = new Vector2[4];
                v[0] = new Vector2(210f, -75f);
                v[1] = new Vector2(320f, -75f);
                v[2] = new Vector2(320f, 75f);
                v[3] = new Vector2(210f, 75f);
                p = new Poly(body, v);
            }
            #endregion
            #region motherships
            else if (shipType == ShipType.HumanMotherShip1)
            {
                v = new Vector2[4];
                v[0] = new Vector2(-307f, -116f);
                v[1] = new Vector2(205f, -116f);
                v[2] = new Vector2(205f, 116f);
                v[3] = new Vector2(-307f, 116f);
                Poly p = new Poly(body, v);
                v = new Vector2[4];
                v[0] = new Vector2(205F, -80f);
                v[1] = new Vector2(265f, -80f);
                v[2] = new Vector2(265f, 80f);
                v[3] = new Vector2(205f, 80f);
                p = new Poly(body, v);
                v = new Vector2[4];
                v[0] = new Vector2(-140F, -170f);
                v[1] = new Vector2(-33f, -170f);
                v[2] = new Vector2(-33f, -116f);
                v[3] = new Vector2(-140f, -116f);
                p = new Poly(body, v);
                v = new Vector2[4];
                v[0] = new Vector2(-166F, -250f);
                v[1] = new Vector2(56f, -250f);
                v[2] = new Vector2(56f, -170f);
                v[3] = new Vector2(-166f, -170f);
                p = new Poly(body, v);
                v = new Vector2[4];
                v[0] = new Vector2(-140F, 170f);
                v[1] = new Vector2(-33f, 170f);
                v[2] = new Vector2(-33f, 116f);
                v[3] = new Vector2(-140f, 116f);
                p = new Poly(body, v);
                v = new Vector2[4];
                v[0] = new Vector2(-166F, 250f);
                v[1] = new Vector2(56f, 250f);
                v[2] = new Vector2(56f, 170f);
                v[3] = new Vector2(-166f, 170f);
                p = new Poly(body, v);
            }
            else if (shipType == ShipType.HumanMotherShip3)
            {
                v = new Vector2[4];
                v[0] = new Vector2(-428f, -175f);
                v[1] = new Vector2(475f, -175f);
                v[2] = new Vector2(475f, 175f);
                v[3] = new Vector2(-428f, 175f);
                Poly p = new Poly(body, v);
                v = new Vector2[4];
                v[0] = new Vector2(-562F, -28f);
                v[1] = new Vector2(-428f, -28f);
                v[2] = new Vector2(-428f, 28f);
                v[3] = new Vector2(-562f, 28f);
                p = new Poly(body, v);
                v = new Vector2[4];
                v[0] = new Vector2(79F, -336f);
                v[1] = new Vector2(212f, -336f);
                v[2] = new Vector2(212f, -175f);
                v[3] = new Vector2(79f, -175f);
                p = new Poly(body, v);
                v = new Vector2[4];
                v[0] = new Vector2(212F, -237f);
                v[1] = new Vector2(522f, -237f);
                v[2] = new Vector2(522f, -175f);
                v[3] = new Vector2(212f, -175f);
                p = new Poly(body, v);
                v = new Vector2[4];
                v[0] = new Vector2(475F, -53f);
                v[1] = new Vector2(532f, -53f);
                v[2] = new Vector2(532f, 53f);
                v[3] = new Vector2(475f, 53f);
                p = new Poly(body, v);
                v = new Vector2[4];
                v[0] = new Vector2(79F, 336f);
                v[1] = new Vector2(212f, 336f);
                v[2] = new Vector2(212f, 175f);
                v[3] = new Vector2(79f, 175f);
                p = new Poly(body, v);
                v = new Vector2[4];
                v[0] = new Vector2(212F, 237f);
                v[1] = new Vector2(522f, 237f);
                v[2] = new Vector2(522f, 175f);
                v[3] = new Vector2(212f, 175f);
                p = new Poly(body, v);
            }
            else if (shipType == ShipType.HumanLargeStation1)
            {
                v = new Vector2[4];
                v[0] = new Vector2(-285f, -445f);
                v[1] = new Vector2(225f, -445f);
                v[2] = new Vector2(225f, 445f);
                v[3] = new Vector2(-285f, 445f);
                Poly p = new Poly(body, v);

                v = new Vector2[4];
                v[0] = new Vector2(-490f, -445f);
                v[1] = new Vector2(-285f, -445f);
                v[2] = new Vector2(-285f, -320f);
                v[3] = new Vector2(-490f, -320f);
                p = new Poly(body, v);

                v = new Vector2[4];
                v[0] = new Vector2(225f, 325f);
                v[1] = new Vector2(490f, 325f);
                v[2] = new Vector2(490f, 445f);
                v[3] = new Vector2(225f, 445f);
                p = new Poly(body, v);
            }
            else if (shipType == ShipType.EnemyMotherShip)
            {
                v = new Vector2[4];
                v[0] = new Vector2(-124F, 294f);
                v[1] = new Vector2(250f, 294f);
                v[2] = new Vector2(250f, -294f);
                v[3] = new Vector2(-124F, -294f);
                Poly p = new Poly(body, v);

                v = new Vector2[3];
                v[0] = new Vector2(-445F, 0f);
                v[1] = new Vector2(-124f, 100f);
                v[2] = new Vector2(-124f, -100f);
                p = new Poly(body, v);

                v = new Vector2[4];
                v[0] = new Vector2(250f, 112f);
                v[1] = new Vector2(441f, 112f);
                v[2] = new Vector2(441f, -112f);
                v[3] = new Vector2(250F, -112f);
                p = new Poly(body, v);

                v = new Vector2[3];
                v[0] = new Vector2(-387f, 210f);
                v[1] = new Vector2(-124f, 294f);
                v[2] = new Vector2(-124f, 210f);
                p = new Poly(body, v);

                v = new Vector2[3];
                v[0] = new Vector2(-387f, -210f);
                v[1] = new Vector2(-124f, -294f);
                v[2] = new Vector2(-124f, -210f);
                p = new Poly(body, v);

                v = new Vector2[3];
                v[0] = new Vector2(250f, 294f);
                v[1] = new Vector2(428f, 215f);
                v[2] = new Vector2(250f, 215f);
                p = new Poly(body, v);

                v = new Vector2[3];
                v[0] = new Vector2(250f, -294f);
                v[1] = new Vector2(428f, -215f);
                v[2] = new Vector2(250f, -215f);
                p = new Poly(body, v);
            }
            else if (shipType == ShipType.CivMotherShip1)
            {
                v = new Vector2[5];
                v[0] = new Vector2(-520, -174);
                v[1] = new Vector2(-491, -208);
                v[2] = new Vector2(-338, -240);
                v[3] = new Vector2(-338, -205);
                v[4] = new Vector2(-500, -165);
                Poly p = new Poly(body, v);

                v = new Vector2[4];
                v[0] = new Vector2(-338, -240);
                v[1] = new Vector2(-164, -260);
                v[2] = new Vector2(-164, -208);
                v[3] = new Vector2(-338, -205);
                p = new Poly(body, v);

                v = new Vector2[4];
                v[0] = new Vector2(-164, -260);
                v[1] = new Vector2(57, -270);
                v[2] = new Vector2(80, -130);
                v[3] = new Vector2(-164, -208);
                p = new Poly(body, v);

                v = new Vector2[4];
                v[0] = new Vector2(57, -270);
                v[1] = new Vector2(275, -250);
                v[2] = new Vector2(275, 250);
                v[3] = new Vector2(57, 270);
                p = new Poly(body, v);

                v = new Vector2[4];
                v[0] = new Vector2(275, -250);
                v[2] = new Vector2(522, -108);
                v[1] = new Vector2(473, -166);
                v[3] = new Vector2(275, -36);
                p = new Poly(body, v);

                v = new Vector2[6];
                v[0] = new Vector2(-345, -17);
                v[1] = new Vector2(-192, -80);
                v[2] = new Vector2(-44, -84);
                v[3] = new Vector2(-44, 84);
                v[4] = new Vector2(-192, 80);
                v[5] = new Vector2(-345, 17);
                p = new Poly(body, v);

                v = new Vector2[4];
                v[0] = new Vector2(-44, -84);
                v[1] = new Vector2(57, -120);
                v[2] = new Vector2(57, 120);
                v[3] = new Vector2(-44, 84);
                p = new Poly(body, v);

                v = new Vector2[5];
                v[0] = new Vector2(-520, 174);
                v[1] = new Vector2(-491, 208);
                v[2] = new Vector2(-338, 240);
                v[3] = new Vector2(-338, 205);
                v[4] = new Vector2(-500, 165);
                p = new Poly(body, v);

                v = new Vector2[4];
                v[0] = new Vector2(-338, 240);
                v[1] = new Vector2(-164, 260);
                v[2] = new Vector2(-164, 208);
                v[3] = new Vector2(-338, 205);
                p = new Poly(body, v);

                v = new Vector2[4];
                v[0] = new Vector2(-164, 260);
                v[1] = new Vector2(57, 270);
                v[2] = new Vector2(80, 130);
                v[3] = new Vector2(-164, 208);
                p = new Poly(body, v);

                v = new Vector2[4];
                v[0] = new Vector2(275, 250);
                v[2] = new Vector2(522, 108);
                v[1] = new Vector2(473, 166);
                v[3] = new Vector2(275, 36);
                p = new Poly(body, v);
            }
            #endregion
        }
        public static float[] CreateEngine(int type, Ship s)
        {
            float[] f = new float[4];
            if (type == 1)
            {
                f[0] = s.defaultSpeed + ((s.defaultSpeed / 5F) * 2F);
                f[1] = (f[0] * 3F) / 5F;
                f[2] = (f[0] * 4F) / 5F;
                f[3] = s.defaultRotSpeed + ((s.defaultRotSpeed / 5F) * 2F);
            }
            else if (type == 2)
            {
                f[0] = s.defaultSpeed + ((s.defaultSpeed / 5F) * 3F);
                f[1] = (f[0] * 3F) / 5F;
                f[2] = (f[0] * 4F) / 5F;
                f[3] = s.defaultRotSpeed + ((s.defaultRotSpeed / 5F) * 3F);
            }
            else if (type == 3)
            {
                f[0] = s.defaultSpeed + ((s.defaultSpeed / 5F) * 5F);
                f[1] = (f[0] * 3) / 10F;
                f[2] = (f[0] * 4) / 10F;
                f[3] = s.defaultRotSpeed + ((s.defaultRotSpeed / 5F) * 3F);
            }
            else if (type == 4)
            {
                f[0] = s.defaultSpeed + ((s.defaultSpeed / 5F) * 5F);
                f[1] = (f[0] * 3) / 5F;
                f[2] = (f[0] * 4) / 5F;
                f[3] = s.defaultRotSpeed + ((s.defaultRotSpeed / 5F) * 5F);
            }
            return f;
        }
        public static float[] CreateReactor(int type, Ship s)
        {
            float[] f = new float[2];
            if (type == 1)
            {
                f[0] = s.defaultEnergy + ((s.defaultEnergy / 5F) * 2F);
                f[1] = s.defaultEnergyRegen + ((s.defaultEnergyRegen / 5F) * 2F);
            }
            else if (type == 2)
            {
                f[0] = s.defaultEnergy + ((s.defaultEnergy / 5F) * 3F);
                f[1] = s.defaultEnergyRegen + ((s.defaultEnergyRegen / 5F) * 3F);
            }
            else if (type == 3)
            {
                f[0] = s.defaultEnergy + ((s.defaultEnergy / 5F) * 4F);
                f[1] = s.defaultEnergyRegen + ((s.defaultEnergyRegen / 5F) * 4F);
            }
            else if (type == 4)
            {
                f[0] = s.defaultEnergy + ((s.defaultEnergy / 5F) * 5F);
                f[1] = s.defaultEnergyRegen + ((s.defaultEnergyRegen / 5F) * 5F);
            }
            return f;
        }
        public static float[] CreateShield(int type, Ship s)
        {
            float[] f = new float[3];
            if (type == 1)
            {
                f[0] = s.defaultShield + ((s.defaultShield / 5F) * 2F);
                f[1] = s.defaultShieldRegen + ((s.defaultShieldRegen / 5F) * 2F);
                f[2] = 200;
            }
            else if (type == 2)
            {
                f[0] = s.defaultShield + ((s.defaultShield / 5F) * 3F);
                f[1] = s.defaultShieldRegen + ((s.defaultShieldRegen / 5F) * 3F);
                f[2] = 200;
            }
            else if (type == 3)
            {
                f[0] = s.defaultShield + ((s.defaultShield / 5F) * 4F);
                f[1] = s.defaultShieldRegen + ((s.defaultShieldRegen / 5F) * 4F);
                f[2] = 200;
            }
            else if (type == 4)
            {
                f[0] = s.defaultShield + ((s.defaultShield / 5F) * 5F);
                f[1] = s.defaultShieldRegen + ((s.defaultShieldRegen / 5F) * 5F);
                f[2] = 200;
            }
            return f;
        }
        public static float[] CreateArmor(int type, Ship s)
        {
            float[] f = new float[3];
            if (type == 1)
            {
                f[0] = s.defaultArmor + ((s.defaultArmor / 5F) * 2F);
                f[1] = s.defaultArmorRegen + ((s.defaultArmorRegen / 5F) * 2F);
                f[2] = 0.5F;
            }
            else if (type == 2)
            {
                f[0] = s.defaultArmor + ((s.defaultArmor / 5F) * 3F);
                f[1] = s.defaultArmorRegen + ((s.defaultArmorRegen / 5F) * 3F);
                f[2] = 1F;
            }
            else if (type == 3)
            {
                f[0] = s.defaultArmor + ((s.defaultArmor / 5F) * 4F);
                f[1] = s.defaultArmorRegen + ((s.defaultArmorRegen / 5F) * 4F);
                f[2] = 2F;
            }
            else if (type == 4)
            {
                f[0] = s.defaultArmor + ((s.defaultArmor / 5F) * 5F);
                f[1] = s.defaultArmorRegen + ((s.defaultArmorRegen / 5F) * 5F);
                f[2] = 1.5F;
            }
            return f;
        }
        public void AddGuns(GunSlot[] guns)//, TurretSlot[] turrets, RocketSlot[] rockets, DronsSlot[] drons)
        {
            if (gunSlots != null && guns != null && guns.Length <= gunSlots.Length)
            {
                for (int i = 0; i < guns.Length; i++)
                {
                     if (guns[i] != null)
                        gunSlots[i] = guns[i];
                }
            }
            /*if (turretSlots != null && turrets != null)
            {
                for (int i = 0; i < turretSlots.Length; i++)
                {
                    if (turrets[i] != null)
                        turretSlots[i] = turrets[i];
                }
            }
            if (rocketSlots != null && rockets != null)
            {
                for (int i = 0; i < rocketSlots.Length; i++)
                {
                    if (rockets[i] != null)
                        rocketSlots[i] = rockets[i];
                }
            }
            if (dronSlots != null && drons != null)
            {
                for (int i = 0; i < dronSlots.Length; i++)
                {
                    if (drons[i] != null)
                        dronSlots[i] = drons[i];
                }
            }*/
        }
        public override void Update()
        {
            if (isJumping)
            {
                JumpIn();
            }
            else
            {
                UpdateNoJumping();
            }
            if (world.isRemote && world.locType == LocationType.Dark)
            {
                base.UpdateColor();
                Vector2 v = new Vector2(cos * -0 - cos1 * 0, sin * -0 - sin1 * 0);
                Light2D light = new Light2D()
                {
                    Texture = Textures.lightKrypton,
                    Range = 750,
                    Color = new Color(255, 255, 255),
                    //Intensity = (float)(this.mRandom.NextDouble() * 0.25 + 0.75),
                    Intensity = 1f,
                    Angle = Rotation,
                    X = Position.X + v.X,
                    Y = Position.Y + v.Y,
                };
                core.krypton.Lights.Add(light);
            }
        }
        public virtual void UpdateNoJumping()
        {
            if (timeToDestroy <= 0)
            {
                RegenShip();
                if (isDebugMode)
                {
                    //ShipEngine(0, cos, cos1, sin, sin1);
                    //ShipEngine(1, cos, cos1, sin, sin1);
                   // ShipEngine(2, cos, cos1, sin, sin1);
                    //ShipEngine(3, cos, cos1, sin, sin1);
                    Position = Vector2.Zero;
                    hull = 1;
                    //Rotation = MathHelper.ToRadians(90);
                    Rotation = 0;
                }
                else
                {
                    Ai();
                }
            }
            else
            {
                DestroingShip();
            }
            if (hull <= 0 && !isSetDestroingTimer)
            {
                SetDestroingTimer();
            }
            if (aiDir == 0)
            {
                float horiz = velocity.X;
                float vertic = velocity.Y;
                velocity.X = horiz -= Settings.gravity * horiz;
                velocity.Y = vertic -= Settings.gravity * vertic;
            }
            Position += velocity;
            UpdateShipComponents();
            UpdateParticles();
            base.Update();
            if (world.isRemote)
            {
                ClientNetwork net = ClientNetwork.GetClientNetwork();
                if (net.IsOnline() && core.GetPlayerName() != null && shipName.Equals(core.GetPlayerName()))
                    ClientPacketSender.SendShip(this);
            }
        }
        public virtual void Ai() { }
        private void UpdateShipComponents()
        {
            float procent = hull / maxHull;
            shieldBase.Update();
            if (gunSlots != null)
            {
                for (int i = 0; i < gunSlots.Length; i++)
                {
                    if (gunSlots[i] != null)
                    {
                        gunSlots[i].Update(cos1 * gunPos[i].X, cos * gunPos[i].Y, sin1 * gunPos[i].X, sin * gunPos[i].Y);
                    }
                }
            }
            if (world.isRemote)
            {
                for (int i = 0; i < shipDamages.Length; i++)
                {
                    shipDamages[i].Update(cos1 * dmgPos[i].X, cos * dmgPos[i].Y, sin1 * dmgPos[i].X, sin * dmgPos[i].Y);
                }
            }
        }
        private void RegenShip()
        {
            if (shield < maxShield && shield > 0)
            {
                shield += shieldRegen;
            }
            else if (shield > maxShield)
            {
                shield = maxShield;
            }
            if (shield < 0)
            {
                shield = 0;
            }
            if (hull < maxHull)
            {
                hull += (hullRegen + (crewSize / 10000f));
            }
            else if (hull > maxHull)
            {
                hull = maxHull;
            }
            if (hull < 0)
            {
                hull = 0;
            }
            if (armorPlates != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (armorPlates[i] != null)
                    {
                        if (armorPlates[i].armor < armorPlates[i].maxArmor)
                        {
                            armorPlates[i].armor += armorPlates[i].armorRegen;
                        }
                        else if (armorPlates[i].armor > armorPlates[i].maxArmor)
                        {
                            armorPlates[i].armor = armorPlates[i].maxArmor;
                        }
                    }
                }
            }
            if (shieldTimer > 0)
                shieldTimer -= 1;
            if (shield <= 0 && shieldTimer == 0)
            {
                shield = maxShield / 5F;
            }
            if (energy < maxEnergy)
            {
                energy += energyRegen;
            }
            else if (energy > maxEnergy)
            {
                energy = maxEnergy;
            }
            if (energy < 0)
            {
                energy = 0;
            }
        }
        private void JumpIn()
        {
            if (jumpTimer == 25)
            {
                if (world.isRemote)
                    Rotation = core.ps.Jumping(Position, shipInfo.jumpSize, jumpX, jumpY);
                else
                {
                    float angle = 0;
                    Vector2 spawnposition = new Vector2(Position.X + jumpX, Position.Y + jumpY);
                    var mDx = (spawnposition.X) - Position.X;
                    var mDy = (spawnposition.Y) - Position.Y;
                    angle = (float)Math.Atan2(mDy, mDx);
                    if (MathHelper.ToDegrees(angle) == -45)
                        angle = MathHelper.ToRadians(315);
                    else if (MathHelper.ToDegrees(angle) == -135)
                        angle = MathHelper.ToRadians(225);
                    Rotation = angle;
                }
                --jumpTimer;
            }
            else if (--jumpTimer <= 0)
            {
                isJumping = false;
                if (world.isRemote)
                {
                    core.ps.Jump(Position, shipInfo.jumpSize);
                    core.ps.Lighting(Position, shipInfo.jumpSize * 2, new Vector4(0.5F, 0.5F, 1, 0), .05F);
                    Sounds.PlayJumpSound(Position);
                }
                velocity = new Vector2(-(float)Math.Cos(Rotation) * maxSpeedFace * 2, -(float)Math.Sin(Rotation) * maxSpeedFace * 2) / (mass + armorMass);
            }
        }
        private void SetDestroingTimer()
        {
            timeToDestroy = shipInfo.destroyTime;
            if (shipInfo.bigExplosion)
                world.destroingShips.Add(this);
            isSetDestroingTimer = true;
        }
        private void DestroingShip()
        {
            if (--timeToDestroy > 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (crewSize > 1)
                    {
                        int rand = core.random.Next(2);
                        if (rand == 0)
                        {
                            //CreatePod();
                        }
                        else
                        {
                            crewSize -= 2;
                        }
                    }
                }
                Rotation += rotationSpeed / 2F;
                Vector2 randomVector1 = Vector2.Zero;
                if (--destroingTimer <= 0)
                {
                    if (world.isRemote)
                    {
                        if (shipInfo.exType == ShipInfo.ExplosionType.Large || shipInfo.exType == ShipInfo.ExplosionType.Huge ||
                            shipInfo.exType == ShipInfo.ExplosionType.Mother || shipInfo.exType == ShipInfo.ExplosionType.Station)
                        {
                            randomVector1 = new Vector2(Position.X + core.random.Next(-100, 100), Position.Y + core.random.Next(-100, 100));
                            core.ps.ShipOst(randomVector1, this);
                            core.ps.ShipDerbis(2, 0, 0.05f + (float)core.random.NextDouble() / 4, randomVector1, this);
                            core.ps.ShipDamage(4, randomVector1, this, 0.25F);
                            core.ps.Explosion(randomVector1, 0.75F);
                            core.ps.DestroyShipEffect(randomVector1, 1.5F, .04f, new Vector4(1.0f, 0.5f, 0.0f, 0.0f));
                            core.ps.Light(randomVector1, 7F, new Vector4(1.0f, 0.5f, 0.5f, 0.7f), .04f);
                            core.ps.ShipDamageDerbis(1, velocity, randomVector1);
                            core.ps.DamageSmoke(randomVector1, 2);
                            core.ps.DamageSmoke(randomVector1, 2);
                            core.ps.DamageSmoke(randomVector1, 2);
                            core.ps.ShipDerbis(2, 1, 0.75f + (float)core.random.NextDouble(), Position, this);
                            core.ps.ShipDerbis(1, 0, 0.75f + (float)core.random.NextDouble(), Position, this);
                        }
                        else if (shipInfo.exType == ShipInfo.ExplosionType.Small || shipInfo.exType == ShipInfo.ExplosionType.Medium)
                        {
                            randomVector1 = new Vector2(Position.X + core.random.Next(-10, 10), Position.Y + core.random.Next(-10, 10));
                            core.ps.ShipOst(randomVector1, this);
                            core.ps.ShipDerbis(core.random.Next(2), 0, 0.05f + (float)core.random.NextDouble() / 4, randomVector1, this);
                            core.ps.ShipDamage(4, randomVector1, this, 0.25F);
                            core.ps.Explosion(randomVector1, 0.125F);
                            core.ps.DestroyShipEffect(randomVector1, 0.5F, .04f, new Vector4(1.0f, 0.5f, 0.0f, 0.0f));
                            core.ps.Light(randomVector1, 5F, new Vector4(1.0f, 0.5f, 0.5f, 0.7f), .04f);
                            core.ps.ShipDamageDerbis(1, velocity, randomVector1);
                        }
                        Sounds.PlayDestroyingSound(Position);
                    }
                    destroingTimer = 20 + core.random.Next(0, 10);
                }
                if (world.isRemote && randomVector1 != Vector2.Zero)
                    core.ps.Explosion(randomVector1, 0.75F);
            }
            else
            {
                DestroyShip();
            }
        }
        private void DestroyShip()
        {
            if (isLive)
            {
                /*if (dronSlots != null)
                {
                    for (int i = 0; i < dronSlots.Length; i++)
                    {
                        if (dronSlots[i] != null)
                        {
                            dronSlots[i].DestroiDrons();
                        }
                    }
                }*/
                if (world.isRemote)
                {
                    if (shipInfo.exType == ShipInfo.ExplosionType.Small)
                    {
                        core.ps.Skockwave(2, Position, 0.2F);
                        core.ps.ShipDerbis(8, 0, 0.75f + (float)core.random.NextDouble(), Position, this);
                        core.ps.ShipDamageWrecks(2, velocity, Position);
                        core.ps.ShipDamageDerbis(4, velocity, Position);
                        core.ps.DestroyShipEffect(Position, 2, .03f, new Vector4(1.0f, 0.5f, 0.0f, 1.0f));
                        core.ps.Lighting(Position, 5, new Vector4(1.0f, 0.5f, 0.5f, 1.0f), .03f);
                        core.ps.RocketShoot(Position, 2.5F);
                    }
                    else if (shipInfo.exType == ShipInfo.ExplosionType.Medium)
                    {
                        core.ps.Skockwave(2, Position, 0.6F);
                        core.ps.ShipDerbis(8, 0, 0.75f + (float)core.random.NextDouble(), Position, this);
                        core.ps.ShipDerbis(3, 1, 0.55f + (float)core.random.NextDouble(), Position, this);
                        core.ps.ShipDamageWrecks(3, velocity, Position);
                        core.ps.ShipDamageDerbis(5, velocity, Position);
                        core.ps.DestroyShipEffect(Position, 3, .025f, new Vector4(1.0f, 0.5f, 0.0f, 1.0f));
                        core.ps.Lighting(Position, 12, new Vector4(1.0f, 0.5f, 0.5f, 1.0f), .025f);
                        core.ps.RocketShoot(Position, 5);
                    }
                    else if (shipInfo.exType == ShipInfo.ExplosionType.Huge)
                    {
                        core.ps.Skockwave(1, Position, 0.3F);
                        core.ps.Skockwave(2, Position, 0.3F);
                        core.ps.ShipDamageWrecks(4, velocity, Position);
                        core.ps.ShipDamageDerbis(8, velocity, Position);
                        core.ps.ShipDerbis(10, 1, 0.75f + (float)core.random.NextDouble(), Position, this);
                        core.ps.ShipDerbis(8, 0, 0.75f + (float)core.random.NextDouble(), Position, this);
                        core.ps.DestroyShipEffect(Position, 6, .01f, new Vector4(1.0f, 0.5f, 0.0f, 1.0f));
                        core.ps.Lighting(Position, 25, new Vector4(1.0f, 0.5f, 0.5f, 1.0f), .01f);
                        core.ps.ShipWrecks(shipType, velocity, Position, Rotation);
                        core.ps.RocketShoot(Position, 8);
                    }
                    else if (shipInfo.exType == ShipInfo.ExplosionType.Mother)
                    {
                        core.ps.Skockwave(1, Position, 0.75F);
                        core.ps.Skockwave(2, Position, 0.75F);
                        core.ps.ShipDamageWrecks(6, velocity, Position);
                        core.ps.ShipDamageDerbis(12, velocity, Position);
                        core.ps.ShipDerbis(14, 1, 0.75f + (float)core.random.NextDouble(), Position, this);
                        core.ps.ShipDerbis(8, 0, 0.75f + (float)core.random.NextDouble(), Position, this);
                        core.ps.DestroyShipEffect(Position, 7, .005f, new Vector4(1.0f, 0.5f, 0.0f, 1f));
                        core.ps.Lighting(Position, 30, new Vector4(1.0f, 0.5f, 0.0f, 1f), .005f);
                        core.ps.ShipWrecks(shipType, velocity, Position, Rotation);
                        core.ps.RocketShoot(Position, 10);
                    }
                    else if (shipInfo.exType == ShipInfo.ExplosionType.Station)
                    {
                        core.ps.Skockwave(1, Position, 1F);
                        core.ps.Skockwave(2, Position, 1F);
                        core.ps.ShipDamageWrecks(6, velocity, Position);
                        core.ps.ShipDamageDerbis(12, velocity, Position);
                        core.ps.ShipDerbis(14, 1, 0.75f + (float)core.random.NextDouble(), Position, this);
                        core.ps.ShipDerbis(8, 0, 0.75f + (float)core.random.NextDouble(), Position, this);
                        core.ps.DestroyShipEffect(Position, 10, .005f, new Vector4(1.0f, 0.5f, 0.0f, 1f));
                        core.ps.Lighting(Position, 50, new Vector4(1.0f, 0.5f, 0.0f, 1f), .005f);
                        core.ps.ShipWrecks(shipType, velocity, Position, Rotation);
                        core.ps.RocketShoot(Position, 15);
                    }
                    Sounds.PlayDestroySound(Position, shipInfo.bigExplosion);
                    for (int i = 0; i < world.ships.Count; i++)
                    {
                        var dis = Vector2.Distance(world.ships[i].Position, Position);
                        CreateWave(world.ships[i], (float)dis);//, null);
                    }
                }
                else
                {
                    for (int i = 0; i < world.ships.Count; i++)
                    {
                        var dis = Vector2.Distance(world.ships[i].Position, Position);
                        CreateWave(world.ships[i], (float)dis);//, null);
                    }
                    if (id != 0)
                    {
                        World w = ServerCore.GetServerCore().GetWorld();
                        w.MinusNpcId(id);
                    }
                }
                /*for (int i = 0; i < core.drons.Count; i++)
                {
                    dis = Vector2.Distance(core.drons[i].Position, Position);
                    CreateWave(null, (float)dis, core.drons[i]);
                }*/
            }
            isLive = false;
            if (world.isRemote)
            {
                if (shipInfo.bigExplosion)
                    world.destroingShips.Remove(this);
            }
            else
            {
                if (shipInfo.bigExplosion)
                    world.destroingShips.Remove(this);
            }
            //CreatePickups();
            if (world.isRemote)
            {
                if (shipName.Equals(core.GetPlayerName()))
                {
                    if (lastAttackShip != null && lastAttackShip.shipName != null && lastAttackShip.shipName.Length > 0)
                        core.currentGuiAdd = new GuiAddDestroyed(Language.GetString(StringName.Info), Language.GetString(StringName.YouHasBeenDestroyedBy) + " " + lastAttackShip.shipName);
                    else
                        core.currentGuiAdd = new GuiAddDestroyed(Language.GetString(StringName.Info), Language.GetString(StringName.YouHasBeenDestroyedBy));
                    core.cam.control = false;
                }
            }
            else
            {
                ServerPacketSender.SendDestroyShip(this);
            }
        }
        private void CreateWave(Ship go, float dis)//, //Dron dron)
        {
            if (dis <= shipInfo.waveSize)
            {
                //if (dron != null)
                    //dron.hull -= (shipInfo.waveSize - (float)dis) / 20F;
                //else
                    go.DamageShipWave(this, (shipInfo.waveSize - (float)dis) / 20F);
            }
        }
        private void DamageShipWave(CollisionObject go, float damage)
        {
            String side = SideDamageArmor(go);
            if (shield > 0)
            {
                shield -= damage*4;
            }
            if (hull > 0 && shield <= 0)
            {
                shieldTimer = shieldMaxTimer;
                if (world.isRemote)
                {
                    core.ps.ShipOst(Position, this);
                    core.ps.ShipDamage(core.random.Next(6), Position, this, 0.25F);
                }
                if (side == "Face")
                {
                    if (armorPlates[0] != null && armorPlates[0].armor > 0)
                    {
                        if (armorPlates[0].armorType == 1)
                        {
                            armorPlates[0].armor -= damage;
                            hull -= (damage * 9) / 10;
                        }
                    }
                    else
                    {
                        hull -= damage;
                    }
                }
                else if (side == "Front")
                {
                    if (armorPlates[0] != null && armorPlates[2].armor > 0)
                    {
                        if (armorPlates[2].armorType == 1)
                        {
                            armorPlates[2].armor -= damage;
                            hull -= (damage * 9) / 10;
                        }
                    }
                    else
                    {
                        hull -= damage;
                    }
                }
                else if (side == "Left")
                {
                    if (armorPlates[0] != null && armorPlates[3].armor > 0)
                    {
                        if (armorPlates[3].armorType == 1)
                        {
                            armorPlates[3].armor -= damage;
                            hull -= (damage * 9) / 10;
                        }
                    }
                    else
                    {
                        hull -= damage;
                    }
                }
                else if (side == "Right")
                {
                    if (armorPlates[0] != null && armorPlates[1].armor > 0)
                    {
                        if (armorPlates[1].armorType == 1)
                        {
                            armorPlates[1].armor -= damage;
                            hull -= (damage * 9) / 10;
                        }
                    }
                    else
                    {
                        hull -= damage;
                    }
                }
            }
        }
        public void MoveToVector(Vector2 vector, Vector2 velocityNew)
        {
            RotateToVector(vector, velocityNew);
            Vector2 ShipAcceleration = Vector2.Zero;
            Vector2 Forcevelocity = Vector2.Zero;
            var mDx = (vector.X) - Position.X;
            var mDy = (vector.Y) - Position.Y;
            var intRotationShip = Math.Abs(MathHelper.ToDegrees(Rotation));
            rotationToObject = (float)(Math.Atan2(mDy, mDx) / Math.PI * 180);
            rotationToObject = (rotationToObject < 0) ? rotationToObject + 360 : rotationToObject;
            var dis = Vector2.Distance(vector, Position);
            int[] minmax = GetRotationToMove();
            if (target != null)
            {
                if (dis > maxDis && Math.Abs(rotationToObject - intRotationShip) >= minmax[0] && Math.Abs(rotationToObject - intRotationShip) < minmax[1])
                {
                    aiDir = AiDirection.Face;
                    directionTimer = 0;
                }
                else if (shipInfo.manevering && aiAgrType != AiAgressiveType.Mining)
                {
                    if (dis < minDis && dis > 0)
                    {
                        aiDir = AiDirection.Back;
                        directionTimer = 0;
                    }
                    else
                    {
                        if (--directionTimer <= 0)
                        {
                            int rndechka = core.random.Next(3);
                            if (rndechka == 0)
                            {
                                aiDir = AiDirection.Left;
                                directionTimer = 500 + core.random.Next(0, 2000);
                            }
                            else if (rndechka == 1)
                            {
                                aiDir = AiDirection.Right;
                                directionTimer = 500 + core.random.Next(0, 2000);
                            }
                            else
                            {
                                aiDir = 0;
                                directionTimer = 250 + core.random.Next(0, 150);
                            }
                        }
                    }
                }
                else
                {
                    aiDir = 0;
                }
            }
            else
            {
                if (dis >= 150)
                {
                    if (Math.Abs(rotationToObject - intRotationShip) >= minmax[0] && Math.Abs(rotationToObject - intRotationShip) < minmax[1] && movingTime < 750)
                    {
                        aiDir = AiDirection.Face;
                        stopTimer = 100;
                        if (runnigOutVector == Vector2.Zero)
                            movingTime++;
                        if (movingTime >= 750)
                        {
                            stopTimer = 50;
                        }
                    }
                    else if (movingTime >= 750)
                    {
                        if (--stopTimer > 0)
                        {
                            aiDir = AiDirection.Stop;
                        }
                        else
                        {
                            movingTime = 0;
                        }
                    }
                    else
                    {
                        aiDir = 0;
                    }
                }
                else
                {
                    if (aiAgrType == AiAgressiveType.Mining)
                    {
                        moveToVector = Position + new Vector2(core.random.Next(-2500, 2500), core.random.Next(-2500, 2500));
                        movingTime = 0;
                        //SeachAsteroids();
                    }
                    else
                    {
                        if (--stopTimer >= 0)
                        {
                            aiDir = AiDirection.Stop;
                        }
                        else
                        {
                            aiDir = 0;
                            moveToVector = Vector2.Zero;
                            movingTime = 0;
                        }
                    }
                }
            }
            Vector2 cargoMass = new Vector2(cargoSize / 4f, cargoSize / 4f);
            if (aiDir == AiDirection.Face)
            {
                ShipAcceleration = new Vector2(-cos, -sin) / (mass + armorMass + cargoMass);
                //ShipEngine(0, x, x1, y, y1);
            }
            else if (aiDir == AiDirection.Back)
            {
                ShipAcceleration = new Vector2(cos, sin) / (mass + armorMass + cargoMass);
                //ShipEngine(3, x, x1, y, y1);
            }
            else if (aiDir == AiDirection.Left)
            {
                Forcevelocity = new Vector2(cos1, sin1) / (mass + armorMass + cargoMass);
                //ShipEngine(1, x, x1, y, y1);
            }
            else if (aiDir == AiDirection.Right)
            {
                Forcevelocity = new Vector2((float)Math.Cos(Rotation - 1.57F), (float)Math.Sin(Rotation - 1.57F)) / (mass + armorMass + cargoMass);
                //ShipEngine(2, x, x1, y, y1);
            }
            else if (aiDir == AiDirection.Stop)
            {
                float horiz = velocity.X;
                float vertic = velocity.Y;
                velocity.X = horiz -= Settings.gravity * 20 * horiz;
                velocity.Y = vertic -= Settings.gravity * 20 * vertic;
                //ShipEngine(3, x, x1, y, y1);
            }
            if (ShipAcceleration != Vector2.Zero)
            {
                Vector2 newvelocity = velocity + ShipAcceleration;

                if (newvelocity.Length() > velocity.Length())
                {
                    double b;
                    if (aiDir == AiDirection.Face)
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
        public void RotateToVector(Vector2 vector, Vector2 velocityNew)
        {
            float mDx = 0;
            float mDy = 0;
            var dis = Vector2.Distance(vector, Position);
            if (velocityNew == Vector2.Zero || bulletSpeed == 1)
            {
                mDx = (vector.X) - Position.X;
                mDy = (vector.Y) - Position.Y;
            }
            else
            {
                mDx = (float)(vector.X + ((velocityNew.X) * (dis / bulletSpeed))) - Position.X;
                mDy = (float)(vector.Y + ((velocityNew.Y) * (dis / bulletSpeed))) - Position.Y;
            }
            var intRotationShip = Math.Abs(MathHelper.ToDegrees(Rotation));

            rotationToObject = (float)(Math.Atan2(mDy, mDx) / Math.PI * 180);
            rotationToObject = (rotationToObject < 0) ? rotationToObject + 360 : rotationToObject;
            rotateDifference = (int)Math.Abs(rotationToObject - intRotationShip);

            if ((rotationToObject - intRotationShip >= 180 && rotationToObject - intRotationShip < 360) ||
                (rotationToObject - intRotationShip >= -180 && rotationToObject - intRotationShip < 0))
            {
                if (Math.Abs(rotationToObject - intRotationShip) >= 175 && Math.Abs(rotationToObject - intRotationShip) < 185)
                {
                    if (Math.Abs(rotationToObject - intRotationShip) >= 178 && Math.Abs(rotationToObject - intRotationShip) < 182)
                    {

                    }
                    else
                    {
                        Rotation += rotationSpeed / 4F;
                        //RotateTurrets(true, true);
                    }
                }
                else
                {
                    Rotation += rotationSpeed;
                    //RotateTurrets(true, false);
                }
            }
            else if ((rotationToObject - intRotationShip >= 0 && rotationToObject - intRotationShip < 180) ||
                (rotationToObject - intRotationShip >= -360 && rotationToObject - intRotationShip < -180))
            {
                if (Math.Abs(rotationToObject - intRotationShip) >= 175 && Math.Abs(rotationToObject - intRotationShip) < 185)
                {
                    if (Math.Abs(rotationToObject - intRotationShip) >= 178 && Math.Abs(rotationToObject - intRotationShip) < 182)
                    {

                    }
                    else
                    {
                        Rotation -= rotationSpeed / 4F;
                        //RotateTurrets(false, true);
                    }
                }
                else
                {
                    Rotation -= rotationSpeed;
                    //RotateTurrets(false, false);
                }
            }
            cos = (float)Math.Cos(Rotation);
            sin = (float)Math.Sin(Rotation);
            cos1 = (float)Math.Cos(Rotation + 1.57F);
            sin1 = (float)Math.Sin(Rotation + 1.57F);
        }
        private int[] GetRotationToMove()
        {
            int[] minmax = new int[2];
            minmax[0] = 135;
            minmax[1] = 225;
            if (shipInfo.moreRotForMove)
            {
                minmax[0] = 165;
                minmax[1] = 195;
            }
            else
            {

            }
            return minmax;
        }
        public void DamageShip(CollisionObject go)
        {
            if (go.GetType() == typeof(Bullet))
            {
                Bullet b = (Bullet)go;
                String side = SideDamageArmor(b);
                DamageShield(b.bulletDamageShield);
                DamageArmor(side, b.bulletDamageArmor, b.bulletDamageHull);
                if (hull > 0)
                    lastAttackShip = b.owner;
            }
            /*else if (go.GetType() == typeof(Rocket))
            {
                Rocket r = (Rocket)go;
                String side = SideDamageArmor(go);
                DamageShield(r.bulletDamageShield);
                DamageArmor(side, r.bulletDamageArmor, r.bulletDamageHull);
                if (hull > 0)
                    lastAttackShip = r.shooterObject;
            }
            /*else if (go.GetType() == typeof(Beam))
            {
                Beam b = (Beam)go;
                String side = SideDamageArmor(go);
                DamageShield(b.bulletDamageshieldBase);
                DamageArmor(side, b.bulletDamageArmor, b.bulletDamageHull);
                if (hull > 0)
                    lastAttackShip = b.shooterObject;
            }*/
            if (!world.isRemote)
                if (shield <= 0 && maxShield != 0)
                {
                    if (shieldTimer <= 0)
                    {
                        ServerPacketSender.SendDestroyShield(this);
                    }
                    shieldTimer = shieldMaxTimer;
                }
        }
        public void DestroyShield()
        {
            shield = -0.1f;
            if (shield <= 0 && maxShield != 0)
            {
                if (shieldTimer <= 0)
                {
                    core.ps.ShieldDown(Position, shipInfo.removeShieldSize);
                    Sounds.PlayShieldDownSound(Position);
                }
                shieldTimer = shieldMaxTimer;
            }
        }
        private String SideDamageArmor(CollisionObject go)
        {
            var mDx = (go.Position.X) - Position.X;
            var mDy = (go.Position.Y) - Position.Y;

            var intRotationShip = Math.Abs(MathHelper.ToDegrees(Rotation));

            rotationToObject = (float)(Math.Atan2(mDy, mDx) / Math.PI * 180);
            rotationToObject = (rotationToObject < 0) ? rotationToObject + 360 : rotationToObject;

            if ((rotationToObject - intRotationShip > -45 && rotationToObject - intRotationShip <= 45) ||
                (rotationToObject - intRotationShip > -360 && rotationToObject - intRotationShip <= -315) ||
                (rotationToObject - intRotationShip > 315 && rotationToObject - intRotationShip <= 360))
            {
                return "Front";
            }
            else if ((rotationToObject - intRotationShip > 45 && rotationToObject - intRotationShip <= 135) ||
                (rotationToObject - intRotationShip > -315 && rotationToObject - intRotationShip <= -225))
            {
                return "Left";
            }
            else if ((rotationToObject - intRotationShip > 135 && rotationToObject - intRotationShip <= 225) ||
                (rotationToObject - intRotationShip > -180 && rotationToObject - intRotationShip < -135) ||
                (rotationToObject - intRotationShip > -225 && rotationToObject - intRotationShip <= -180))
            {
                return "Face";
            }
            else if (rotationToObject - intRotationShip > 225 || rotationToObject - intRotationShip <= -45)
            {
                return "Right";
            }
            else
            {
                return null;
            }
        }
        private void DamageShield(float damage)
        {
            if (shield > 0)
            {
                shield -= damage;
            }
        }
        private void DamageArmor(String side, float damagearmor, float damagehull)
        {
            if (hull > 0 && shield <= 0)
            {
                if (armorPlates != null)
                {
                    if (side == "Face")
                    {
                        if (armorPlates[0] != null && armorPlates[0].armor > 0)
                        {
                            if (armorPlates[0].armorType == 1)
                            {
                                armorPlates[0].armor -= damagearmor;
                                hull -= ((damagehull) * 9F) / 10F;
                            }
                            else if (armorPlates[0].armorType == 2)
                            {
                                armorPlates[0].armor -= damagearmor;
                                hull -= ((damagehull) * 3F) / 4F;
                            }
                            else if (armorPlates[0].armorType == 3)
                            {
                                armorPlates[0].armor -= damagearmor;
                                hull -= (damagehull) / 2F;
                            }
                            else if (armorPlates[0].armorType == 4)
                            {
                                armorPlates[0].armor -= damagearmor;
                                hull -= (damagehull) / 4F;
                            }
                        }
                        else
                        {
                            hull -= damagehull;
                        }
                    }
                    else if (side == "Front")
                    {
                        if (armorPlates[2] != null && armorPlates[2].armor > 0)
                        {
                            if (armorPlates[2].armorType == 1)
                            {
                                armorPlates[2].armor -= damagearmor;
                                hull -= ((damagehull) * 9F) / 10F;
                            }
                            else if (armorPlates[2].armorType == 2)
                            {
                                armorPlates[2].armor -= damagearmor;
                                hull -= ((damagehull) * 3F) / 4F;
                            }
                            else if (armorPlates[2].armorType == 3)
                            {
                                armorPlates[2].armor -= damagearmor;
                                hull -= (damagehull) / 2F;
                            }
                            else if (armorPlates[2].armorType == 4)
                            {
                                armorPlates[2].armor -= damagearmor;
                                hull -= (damagehull) / 4F;
                            }
                        }
                        else
                        {
                            hull -= damagehull;
                        }
                    }
                    else if (side == "Left")
                    {
                        if (armorPlates[3] != null && armorPlates[3].armor > 0)
                        {
                            if (armorPlates[3].armorType == 1)
                            {
                                armorPlates[3].armor -= damagearmor;
                                hull -= ((damagehull) * 9F) / 10F;
                            }
                            else if (armorPlates[3].armorType == 2)
                            {
                                armorPlates[3].armor -= damagearmor;
                                hull -= ((damagehull) * 3F) / 4F;
                            }
                            else if (armorPlates[3].armorType == 3)
                            {
                                armorPlates[3].armor -= damagearmor;
                                hull -= (damagehull) / 2F;
                            }
                            else if (armorPlates[3].armorType == 4)
                            {
                                armorPlates[3].armor -= damagearmor;
                                hull -= (damagehull) / 4F;
                            }
                        }
                        else
                        {
                            hull -= damagehull;
                        }
                    }
                    else if (side == "Right")
                    {
                        if (armorPlates[1] != null && armorPlates[1].armor > 0)
                        {
                            if (armorPlates[1].armorType == 1)
                            {
                                armorPlates[1].armor -= damagearmor;
                                hull -= ((damagehull) * 9F) / 10F;
                            }
                            else if (armorPlates[1].armorType == 2)
                            {
                                armorPlates[1].armor -= damagearmor;
                                hull -= ((damagehull) * 3F) / 4F;
                            }
                            else if (armorPlates[1].armorType == 3)
                            {
                                armorPlates[1].armor -= damagearmor;
                                hull -= (damagehull) / 2F;
                            }
                            else if (armorPlates[1].armorType == 4)
                            {
                                armorPlates[1].armor -= damagearmor;
                                hull -= (damagehull) / 4F;
                            }
                        }
                        else
                        {
                            hull -= damagehull;
                        }
                    }
                }
                else
                {
                    hull -= damagehull;
                }
            }
        }
        private void UpdateParticles()
        {
            for (int particle = 0; particle < particles.Count; particle++)
            {
                particles[particle].Update();
                if (particles[particle].Size <= 0 || (particles[particle].alphaVelocity >= 0 && particles[particle].color.W <= 0.0001F))
                {
                    particles.RemoveAt(particle);
                    particle--;
                }
            }
        }
        public void ShipEngine(int dir, float x, float x1, float y, float y1)
        {
            if (shipType == ShipType.HumanSmall1)
            {
                if (dir == 0)
                {
                    EngineEmpuls(new Vector2(Position.X + x * 25, Position.Y + y * 25), this, 0.2F, 0.01F, new Vector4(0.5f, 0.5f, 1f, 1f));
                    Light(new Vector2(Position.X + x * 15, Position.Y + y * 15), 1.5F, new Vector4(0, 0, 0.5F, 0.5F), .2F);
                }
                else if (dir == 1)
                {
                    EngineAnother(new Vector2(Position.X - x1 * 30, Position.Y - y1 * 30));
                }
                else if (dir == 2)
                {
                    EngineAnother(new Vector2(Position.X + x1 * 30, Position.Y + y1 * 30));
                }
                else if (dir == 3)
                {
                    EngineAnother(new Vector2(Position.X - x * 30, Position.Y - y * 30));
                }
            }
            else if (shipType == ShipType.EnemySmall1)
            {
                if (dir == 0)
                {
                    EngineEmpuls(new Vector2(Position.X + x * 37, Position.Y + y * 37), this, 0.3F, 0.25F, new Vector4(1f, 0.4f, 0.2F, 1f));
                    Light(new Vector2(Position.X + x * 30, Position.Y + y * 30), 1.5F, new Vector4(1f, 0.4f, 0.2F, 0.5f), .2F);
                }
                else if (dir == 1)
                {
                    EngineAnother(new Vector2(Position.X - x1 * 35, Position.Y - y1 * 35));
                }
                else if (dir == 2)
                {
                    EngineAnother(new Vector2(Position.X + x1 * 35, Position.Y + y1 * 35));
                }
                else if (dir == 3)
                {
                    EngineAnother(new Vector2(Position.X - x * 40, Position.Y - y * 40));
                }
            }
            else if (shipType == ShipType.CivSmall1)
            {
                if (dir == 0)
                {
                    EngineEmpuls(new Vector2(Position.X + x * 37, Position.Y + y * 37), this, 0.2F, 0.3F, new Vector4(0.5f, 1f, 0.5f, 1f));
                    EngineEmpuls(new Vector2(Position.X + x * 30 - x1 * 10, Position.Y + y * 30 - y1 * 10), this, 0.2F, 0.3F, new Vector4(0.5f, 1f, 0.5f, 1f));
                    EngineEmpuls(new Vector2(Position.X + x * 30 + x1 * 10, Position.Y + y * 30 + y1 * 10), this, 0.2F, 0.3F, new Vector4(0.5f, 1f, 0.5f, 1f));
                    Light(new Vector2(Position.X + x * 30, Position.Y + y * 30), 1.2F, new Vector4(0.5f, 1f, 0.5f, 0.5f), .2F);
                }
                else if (dir == 1)
                {
                    EngineAnother(new Vector2(Position.X - x1 * 10, Position.Y - y1 * 10));
                }
                else if (dir == 2)
                {
                    EngineAnother(new Vector2(Position.X + x1 * 10, Position.Y + y1 * 10));
                }
                else if (dir == 3)
                {
                    EngineAnother(new Vector2(Position.X - x * 30, Position.Y - y * 30));
                }
            }
        }
        private void EngineEmpuls(Vector2 position, GameObject go, float size, float alphaVel, Vector4 color)
        {
            for (int a = 0; a < 1; a++)
            {
                Vector2 velocity = Vector2.Zero;
                float angle = go.Rotation;
                float angleVel = 0;
                float size1 = size * 4F;
                float sizeVel = -.07f;
                float alphaVel1 = alphaVel;
                Particle particle = new Particle(Textures.blue[2], position, velocity, angle, angleVel, color, size1, sizeVel, alphaVel1);
                particles.Add(particle);
            }
            if (core.random.Next(3) == 0)
            {
                for (int a = 0; a < 1; a++)
                {
                    color = new Vector4(color.X, color.Y, color.Z, color.W / 2F);
                    Vector2 velocity = ParticleSystem.AngleToV2((float)(Math.PI * 2d * core.random.NextDouble()), .2f);
                    float angle = MathHelper.ToRadians(core.random.Next(360));
                    float angleVel = 0;
                    float sizeVel = .01F;
                    Particle particle = new Particle(Textures.smoke[2], position, velocity, angle, angleVel, color, 0.1F, sizeVel, 0.01F);
                    particles.Add(particle);
                }
            }
        }
        private void EngineAnother(Vector2 position)
        {
            if (core.random.Next(2) == 0)
                for (int a = 0; a < 1; a++)
                {
                    Vector2 velocity = ParticleSystem.AngleToV2((float)(Math.PI * 2d * core.random.NextDouble()), 0.6f);
                    float angle = MathHelper.ToRadians(core.random.Next(360));
                    float angleVel = 0;
                    Vector4 color = new Vector4(1f, 1f, 1f, 1f);
                    float size = 0.2f;
                    float sizeVel = .03F;
                    float alphaVel = 0.05F;
                    Particle particle = new Particle(Textures.smoke[1], position, velocity, angle, angleVel, color, size, sizeVel, alphaVel);
                    particles.Add(particle);
                }
        }
        private void Light(Vector2 position, float size, Vector4 color, float alphaVel)
        {
            for (int a = 0; a < 1; a++)
            {
                Vector2 velocity = Vector2.Zero;
                float angle = 0;
                float angleVel = 0;
                float sizeVel = 0;
                Particle particle = new Particle(Textures.light, position, velocity, angle, angleVel, color, size, sizeVel, alphaVel);
                particles.Add(particle);
            }
        }
        public override void Render(SpriteBatch spriteBatch)
        {
            if (!isJumping)
            {
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.AnisotropicClamp, null, null, null, core.cam.defaultScreen);
                RenderParticles(spriteBatch);
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, null, null, null, core.cam.defaultScreen);
                base.Render(spriteBatch);
                for (int i = 0; i < shipDamages.Length; i++)
                {
                    shipDamages[i].Render(spriteBatch);
                }
                if (gunSlots != null)
                {
                    for (int i = 0; i < gunSlots.Length; i++)
                    {
                        if (gunSlots[i] != null)
                        {
                            gunSlots[i].Render(spriteBatch);
                        }
                    }
                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.AnisotropicClamp, null, null, null, core.cam.defaultScreen);
                    for (int i = 0; i < gunSlots.Length; i++)
                    {
                        //if (gunSlots[i] != null)
                        //gunSlots[i].RenderBeam(spriteBatch);
                    }
                    for (int i = 0; i < gunSlots.Length; i++)
                    {
                        if (gunSlots[i] != null)
                            gunSlots[i].RenderEffects(spriteBatch);
                    }
                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, null, null, null, core.cam.defaultScreen);
                }
                if (shipName != null)
                    spriteBatch.DrawString(Fonts.basicFont, shipName, Position + new Vector2(0 - Fonts.basicFont.MeasureString(shipName).X / 2, 30), Color.White);
                if (shield > 0)
                {
                    shieldBase.Render(spriteBatch);
                }
                else
                {
                    shieldBase.Size = 0;
                }
            }
        }
        private void RenderParticles(SpriteBatch spriteBatch)
        {
            for (int index = 0; index < particles.Count; index++)
            {
                particles[index].Render(spriteBatch);
            }
        }
    }
}
