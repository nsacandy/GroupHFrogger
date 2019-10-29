﻿using System;
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

        private bool keyboardFrozen;

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

            this.PlayerSprite.NewSpriteCreated += this.onNewSpriteCreated;
        }

        #endregion

        #region Methods

        public event NewSpriteCreatedHandler NewSpriteCreated;

        private void onNewSpriteCreated(object sender, EventArgs e)
        {
            this.keyboardFrozen = false;
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

            this.setPreviousPositionLocation();
            this.Player.MoveLeft();
            if (this.Player.X < this.leftBoundary)
            {
                this.ResetPlayerToPreviousPosition();
            }
        }

        /// <summary>
        ///     Moves the player to the right.
        ///     Precondition: none
        ///     Postcondition: player.X = player.X@prev + player.Width
        /// </summary>
        public void MovePlayerRight()
        {
            if (this.keyboardFrozen)
            {
                return;
            }

            this.setPreviousPositionLocation();
            this.Player.MoveRight();
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

            this.setPreviousPositionLocation();
            this.Player.MoveUp();
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

            this.setPreviousPositionLocation();
            this.Player.MoveDown();
            if (this.Player.Y + this.Player.Height > this.bottomBoundary)
            {
                this.ResetPlayerToPreviousPosition();
            }
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

        public void ResetPlayerToPreviousPosition()
        {
            this.Player.X = this.previousPosition.X;
            this.Player.Y = this.previousPosition.Y;
        }

        public void HandleLifeLost(object sender, EventArgs e)
        {
            this.keyboardFrozen = true;
            this.PlayerSprite.AnimateDeath();
        }

        #endregion
    }
}