﻿using Windows.Foundation;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace FroggerStarter.View.Sprites
{
    /// <summary>
    ///     Holds common functionality for all game sprites.
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.UserControl" />
    public abstract partial class BaseSprite : ISpriteRenderer
    {
        #region Properties

        /// <summary>Gets the hit box.</summary>
        /// <value>The hit box.</value>
        public Rect HitBox { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="BaseSprite" /> class.
        /// </summary>
        protected BaseSprite()
        {
            this.InitializeComponent();
            IsHitTestVisible = true;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Renders user control at the specified (x,y) location in relation
        ///     to the top, left part of the canvas.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public void RenderAt(double x, double y)
        {
            Canvas.SetLeft(this, x);
            Canvas.SetTop(this, y);
            this.HitBox = new Rect(x, y, Width, Height);
        }

        #endregion
    }
}