﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using FroggerStarter.Controller;
using FroggerStarter.View.Sprites;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FroggerStarter.View
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GamePage
    {
        #region Constructors

        public GamePage()
        {
            InitializeComponent();

            ApplicationView.PreferredLaunchViewSize = new Size
                {Width = applicationWidth, Height = applicationHeight};
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            ApplicationView.GetForCurrentView()
                .SetPreferredMinSize(new Size(applicationWidth, applicationHeight));

            Window.Current.CoreWindow.KeyDown += coreWindowOnKeyDown;
            gameManager = new GameManager(applicationHeight, applicationWidth);
            gameManager.InitializeGame(canvas);
            generateLives();
            generateLandingSpotFrogs();

            gameManager.LifeLost += handleLifeLost;

            gameManager.GameOver += handleGameOver;
            gameManager.GameResumed += resumeGame;

            gameManager.PointScored += handlePointScored;
            gameManager.NextLevel += handleNextRound;
            gameManager.TimePowerUp += handleTimePowerUp;
            createVisibleClock();
        }

        #endregion

        #region Data members

        private readonly double applicationHeight = (double) Application.Current.Resources["AppHeight"];
        private readonly double applicationWidth = (double) Application.Current.Resources["AppWidth"];
        private readonly GameManager gameManager;
        private FrogSprite[] lives;
        private IDictionary<LilyPad, FrogSprite> landingSpots;

        private DispatcherTimer timer;

        #endregion

        #region Methods

        private void resumeGame(object sender, EventArgs e)
        {
            emptyTimerBar.Width = 0;
            timer.Start();
        }

        private async void handleGameOver(object sender, EventArgs e)
        {
            emptyTimerBar.Visibility = Visibility.Collapsed;
            fullTimerBar.Visibility = Visibility.Collapsed;
            gameOver.Visibility = Visibility.Visible;
            score.Visibility = Visibility.Visible;
            timer.Stop();
            var result = await this.inputTextDialogAsync();
            this.gameManager.MakeHighScorePlayer(result);
            this.HandleShowHighScoreboard();
        }

        private async void promptUserForRestart()
        {
            restartDialog = new ContentDialog
            {
                Title = "Game Over",
                Content = "Would you like to play again?",
                PrimaryButtonText = "Sure",
                CloseButtonText = "No thanks"
            };
            
            var result = await restartDialog.ShowAsync();

            switch (result)
            {
                case ContentDialogResult.Primary:
                    await CoreApplication.RequestRestartAsync("launch");
                    break;
                case ContentDialogResult.Secondary:
                    Application.Current.Exit();
                    break;
                case ContentDialogResult.None:
                    Application.Current.Exit();
                    break;
            }
        }

        private void coreWindowOnKeyDown(CoreWindow sender, KeyEventArgs args)
        {
            gameManager.MovePlayer(args);
        }

        private bool canRestart(object obj)
        {
            Window.Current.Activate();
            return restartDialog.IsPrimaryButtonEnabled;
        }

        private async void restartApplication(object obj)
        {
             await CoreApplication.RequestRestartAsync("launch");
        }

        private bool canQuit(object obj)
        {
            return restartDialog.IsSecondaryButtonEnabled;
        }

        private void quitApplication(object obj)
        {
            CoreApplication.Exit();
        }

        private void generateLives()
        {
            lives = new FrogSprite[gameManager.Lives];
            for (var i = 0; i < gameManager.Lives; i++)
            {
                var life = new FrogSprite();
                var xLocation = i * (life.Width + 5);
                life.RenderAt(xLocation, 0);
                canvas.Children.Add(life);
                lives[i] = life;
                createVisibleClock();
            }
        }

        private void handleLifeLost(object sender, EventArgs e)
        {
            lives[gameManager.Lives].Visibility = Visibility.Collapsed;
            timer.Stop();
        }

        private void handlePointScored(object sender, GameManager.ScoreArgs e)
        {
            e.LilyPad.Visibility = Visibility.Collapsed;
            landingSpots[e.LilyPad].Visibility = Visibility.Visible;
            score.Text = "Score:" + gameManager.Score;
            emptyTimerBar.Width = 0;
        }

        private void handleNextRound(object sender, EventArgs e)
        {
            foreach (var landingSpot in landingSpots.Values) canvas.Children.Remove(landingSpot);
            generateLandingSpotFrogs();
        }

        private void createVisibleClock()
        {
            timer = new DispatcherTimer();
            timer.Tick += timerOnTick;
            timer.Interval = new TimeSpan(0, 0, 0, 1, 0);
            timer.Start();
        }

        private void timerOnTick(object sender, object e)
        {
            var increment = 10.0 / GameSettings.GameLengthInSeconds;
            emptyTimerBar.Width += increment;
        }

        private void handleTimePowerUp(object sender, EventArgs e)
        {
            var increment = 10.0 / GameSettings.GameLengthInSeconds * 3;
            emptyTimerBar.Width -= increment;
        }

        private void generateLandingSpotFrogs()
        {
            landingSpots = new Dictionary<LilyPad, FrogSprite>();
            foreach (var t in gameManager.GetFrogHomes())
            {
                var newLandingSpotFrog = new FrogSprite {Visibility = Visibility.Collapsed};

                var xLocation = t.HitBox.X;
                var yLocation = t.HitBox.Y;
                newLandingSpotFrog.RenderAt(xLocation, yLocation);
                canvas.Children.Add(newLandingSpotFrog);

                landingSpots.Add(t, newLandingSpotFrog);
            }
        }

        public async void HandleShowHighScoreboard()
        {
            var viewId = 0;

            var newView = CoreApplication.CreateNewView();
            await newView.Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal,
                () =>
                {
                    var frame = new Frame();
                    frame.Navigate(typeof(HighScorePage));
                    Window.Current.Content = frame;

                    viewId = ApplicationView.GetForCurrentView().Id;

                    Window.Current.Activate();
                });
            
            var viewShown = await ApplicationViewSwitcher.TryShowAsStandaloneAsync(viewId);
        }

        private async Task<string> inputTextDialogAsync()
        {
            var inputTextBox = new TextBox {AcceptsReturn = false, Height = 32};
            var dialog = new ContentDialog {
                Content = inputTextBox,
                Title = "Enter Name",
                IsSecondaryButtonEnabled = false,
                PrimaryButtonText = "Ok",
            };
            if (await dialog.ShowAsync() == ContentDialogResult.Primary)
                return inputTextBox.Text;
            else
                return "";
        }

        #endregion
    }
}