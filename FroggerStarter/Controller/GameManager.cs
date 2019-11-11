using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using FroggerStarter.Model;
using FroggerStarter.Model.Vehicles;
using FroggerStarter.View.Sprites;
using FroggerStarter.View.Sprites.PowerUpSprites;
using FroggerStarter.View.Sprites.VehicleSprites;

namespace FroggerStarter.Controller
{
    /// <summary>
    ///     Manages all aspects of the game play including moving the player,
    ///     the vehicles as well as lives and score.
    /// </summary>
    public class GameManager
    {

        #region Types and Delegates


        public event EventHandler LifeLost;
        public event EventHandler GameOver;
        public event EventHandler GameResumed;
        public event EventHandler NextLevel;
        public event EventHandler TimePowerUp;

        public event EventHandler<ScoreArgs> PointScored;

        #endregion

        #region Data members

        
        private readonly double backgroundHeight;
        private readonly double backgroundWidth;

        private Canvas gameCanvas;
        private readonly PlayerManager player;
        private readonly RoadManager roadManager;
        private HomeManager homes;
        private LevelManager level;
        private TimeExtender timeSprite;
        private InvincibilityStar invincibilityStar;

        private TimeSpan invincibilityTimer;
        private TimeSpan timerLength = new TimeSpan(0, 0, GameSettings.GameLengthInSeconds);
        private DispatcherTimer timer;
        private DateTime startTime;
        private TimeSpan currentLifeAndPointTime;
        private int gameTimerTick;
        private bool playerIsInvincible;

        #endregion

        #region Properties


        public int Score { get; private set; }
        public int Lives { get; private set; } = 4;
        public bool IsGameOver { get; private set; }

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

            this.IsGameOver = false;

            this.roadManager = new RoadManager(this.backgroundWidth);
            this.player = new PlayerManager(GameSettings.TOP_LANE_OFFSET, this.backgroundHeight, GameSettings.leftBorder, this.backgroundWidth);
            this.level = new LevelManager();
            this.timeSprite = new TimeExtender();
            this.invincibilityStar = new InvincibilityStar();
            this.invincibilityTimer = new TimeSpan(0,0,0);

            this.currentLifeAndPointTime = this.timerLength;
            this.startTime = DateTime.Now;

            this.player.NewSpriteCreated += this.playerOnNewSpriteCreated;
            this.LifeLost += this.player.HandleLifeLost;
            this.PointScored += this.handlePointScored;
            this.NextLevel += this.moveToNextLevel;
            this.TimePowerUp += this.onTimeExtention;
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
            this.gameCanvas = gamePage ?? throw new ArgumentNullException(nameof(gamePage));
            this.setupGameTimer();
            this.createAndPlacePlayer();
            this.addVehiclesToCanvas();
            this.addTimeSpriteToCanvas();
            this.addInvincibilityStarToCanvas();
            this.homes = new HomeManager(this.gameCanvas);
        }

        public IEnumerable<LilyPad> GetFrogHomes()
        {
            return this.homes;
        }

        private void addTimeSpriteToCanvas()
        {
            this.gameCanvas.Children.Add(this.timeSprite.Sprite);
        }

        private void addInvincibilityStarToCanvas()
        {
            this.gameCanvas.Children.Add(this.invincibilityStar.Sprite);
        }

        private void addVehiclesToCanvas()
        {
            foreach (var vehicle in this.roadManager)
            {
                this.gameCanvas.Children.Add(vehicle.Sprite);
            }
        }

        private void removeVehiclesFromCanvas()
        {
            foreach (var vehicle in this.roadManager)
            {
                this.gameCanvas.Children.Remove(vehicle.Sprite);
            }
        }

        private void moveToNextLevel(object sender, EventArgs e)
        {
            App.AppSoundEffects.Play(Sounds.LevelComplete);
            this.removeVehiclesFromCanvas();
            this.level.MoveToNextLevel();
            this.roadManager.SetLanesByLevel(this.level.CurrentLevel);
            this.addVehiclesToCanvas();
            this.homes.ResetLandingSpots();
        }

        private void timerOnTick(object sender, object e)
        {
            if (this.invincibilityTimer.Seconds >= 0)
            {
                this.invincibilityTimer -= this.timer.Interval;
            }
            this.gameTimerTick++;
            this.currentLifeAndPointTime = DateTime.Now - this.startTime;
            this.showTimeSprite();
            this.showInvincibilityStarSprite();
            this.checkForInvincibilityStarCollision();
            this.checkForPlayerVehicleCollisionAsync();
            this.checkForPointScored();
            this.checkForTimeSpriteCollision();
            this.checkRemainingTime();
            this.roadManager.moveAllVehicles();
        }

        private void checkForTimeSpriteCollision()
        {
            var playerBox = this.player.PlayerSprite.HitBox;
            var objectsAtPlayerLocation = VisualTreeHelper.FindElementsInHostCoordinates(playerBox, null);

            foreach (var uiElement in objectsAtPlayerLocation)
            {
                if (uiElement is TimeExtenderSprite)
                {
                    App.AppSoundEffects.Play(Sounds.PowerUpTime);
                    this.TimePowerUp?.Invoke(this, null);
                }
            }
        }

