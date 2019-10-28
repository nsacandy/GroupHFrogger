using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using FroggerStarter.View.Sprites;

namespace FroggerStarter.Model
{
    class PlayerManager : GameObject
    {
        private double topBoundary;
        private double bottomBoundary;
        private double leftBoundary;
        private double rightBoundary;

        private Point previousPosition;
        
        public Frog Player { get; }
        public BaseSprite PlayerSprite { get; }

        public PlayerManager(double topBoundary, double bottomBoundary, double leftBoundary, double rightBoundary)
        {
            this.Player = new Frog();
            this.PlayerSprite = this.Player.Sprite;
            this.PlayerSprite.IsHitTestVisible = false;

            this.topBoundary = topBoundary;
            this.bottomBoundary = bottomBoundary;
            this.leftBoundary = leftBoundary;
            this.rightBoundary = rightBoundary;
        }


        /// <summary>
        ///     Moves the player to the left.
        ///     Precondition: none
        ///     Postcondition: player.X = player.X@prev - player.Width
        /// </summary>
        public void MovePlayerLeft()
        {
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
            this.setPreviousPositionLocation();
            this.Player.MoveRight();
            if (this.Player.X + this.Player.Width > this.rightBoundary)
            { this.resetPlayerToPreviousPosition(); }
        }

        /// <summary>
        ///     Moves the player up.
        ///     Precondition: none
        ///     Postcondition: player.Y = player.Y@prev - player.Height
        /// </summary>
        public void MovePlayerUp()
        {
            this.setPreviousPositionLocation();
            this.Player.MoveUp();
            if (this.Player.Y < this.topBoundary)
            { this.resetPlayerToPreviousPosition(); }
        }

        /// <summary>
        ///     Moves the player down.
        ///     Precondition: none
        ///     Postcondition: player.Y = player.Y@prev + player.Height
        /// </summary>
        public void MovePlayerDown()
        {
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
    }
}
