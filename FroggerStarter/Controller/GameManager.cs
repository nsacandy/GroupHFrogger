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

namespace FroggerStarter.Controller
{
    /// <summary>
    ///     Manages all aspects of the game play including moving the player,
    ///     the vehicles as well as lives and score.
    /// </summary>
    public class GameManager
    {
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
            if (backgroundHeight <= 0) throw new ArgumentOutOfRangeException(nameof(backgroundHeight));

            if (backgroundWidth <= 0) throw new ArgumentOutOfRangeException(nameof(backgroundWidth));

            this.backgroundHeight = backgroundHeight;
            this.backgroundWidth = backgroundWidth;

            IsGameOver = false;

            roadManager = new RoadManager(this.backgroundWidth);
            player = new PlayerManager(GameSettings.TOP_LANE_OFFSET, this.backgroundHeight, GameSettings.leftBorder,
                this.backgroundWidth);
            level = new LevelManager();
            timeSprite = new TimeExtender();
            invincibilityStar = new InvincibilityStar();
            invincibilityTimer = new TimeSpan(0, 0, 0);

            currentLifeAndPointTime = timerLength;
            startTime = DateTime.Now;

            player.NewSpriteCreated += playerOnNewSpriteCreated;
            LifeLost += player.HandleLifeLost;
            PointScored += handlePointScored;
            NextLevel += moveToNextLevel;
            TimePowerUp += onTimeExtention;
        }

        #endregion

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
        private readonly LevelManager level;
        private readonly TimeExtender timeSprite;
        private readonly InvincibilityStar invincibilityStar;

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

        #region Methods

