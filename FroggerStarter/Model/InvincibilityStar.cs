using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using FroggerStarter.Controller;
using FroggerStarter.View.Sprites.PowerUpSprites;

namespace FroggerStarter.Model
{
    public class InvincibilityStar:GameObject
    {
        private readonly int[] possibleYValuesNonWater = { 305, 255, 205, 155, 105 };

        private Random rand;
        private DispatcherTimer timer;

        public bool IsShowing { get; private set; } = false;

        public InvincibilityStar()
        {
            this.setUpTimer();
            this.rand = new Random();
            this.Sprite = new InvincibilityStarSprite();
            this.Sprite.Visibility = Visibility.Collapsed;
            this.moveNext();

        }

        private void onTick(object sender, object e)
        {
            this.IsShowing = true;
            this.Sprite.Visibility = Visibility.Collapsed;
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
            this.Y = this.possibleYValuesNonWater[nextY];
            this.X = this.rand.Next((int)GameSettings.backgroundWidth - (int)this.Sprite.Width);
        }

        public void Show()
        {
            if (this.rand.NextDouble() < 0.4)
            {
                this.IsShowing = true;
                this.Sprite.Visibility = Visibility.Visible;
                this.timer.Start();
            }
        }

        public void OnHit()
        {
            this.Sprite.Visibility = Visibility.Collapsed;
            this.IsShowing = false;
        }

        public override void setHeading(Heading heading)
        {
            throw new NotImplementedException();
        }

        
    }
}

    
