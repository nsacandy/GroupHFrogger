using System;
using System.Collections.Generic;
using Windows.Media.Core;
using Windows.Media.Playback;

namespace FroggerStarter.Model
{
    public enum Sounds
    {
        Hop,
        HitVehicle,
        HitWater,
        TimeOut,
        GameOver,
        HitWall,
        LandHome,
        LevelComplete,
        PowerUpTime,
        PowerUpStar
    }

    
    public class SoundEffects
    {
        private readonly Dictionary<Sounds, MediaPlayer> effects;
        private MediaPlayer powerStarLoop;

        public MediaPlayer PowerStarLoop
        {
            get { return powerStarLoop; }
            private set { powerStarLoop = value; }
        }

        
        public SoundEffects()
        {
            this.setStarLoop();
            effects = new Dictionary<Sounds, MediaPlayer>();
            loadEfx();
        }

        private void loadEfx()
        {
            MediaPlayer powerStar = loadSoundFileAsync("sound-frogger-star-power.wav");
            powerStar.IsLoopingEnabled = true;
            powerStar.Play();
            effects.Add(Sounds.Hop, loadSoundFileAsync("sound-frogger-hop.wav"));
            effects.Add(Sounds.HitVehicle, loadSoundFileAsync("sound-frogger-squash.wav"));
            effects.Add(Sounds.HitWater, loadSoundFileAsync("sound-frogger-plunk.wav"));
            effects.Add(Sounds.TimeOut, loadSoundFileAsync("sound-frogger-time.wav"));
            effects.Add(Sounds.GameOver, loadSoundFileAsync("sound-frogger-gameover.wav"));
            effects.Add(Sounds.HitWall, loadSoundFileAsync("sound-frogger-hit-wall.wav"));
            effects.Add(Sounds.LandHome, loadSoundFileAsync("sound-frogger-land-home.wav"));
            effects.Add(Sounds.LevelComplete, loadSoundFileAsync("sound-frogger-level-complete.wav"));
            effects.Add(Sounds.PowerUpTime, loadSoundFileAsync("sound-frogger-power-up-time.wav"));
            effects.Add(Sounds.PowerUpStar, loadSoundFileAsync("sound-frogger-star-power.wav"));
        }

        private MediaPlayer loadSoundFileAsync(string fileName)
        {
            var mediaSource =
                MediaSource.CreateFromUri(new Uri("ms-appx:///Sounds///" + fileName, UriKind.RelativeOrAbsolute));
            var playbackItem = new MediaPlaybackItem(mediaSource);

            var sound = new MediaPlayer
            {
                AutoPlay = false,
                Source = playbackItem
            };

            return sound;
        }

        public void Play(Sounds efx)
        {
            var mediaElement = effects[efx];

            mediaElement.Play();
        }

        public void setStarLoop()
        {
            MediaPlayer powerStar = loadSoundFileAsync("sound-frogger-star-power.wav");
            powerStar.IsLoopingEnabled = true;
            this.powerStarLoop = powerStar;
        }
        
    }
}