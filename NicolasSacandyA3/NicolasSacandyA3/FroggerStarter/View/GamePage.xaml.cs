using System;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
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

            this.gameManager.LifeLost += this.handleLifeLost;

            this.gameManager.GameOver += this.handleGameOver;

        }

        private void handleGameOver(object sender, EventArgs e)
        {
            this.gameOver.Visibility = Visibility.Visible;
        }

        #endregion

        #region Methods

        private void coreWindowOnKeyDown(CoreWindow sender, KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case VirtualKey.Left:
                    this.gameManager.MovePlayerLeft();
                    break;
                case VirtualKey.Right:
                    this.gameManager.MovePlayerRight();
                    break;
                case VirtualKey.Up:
                    this.gameManager.MovePlayerUp();
                    break;
                case VirtualKey.Down:
                    this.gameManager.MovePlayerDown();
                    break;
            }
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
                this.statusBar.Children.Add(life);
                this.lives[i] = life;
            }

        }

        public object FrogSprite { get; set; }

        private void handleLifeLost(object sender, EventArgs e)
        {
            this.lives[this.gameManager.Lives].Visibility = Visibility.Collapsed;

            #endregion
        }
    }
}