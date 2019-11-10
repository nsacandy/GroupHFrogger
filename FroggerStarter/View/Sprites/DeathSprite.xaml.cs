using System;
using System.Diagnostics;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace FroggerStarter.View.Sprites
{
    public sealed partial class DeathSprite
    {
        #region Types and Delegates

        public event EventHandler NewSpriteCreated;

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

        

        public void AnimateMove()
        {
            this.Moving.Storyboard.Begin();
        }
    }
    #endregion
}