using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace BattleForSpaceResources
{
    public static class Textures
    {
        public static Texture2D pixel, 
            shipHumanSmall1, shipEnemySmall1, shipCivSmall1,
            guiButtonBasic, guiCursor, guiFon, logoBFSR, logoText1, logoText2, guiSelectFaction,guiButtonSmall,guiAdd,guiSlider,guiShield,guiReactor,guiHudShip,guiNothing,
            guiSelectedShips,guiButtonSmallStrelka,guiAmorPlate,guiButtonControl,guiInfo,guiSelect,guiChat,
            ambientStarLight, ambientPlanetLight,
            gunPlasm1,gunLaser1,gunPlasma1,gunGaus1,
            bulletPlasm,bulletLaser,bulletGaus,
            lightKrypton, light,
            effectExplosion, effectBeamDamage, effectShipDerbiSmall,effectShipDerbi,effectShipDamage,effectRocketSmoke,effectRocketEffect,effectShieldDown,effectJump,effectDirectedSpark,effectDirectedSplat,
            effectLighting,effectSmokeRing,
            particleShipOst1, particleShipOst2
            
            
            
            
            ;
        public static Texture2D[] background, ambient, ambientClouds, ambientStars, ambientPlanets, ambientMoons,
            shieldSmall, shieldMedium, shieldLarge, shieldHuge, shieldStation, 
            blue, smoke, spark,
            particleDerbis = new Texture2D[6], particleDerbisFire = new Texture2D[6], particleWrecks = new Texture2D[3], particleWrecksFire = new Texture2D[3], particleWrecksLight = new Texture2D[3],
            wercksHuge1 = new Texture2D[9], wercksCivilianMotherShip = new Texture2D[9], wercksStation1 = new Texture2D[6],wercksHumanMotherShip1 = new Texture2D[6], wercksEnemyMotherShip = new Texture2D[9],
            effectShockWave= new Texture2D[3],
            guiMap = new Texture2D[2],guiHudShipAdd = new Texture2D[2],
            damage = new Texture2D[4],damageFix = new Texture2D[4],damageFire = new Texture2D[4],damageLight = new Texture2D[2]
            
            ;
        public static ContentManager content;
        public static void LoadTextures(ContentManager con)
        {
            content=con;

            background = new Texture2D[4];
            for (int i = 0; i < 4;i++ )
                background[i] = LoadTexture("ambient\\background\\background"+i);
            ambientClouds = new Texture2D[1];
            ambientStars = new Texture2D[1];
            ambientPlanets = new Texture2D[1];
            ambientMoons = new Texture2D[13];
            ambientStars[0] = LoadTexture("ambient\\star\\largeStar_01");
            ambientPlanets[0] = LoadTexture("ambient\\planet\\planet0");
            ambientClouds[0] = LoadTexture("ambient\\cloud\\cloud_junk");
            ambientStarLight = LoadTexture("ambient\\star\\largeFlair_01");
            ambientPlanetLight = LoadTexture("ambient\\planet\\planet_lightCard");
            for (int i = 0; i < ambientMoons.Length; i++)
            {
                ambientMoons[i] = LoadTexture("ambient\\planet\\moon_" + i);
            }

            pixel = LoadTexture("pixel");

            shipHumanSmall1 = LoadTexture("ship\\human_small1");
            shipEnemySmall1 = LoadTexture("ship\\enemy_small1");
            shipCivSmall1 = LoadTexture("ship\\civ_small1");

            shieldSmall = new Texture2D[1];
            shieldMedium = new Texture2D[1];
            shieldLarge = new Texture2D[1];
            shieldHuge = new Texture2D[1];
            shieldStation = new Texture2D[1];
            for (int i = 0; i < shieldSmall.Length;i++ )
                shieldSmall[i] = LoadTexture("ship\\component\\shield\\small"+i);
            for (int i = 0; i < shieldMedium.Length; i++)
                shieldMedium[i] = LoadTexture("ship\\component\\shield\\medium" + i);
            //for (int i = 0; i < shieldLarge.Length; i++)
                //shieldLarge[i] = LoadTexture("Ships\\Adds\\shield\\large" + i);
            for (int i = 0; i < shieldHuge.Length; i++)
                shieldHuge[i] = LoadTexture("ship\\component\\shield\\huge" + i);
            for (int i = 0; i < shieldStation.Length; i++)
                shieldStation[i] = LoadTexture("ship\\component\\shield\\station" + i);

            gunPlasm1 = LoadTexture("ship\\component\\weapon\\plasm1");
            gunLaser1 = LoadTexture("ship\\component\\weapon\\laser1");
            gunGaus1 = LoadTexture("ship\\component\\weapon\\gaus1");

            bulletPlasm = LoadTexture("particle\\projective\\smallPlasm");
            bulletLaser = LoadTexture("particle\\projective\\laser");
            bulletGaus = LoadTexture("particle\\projective\\gaus");

            guiButtonBasic = LoadTexture("gui\\gui_buttonBase");
            guiButtonSmall = LoadTexture("gui\\gui_buttonSmall");
            guiAdd = LoadTexture("gui\\gui_add");
            guiCursor = LoadTexture("gui\\cursor");
            guiFon = LoadTexture("gui\\gui_fon");
            guiSelectFaction = LoadTexture("gui\\gui_selectFaction");
            guiSlider= LoadTexture("gui\\gui_slider");
            logoBFSR = LoadTexture("gui\\logoBFSR");
            logoText1 = LoadTexture("gui\\bfsr_text1");
            logoText2 = LoadTexture("gui\\bfsr_text2");
            guiShield = LoadTexture("gui\\shield");
            guiReactor = LoadTexture("gui\\reactor");
            guiHudShip = LoadTexture("gui\\gui_hudship");
            guiNothing = LoadTexture("gui\\gui_nothing");
            guiMap[0] = LoadTexture("gui\\gui_map");
            guiMap[1] = LoadTexture("gui\\gui_mapfon");
            guiSelectedShips = LoadTexture("gui\\gui_selectedships");
            guiButtonSmallStrelka = LoadTexture("gui\\gui_buttonstrelka");
            guiAmorPlate = LoadTexture("gui\\armorPlate");
            guiButtonControl = LoadTexture("gui\\gui_buttonControl");
            guiInfo = LoadTexture("gui\\gui_hudshipadd1");
            guiSelect = LoadTexture("gui\\gui_select");
            guiChat = LoadTexture("gui\\gui_chat");
            for (int i = 0; i < guiHudShipAdd.Length; i++)
            {
                guiHudShipAdd[i] = LoadTexture("gui\\gui_hudshipadd" + i);
            }

            blue = new Texture2D[4];
            smoke = new Texture2D[5];
            spark = new Texture2D[4];
            light = LoadTexture("particle\\light");
            lightKrypton = LoadTexture("particle\\lightKrypton");
            for(int i=0;i<4;i++)
                blue[i] = LoadTexture("particle\\blue"+i);
            for (int i = 0; i < 5; i++)
                smoke[i] = LoadTexture("particle\\smoke" + i);
            for (int i = 0; i < 4; i++)
                spark[i] = LoadTexture("particle\\spark" + i);

            effectExplosion = LoadTexture("particle\\explosion");
            effectBeamDamage = LoadTexture("particle\\beamdamage");
            effectShipDerbi = LoadTexture("particle\\shipderbi");
            effectShipDerbiSmall = LoadTexture("particle\\shipderbismall");
            effectShipDamage = LoadTexture("particle\\shipdamage");
            effectRocketSmoke = LoadTexture("particle\\rocketsmoke");
            effectRocketEffect = LoadTexture("particle\\rocketeffect");
            effectShieldDown = LoadTexture("particle\\shielddown"); ;
            effectJump = LoadTexture("particle\\jump");
            effectDirectedSpark = LoadTexture("particle\\directedspark");
            effectDirectedSplat = LoadTexture("particle\\directedsplat");
            effectLighting = LoadTexture("particle\\lighting");
            effectSmokeRing = LoadTexture("particle\\smokering"); ;

            effectShockWave[0] = LoadTexture("particle\\basicshockwave");
            effectShockWave[1] = LoadTexture("particle\\bigshockwave");
            effectShockWave[2] = LoadTexture("particle\\shockwave");

            particleShipOst1 = LoadTexture("particle\\shipost1");
            particleShipOst2 = LoadTexture("particle\\shipost2");
            for (int i = 0; i < 6; i++)
            {
                particleDerbis[i] = LoadTexture("particle\\damage\\ship_debris_0" + (i + 1));
                particleDerbisFire[i] = LoadTexture("particle\\damage\\ship_debris_ember_0" + (i + 1));
            }
            for (int i = 0; i < 3; i++)
            {
                particleWrecks[i] = LoadTexture("particle\\wreck\\wreck" + i);
                particleWrecksFire[i] = LoadTexture("particle\\wreck\\wreckfire" + i);
                particleWrecksLight[i] = LoadTexture("particle\\wreck\\wrecklight" + i);
            }
            for (int i = 0; i < 4; i++)
            {
                damage[i] = LoadTexture("particle\\damage\\damage" + i);
                damageFire[i] = LoadTexture("particle\\damage\\fire" + i);
                damageFix[i] = LoadTexture("particle\\damage\\fix" + i);
            }
            damageLight[0] = LoadTexture("particle\\damage\\light2");
            damageLight[1] = LoadTexture("particle\\damage\\light3");
            for (int i = 0; i < 3; i++)
            {
                wercksHuge1[i] = LoadTexture("particle\\wreck\\HumanHuge1\\werck" + (i + 1));
                wercksHuge1[i + 3] = LoadTexture("particle\\wreck\\HumanHuge1\\werckfire" + (i + 1));
                wercksHuge1[i + 6] = LoadTexture("particle\\wreck\\HumanHuge1\\wercklight" + (i + 1));
                wercksEnemyMotherShip[i] = LoadTexture("particle\\wreck\\EnemyMotherShip\\werck" + i);
                wercksEnemyMotherShip[i + 3] = LoadTexture("particle\\wreck\\EnemyMotherShip\\werckfire" + i);
                wercksEnemyMotherShip[i + 6] = LoadTexture("particle\\wreck\\EnemyMotherShip\\wercklight" + i);
                wercksCivilianMotherShip[i] = LoadTexture("particle\\wreck\\CivilianMotherShip\\werck" + i);
                wercksCivilianMotherShip[i + 3] = LoadTexture("particle\\wreck\\CivilianMotherShip\\werckfire" + (i));
                wercksCivilianMotherShip[i + 6] = LoadTexture("particle\\wreck\\CivilianMotherShip\\wercklight" + (i));
            }
            for (int i = 0; i < 2; i++)
            {
                wercksStation1[i] = LoadTexture("particle\\wreck\\HumanStation1\\werck" + (i + 1));
                wercksStation1[i + 2] = LoadTexture("particle\\wreck\\HumanStation1\\werckfire" + (i + 1));
                wercksStation1[i + 4] = LoadTexture("particle\\wreck\\HumanStation1\\wercklight" + (i + 1));
                wercksHumanMotherShip1[i] = LoadTexture("particle\\wreck\\HumanMotherShip\\mothership1_werck" + (i + 0));
                wercksHumanMotherShip1[i + 2] = LoadTexture("particle\\wreck\\HumanMotherShip\\mothership1_werckfire" + (i + 0));
                wercksHumanMotherShip1[i + 4] = LoadTexture("particle\\wreck\\HumanMotherShip\\mothership1_wercklight" + (i + 0));
            }
        }
        public static Texture2D LoadTexture(string s)
        {
            return content.Load<Texture2D>("texture\\" + s);
        }
    }
}
