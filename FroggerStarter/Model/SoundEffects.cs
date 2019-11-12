using System;
using System.Collections.Generic;
using Windows.Media.Core;
using Windows.Media.Playback;

namespace FroggerStarter.Model
{
    /// <summary>Enum for different sound effects</summary>
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


    /// <summary>
    ///   <para>Class for adding sounds to the game</para>
    /// </summary>
    public class SoundEffects
    {
        #region Data members

        private readonly Dictionary<Sounds, MediaPlayer> effects;

        #endregion

        #region Properties

        /// <summary>Property for controlling InvincibleStar soundeffect loop</summary>
        /// <value>The power star loop.</value>
        public MediaPlayer PowerStarLoop { get; private set; }

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="SoundEffects"/> class.</summary>
        public SoundEffects()
        {
            this.setStarLoop();
            this.effects = new Dictionary<Sounds, MediaPlayer>();
            this.loadEfx();
        }

        #endregion

        #region Methods

        private void loadEfx()
        {
            this.effects.Add(Sounds.Hop, this.loadSoundFileAsync("sound-frogger-hop.wav"));
            this.effects.Add(Sounds.HitVehicle, this.loadSoundFileAsync("sound-frogger-squash.wav"));
            this.effects.Add(Sounds.HitWater, this.loadSoundFileAsync("sound-frogger-plunk.wav"));
            this.effects.Add(Sounds.TimeOut, this.loadSoundFileAsync("sound-frogger-time.wav"));
            this.effects.Add(Sounds.GameOver, this.loadSoundFileAsync("sound-frogger-gameover.wav"));
            this.effects.Add(Sounds.HitWall, this.loadSoundFileAsync("sound-frogger-hit-wall.wav"));
            this.effects.Add(Sounds.LandHome, this.loadSoundFileAsync("sound-frogger-land-home.wav"));
            this.effects.Add(Sounds.LevelComplete, this.loadSoundFileAsync("sound-frogger-level-complete.wav"));
            this.effects.Add(Sounds.PowerUpTime, this.loadSoundFileAsync("sound-frogger-power-up-time.wav"));
            this.effects.Add(Sounds.PowerUpStar, this.loadSoundFileAsync("sound-frogger-star-power.wav"));
        }

        private MediaPlayer loadSoundFileAsync(string fileName)
        {
            var mediaSource =
                MediaSource.CreateFromUri(new Uri("ms-appx:///Sounds///" + fileName, UriKind.RelativeOrAbsolute));
            var playbackItem = new MediaPlaybackItem(mediaSource);

            var sound = new MediaPlayer {
                AutoPlay = false,
                Source = playbackItem
            };

            return sound;
        }

        /// <summary>Plays the specified sound.</summary>
        /// <param name="efx">The sound effect.</param>
        public void Play(Sounds efx)
        {
            var mediaElement = this.effects[efx];
            mediaElement.Play();
        }

        private void setStarLoop()
        {
            var powerStar = this.loadSoundFileAsync("sound-frogger-star-power.wav");
            powerStar.IsLoopingEnabled = true;
            this.PowerStarLoop = powerStar;
        }

        #endregion
    }
}