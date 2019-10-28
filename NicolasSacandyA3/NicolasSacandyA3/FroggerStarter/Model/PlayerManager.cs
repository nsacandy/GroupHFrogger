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
            double previousX = this.Player.X;
            this.Player.MoveLeft();
            if (this.Player.X < this.leftBoundary)
            {
                this.Player.X = previousX;
            }
        }

        /// <summary>
        ///     Moves the player to the right.
        ///     Precondition: none
        ///     Postcondition: player.X = player.X@prev + player.Width
        /// </summary>
        public void MovePlayerRight()
        {
            var previousX = this.Player.X;
            this.Player.MoveRight();
            if (this.Player.X + this.Player.Width > this.rightBoundary)
            { this.Player.X = previousX; }
        }

        /// <summary>
        ///     Moves the player up.
        ///     Precondition: none
        ///     Postcondition: player.Y = player.Y@prev - player.Height
        /// </summary>
        public void MovePlayerUp()
        {
            var previousY = this.Player.Y;
            this.Player.MoveUp();
        }

        /// <summary>
        ///     Moves the player down.
        ///     Precondition: none
        ///     Postcondition: player.Y = player.Y@prev + player.Height
        /// </summary>
        public void MovePlayerDown()
        {
            var previousY = this.Player.Y;
            this.Player.MoveDown();
            if (this.Player.Y + this.Player.Height > this.bottomBoundary)
            {
                this.Player.Y = previousY;
            }
        }

        public void SetPlayerLocation(double x, double y)
        {
            this.Player.X = x;
            this.Player.Y = y;
        }
        
        public void handleMove(Object sender, EventArgs e)
        {

        }
    }
}
