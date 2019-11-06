using Windows.Foundation;
using Windows.UI.Xaml.Media;
using FroggerStarter.View.Sprites;

namespace FroggerStarter.Model
{
    /// <summary>
    ///     Defines vehicle models
    /// </summary>
    public abstract class Vehicle : GameObject
    {
        
        protected Heading vehicleHeading;

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
        

        #region Data members

        public Heading VehicleDirection;

        #endregion

        #region Constructors


        #endregion

        #region Methods

        protected void flipHorizontally()
        {
            this.Sprite.RenderTransformOrigin = new Point(0.5, 0.5);
            this.Sprite.RenderTransform = new ScaleTransform { ScaleX = -1 };
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
