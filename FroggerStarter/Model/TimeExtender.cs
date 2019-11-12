using System;
using Windows.UI.Xaml;
using FroggerStarter.Controller;
using FroggerStarter.View.Sprites.PowerUpSprites;

namespace FroggerStarter.Model
{
    public class TimeExtender : GameObject
    {
        private readonly int[] possibleYValuesNonWater = {305, 255, 205, 155, 105};

        private readonly Random rand;
        private DispatcherTimer timer;

        public TimeExtender()
        {
            setUpTimer();
            rand = new Random();
            Sprite = new TimeExtenderSprite();
            Sprite.Visibility = Visibility.Collapsed;
            moveNext();
        }

        public bool IsShowing { get; private set; }

        private void onTick(object sender, object e)
        {
            IsShowing = false;
            Sprite.Visibility = Visibility.Collapsed;
            moveNext();
            timer.Stop();
        }

        private void setUpTimer()
        {
            timer = new DispatcherTimer();
            timer.Tick += onTick;
            timer.Interval = new TimeSpan(0, 0, 0, 3);
        }

        private void moveNext()
        {
            var nextY = rand.Next(possibleYValuesNonWater.Length);
            Y = possibleYValuesNonWater[nextY];
            X = rand.Next((int) GameSettings.backgroundWidth - (int) Sprite.Width);
        }

        public void Show()
        {
            if (rand.NextDouble() < 0.4)
            {
                IsShowing = true;
                Sprite.Visibility = Visibility.Visible;
                timer.Start();
            }
        }

        public void OnHit()
        {
            Sprite.Visibility = Visibility.Collapsed;
            IsShowing = false;
        }

        public override void setHeading(Heading heading)
        {
            throw new NotImplementedException();
        }
    }
}