using System;
using Windows.Foundation;
using FroggerStarter.View.Sprites;

namespace FroggerStarter.Model
{
    internal class PlayerManager : GameObject
    {
        #region Types and Delegates

        public delegate void NewSpriteCreatedHandler(object sender, EventArgs e);

        #endregion

        #region Data members

        private readonly double topBoundary;
        private readonly double bottomBoundary;
        private readonly double leftBoundary;
        private readonly double rightBoundary;

        private Point previousPosition;

        private bool controlsFrozen;

        #endregion

        #region Properties

        public Frog Player { get; }
        public DeathSprite PlayerSprite { get; }

        #endregion

        #region Constructors

        public PlayerManager(double topBoundary, double bottomBoundary, double leftBoundary, double rightBoundary)
        {
            this.Player = new Frog();
            this.PlayerSprite = (DeathSprite) this.Player.Sprite;
            this.PlayerSprite.IsHitTestVisible = false;

            this.topBoundary = topBoundary;
            this.bottomBoundary = bottomBoundary;
            this.leftBoundary = leftBoundary;
            this.rightBoundary = rightBoundary;

            this.PlayerSprite.NewSpriteCreated += this.OnNewSpriteCreated;
        }

        #endregion

        #region Methods

        public event NewSpriteCreatedHandler NewSpriteCreated;

        private void OnNewSpriteCreated(object sender, EventArgs e)
        {
            this.controlsFrozen = false;
            this.NewSpriteCreated?.Invoke(this, null);
        }

        /// <summary>
        ///     Moves the player to the left.
        ///     Precondition: none
        ///     Postcondition: player.X = player.X@prev - player.Width
        /// </summary>
        public void MovePlayerLeft()
        {
            if (this.controlsFrozen)
            {
                return;
            }

            this.setPreviousPositionLocation();
            this.Player.MoveLeft();
            if (this.Player.X < this.leftBoundary)
            {
                this.resetPlayerToPreviousPosition();
            }
        }

        /// <summary>
        ///     Moves the player to the right.
        ///     Precondition: none
        ///     Postcondition: player.X = player.X@prev + player.Width
        /// </summary>
        public void MovePlayerRight()
        {
            if (this.controlsFrozen)
            {
                return;
            }

            this.setPreviousPositionLocation();
            this.Player.MoveRight();
            if (this.Player.X + this.Player.Width > this.rightBoundary)
            {
                this.resetPlayerToPreviousPosition();
            }
        }

        /// <summary>
        ///     Moves the player up.
        ///     Precondition: none
        ///     Postcondition: player.Y = player.Y@prev - player.Height
        /// </summary>
        public void MovePlayerUp()
        {
            if (this.controlsFrozen)
            {
                return;
            }

            this.setPreviousPositionLocation();
            this.Player.MoveUp();
            if (this.Player.Y < this.topBoundary)
            {
                this.resetPlayerToPreviousPosition();
            }
        }

        /// <summary>
        ///     Moves the player down.
        ///     Precondition: none
        ///     Postcondition: player.Y = player.Y@prev + player.Height
        /// </summary>
        public void MovePlayerDown()
        {
            if (this.controlsFrozen)
            {
                return;
            }

            this.setPreviousPositionLocation();
            this.Player.MoveDown();
            if (this.Player.Y + this.Player.Height > this.bottomBoundary)
            {
                this.resetPlayerToPreviousPosition();
            }
        }

        public void resetPlayerToPreviousPosition()
        {
            this.Player.X = this.previousPosition.X;
            this.Player.Y = this.previousPosition.Y;
        }

        private void setPreviousPositionLocation()
        {
            this.previousPosition = new Point(this.PlayerSprite.HitBox.X, this.PlayerSprite.HitBox.Y);
        }

        public void SetPlayerLocation(double x, double y)
        {
            this.Player.X = x;
            this.Player.Y = y;
            this.setPreviousPositionLocation();
        }

        public void handleLifeLost(object sender, EventArgs e)
        {
            this.controlsFrozen = true;
            this.PlayerSprite.AnimateDeath();
        }

        #endregion
    }
}