using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.System;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using FroggerStarter.Model;
using FroggerStarter.View.Sprites;

namespace FroggerStarter.Controller
{
    /// <summary>
    ///     Manages all aspects of the game play including moving the player,
    ///     the vehicles as well as lives and score.
    /// </summary>
    public class GameManager
    {
        #region Data members

        private readonly double TopLaneOffset = (double)App.Current.Resources["HighRoadYLocation"];
        private const int BottomLaneOffset = 5;
        private readonly double backgroundHeight;
        private readonly double backgroundWidth;
        public readonly double TopBorder = (double)App.Current.Resources["HighRoadYLocation"] + (double)App.Current.Resources["RoadHeight"];
        private readonly double leftBorder = 0;
        private Canvas gameCanvas;
        private PlayerManager player;
        private DispatcherTimer timer;
        private readonly RoadManager roadManager;
        private DateTime runningTime;
        public int Score { get; private set; }
        public int Lives { get; private set; } = 4;

        public delegate void LifeLostHandler(object sender, EventArgs e);
        public event LifeLostHandler LifeLost;

        public delegate void GameOverHandler(object sender, EventArgs e);
        public event GameOverHandler GameOver;

        public delegate void PointScoredHandler(object sender, EventArgs e);
        public event PointScoredHandler PointScored;
        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GameManager" /> class.
        /// </summary>
        /// <param name="backgroundHeight">Height of the background.</param>
        /// <param name="backgroundWidth">Width of the background.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     backgroundHeight &lt;= 0
        ///     or
        ///     backgroundWidth &lt;= 0
        /// </exception>
        public GameManager(double backgroundHeight, double backgroundWidth)
        {
            if (backgroundHeight <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(backgroundHeight));
            }

            if (backgroundWidth <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(backgroundWidth));
            }

            
            this.backgroundHeight = backgroundHeight;
            this.backgroundWidth = backgroundWidth;

            this.roadManager = new RoadManager(this.backgroundWidth);
            this.player = new PlayerManager(this.TopBorder, this.backgroundHeight, 0, this.backgroundWidth);
            this.setupGameTimer();


            LifeLost += this.handleLifeLost;

            GameOver += this.handleGameOver;

            PointScored += this.handlePointScored;


        }

        private void handlePointScored(object sender, EventArgs e)
        {
            this.setPlayerToCenterOfBottomLane();
            this.updateScore();
        }

        #endregion

        #region Methods

        private void setupGameTimer()
        {
            this.timer = new DispatcherTimer();
            this.timer.Tick += this.timerOnTick;
            this.timer.Interval = new TimeSpan(0, 0, 0, 0, 15);
            this.timer.Start();
            this.runningTime = DateTime.Now;
        }

        /// <summary>
        ///     Initializes the game working with appropriate classes to play frog
        ///     and vehicle on game screen.
        ///     Precondition: background != null
        ///     Postcondition: Game is initialized and ready for play.
        /// </summary>
        /// <param name="gamePage">The game page.</param>
        /// <exception cref="ArgumentNullException">gameCanvas</exception>
        public void InitializeGame(Canvas gamePage)
        {
            this.gameCanvas = gamePage ?? throw new ArgumentNullException(nameof(gamePage));
            this.createAndPlacePlayer();
            this.placeVehiclesOnCanvas();
        }

        private void placeVehiclesOnCanvas()
        {
            foreach (var vehicle in this.roadManager.getVehicles())
            {
                this.gameCanvas.Children.Add(vehicle.Sprite);
            }
        }

        private void createAndPlacePlayer()
        {
            this.gameCanvas.Children.Add(this.player.PlayerSprite);
            this.setPlayerToCenterOfBottomLane();
        }

        private void setPlayerToCenterOfBottomLane()
        {
            var centeredX = this.backgroundWidth / 2 - this.player.PlayerSprite.Width / 2;
            var centeredY = this.backgroundHeight - this.player.PlayerSprite.Height - BottomLaneOffset;

            this.player.SetPlayerLocation(centeredX, centeredY);
        }

        private void timerOnTick(object sender, object e)
        {
            this.checkForCollision();
            this.updateScore();

            this.roadManager.moveAllVehicles();
        }


        private void checkForCollision()
        {
            var playerBox = this.player.GetPlayerBox();
            var objectsAtPlayerLocation =
                VisualTreeHelper.FindElementsInHostCoordinates(playerBox, null);
            foreach (var uiElement in objectsAtPlayerLocation)
            {
                if (uiElement is BaseSprite)
                {
                    this.RaiseLifeLost();
                }
            }
        }

        private void updateScore()
        {
            if (this.player.GetPlayerBox().Y < this.TopBorder)
            {
                this.RaisePointScored();
            }

            if (this.Score == 4)
            {
                this.RaiseGameOver();
            }
        }

        private void RaisePointScored()
        {
            this.Score += 1;
            this.PointScored?.Invoke(this, null);
        }

        private void RaiseLifeLost()
        {
            this.Lives -= 1;
            this.LifeLost?.Invoke(this, null);
        }

        private void handleLifeLost(Object sender, EventArgs e)
        { 
            this.setPlayerToCenterOfBottomLane();
            if (this.Lives == 0)
            {
                this.RaiseGameOver();
            }

            else
            {
                this.roadManager.resetLaneSpeeds();
            }
        }

        private void RaiseGameOver()
        {
            this.GameOver?.Invoke(this, null);
        }

        private void handleGameOver(Object sender, EventArgs e)
        {
            this.timer.Stop();
            this.player.PlayerSprite.Visibility = Visibility.Collapsed;
        }

        #endregion

        public void MovePlayer(KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case VirtualKey.Left:
                    this.player.MovePlayerLeft();
                    break;
                case VirtualKey.Right:
                    this.player.MovePlayerRight();
                    break;
                case VirtualKey.Up:
                    this.player.MovePlayerUp();
                    break;
                case VirtualKey.Down:
                    this.player.MovePlayerDown();
                    break;
            }
        }
    }
}