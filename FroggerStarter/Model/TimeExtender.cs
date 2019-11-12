using System;
using Windows.UI.Xaml;
using FroggerStarter.Controller;
using FroggerStarter.View.Sprites.PowerUpSprites;

namespace FroggerStarter.Model
{
    /// <summary>A powerup that extends player's life-time</summary>
    /// <seealso cref="FroggerStarter.Model.GameObject" />
    public class TimeExtender : GameObject
    {
        #region Data members

        private readonly int[] possibleYValuesNonWater = {305, 255, 205, 155, 105};

        private readonly Random rand;
        private DispatcherTimer timer;

        #endregion

        #region Properties

        /// <summary>Gets a value indicating whether this instance is visible.</summary>
        /// <value>
        ///     <c>true</c> if this instance is showing; otherwise, <c>false</c>.
        /// </value>
        public bool IsShowing { get; private set; }

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="TimeExtender" /> class.</summary>
        public TimeExtender()
        {
            this.setUpTimer();
            this.rand = new Random();
            Sprite = new TimeExtenderSprite {Visibility = Visibility.Collapsed};
            this.moveNext();
        }

        #endregion

        #region Methods

        private void onTick(object sender, object e)
        {
            this.IsShowing = false;
            Sprite.Visibility = Visibility.Collapsed;
            this.moveNext();
            this.timer.Stop();
        }

        private void setUpTimer()
        {
            this.timer = new DispatcherTimer();
            this.timer.Tick += this.onTick;
            this.timer.Interval = new TimeSpan(0, 0, 0, 3);
        }

        private void moveNext()
        {
            var nextY = this.rand.Next(this.possibleYValuesNonWater.Length);
            Y = this.possibleYValuesNonWater[nextY];
            X = this.rand.Next((int) GameSettings.BackgroundWidth - (int) Sprite.Width);
        }

        /// <summary>Randomly shows the powerup</summary>
        public void RandomlyShow()
        {
            if (this.rand.NextDouble() < 0.4)
            {
                this.IsShowing = true;
                Sprite.Visibility = Visibility.Visible;
                this.timer.Start();
            }
        }

        /// <summary>Called when [hit]. Collapses powerup visibility</summary>
        public void OnHit()
        {
            Sprite.Visibility = Visibility.Collapsed;
            this.IsShowing = false;
        }

        /// <summary>Sets the heading.</summary>
        /// <param name="heading">The heading.</param>
        public override void SetHeading(Heading heading)
        {
            
        }

        #endregion
    }
}