﻿// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace FroggerStarter.View.Sprites
{
    /// <summary>Creates lilypad sprites</summary>
    /// <seealso cref="FroggerStarter.View.Sprites.BaseSprite" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class LilyPad : BaseSprite
    {
        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="LilyPad" /> class.</summary>
        public LilyPad()
        {
            this.InitializeComponent();
        }

        #endregion
    }
}