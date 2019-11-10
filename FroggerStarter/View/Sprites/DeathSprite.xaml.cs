using System;
using System.Diagnostics;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using FroggerStarter.Controller;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace FroggerStarter.View.Sprites
{
    public sealed partial class DeathSprite
    {
        #region Types and Delegates

        public event EventHandler NewSpriteCreated;

        #endregion

        private bool isInvincible;

        public bool IsInvincible
        {
            get { return isInvincible; }
            set { isInvincible = value; }
        }


        #region Constructors

        public DeathSprite()
        {
            this.InitializeComponent();
            IsHitTestVisible = false;
            this.Invincible.Storyboard.Duration = GameSettings.InvincibilityLength;
            this.Dying.Storyboard.Completed += this.onNewSpriteCreated;
            this.Invincible.Storyboard.Completed += this.StopInvincibility;
        }

        #endregion

        #region Methods

        public void AnimateDeath()
        {
            VisualStateManager.GoToState(this, "Dying", false);
            this.Dying.Storyboard.Begin();
        }

        private void onNewSpriteCreated(object e, object f)
        {
            VisualStateManager.GoToState(this, "OriginalSprite", false);
            this.NewSpriteCreated?.Invoke(this, null);
        }

        public void AnimateMove()
        {
            this.Moving.Storyboard.Begin();
        }

        public void AnimateInvincibility()
        {
            this.Invincible.Storyboard.Begin();
        }

        public void StopInvincibility(object e, object f)
        {
            this.isInvincible = false;
            VisualStateManager.GoToState(this, "OriginalSprite", true);
        }
    }
    #endregion
}