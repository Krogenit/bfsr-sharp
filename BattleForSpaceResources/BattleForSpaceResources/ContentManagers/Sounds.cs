using BattleForSpaceResources.ShipComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleForSpaceResources
{
    public static class Sounds
    {
        public static SoundEffect buttonCollide, buttonClick, jump, shieldDown;
        public static SoundEffect[] laser = new SoundEffect[2], gaus = new SoundEffect[3], plasm = new SoundEffect[3], explosion = new SoundEffect[3];
        public static ContentManager conM;

        public static AudioListener listener = new AudioListener();
        public static List<SoundEffectInstance> soundsList = new List<SoundEffectInstance>();
        public static List<AudioEmitter> emitterList = new List<AudioEmitter>();
        private static Core core = Core.GetCore();
        private static World world = Core.GetCore().GetWorld();
        public static void LoadSounds(ContentManager cm)
        {
            conM = cm;
            buttonCollide = LoadSound("gui\\buttonCollide");
            buttonClick = LoadSound("gui\\buttonClick");
            jump = LoadSound("ship\\jump");
            shieldDown = LoadSound("ship\\shielddown");

            for(int i=0;i<laser.Length;i++)
                laser[i] = LoadSound("ship\\weapon\\laser"+i);
            for (int i = 0; i < gaus.Length; i++)
                gaus[i] = LoadSound("ship\\weapon\\gaus" + i);
            for (int i = 0; i < plasm.Length; i++)
                plasm[i] = LoadSound("ship\\weapon\\plasm" + i);
            for(int i=0;i<explosion.Length;i++)
                explosion[i] = LoadSound("ship\\weapon\\explosion" + i);
        }
        public static void CheckSound()
        {
            if (Settings.soundVolume < 0)
                Settings.soundVolume = 0;
            else if(Settings.soundVolume > 1)
                Settings.soundVolume = 1;
        }
        public static void Update()
        {
            CheckSound();
            listener.Position = new Vector3(core.cam.screenCenter.X / 100, core.cam.screenCenter.Y / 100, 2);
            for (int i = 0; i < soundsList.Count; i++)
            {
                soundsList[i].Apply3D(listener, emitterList[i]);
                if (soundsList[i].State == SoundState.Stopped)
                {
                    soundsList.RemoveAt(i);
                    emitterList.RemoveAt(i);
                }
            }
        }
        public static SoundEffect LoadSound(string s)
        {
            return conM.Load<SoundEffect>("sound\\" + s);
        }

        public static void Play(SoundEffect s, float volume, float pitch)
        {
            CheckSound();
            s.Play(volume,pitch,0);
        }
        public static void PlayShieldDownSound(Vector2 pos)
        {
            CheckSound();
            float dis = Vector2.Distance(core.cam.screenCenter, pos);
            if (dis <= 1500)
            {
                AudioEmitter emitter = new AudioEmitter();
                emitter.Position = new Vector3(pos.X / 100, pos.Y / 100, 0);
                SoundEffectInstance s = null;
                s = shieldDown.CreateInstance();
                s.Volume = Settings.soundVolume;
                s.Pitch = (float)core.random.Next(-200, 200) / 1000F;
                s.Apply3D(listener, emitter);
                s.Play();
                soundsList.Add(s);
                emitterList.Add(emitter);
            }
        }
        public static void PlayJumpSound(Vector2 pos)
        {
            CheckSound();
            float dis = Vector2.Distance(core.cam.screenCenter, pos);
            if (dis <= 1500)
            {
                AudioEmitter emitter = new AudioEmitter();
                emitter.Position = new Vector3(pos.X / 100, pos.Y / 100, 0);
                SoundEffectInstance s = null;
                s = jump.CreateInstance();
                s.Volume = Settings.soundVolume;
                s.Pitch = (float)core.random.Next(-200, 200) / 1000F;
                s.Apply3D(listener, emitter);
                s.Play();
                soundsList.Add(s);
                emitterList.Add(emitter);
            }
        }
        public static void PlayDestroySound(Vector2 pos, bool isBig)
        {
            CheckSound();
            float dis = Vector2.Distance(core.cam.screenCenter, pos);
            if (dis <= 1500)
            {
                AudioEmitter emitter = new AudioEmitter();
                emitter.Position = new Vector3(pos.X / 100, pos.Y / 100, 0);
                SoundEffectInstance s = null;
                if (isBig)
                {
                    s = explosion[2].CreateInstance();
                }
                else
                {
                    s = explosion[1].CreateInstance();
                }
                s.Volume = Settings.soundVolume;
                s.Apply3D(listener, emitter);
                s.Play();
                soundsList.Add(s);
                emitterList.Add(emitter);
            }
        }
        public static void PlayDestroyingSound(Vector2 pos)
        {
            CheckSound();
            float dis = Vector2.Distance(core.cam.screenCenter, pos);
            if (dis <= 1500)
            {
                AudioEmitter emitter = new AudioEmitter();
                emitter.Position = new Vector3(pos.X / 100, pos.Y / 100, 0);
                SoundEffectInstance s = explosion[0].CreateInstance();
                s.Pitch = (float)core.random.Next(-200, 200) / 1000F;
                s.Volume = Settings.soundVolume;
                s.Apply3D(listener, emitter);
                s.Play();
                soundsList.Add(s);
                emitterList.Add(emitter);
            }
        }
        public static void PlayGunSound(GunType gt, Vector2 pos)
        {
            CheckSound();
            float dis = Vector2.Distance(core.cam.screenCenter, pos);
            if (dis <= 1500)
            {
                AudioEmitter emitter = new AudioEmitter();
                emitter.Position = new Vector3(pos.X / 100, pos.Y / 100, 0);
                SoundEffectInstance s = null;
                if (gt == GunType.LaserSmall)
                {
                    int rndInt = core.random.Next(2);
                    s = laser[rndInt].CreateInstance();
                    s.Pitch = 0.0F;
                }
                else if (gt == GunType.PlasmSmall)
                {
                    int rndInt = core.random.Next(3);
                    s = plasm[rndInt].CreateInstance();
                    s.Pitch = 0.0F;
                }
                if (gt == GunType.GausSmall)
                {
                    int rndInt = core.random.Next(3);
                    s = gaus[rndInt].CreateInstance();
                    s.Pitch = 0.0F;
                }
                s.Volume = Settings.soundVolume;
                s.Apply3D(listener, emitter);
                s.Play();
                soundsList.Add(s);
                emitterList.Add(emitter);
            }
        }
    }
}
