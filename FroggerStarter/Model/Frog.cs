using Windows.Foundation;
using Windows.UI.Xaml.Media;
using FroggerStarter.View.Sprites;

namespace FroggerStarter.Model
{
    /// <summary>
    ///     Defines the frog model
    /// </summary>
    /// <seealso cref="FroggerStarter.Model.GameObject" />
    public class Frog : GameObject
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Frog" /> class.
        /// </summary>
        public Frog()
        {
            Sprite = new FrogSprite();
            currentHeading = Heading.Up;
            SetSpeed(SpeedXDirection, SpeedYDirection);
        }

        #endregion

        /// <summary>
        ///     Sets the heading.
        /// </summary>
        /// <param name="heading">The heading.</param>
        public override void setHeading(Heading heading)
        {
            currentHeading = heading;
            switch (currentHeading)
            {
                case Heading.Down:
                    Sprite.RenderTransformOrigin = new Point(0.5, 0.5);
                    Sprite.RenderTransform = new RotateTransform();
                    Sprite.RenderTransform.SetValue(RotateTransform.AngleProperty, "180");
                    break;
                case Heading.Up:
                    Sprite.RenderTransformOrigin = new Point(0.5, 0.5);
                    Sprite.RenderTransform = new RotateTransform();
                    Sprite.RenderTransform.SetValue(RotateTransform.AngleProperty, "0");
                    break;
                case Heading.Left:
                    Sprite.RenderTransformOrigin = new Point(0.5, 0.5);
                    Sprite.RenderTransform = new RotateTransform();
                    Sprite.RenderTransform.SetValue(RotateTransform.AngleProperty, "270");
                    break;
                case Heading.Right:

                    Sprite.RenderTransformOrigin = new Point(0.5, 0.5);
                    Sprite.RenderTransform = new RotateTransform();
                    Sprite.RenderTransform.SetValue(RotateTransform.AngleProperty, "90");
                    break;
            }
        }

        #region Data members

        private const int SpeedXDirection = 50;
        private const int SpeedYDirection = 50;

        #endregion
    }
}