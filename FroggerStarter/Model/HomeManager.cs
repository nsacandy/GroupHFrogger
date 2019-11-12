using System.Collections;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using FroggerStarter.View.Sprites;

namespace FroggerStarter.Model
{
    /// <summary>Class for managing home bases</summary>
    public class HomeManager : IEnumerable<LilyPad>
    {
        #region Data members

        private const int NumberHomes = 5;
        private const int FrogHomeBuffer = 100;
        private readonly Canvas gameCanvas;
        private IList<LilyPad> landingSpots;

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="HomeManager" /> class, adds landing spots</summary>
        /// <param name="gameCanvas">The game canvas.</param>
        public HomeManager(Canvas gameCanvas)
        {
            this.gameCanvas = gameCanvas;
            this.addLandingSpotsToCanvas();
        }

        #endregion

        #region Methods

        /// <summary>Returns an enumerator that iterates through the bases.</summary>
        /// <returns>An enumerator that can be used to iterate through the bases.</returns>
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

        /// <summary>Resets the landing spots, typically after new level</summary>
        public void ResetLandingSpots()
        {
            this.addLandingSpotsToCanvas();
        }

        /// <summary>Alls the homes filled.</summary>
        /// <returns>boolean value representing whether the remaining homespots == 0</returns>
        public bool IsAllHomesFilled()
        {
            return this.landingSpots.Count == 0;
        }

        /// <summary>Removes the home.</summary>
        /// <param name="home">The home.</param>
        public void RemoveHome(LilyPad home)
        {
            this.landingSpots.Remove(home);
            this.gameCanvas.Children.Remove(home);
        }

        #endregion
    }
}