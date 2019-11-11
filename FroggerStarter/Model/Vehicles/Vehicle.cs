using Windows.Foundation;
using Windows.UI.Xaml.Media;

namespace FroggerStarter.Model.Vehicles
{
    /// <summary>
    ///     Defines vehicle models
    /// </summary>
    public abstract class Vehicle : GameObject
    {
        #region Data members

        #endregion

        #region Constructors

        #endregion

        #region Methods

        public override void setHeading(Heading heading)
        {
            currentHeading = heading;
            switch (currentHeading)
            {
                case Heading.Right:
                    headRight();
                    break;
                case Heading.Left:
                    break;
            }
        }

        protected void headRight()
        {
            Sprite.RenderTransformOrigin = new Point(0.5, 0.5);
            Sprite.RenderTransform = new ScaleTransform {ScaleX = -1};
        }

        public void MoveVehicle()
        {
            switch (currentHeading)
            {
                case Heading.Left:
                    X -= SpeedX;
                    break;
                case Heading.Right:
                    X += SpeedX;
                    break;
                case Heading.Down:
                    Y += SpeedY;
                    break;
                case Heading.Up:
                    Y -= SpeedY;
                    break;
            }
        }

        public virtual void SetSpeed(int speed)
        {
            base.SetSpeed(speed, speed);
        }

        #endregion
    }
}