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

        private readonly double backgroundHeight;
        private readonly double backgroundWidth;

        private readonly double TopLaneOffset = (double)App.Current.Resources["HighRoadYLocation"];
        private const int BottomLaneOffset = 5;
        
        public readonly double TopBorder = (double)App.Current.Resources["HighRoadYLocation"] + (double)App.Current.Resources["RoadHeight"];
        private readonly double leftBorder = 0;
        private Canvas gameCanvas;
        private PlayerManager player;
        private DispatcherTimer timer;
        private readonly RoadManager roadManager;
        private DateTime startTime;

        public readonly TimeSpan timerLength = new TimeSpan(0,0,20);

        private TimeSpan currentLifeAndPointTime;

        

        private List<UIElement> gameObjectsToBeAddedToCanvas;
        

        public List<LilyPad> LandingSpots { get; private set; }
        public int Score { get; private set; }
        public int Lives { get; private set; } = 4;

        public delegate void LifeLostHandler(object sender, EventArgs e);
        public event LifeLostHandler LifeLost;

        public delegate void GameOverHandler(object sender, EventArgs e);
        public event GameOverHandler GameOver;

        public event EventHandler<ScoreArgs> PointScored;
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
            this.player = new PlayerManager(this.TopLaneOffset, this.backgroundHeight, 0, this.backgroundWidth);
            this.gameObjectsToBeAddedToCanvas = new List<UIElement>();
            
            this.currentLifeAndPointTime = this.timerLength;
            LifeLost += this.handleLifeLost;
            GameOver += this.handleGameOver;
            PointScored += this.handlePointScored;
            this.startTime = DateTime.Now;
        }

        private void handlePointScored(object sender, ScoreArgs e)
        {
            this.setPlayerToCenterOfBottomLane();
            this.updateScore(e);
        }

        #endregion

        #region Methods

        private void setupGameTimer()
        {
            this.timer = new DispatcherTimer();
            this.timer.Tick += this.timerOnTick;
            this.timer.Interval = new TimeSpan(0, 0, 0, 0, 15);
            this.timer.Start();
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
            this.setupGameTimer();
            this.createAndPlacePlayer();
            this.addVehiclesToCanvasObjectsList();
            this.addLandingSpotsToCanvasList();
            this.gameCanvas = gamePage ?? throw new ArgumentNullException(nameof(gamePage));
            this.addObjectsToCanvas();
        }

        public void addObjectsToCanvas()
        {
            foreach (var UIElement in this.gameObjectsToBeAddedToCanvas)
            {
                this.gameCanvas.Children.Add(UIElement);
            }
        }

        private void addVehiclesToCanvasObjectsList()
        {
            foreach (var lane in this.roadManager)
            {
                foreach (var vehicle in lane)
                {
                    this.gameObjectsToBeAddedToCanvas.Add(vehicle.Sprite);
                }
            }
        }

        private void createAndPlacePlayer()
        {
            this.gameObjectsToBeAddedToCanvas.Add(this.player.PlayerSprite);
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
            this.currentLifeAndPointTime = (DateTime.Now - this.startTime);
            this.checkForPlayerVehicleCollision();
            this.checkForPointScored();
            this.checkRemainingTime();

            this.roadManager.moveAllVehicles();
        }

        private void checkRemainingTime()
        {
            if (this.currentLifeAndPointTime.Seconds >= this.timerLength.Seconds)
            {
                this.RaiseLifeLost();
            }
        }

        private void checkForPointScored()
        {
            var playerBox = this.player.PlayerSprite.HitBox;
            var objectsAtPlayerLocation = VisualTreeHelper.FindElementsInHostCoordinates(playerBox, null);

            foreach (var uiElement in objectsAtPlayerLocation)
            {
                if (uiElement is LilyPad pad)
                {
                    this.OnRaisePointScored(new ScoreArgs(uiElement as LilyPad));
                }

                else if (playerBox.Y < this.TopBorder)
                {
                    this.player.resetPlayerToPreviousPosition();
                }
            }
        }

        protected virtual void OnRaisePointScored(ScoreArgs lilyPadHitBox)
        {
            EventHandler<ScoreArgs> handler = PointScored;

            if (handler != null)
            {
                handler(this, lilyPadHitBox);
            }
        }

        private void checkForPlayerVehicleCollision()
        {
            var playerBox = this.player.PlayerSprite.HitBox;
            var objectsAtPlayerLocation = VisualTreeHelper.FindElementsInHostCoordinates(playerBox, null);

            foreach (var uiElement in objectsAtPlayerLocation)
            {
                if (uiElement is CarSprite || uiElement is TruckSprite)
                {
                    this.RaiseLifeLost();
                }
            }
        }

        private void updateScore(ScoreArgs e)
        {
            this.Score += this.timerLength.Seconds - this.currentLifeAndPointTime.Seconds;
            this.startTime = DateTime.Now;
            this.LandingSpots.Remove(e.LilyPad);
            if (LandingSpots.Count == 0)
            {
                this.RaiseGameOver();
            }
        }

        private void RaiseLifeLost()
        {
            this.Lives -= 1;
            this.startTime = DateTime.Now;
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
                this.roadManager.resetNumVehicles();
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

        private void addLandingSpotsToCanvasList()
        {
            this.LandingSpots = new List<LilyPad>();
            int numLandingSpots = 5;
            for (int i = 0; i < numLandingSpots; i++)
            {
                LilyPad newLandingSpot = new LilyPad();

                double xLocation = (this.backgroundWidth / numLandingSpots) * i;
                double yLocation = (double) Application.Current.Resources["HighRoadYLocation"];

                newLandingSpot.RenderAt(xLocation, yLocation);
                
                this.LandingSpots.Add(newLandingSpot);
                this.gameObjectsToBeAddedToCanvas.Add(newLandingSpot);
            }
        }

        public class ScoreArgs : EventArgs
        {
            public ScoreArgs(LilyPad hitLilyPad)
            {
                this.lilyPad = hitLilyPad;
            }

            private LilyPad lilyPad;

            public LilyPad LilyPad
            {
                get { return this.lilyPad; }
            }
        }
    }
}