        private void checkForInvincibilityStarCollision()
        {
            var playerBox = this.player.PlayerSprite.HitBox;
            var objectsAtPlayerLocation = VisualTreeHelper.FindElementsInHostCoordinates(playerBox, null);

            foreach (var uiElement in objectsAtPlayerLocation)
            {
                if (uiElement is InvincibilityStarSprite)
                {
                    App.AppSoundEffects.Play(Sounds.PowerUpStar);
                    this.player.onInvincibilityTriggered();
                    this.invincibilityTimer = GameSettings.InvincibilityLength;
                    this.invincibilityStar.OnHit();
                }
            }
        }

        private void onTimeExtention(object sender, EventArgs e)
        {
            this.timeSprite.OnHit();
            this.currentLifeAndPointTime -= new TimeSpan(0, 0, 5);
            this.currentLifeAndPointTime -= new TimeSpan(0, 0, 4);
        }

        private void showTimeSprite()
        {
            if (this.gameTimerTick % GameSettings.TimeSpriteShowInterval == 0 && !this.timeSprite.IsShowing)
            {
                this.timeSprite.Show();
            }
        }

        private void showInvincibilityStarSprite()
        {
            if (this.gameTimerTick % GameSettings.TimeSpriteShowInterval == 0 && !this.invincibilityStar.IsShowing)
            {
                this.invincibilityStar.Show();
            }
        }

        private void checkRemainingTime()
        {
            if (this.currentLifeAndPointTime.Seconds >= this.timerLength.Seconds)
            {
                App.AppSoundEffects.Play(Sounds.TimeOut);
                this.onLifeLost();
            }
        }

        private void checkForPointScored()
        {
            var playerBox = this.player.PlayerSprite.HitBox;
            var objectsAtPlayerLocation = VisualTreeHelper.FindElementsInHostCoordinates(playerBox, null);

            foreach (var uiElement in objectsAtPlayerLocation)
            {
                this.handlePlayerHitsHome(uiElement);
                this.handlePlayerDoesNotHitHome(playerBox);
            }
        }

        private void handlePlayerDoesNotHitHome(Rect playerBox)
        {
            if (playerBox.Y < GameSettings.TopBorder)
            {
                this.player.ResetPlayerToPreviousPosition();
            }
        }

        private void handlePlayerHitsHome(UIElement uiElement)
        {
            if (uiElement is LilyPad pad)
            {
                App.AppSoundEffects.Play(Sounds.LandHome);
                this.PointScored?.Invoke(this, new ScoreArgs(pad));
                if (this.homes.IsAllHomesFilled() && !this.level.CurrentLevel.Equals(LevelManager.GameLevel.Final))
                {
                    this.NextLevel?.Invoke(this, null);
                }
            }
        }

        private void handlePointScored(object sender, ScoreArgs e)
        {
            this.setPlayerToCenterOfBottomLane();
            this.homes.RemoveHome(e.LilyPad);
            this.updateScore(e);
            if (this.homes.IsAllHomesFilled() && this.level.CurrentLevel.Equals(LevelManager.GameLevel.Final))
            { 
                this.raiseGameOver();
            }
        }

        private void updateScore(ScoreArgs e)
        {
            this.Score += this.timerLength.Seconds - this.currentLifeAndPointTime.Seconds;
            this.startTime = DateTime.Now;
        }

        private void createAndPlacePlayer()
        {
            this.gameCanvas.Children.Add(this.player.PlayerSprite);
            this.setPlayerToCenterOfBottomLane();
        }

        private void setPlayerToCenterOfBottomLane()
        {
            var centeredX = this.backgroundWidth / 2 - this.player.PlayerSprite.Width / 2;
            var centeredY = this.backgroundHeight - this.player.PlayerSprite.Height - GameSettings.bottomLaneOffset;

            this.player.SetPlayerLocation(centeredX, centeredY);
        }

        private void checkForPlayerVehicleCollisionAsync()
        {
            if (this.invincibilityTimer.TotalSeconds > 0)
            {
                return;
            }
            var playerBox = this.player.PlayerSprite.HitBox;
            var objectsAtPlayerLocation = VisualTreeHelper.FindElementsInHostCoordinates(playerBox, null);

            foreach (var uiElement in objectsAtPlayerLocation)
            {
                if (uiElement is IVehicleSprite)
                {
                    App.AppSoundEffects.Play(Sounds.HitVehicle);
                    this.onLifeLost();
                    break;
                }
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
                this.handleGameOver();
                this.raiseGameOver();
            }
            else
            {
                this.roadManager.resetNumVehicles();
            }
        }

        private void playerOnNewSpriteCreated(object sender, EventArgs e)
        {
            if (this.IsGameOver)
            {
                this.player.PlayerSprite.Visibility = Visibility.Collapsed;
                return;
            }

            this.setPlayerToCenterOfBottomLane();
            this.timer.Start();
            this.startTime = DateTime.Now;
            this.onGameResumed();
        }

        private void onGameResumed()
        {
            this.GameResumed?.Invoke(this, null);
        }

        private void raiseGameOver()
        {
            this.GameOver?.Invoke(this, null);
        }

        private void handleGameOver()
        {
            App.AppSoundEffects.Play(Sounds.GameOver);
            this.timer.Stop();
            this.IsGameOver = true;
            this.roadManager.CollapseAllVehicles();
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