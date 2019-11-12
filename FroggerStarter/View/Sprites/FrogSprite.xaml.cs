// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

using System;
using Windows.UI.Xaml;
using FroggerStarter.Controller;
using FroggerStarter.Model;

namespace FroggerStarter.View.Sprites
{
    public sealed partial class FrogSprite
    {
        #region Properties

        public bool IsInvincible { get; set; }

        #endregion

        #region Constructors

        public FrogSprite()
        {
            this.InitializeComponent();
            IsHitTestVisible = false;

            this.Invincible.Storyboard.Duration = GameSettings.InvincibilityLength;
            this.Dying.Storyboard.Completed += this.onNewSpriteCreated;
            this.Invincible.Storyboard.Completed += this.StopInvincibility;
        }

        #endregion

        #region Methods

        public event EventHandler NewSpriteCreated;

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

            App.AppSoundEffects.Play(Sounds.PowerUpStar);
        }

        public void StopInvincibility(object e, object f)
        {
            this.IsInvincible = false;
            VisualStateManager.GoToState(this, "OriginalSprite", true);
        }

        #endregion
    }

    
}