        private void setupGameTimer()
        {
            timer = new DispatcherTimer();
            timer.Tick += timerOnTick;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 15);
            timer.Start();
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
            gameCanvas = gamePage ?? throw new ArgumentNullException(nameof(gamePage));
            setupGameTimer();
            createAndPlacePlayer();
            addVehiclesToCanvas();
            addTimeSpriteToCanvas();
            addInvincibilityStarToCanvas();
            homes = new HomeManager(gameCanvas);
        }

        public IEnumerable<LilyPad> GetFrogHomes()
        {
            return homes;
        }

        private void addTimeSpriteToCanvas()
        {
            gameCanvas.Children.Add(timeSprite.Sprite);
        }

        private void addInvincibilityStarToCanvas()
        {
            gameCanvas.Children.Add(invincibilityStar.Sprite);
        }

        private void addVehiclesToCanvas()
        {
            foreach (var vehicle in roadManager) gameCanvas.Children.Add(vehicle.Sprite);
        }

        private void removeVehiclesFromCanvas()
        {
            foreach (var vehicle in roadManager) gameCanvas.Children.Remove(vehicle.Sprite);
        }

        private void moveToNextLevel(object sender, EventArgs e)
        {
            App.AppSoundEffects.Play(Sounds.LevelComplete);
            removeVehiclesFromCanvas();
            level.MoveToNextLevel();
            roadManager.SetLanesByLevel(level.CurrentLevel);
            addVehiclesToCanvas();
            homes.ResetLandingSpots();
        }

        private void timerOnTick(object sender, object e)
        {
            if (invincibilityTimer.Seconds >= 0) invincibilityTimer -= timer.Interval;
            gameTimerTick++;
            currentLifeAndPointTime = DateTime.Now - startTime;
            showTimeSprite();
            showInvincibilityStarSprite();
            checkForInvincibilityStarCollision();
            checkForPlayerVehicleCollisionAsync();
            checkForPointScored();
            checkForTimeSpriteCollision();
            checkRemainingTime();
            roadManager.moveAllVehicles();
        }

        private void checkForTimeSpriteCollision()
        {
            var playerBox = player.PlayerSprite.HitBox;
            var objectsAtPlayerLocation = VisualTreeHelper.FindElementsInHostCoordinates(playerBox, null);

            foreach (var uiElement in objectsAtPlayerLocation)
                if (uiElement is TimeExtenderSprite)
                {
                    App.AppSoundEffects.Play(Sounds.PowerUpTime);
                    TimePowerUp?.Invoke(this, null);
                }
        }

        private void checkForInvincibilityStarCollision()
        {
            var playerBox = player.PlayerSprite.HitBox;
            var objectsAtPlayerLocation = VisualTreeHelper.FindElementsInHostCoordinates(playerBox, null);

            foreach (var uiElement in objectsAtPlayerLocation)
                if (uiElement is InvincibilityStarSprite)
                {
                    App.AppSoundEffects.Play(Sounds.PowerUpStar);
                    player.onInvincibilityTriggered();
                    invincibilityTimer = GameSettings.InvincibilityLength;
                    invincibilityStar.OnHit();
                }
        }

        private void onTimeExtention(object sender, EventArgs e)
        {
            timeSprite.OnHit();
            currentLifeAndPointTime -= new TimeSpan(0, 0, 5);
            currentLifeAndPointTime -= new TimeSpan(0, 0, 4);
        }

        private void showTimeSprite()
        {
            if (gameTimerTick % GameSettings.TimeSpriteShowInterval == 0 && !timeSprite.IsShowing) timeSprite.Show();
        }

        private void showInvincibilityStarSprite()
        {
            if (gameTimerTick % GameSettings.TimeSpriteShowInterval == 0 && !invincibilityStar.IsShowing)
                invincibilityStar.Show();
        }

        private void checkRemainingTime()
        {
            if (currentLifeAndPointTime.Seconds >= timerLength.Seconds)
            {
                App.AppSoundEffects.Play(Sounds.TimeOut);
                onLifeLost();
            }
        }

        private void checkForPointScored()
        {
            var playerBox = player.PlayerSprite.HitBox;
            var objectsAtPlayerLocation = VisualTreeHelper.FindElementsInHostCoordinates(playerBox, null);

            foreach (var uiElement in objectsAtPlayerLocation)
            {
                handlePlayerHitsHome(uiElement);
                handlePlayerDoesNotHitHome(playerBox);
            }
        }

        private void handlePlayerDoesNotHitHome(Rect playerBox)
        {
            if (playerBox.Y < GameSettings.TopBorder) player.ResetPlayerToPreviousPosition();
        }

        private void handlePlayerHitsHome(UIElement uiElement)
        {
            if (uiElement is LilyPad pad)
            {
                App.AppSoundEffects.Play(Sounds.LandHome);
                PointScored?.Invoke(this, new ScoreArgs(pad));
                if (homes.IsAllHomesFilled() && !level.CurrentLevel.Equals(LevelManager.GameLevel.Final))
                    NextLevel?.Invoke(this, null);
            }
        }

        private void handlePointScored(object sender, ScoreArgs e)
        {
            setPlayerToCenterOfBottomLane();
            homes.RemoveHome(e.LilyPad);
            updateScore(e);
            if (homes.IsAllHomesFilled() && level.CurrentLevel.Equals(LevelManager.GameLevel.Final)) raiseGameOver();
        }

        private void updateScore(ScoreArgs e)
        {
            Score += timerLength.Seconds - currentLifeAndPointTime.Seconds;
            startTime = DateTime.Now;
        }

        private void createAndPlacePlayer()
        {
            gameCanvas.Children.Add(player.PlayerSprite);
            setPlayerToCenterOfBottomLane();
        }

        private void setPlayerToCenterOfBottomLane()
        {
            var centeredX = backgroundWidth / 2 - player.PlayerSprite.Width / 2;
            var centeredY = backgroundHeight - player.PlayerSprite.Height - GameSettings.bottomLaneOffset;

            player.SetPlayerLocation(centeredX, centeredY);
        }

        private void checkForPlayerVehicleCollisionAsync()
        {
            if (invincibilityTimer.TotalSeconds > 0) return;
            var playerBox = player.PlayerSprite.HitBox;
            var objectsAtPlayerLocation = VisualTreeHelper.FindElementsInHostCoordinates(playerBox, null);

            foreach (var uiElement in objectsAtPlayerLocation)
                if (uiElement is VehicleSprite)
                {
                    App.AppSoundEffects.Play(Sounds.HitVehicle);
                    onLifeLost();
                    break;
                }
        }

        private void onLifeLost()
        {
            handleLifeLost();
            LifeLost?.Invoke(this, null);
        }

        private void handleLifeLost()
        {
            timer.Stop();
            Lives -= 1;
            if (Lives == 0)
            {
                handleGameOver();
                raiseGameOver();
            }
            else
            {
                roadManager.resetNumVehicles();
            }
        }

        private void playerOnNewSpriteCreated(object sender, EventArgs e)
        {
            if (IsGameOver)
            {
                player.PlayerSprite.Visibility = Visibility.Collapsed;
                return;
            }

            setPlayerToCenterOfBottomLane();
            timer.Start();
            startTime = DateTime.Now;
            onGameResumed();
        }

        private void onGameResumed()
        {
            GameResumed?.Invoke(this, null);
        }

        private void raiseGameOver()
        {
            GameOver?.Invoke(this, null);
        }

        private void handleGameOver()
        {
            App.AppSoundEffects.Play(Sounds.GameOver);
            timer.Stop();
            IsGameOver = true;
            roadManager.CollapseAllVehicles();
        }

        public void MovePlayer(KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case VirtualKey.Left:
                    player.MovePlayerLeft();
                    break;
                case VirtualKey.Right:
                    player.MovePlayerRight();
                    break;
                case VirtualKey.Up:
                    player.MovePlayerUp();
                    break;
                case VirtualKey.Down:
                    player.MovePlayerDown();
                    break;
            }
        }

        public class ScoreArgs : EventArgs
        {
            #region Constructors

            public ScoreArgs(LilyPad hitLilyPad)
            {
                LilyPad = hitLilyPad;
            }

            #endregion

            #region Properties

            public LilyPad LilyPad { get; }

            #endregion

            #region Data members

            #endregion
        }

        #endregion
    }
}