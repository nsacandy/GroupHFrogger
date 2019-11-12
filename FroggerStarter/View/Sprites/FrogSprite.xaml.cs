// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

using System;
using Windows.UI.Xaml;
using FroggerStarter.Controller;
using FroggerStarter.Model;

namespace FroggerStarter.View.Sprites
{
    /// <summary>CodeBehind for FrogSprite</summary>
    /// <seealso cref="FroggerStarter.View.Sprites.BaseSprite" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class FrogSprite
    {
        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="FrogSprite" /> class.</summary>
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

        /// <summary>Creates new spritecreated.</summary>
        public event EventHandler NewSpriteCreated;

        /// <summary>Animates the death storyboard.</summary>
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

        /// <summary>Animates the move storyboard.</summary>
        public void AnimateMove()
        {
            this.Moving.Storyboard.Begin();
        }

        /// <summary>Animates the invincibility.</summary>
        public void AnimateInvincibility()
        {
            this.Invincible.Storyboard.Begin();

            App.AppSoundEffects.Play(Sounds.PowerUpStar);
        }

        /// <summary>Stops the invincibility.</summary>
        /// <param name="e">The e.</param>
        /// <param name="f">The f.</param>
        public void StopInvincibility(object e, object f)
        {
            VisualStateManager.GoToState(this, "OriginalSprite", true);
        }

        #endregion
    }
}