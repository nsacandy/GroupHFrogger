using System;
using System.Collections.Generic;
using System.Windows.Input;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using FroggerStarter.Controller;
using FroggerStarter.View.Sprites;
using Visibility = Windows.UI.Xaml.Visibility;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FroggerStarter.View
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GamePage
    {
        #region Data members

        private readonly double applicationHeight = (double) Application.Current.Resources["AppHeight"];
        private readonly double applicationWidth = (double) Application.Current.Resources["AppWidth"];
        private readonly GameManager gameManager;
        private FrogSprite[] lives;
        private IDictionary<LilyPad, FrogSprite> landingSpots;

        private DispatcherTimer timer;
        

        #endregion

        #region Constructors

        public GamePage()
        {
            this.InitializeComponent();

            ApplicationView.PreferredLaunchViewSize = new Size
                {Width = this.applicationWidth, Height = this.applicationHeight};
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            ApplicationView.GetForCurrentView()
                           .SetPreferredMinSize(new Size(this.applicationWidth, this.applicationHeight));

            Window.Current.CoreWindow.KeyDown += this.coreWindowOnKeyDown;
            this.gameManager = new GameManager(this.applicationHeight, this.applicationWidth);
            this.gameManager.InitializeGame(this.canvas);
            this.generateLives();
            this.generateLandingSpotFrogs();

            this.gameManager.LifeLost += this.handleLifeLost;

            this.gameManager.GameOver += this.handleGameOver;
            this.gameManager.GameResumed += this.resumeGame;

            this.gameManager.PointScored += this.handlePointScored;
            this.gameManager.NextLevel += this.handleNextRound;
            this.gameManager.TimePowerUp += this.handleTimePowerUp;
            this.createVisibleClock();
        }

        #endregion

        #region Methods

        private void resumeGame(object sender, EventArgs e)
        {
            this.emptyTimerBar.Width = 0;
            this.timer.Start();
        }

        private void handleGameOver(object sender, EventArgs e)
        {
            this.emptyTimerBar.Visibility = Visibility.Collapsed;
            this.fullTimerBar.Visibility = Visibility.Collapsed;
            this.gameOver.Visibility = Visibility.Visible;
            this.score.Visibility = Visibility.Visible;
            this.timer.Stop();
            this.promptUserForRestart();
        }

        private async void promptUserForRestart()
        {
            ContentDialog restartPrompt = new ContentDialog()
            {
                Title = "Game Over",
                Content = "Would you like to play again?",
                PrimaryButtonText = "Sure",
                CloseButtonText = "No thanks"
            };
            restartPrompt.PrimaryButtonCommand = restartGame(restartPrompt.IsPrimaryButtonEnabled);
            await restartPrompt.ShowAsync();
        }

        private void coreWindowOnKeyDown(CoreWindow sender, KeyEventArgs args)
        {
            this.gameManager.MovePlayer(args);
        }

        private class restartGame:ICommand
        {
            private bool restartButtonEnabled;
            restartGame(bool buttonEnable)
            {
                this.restartButtonEnabled = buttonEnable;
            }
            public bool CanExecute(object parameter)
            {
                return this.restartButtonEnabled;
            }

            public void Execute(object parameter)
            {
                if (CanExecute(object))
                {
                    execute();
                }
            }

            public event EventHandler CanExecuteChanged;
        }

        private void generateLives()
        {
            this.lives = new FrogSprite[this.gameManager.Lives];
            for (var i = 0; i < this.gameManager.Lives; i++)
            {
                var life = new FrogSprite();
                var xLocation = i * (life.Width + 5);
                life.RenderAt(xLocation, 0);
                this.canvas.Children.Add(life);
                this.lives[i] = life;
                this.createVisibleClock();
            }
        }

        private void handleLifeLost(object sender, EventArgs e)
        {
            this.lives[this.gameManager.Lives].Visibility = Visibility.Collapsed;
            this.timer.Stop();
        }

        private void handlePointScored(object sender, GameManager.ScoreArgs e)
        {
            e.LilyPad.Visibility = Visibility.Collapsed;
            this.landingSpots[e.LilyPad].Visibility = Visibility.Visible;
            this.score.Text = "Score:" + this.gameManager.Score;
            this.emptyTimerBar.Width = 0;
        }

        private void handleNextRound(object sender, EventArgs e)
        {
            foreach (var landingSpot in this.landingSpots.Values)
            {
                this.canvas.Children.Remove(landingSpot);
            }
            this.generateLandingSpotFrogs();
        }

        private void createVisibleClock()
        {
            this.timer = new DispatcherTimer();
            this.timer.Tick += this.timerOnTick;
            this.timer.Interval = new TimeSpan(0, 0, 0, 1, 0);
            this.timer.Start();
        }

        private void timerOnTick(object sender, object e)
        {
            var increment = 10.0 / GameSettings.GameLengthInSeconds;
            this.emptyTimerBar.Width += increment;
        }

        private void handleTimePowerUp(object sender, EventArgs e)
        {
            var increment = (10.0 / GameSettings.GameLengthInSeconds) * 3;
            this.emptyTimerBar.Width -= increment;
        }

        private void generateLandingSpotFrogs()
        {
            this.landingSpots = new Dictionary<LilyPad, FrogSprite>();
            foreach (var t in this.gameManager.GetFrogHomes())
            {
                var newLandingSpotFrog = new FrogSprite {Visibility = Visibility.Collapsed};

                var xLocation = t.HitBox.X;
                var yLocation = t.HitBox.Y;
                newLandingSpotFrog.RenderAt(xLocation, yLocation);
                this.canvas.Children.Add(newLandingSpotFrog);

                this.landingSpots.Add(t, newLandingSpotFrog);
            }
        }

        #endregion
    }
}