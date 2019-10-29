using System;
using System.Collections;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;
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
        #region Data members

        private readonly double applicationHeight = (double) Application.Current.Resources["AppHeight"];
        private readonly double applicationWidth = (double) Application.Current.Resources["AppWidth"];
        private readonly GameManager gameManager;
        private FrogSprite[] lives;
        private IDictionary<LilyPad, FrogSprite> landingSpots;

        private TimeSpan timerLength;
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

            this.gameManager.PointScored += this.handlePointScored;
            this.createVisibleClock();
            }

        private void handleGameOver(object sender, EventArgs e)
        {
            this.gameOver.Visibility = Visibility.Visible;
            this.timer.Stop();
        }

        #endregion

        #region Methods

        private void coreWindowOnKeyDown(CoreWindow sender, KeyEventArgs args)
        {
            this.gameManager.MovePlayer(args);
        }


        //TODO consider moving all sprites into a view-type folder.
        private void generateLives()
        {
            this.lives = new FrogSprite[this.gameManager.Lives];
            for (int i = 0; i < this.gameManager.Lives; i++)
            {
                FrogSprite life = new FrogSprite();
                double xLocation = i * (life.Width + 5);
                life.RenderAt(xLocation, 0);
                this.canvas.Children.Add(life);
                this.lives[i] = life;
                this.timerLength = this.gameManager.timerLength;
                this.createVisibleClock();
    }

        }

        public object FrogSprite { get; set; }

        private void handleLifeLost(object sender, EventArgs e)
        {
            this.lives[this.gameManager.Lives].Visibility = Visibility.Collapsed;

            this.emptyTimerBar.Width = 0;
        }

        private void handlePointScored(Object sender, GameManager.ScoreArgs e)
        {
            e.LilyPad.Visibility = Visibility.Collapsed;
            this.landingSpots[e.LilyPad].Visibility = Visibility.Visible;
            this.score.Text = "Score:" + this.gameManager.Score;
            this.emptyTimerBar.Width = 0;
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
           this.emptyTimerBar.Width += 1;
        }

        private void generateLandingSpotFrogs()
        {

            this.landingSpots = new Dictionary<LilyPad, FrogSprite>();
            for (int i = 0; i < this.gameManager.LandingSpots.Count; i++)
            {
                FrogSprite newLandingSpotFrog = new FrogSprite();
                newLandingSpotFrog.Visibility = Visibility.Collapsed;

                double xLocation = this.gameManager.LandingSpots[i].HitBox.X;
                double yLocation = this.gameManager.LandingSpots[i].HitBox.Y;
                newLandingSpotFrog.RenderAt(xLocation, yLocation);
                this.canvas.Children.Add(newLandingSpotFrog);

                this.landingSpots.Add(this.gameManager.LandingSpots[i], newLandingSpotFrog);
            }
        }
        #endregion
    }
}