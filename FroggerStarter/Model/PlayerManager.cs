using System;
using Windows.Foundation;
using FroggerStarter.View.Sprites;

namespace FroggerStarter.Model
{
    internal class PlayerManager : GameObject
    {
        #region Data members

        private readonly double topBoundary;
        private readonly double bottomBoundary;
        private readonly double leftBoundary;
        private readonly double rightBoundary;

        private Point previousPosition;

        private bool keyboardFrozen;

        #endregion

        #region Properties

        /// <summary>Gets the player's class</summary>
        /// <value>The player's frog class </value>
        public Frog Player { get; }

        /// <summary>Gets the player sprite.</summary>
        /// <value>The player sprite.</value>
        public FrogSprite PlayerSprite { get; }

        #endregion

        #region Constructors

        public PlayerManager(double topBoundary, double bottomBoundary, double leftBoundary, double rightBoundary)
        {
            this.Player = new Frog();
            this.PlayerSprite = (FrogSprite) this.Player.Sprite;
            this.PlayerSprite.IsHitTestVisible = false;

            this.topBoundary = topBoundary;
            this.bottomBoundary = bottomBoundary;
            this.leftBoundary = leftBoundary;
            this.rightBoundary = rightBoundary;

            this.PlayerSprite.NewSpriteCreated += this.onNewSpriteCreated;
        }

        #endregion

        #region Methods

        public event EventHandler NewSpriteCreated;

        private void onNewSpriteCreated(object sender, EventArgs e)
        {
            this.keyboardFrozen = false;
            this.SetHeading(Heading.Up);
            this.NewSpriteCreated?.Invoke(this, null);
        }

        /// <summary>
        ///     Moves the player to the left.
        ///     Precondition: none
        ///     Postcondition: player.X = player.X@prev - player.Width
        /// </summary>
        public void MovePlayerLeft()
        {
            if (this.keyboardFrozen)
            {
                return;
            }

            this.PlayerSprite.AnimateMove();
            this.setPreviousPositionLocation();
            this.SetHeading(Heading.Left);
            this.Player.MoveLeft();
            if (this.Player.X < this.leftBoundary)
            {
                this.ResetPlayerToPreviousPosition();
            }
        }

        /// <summary>Moves the player to the right.
        ///  <@postcondition>player x + player speed </@precondition></summary>
        public void MovePlayerRight()
        {
            if (this.keyboardFrozen)
            {
                return;
            }

            this.PlayerSprite.AnimateMove();
            this.setPreviousPositionLocation();
            this.Player.MoveRight();
            this.SetHeading(Heading.Right);
            if (this.Player.X + this.Player.Width > this.rightBoundary)
            {
                this.ResetPlayerToPreviousPosition();
            }
        }

        /// <summary>
        ///     Moves the player up.
        ///     Precondition: none
        ///     Postcondition: player.Y = player.Y@prev - player.Height
        /// </summary>
        public void MovePlayerUp()
        {
            if (this.keyboardFrozen)
            {
                return;
            }

            this.PlayerSprite.AnimateMove();
            this.setPreviousPositionLocation();
            this.Player.MoveUp();
            this.SetHeading(Heading.Up);
            if (this.Player.Y < this.topBoundary)
            {
                this.ResetPlayerToPreviousPosition();
            }
        }

        /// <summary>
        ///     Moves the player down.
        ///     Precondition: none
        ///     Postcondition: player.Y = player.Y@prev + player.Height
        /// </summary>
        public void MovePlayerDown()
        {
            if (this.keyboardFrozen)
            {
                return;
            }

            this.PlayerSprite.AnimateMove();
            this.setPreviousPositionLocation();
            this.Player.MoveDown();
            this.SetHeading(Heading.Down);
            if (this.Player.Y + this.Player.Height > this.bottomBoundary)
            {
                this.ResetPlayerToPreviousPosition();
            }
        }

        private void setPreviousPositionLocation()
        {
            this.previousPosition = new Point(this.PlayerSprite.HitBox.X, this.PlayerSprite.HitBox.Y);
        }

        /// <summary>Keeps track of most recent location.</summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public void LastLocationTracker(double x, double y)
        {
            this.Player.X = x;
            this.Player.Y = y;
            this.setPreviousPositionLocation();
        }

        /// <summary>Resets the player to previous position.</summary>
        public void ResetPlayerToPreviousPosition()
        {
            this.Player.X = this.previousPosition.X;
            this.Player.Y = this.previousPosition.Y;
        }

        /// <summary>Handles the life lost.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        public void HandleLifeLost(object sender, EventArgs e)
        {
            this.keyboardFrozen = true;
            this.PlayerSprite.AnimateDeath();
        }

        /// <summary>Called when [invincibility triggered].</summary>
        public void OnInvincibilityTriggered()
        {
            this.PlayerSprite.AnimateInvincibility();
        }

        /// <summary>
        ///   <para>
        ///  Sets the heading.
        /// </para>
        /// </summary>
        /// <param name="heading">The heading.</param>
        public override void SetHeading(Heading heading)
        {
            this.Player.SetHeading(heading);
        }

        #endregion
    }
}