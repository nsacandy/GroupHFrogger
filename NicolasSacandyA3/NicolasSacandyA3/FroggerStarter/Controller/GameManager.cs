using System;
using System.Collections.Generic;
using Windows.System;
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
        #region Types and Delegates

        public delegate void GameOverHandler(object sender, EventArgs e);

        public delegate void GameResumedHandler(object sender, EventArgs e);

        public delegate void LifeLostHandler(object sender, EventArgs e);

        #endregion

        #region Data members

        private const int BottomLaneOffset = 5;

        public readonly double TopBorder = (double) Application.Current.Resources["HighRoadYLocation"] +
                                           (double) Application.Current.Resources["RoadHeight"];

        public readonly TimeSpan TimerLength = new TimeSpan(0, 0, 20);

        private readonly double backgroundHeight;
        private readonly double backgroundWidth;

        private readonly double topLaneOffset = (double) Application.Current.Resources["HighRoadYLocation"];
        public readonly double LeftBorder = 0;
        private Canvas gameCanvas;
        private readonly PlayerManager player;
        private DispatcherTimer timer;
        private readonly RoadManager roadManager;
        private DateTime startTime;

        private TimeSpan currentLifeAndPointTime;

        private readonly List<UIElement> gameObjectsToBeAddedToCanvas;

        #endregion

        #region Properties

        public List<LilyPad> LandingSpots { get; private set; }
        public int Score { get; private set; }
        public int Lives { get; private set; } = 4;

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
            this.player = new PlayerManager(this.topLaneOffset, this.backgroundHeight, 0, this.backgroundWidth);
            this.gameObjectsToBeAddedToCanvas = new List<UIElement>();

            this.currentLifeAndPointTime = this.TimerLength;
            this.LifeLost += this.player.handleLifeLost;
            this.GameOver += this.handleGameOver;
            this.PointScored += this.handlePointScored;
            this.player.NewSpriteCreated += this.playerOnNewSpriteCreated;
            this.startTime = DateTime.Now;
        }

        #endregion

        #region Methods

        public event LifeLostHandler LifeLost;
        public event GameOverHandler GameOver;
        public event GameResumedHandler GameResumed;

        public event EventHandler<ScoreArgs> PointScored;

        private void playerOnNewSpriteCreated(object sender, EventArgs e)
        {
            this.setPlayerToCenterOfBottomLane();
            this.timer.Start();
            this.startTime = DateTime.Now;
            this.onGameResumed();
        }

        private void onGameResumed()
        {
            this.GameResumed?.Invoke(this, null);
        }

        private void handlePointScored(object sender, ScoreArgs e)
        {
            this.setPlayerToCenterOfBottomLane();
            this.updateScore(e);
        }

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
            this.AddObjectsToCanvas();
        }

        public void AddObjectsToCanvas()
        {
            foreach (var uiElement in this.gameObjectsToBeAddedToCanvas)
            {
                this.gameCanvas.Children.Add(uiElement);
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
            this.currentLifeAndPointTime = DateTime.Now - this.startTime;
            this.checkForPlayerVehicleCollision();
            this.checkForPointScored();
            this.checkRemainingTime();

            this.roadManager.moveAllVehicles();
        }

        private void checkRemainingTime()
        {
            if (this.currentLifeAndPointTime.Seconds >= this.TimerLength.Seconds)
            {
                this.onLifeLost();
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
                    this.OnRaisePointScored(new ScoreArgs(pad));
                }

                else if (playerBox.Y < this.TopBorder)
                {
                    this.player.resetPlayerToPreviousPosition();
                }
            }
        }

        protected virtual void OnRaisePointScored(ScoreArgs lilyPadHitBox)
        {
            var handler = this.PointScored;

            handler?.Invoke(this, lilyPadHitBox);
        }

        private void checkForPlayerVehicleCollision()
        {
            var playerBox = this.player.PlayerSprite.HitBox;
            var objectsAtPlayerLocation = VisualTreeHelper.FindElementsInHostCoordinates(playerBox, null);

            foreach (var uiElement in objectsAtPlayerLocation)
            {
                if (uiElement is CarSprite || uiElement is TruckSprite)
                {
                    this.onLifeLost();
                }
            }
        }

        private void updateScore(ScoreArgs e)
        {
            this.Score += this.TimerLength.Seconds - this.currentLifeAndPointTime.Seconds;
            this.startTime = DateTime.Now;
            this.LandingSpots.Remove(e.LilyPad);
            if (this.LandingSpots.Count == 0)
            {
                this.raiseGameOver();
            }
        }

        private void onLifeLost()
        {
            this.handleLifeLost();
            this.LifeLost?.Invoke(this, null);
        }

        private void handleLifeLost()
        {
            this.timer.Stop();
            this.Lives -= 1;
            if (this.Lives == 0)
            {
                
                this.raiseGameOver();
            }

            else
            {
                this.roadManager.resetNumVehicles();
            }
        }

        private void raiseGameOver()
        {
            this.GameOver?.Invoke(this, null);
        }

        private void handleGameOver(object sender, EventArgs e)
        {
            this.timer.Stop();
            this.player.PlayerSprite.Visibility = Visibility.Collapsed;
        }

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
            var numLandingSpots = 5;
            for (var i = 0; i < numLandingSpots; i++)
            {
                var newLandingSpot = new LilyPad();

                var xLocation = this.backgroundWidth / numLandingSpots * i;
                var yLocation = (double) Application.Current.Resources["HighRoadYLocation"];

                newLandingSpot.RenderAt(xLocation, yLocation);

                this.LandingSpots.Add(newLandingSpot);
                this.gameObjectsToBeAddedToCanvas.Add(newLandingSpot);
            }
        }

        public class ScoreArgs : EventArgs
        {
            #region Data members

            #endregion

            #region Properties

            public LilyPad LilyPad { get; }

            #endregion

            #region Constructors

            public ScoreArgs(LilyPad hitLilyPad)
            {
                this.LilyPad = hitLilyPad;
            }

            #endregion
        }

        #endregion
    }
}