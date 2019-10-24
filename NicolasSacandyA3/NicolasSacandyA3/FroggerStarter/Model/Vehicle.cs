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

        //TODO make backwards facing sprites. Do this spec too.
        public Vehicle(VehicleType vehicleType, Lane.Direction leftOrRight)
        {
            if (vehicleType.Equals(VehicleType.Car))
            {
                Sprite = new CarSprite();
            }

            if (vehicleType.Equals(VehicleType.Truck))
            {
                Sprite = new TruckSprite();
            }

            if (leftOrRight.Equals(Lane.Direction.Right))
            {
                this.flipHorizontally();
            }
        }

        private void flipHorizontally()
        {
            this.Sprite.RenderTransformOrigin = new Point(0.5, 0.5);
            this.Sprite.RenderTransform = new ScaleTransform() { ScaleX = -1 };
        }
    }
}