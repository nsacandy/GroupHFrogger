
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
        PowerUp,
        PowerUpStar
    }

    public class SoundEffects
    {
        private Dictionary<Sounds, MediaPlayer> effects;

        public SoundEffects()
        {
            this.effects = new Dictionary<Sounds, MediaPlayer>();
            this.loadEfx();
        }

        private void loadEfx()
        {
            this.effects.Add(Sounds.Hop, this.loadSoundFile("sound-frogger-hop.wav"));
            this.effects.Add(Sounds.HitVehicle, this.loadSoundFile("sound-frogger-squash.wav"));
            this.effects.Add(Sounds.HitWater, this.loadSoundFile("sound-frogger-plunk.wav"));
            this.effects.Add(Sounds.TimeOut, this.loadSoundFile("sound-frogger-time.wav"));
            this.effects.Add(Sounds.GameOver, this.loadSoundFile("sound-frogger-gameover.wav"));
            this.effects.Add(Sounds.HitWall, this.loadSoundFile("sound-frogger-hit-wall.wav"));
            this.effects.Add(Sounds.LandHome, this.loadSoundFile("sound-frogger-land-home.wav"));
            this.effects.Add(Sounds.LevelComplete, this.loadSoundFile("sound-frogger-level-complete.wav"));
            this.effects.Add(Sounds.PowerUp, this.loadSoundFile("sound-frogger-power-up-time.wav"));
            this.effects.Add(Sounds.PowerUpStar, this.loadSoundFile("sound-frogger-star-power.wav"));
        }

        private MediaPlayer loadSoundFile(string fileName)
        {
            var mediaSource =
                MediaSource.CreateFromUri(new Uri("ms-appx:///Sounds///" + fileName, UriKind.RelativeOrAbsolute));
            var playbackItem = new MediaPlaybackItem(mediaSource);

            var sound = new MediaPlayer
            {
                AutoPlay = false,
                Source = playbackItem,
            };

            return sound;
        }

        public void Play(Sounds efx)
        {
            var mediaElement = this.effects[efx];

            mediaElement.Play();
        }
    }
}