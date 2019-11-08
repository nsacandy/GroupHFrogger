using FroggerStarter.View.Sprites;

namespace FroggerStarter.Model.Vehicles
{
    class Truck : Vehicle
    {

        public Truck(Vehicle.Heading heading)
        {
            this.Sprite = new TruckSprite();

            this.VehicleDirection = heading;
            if (heading.Equals(Vehicle.Heading.Right))
            {
                this.flipHorizontally();
            }
        }
    }
}
