// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

using System;
using Windows.UI.Xaml;
using FroggerStarter.Controller;
using FroggerStarter.Model;

namespace FroggerStarter.View.Sprites
{
    public sealed partial class FrogSprite
    {

        private bool isInvincible;

        public bool IsInvincible
        {
            get { return isInvincible; }
            set { isInvincible = value; }
        }
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
            
            App.AppSoundEffects.Play(Sounds.PowerUpStar);
            
        }

        public void StopInvincibility(object e, object f)
        {
            IsInvincible = false;
            VisualStateManager.GoToState(this, "OriginalSprite", true);
        }
    }

    #endregion
}
    
