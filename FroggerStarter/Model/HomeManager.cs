using System.Collections;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using FroggerStarter.View.Sprites;

namespace FroggerStarter.Model
{
    public class HomeManager : IEnumerable<LilyPad>
    {
        private const int NumberHomes = 5;
        private const int FrogHomeBuffer = 100;
        private readonly Canvas gameCanvas;
        private IList<LilyPad> landingSpots;

        public HomeManager(Canvas gameCanvas)
        {
            this.gameCanvas = gameCanvas;
            addLandingSpotsToCanvas();
        }

        public IEnumerator<LilyPad> GetEnumerator()
        {
            return landingSpots.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return landingSpots.GetEnumerator();
        }

        private void addLandingSpotsToCanvas()
        {
            landingSpots = new List<LilyPad>();

            var currX = 0.0;
            for (var i = 0; i < NumberHomes; i++)
            {
                var home = new LilyPad();

                var xLocation = currX;
                var yLocation = (double) Application.Current.Resources["HighRoadYLocation"];

                home.RenderAt(xLocation, yLocation);

                landingSpots.Add(home);
                gameCanvas.Children.Add(home);

                currX += home.Width + FrogHomeBuffer;
            }
        }

        public void ResetLandingSpots()
        {
            addLandingSpotsToCanvas();
        }

        public bool IsAllHomesFilled()
        {
            return landingSpots.Count == 0;
        }

        public void RemoveHome(LilyPad home)
        {
            landingSpots.Remove(home);
            gameCanvas.Children.Remove(home);
        }
    }
}