using System;
using Windows.UI.Xaml;
using FroggerStarter.Controller;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace FroggerStarter.View.Sprites
{
    public sealed partial class DeathSprite
    {
        #region Constructors

        public DeathSprite()
        {
            InitializeComponent();
            IsHitTestVisible = false;
            Invincible.Storyboard.Duration = GameSettings.InvincibilityLength;
            Dying.Storyboard.Completed += onNewSpriteCreated;
            Invincible.Storyboard.Completed += StopInvincibility;
        }

        #endregion

        public bool IsInvincible { get; set; }

        #region Types and Delegates

        public event EventHandler NewSpriteCreated;

        #endregion

        #region Methods

        public void AnimateDeath()
        {
            VisualStateManager.GoToState(this, "Dying", false);
            Dying.Storyboard.Begin();
        }

        private void onNewSpriteCreated(object e, object f)
        {
            VisualStateManager.GoToState(this, "OriginalSprite", false);
            NewSpriteCreated?.Invoke(this, null);
        }

        public void AnimateMove()
        {
            Moving.Storyboard.Begin();
        }

        public void AnimateInvincibility()
        {
            Invincible.Storyboard.Begin();
        }

        public void StopInvincibility(object e, object f)
        {
            IsInvincible = false;
            VisualStateManager.GoToState(this, "OriginalSprite", true);
        }
    }

    #endregion
}