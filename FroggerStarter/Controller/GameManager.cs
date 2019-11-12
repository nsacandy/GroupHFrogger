using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using FroggerStarter.Model;
using FroggerStarter.View.Sprites;
using FroggerStarter.View.Sprites.PowerUpSprites;
using FroggerStarter.View.Sprites.VehicleSprites;
using FroggerStarter.ViewModel;

namespace FroggerStarter.Controller
{
    /// <summary>
    ///     Manages all aspects of the game play including moving the player,
    ///     the vehicles as well as lives and score.
    /// </summary>
    public class GameManager
    {
        #region Data members

        private Canvas gameCanvas;
        private readonly PlayerManager player;
        private readonly RoadManager roadManager;
        private HomeManager homes;
        private readonly LevelManager level;
        private readonly TimeExtender timeSprite;
        private readonly InvincibilityStar invincibilityStar;
        //private readonly HighScoreViewModel viewModel;

        private DispatcherTimer invincibilityTimer;
        private TimeSpan timerLength = new TimeSpan(0, 0, GameSettings.LifeLengthInSeconds);
        private DispatcherTimer timer;
        private DateTime startTime;
        private TimeSpan currentLifeAndPointTime;
        private int gameTimerTick;

        #endregion

        #region Properties

        /// <summary>Player's score set when they reach a lilypad based on time remaining.</summary>
        /// <value>The score.</value>
        public int Score { get; private set; }

        /// <summary>Player's remaining lives.</summary>
        public int Lives { get; private set; }

        /// <summary>Gets a value indicating whether the game has ended, when all lives have been lost.</summary>
        /// <value>
        ///     <c>true</c> if this instance final life was lost; otherwise, <c>false</c>.
        /// </value>
        public bool IsGameOver { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GameManager" /> class.
        ///     Sets initial values, sets event handling for gameOver, pointScored, nextLevel,
        ///     newSprite, and powerups
        /// </summary>
        public GameManager()
        {
            this.IsGameOver = false;

            this.roadManager = new RoadManager(GameSettings.BackgroundWidth);
            this.player = new PlayerManager(GameSettings.TopLaneOffset, GameSettings.BackgroundHeight,
                GameSettings.LeftBorder,
                GameSettings.BackgroundWidth);
            this.level = new LevelManager();
            this.timeSprite = new TimeExtender();
            this.invincibilityStar = new InvincibilityStar();
            this.invincibilityTimer = new DispatcherTimer();
            //this.viewModel = new HighScoreViewModel();

            this.currentLifeAndPointTime = this.timerLength;
            this.startTime = DateTime.Now;
            this.Lives = GameSettings.InitialNumLives;
            this.player.NewSpriteCreated += this.playerOnNewSpriteCreated;
            this.LifeLost += this.player.HandleLifeLost;
            this.PointScored += this.handlePointScored;
            this.NextLevel += this.moveToNextLevel;
            this.TimePowerUp += this.onTimeExtension;
        }

        #endregion

        #region Methods

        /// <summary>Occurs when [life lost], player collides with vehicle</summary>
        public event EventHandler LifeLost;

        /// <summary>Occurs when all lives have been lost.</summary>
        public event EventHandler GameOver;

        /// <summary>Occurs when the timer starts back after death.</summary>
        public event EventHandler GameResumed;

        /// <summary>Occurs when player has filled all lilypads in current level.</summary>
        public event EventHandler NextLevel;

        /// <summary>
        ///     <para>
        ///         Occurs when [time power up] is hit.
        ///     </para>
        /// </summary>
        public event EventHandler TimePowerUp;

        /// <summary>Occurs when player reaches a lilypad.</summary>
        public event EventHandler<ScoreArgs> PointScored;

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

        /// <summary>Gets the frog homes.</summary>
        /// <returns>An enumerable instance of lilypads</returns>
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
                    App.AppSoundEffects.PowerStarLoop.Play();

                    this.player.OnInvincibilityTriggered();
                    this.invincibilityTimer = new DispatcherTimer();
                    this.invincibilityTimer.Interval = GameSettings.InvincibilityLength;
                    this.invincibilityTimer.Start();
                    this.invincibilityTimer.Tick += (sender, o) => this.onInvincibilityTimerTick();
                    this.invincibilityTimer.Tick += (sender, o) => App.AppSoundEffects.PowerStarLoop.Pause();
                    this.invincibilityStar.OnHit();
                }
            }
        }

        private void onInvincibilityTimerTick()
        {
            this.invincibilityTimer.Stop();
            App.AppSoundEffects.PowerStarLoop.Pause();
        }

        private void onTimeExtension(object sender, EventArgs e)
        {
            this.timeSprite.OnHit();
            this.currentLifeAndPointTime -= new TimeSpan(0, 0, 5);
            this.currentLifeAndPointTime -= new TimeSpan(0, 0, 4);
        }

        private void showTimeSprite()
        {
            if (this.currentLifeAndPointTime.Seconds % GameSettings.TimeSpriteAppearInterval == 0 && !this.timeSprite.IsShowing)
            {
                this.timeSprite.RandomlyShow();
            }
        }

        private void showInvincibilityStarSprite()
        {
            if (this.currentLifeAndPointTime.Seconds % GameSettings.InvincibilityAppearInterval == 0 && !this.invincibilityStar.IsShowing)
            {
                this.invincibilityStar.RandomlyShow();
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
                if (this.homes.IsAllHomesFilled() && !this.level.CurrentLevel.Equals(LevelManager.GameLevel.Three))
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
            if (this.homes.IsAllHomesFilled() && this.level.CurrentLevel.Equals(LevelManager.GameLevel.Three))
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
            var centeredX = GameSettings.BackgroundWidth / 2 - this.player.PlayerSprite.Width / 2;
            var centeredY = GameSettings.BackgroundHeight - this.player.PlayerSprite.Height -
                            GameSettings.BottomLaneOffset;

            this.player.LastLocationTracker(centeredX, centeredY);
        }

        private void checkForPlayerVehicleCollisionAsync()
        {
            if (this.invincibilityTimer.IsEnabled)
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

        /// <summary>Moves the player.</summary>
        /// <postcondition>Player X or Y different</postcondition>
        /// <param name="args">The <see cref="KeyEventArgs" /> instance containing the event data.</param>
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

        //public void MakeHighScorePlayer(string name)
        //{
        //    this.viewModel.AddPlayerToHighScore(this.level.CurrentLevel, this.Score, name);
        //}

        /// <summary>Custom EventArgs class made to pass score and location data</summary>
        /// <seealso cref="System.EventArgs" />
        public class ScoreArgs : EventArgs
        {
            #region Properties

            /// <summary>Gets the lily pad that fired the event.</summary>
            /// <value>The lily pad.</value>
            public LilyPad LilyPad { get; }

            #endregion

            #region Constructors

            /// <summary>Initializes a new instance of the <see cref="ScoreArgs" /> class.</summary>
            /// <param name="hitLilyPad">The hit lily pad.</param>
            public ScoreArgs(LilyPad hitLilyPad)
            {
                this.LilyPad = hitLilyPad;
            }

            #endregion
        }

        #endregion
    }
}