using Windows.Foundation;
using Windows.UI.Xaml.Media;

namespace FroggerStarter.Model.Vehicles
{
    /// <summary>
    ///     Defines vehicle models
    /// </summary>
    public abstract class Vehicle : GameObject
    {

        public enum Heading
        {
            Left,
            Right,
            Up,
            Down
        }
        #region Data members

        protected Heading VehicleHeading;

        #endregion

        #region Constructors


        #endregion

        #region Methods

        protected void setHeading(Heading heading)
        {
            this.VehicleHeading = heading;
            if (this.VehicleHeading.Equals(Heading.Right))
            {
                this.headRight();
            }
        }

        protected void headRight()
        {
            this.Sprite.RenderTransformOrigin = new Point(0.5, 0.5);
            this.Sprite.RenderTransform = new ScaleTransform { ScaleX = -1 };
        }

        public void MoveVehicle()
        {
            switch (this.VehicleHeading)
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

        public void SetSpeed(int speed)
        {
            base.SetSpeed(speed, speed);
        }

        #endregion
    }
}
