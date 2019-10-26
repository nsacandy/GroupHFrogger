using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI;
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

        private const int IntervalBetweenSpeedIncrease = 1;
        private readonly double TopLaneOffset = (double)App.Current.Resources["HighRoadYLocation"];
        private const int BottomLaneOffset = 5;
        private readonly double backgroundHeight;
        private readonly double backgroundWidth;
        public readonly double TopBorder = (double)App.Current.Resources["HighRoadYLocation"] + (double)App.Current.Resources["RoadHeight"];
        private readonly double leftBorder = 0;
        private Canvas gameCanvas;
        private Frog player;
        private DispatcherTimer timer;
        private readonly RoadManager roadManager;
        private DateTime runningTime;
        private int score;
        private TextBlock scoreDisplay;
        public int Lives { get; private set; } = 4;

        public delegate void LifeLostHandler(object sender, EventArgs e);
        public event LifeLostHandler LifeLost;

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
            this.setupGameTimer();
            
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
//            this.GenerateLives();
            this.createScoreDisplay();

        }

        private void createScoreDisplay()
        {
            this.scoreDisplay = new TextBlock {
                Foreground = new SolidColorBrush(Colors.White), FontSize = 30, Text = "Score: " + this.score
            };
            this.gameCanvas.Children.Add(this.scoreDisplay);
           Canvas.SetLeft(this.scoreDisplay,this.gameCanvas.Width - 150);
           Canvas.SetTop(this.scoreDisplay,10);
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
            this.player = new Frog();
            this.gameCanvas.Children.Add(this.player.Sprite);
            this.setPlayerToCenterOfBottomLane();
        }

        private void setPlayerToCenterOfBottomLane()
        {
            this.player.X = this.backgroundWidth / 2 - this.player.Width / 2;
            this.player.Y = this.backgroundHeight - this.player.Height - BottomLaneOffset;
            
        }

        private void timerOnTick(object sender, object e)
        {
            this.increaseLaneSpeedIfApplicable();
            this.checkForCollision();

            if (this.player.Y < this.TopBorder)
            {
                this.updateScore();
            }

            this.roadManager.moveAllVehicles();
        }

        private void increaseLaneSpeedIfApplicable()
        {
            var sinceLastUpdate = DateTime.Now - this.runningTime;
            if (sinceLastUpdate.Seconds <= IntervalBetweenSpeedIncrease)
            {
                return;
            }

            this.roadManager.increaseAllLaneSpeeds();
            this.runningTime = DateTime.Now;
        }

        private void checkForCollision()
        {
            var playerBox = new Rect(this.player.X, this.player.Y, this.player.Width, this.player.Height);
            var objectsAtPlayerLocation =
                VisualTreeHelper.FindElementsInHostCoordinates(playerBox, null);
            foreach (var uiElement in objectsAtPlayerLocation)
            {
                if (uiElement is BaseSprite)
                {
                    this.RaiseLifeLost();
                    
                    this.setPlayerToCenterOfBottomLane();
                }
            }
        }

        private void updateScore()
        {
            this.score += 1;
            this.scoreDisplay.Text = "Score: " + this.score;
            this.setPlayerToCenterOfBottomLane();
            if (this.score == 3)
            {
                this.handleGameOver();
            }
        }

        //TODO handle new gameover
        private void RaiseLifeLost()
        {
            this.Lives -= 1;
            if (LifeLost != null)
                LifeLost(this, null);
            this.roadManager.resetLaneSpeeds();

        }

        private void handleGameOver()
        {
            this.timer.Stop();
            this.player.Sprite.Visibility = Visibility.Collapsed;
            var gameOver = new TextBlock {
                Text = "Game Over",
                Foreground = new SolidColorBrush(Colors.White),
                FontSize = 30,
                TextAlignment = TextAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            this.gameCanvas.Children.Add(gameOver);
            Canvas.SetLeft(gameOver, this.backgroundWidth/2 - 50);
            
            Canvas.SetTop(gameOver, this.backgroundHeight/2);
        }

        /// <summary>
        ///     Moves the player to the left.
        ///     Precondition: none
        ///     Postcondition: player.X = player.X@prev - player.Width
        /// </summary>
        public void MovePlayerLeft()
        {
            double previousX = this.player.X;
            this.player.MoveLeft();
            if (this.player.X < 0)
            {
                this.player.X = previousX;
            }
        }

        /// <summary>
        ///     Moves the player to the right.
        ///     Precondition: none
        ///     Postcondition: player.X = player.X@prev + player.Width
        /// </summary>
        public void MovePlayerRight()
        {
            var previousX = this.player.X;
            this.player.MoveRight();
            if (this.player.X + this.player.Width >  this.backgroundWidth)
            {
                this.player.X = previousX;
            }
        }

        /// <summary>
        ///     Moves the player up.
        ///     Precondition: none
        ///     Postcondition: player.Y = player.Y@prev - player.Height
        /// </summary>
        public void MovePlayerUp()
        {
            var previousY = this.player.Y;
            this.player.MoveUp();
        }

        /// <summary>
        ///     Moves the player down.
        ///     Precondition: none
        ///     Postcondition: player.Y = player.Y@prev + player.Height
        /// </summary>
        public void MovePlayerDown()
        {
            var previousY = this.player.Y;
            this.player.MoveDown();
            if (this.player.Y + this.player.Height > this.backgroundHeight)
            {
                this.player.Y = previousY;
            }
        }

        #endregion
    }
}