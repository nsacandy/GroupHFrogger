using System;
using Windows.UI.Xaml;
using FroggerStarter.Controller;
using FroggerStarter.View.Sprites.PowerUpSprites;

namespace FroggerStarter.Model
{
    /// <summary>Invincibility stars that render the player un-hittable for a few seconds</summary>
    /// <seealso cref="FroggerStarter.Model.GameObject" />
    public class InvincibilityStar : GameObject
    {
        #region Data members

        private readonly int[] possibleYValuesNonWater = {305, 255, 205, 155, 105};

        private readonly Random rand;
        private DispatcherTimer timer;

        #endregion

        #region Properties

        /// <summary>Gets a value indicating whether this instance is visible.</summary>
        /// <value>
        ///     <c>true</c> if this instance is visible; otherwise, <c>false</c>.
        /// </value>
        public bool IsShowing { get; private set; }

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="InvincibilityStar" /> class.</summary>
        public InvincibilityStar()
        {
            this.setUpTimer();
            this.rand = new Random();
            Sprite = new InvincibilityStarSprite {Visibility = Visibility.Collapsed};
            this.moveNext();
        }

        #endregion

        #region Methods

        private void onTick(object sender, object e)
        {
            this.IsShowing = true;
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

        /// <summary>Randomly determines if star will display, and displays it</summary>
        public void RandomlyShow()
        {
            if (this.rand.NextDouble() < 0.4)
            {
                this.IsShowing = true;
                Sprite.Visibility = Visibility.Visible;
                this.timer.Start();
            }
        }

        /// <summary>Called when [hit]. Collapses star, restarts show timer</summary>
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