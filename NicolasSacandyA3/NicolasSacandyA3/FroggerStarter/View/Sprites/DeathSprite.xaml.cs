using System;
using Windows.UI.Xaml;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace FroggerStarter.View.Sprites
{
    public sealed partial class DeathSprite
    {
        #region Types and Delegates

        public delegate void NewSpriteCreatedHandler(object sender, EventArgs e);
        public event NewSpriteCreatedHandler NewSpriteCreated;

        #endregion

        #region Constructors

        public DeathSprite()
        {
            this.InitializeComponent();
            IsHitTestVisible = false;
            this.Dying.Storyboard.Completed += this.onNewSpriteCreated;
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

        #endregion
    }
}