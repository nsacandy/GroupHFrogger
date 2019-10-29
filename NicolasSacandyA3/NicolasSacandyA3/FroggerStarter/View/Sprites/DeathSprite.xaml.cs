using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace FroggerStarter.View.Sprites
{
   
    public sealed partial class DeathSprite
    {
        public delegate void NewSpriteCreatedHandler(object sender, EventArgs e);
        public event NewSpriteCreatedHandler NewSpriteCreated;
        public DeathSprite()
        {
            this.InitializeComponent();
            this.IsHitTestVisible = false;
            this.Dying.Storyboard.Completed += this.OnNewSpriteCreated;
        }

        public void AnimateDeath()
        {
                VisualStateManager.GoToState(this, "Dying", false);
                this.Dying.Storyboard.Begin();
        }

        private void OnNewSpriteCreated(object e, object f)
        {
            VisualStateManager.GoToState(this, "OriginalSprite", false);
            this.NewSpriteCreated?.Invoke(this,null);
        }

    }
}
