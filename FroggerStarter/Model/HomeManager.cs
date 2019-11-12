using System.Collections;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using FroggerStarter.View.Sprites;

namespace FroggerStarter.Model
{
    public class HomeManager : IEnumerable<LilyPad>
    {
        #region Data members

        private const int NumberHomes = 5;
        private const int FrogHomeBuffer = 100;
        private readonly Canvas gameCanvas;
        private IList<LilyPad> landingSpots;

        #endregion

        #region Constructors

        public HomeManager(Canvas gameCanvas)
        {
            this.gameCanvas = gameCanvas;
            this.addLandingSpotsToCanvas();
        }

        #endregion

        #region Methods

        public IEnumerator<LilyPad> GetEnumerator()
        {
            return this.landingSpots.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.landingSpots.GetEnumerator();
        }

        private void addLandingSpotsToCanvas()
        {
            this.landingSpots = new List<LilyPad>();

            var currX = 0.0;
            for (var i = 0; i < NumberHomes; i++)
            {
                var home = new LilyPad();

                var xLocation = currX;
                var yLocation = (double) Application.Current.Resources["HighRoadYLocation"];

                home.RenderAt(xLocation, yLocation);

                this.landingSpots.Add(home);
                this.gameCanvas.Children.Add(home);

                currX += home.Width + FrogHomeBuffer;
            }
        }

        public void ResetLandingSpots()
        {
            this.addLandingSpotsToCanvas();
        }

        public bool IsAllHomesFilled()
        {
            return this.landingSpots.Count == 0;
        }

        public void RemoveHome(LilyPad home)
        {
            this.landingSpots.Remove(home);
            this.gameCanvas.Children.Remove(home);
        }

        #endregion
    }
}