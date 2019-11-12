using System;
using Windows.Foundation;
using FroggerStarter.View.Sprites;

namespace FroggerStarter.Model
{
    internal class PlayerManager : GameObject
    {
        #region Constructors

        public PlayerManager(double topBoundary, double bottomBoundary, double leftBoundary, double rightBoundary)
        {
            Player = new Frog();
            PlayerSprite = (FrogSprite) Player.Sprite;
            PlayerSprite.IsHitTestVisible = false;

            this.topBoundary = topBoundary;
            this.bottomBoundary = bottomBoundary;
            this.leftBoundary = leftBoundary;
            this.rightBoundary = rightBoundary;

            PlayerSprite.NewSpriteCreated += onNewSpriteCreated;
        }

        #endregion

        #region Types and Delegates

        #endregion

        #region Data members

        private readonly double topBoundary;
        private readonly double bottomBoundary;
        private readonly double leftBoundary;
        private readonly double rightBoundary;

        private Point previousPosition;

        private bool keyboardFrozen;

        #endregion

        #region Properties

        public Frog Player { get; }
        public FrogSprite PlayerSprite { get; }

        #endregion

        #region Methods

        public event EventHandler NewSpriteCreated;

        private void onNewSpriteCreated(object sender, EventArgs e)
        {
            keyboardFrozen = false;
            setHeading(Heading.Up);
            NewSpriteCreated?.Invoke(this, null);
        }

        /// <summary>
        ///     Moves the player to the left.
        ///     Precondition: none
        ///     Postcondition: player.X = player.X@prev - player.Width
        /// </summary>
        public void MovePlayerLeft()
        {
            if (keyboardFrozen) return;
            PlayerSprite.AnimateMove();
            setPreviousPositionLocation();
            setHeading(Heading.Left);
            Player.MoveLeft();
            if (Player.X < leftBoundary) ResetPlayerToPreviousPosition();
        }

        /// <summary>
        ///     Moves the player to the right.
        ///     Precondition: none
        ///     Postcondition: player.X = player.X@prev + player.Width
        /// </summary>
        public void MovePlayerRight()
        {
            if (keyboardFrozen) return;

            PlayerSprite.AnimateMove();
            setPreviousPositionLocation();
            Player.MoveRight();
            setHeading(Heading.Right);
            if (Player.X + Player.Width > rightBoundary) ResetPlayerToPreviousPosition();
        }

        /// <summary>
        ///     Moves the player up.
        ///     Precondition: none
        ///     Postcondition: player.Y = player.Y@prev - player.Height
        /// </summary>
        public void MovePlayerUp()
        {
            if (keyboardFrozen) return;

            PlayerSprite.AnimateMove();
            setPreviousPositionLocation();
            Player.MoveUp();
            setHeading(Heading.Up);
            if (Player.Y < topBoundary) ResetPlayerToPreviousPosition();
        }

        /// <summary>
        ///     Moves the player down.
        ///     Precondition: none
        ///     Postcondition: player.Y = player.Y@prev + player.Height
        /// </summary>
        public void MovePlayerDown()
        {
            if (keyboardFrozen) return;

            PlayerSprite.AnimateMove();
            setPreviousPositionLocation();
            Player.MoveDown();
            setHeading(Heading.Down);
            if (Player.Y + Player.Height > bottomBoundary) ResetPlayerToPreviousPosition();
        }

        private void setPreviousPositionLocation()
        {
            previousPosition = new Point(PlayerSprite.HitBox.X, PlayerSprite.HitBox.Y);
        }

        public void SetPlayerLocation(double x, double y)
        {
            Player.X = x;
            Player.Y = y;
            setPreviousPositionLocation();
        }

        public void ResetPlayerToPreviousPosition()
        {
            Player.X = previousPosition.X;
            Player.Y = previousPosition.Y;
        }

        public void HandleLifeLost(object sender, EventArgs e)
        {
            keyboardFrozen = true;
            PlayerSprite.AnimateDeath();
        }

        public void onInvincibilityTriggered()
        {
            PlayerSprite.AnimateInvincibility();
        }

        public override void setHeading(Heading heading)
        {
            Player.setHeading(heading);
        }

        #endregion
    }
}