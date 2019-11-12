using System;
using Windows.UI.Xaml;
using FroggerStarter.Controller;
using FroggerStarter.View.Sprites.PowerUpSprites;

namespace FroggerStarter.Model
{
    public class InvincibilityStar : GameObject
    {
        #region Data members

        private readonly int[] possibleYValuesNonWater = {305, 255, 205, 155, 105};

        private readonly Random rand;
        private DispatcherTimer timer;

        #endregion

        #region Properties

        public bool IsShowing { get; private set; }

        #endregion

        #region Constructors

        public InvincibilityStar()
        {
            this.setUpTimer();
            this.rand = new Random();
            Sprite = new InvincibilityStarSprite();
            Sprite.Visibility = Visibility.Collapsed;
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

        public void Show()
        {
            if (this.rand.NextDouble() < 0.4)
            {
                this.IsShowing = true;
                Sprite.Visibility = Visibility.Visible;
                this.timer.Start();
            }
        }

        public void OnHit()
        {
            Sprite.Visibility = Visibility.Collapsed;
            this.IsShowing = false;
        }

        public override void setHeading(Heading heading)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}