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
        public enum VehicleType
        {
            Car,
            Truck
        };

        public enum Heading
        {
            Left,
            Right,
            Up,
            Down
        }

        public Heading vehicleDirection;
        
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

            this.vehicleDirection = heading;
            if (heading.Equals(Heading.Right))
            {
                this.flipHorizontally();
            }
        }

        private void flipHorizontally()
        {
            this.Sprite.RenderTransformOrigin = new Point(0.5, 0.5);
            this.Sprite.RenderTransform = new ScaleTransform() { ScaleX = -1 };
        }

        public void moveVehicle()
        {
            switch (this.vehicleDirection)
            {
                case Heading.Left:
                    this.X -= this.SpeedX;
                    break;
                case Heading.Right:
                    this.X += this.SpeedX;
                    break;
                case Heading.Down:
                    this.Y += this.SpeedY;
                    break;
                case Heading.Up:
                    this.Y -= this.SpeedY;
                    break;
            }
            
        }

        public void SetSpeed(int speed)
        {
            base.SetSpeed(speed, speed);
        }
    }
}