using Windows.Foundation;
using Windows.UI.Xaml.Media;
using FroggerStarter.View.Sprites;

namespace FroggerStarter.Model
{
    /// <summary>
    ///     Defines vehicle models
    /// </summary>
    public class Vehicle : GameObject
    {
        #region Types and Delegates

        public enum Heading
        {
            Left,
            Right,
            Up,
            Down
        }

        public enum VehicleType
        {
            Car,
            Truck
        }

        #endregion

        #region Data members

        public Heading VehicleDirection;

        #endregion

        #region Constructors

        public Vehicle(VehicleType vehicleType, Heading heading)
        {
            if (vehicleType.Equals(VehicleType.Car))
            {
                Sprite = new CarSprite();
            }

            if (vehicleType.Equals(VehicleType.Truck))
            {
                Sprite = new TruckSprite();
            }

            this.VehicleDirection = heading;
            if (heading.Equals(Heading.Right))
            {
                this.flipHorizontally();
            }
        }

        #endregion

        #region Methods

        private void flipHorizontally()
        {
            Sprite.RenderTransformOrigin = new Point(0.5, 0.5);
            Sprite.RenderTransform = new ScaleTransform {ScaleX = -1};
        }

        public void MoveVehicle()
        {
            switch (this.VehicleDirection)
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