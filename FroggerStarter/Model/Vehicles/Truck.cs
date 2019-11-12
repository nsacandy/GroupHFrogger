using FroggerStarter.View.Sprites;

namespace FroggerStarter.Model.Vehicles
{
    internal class Truck : Vehicle
    {
        public Truck(Heading heading)
        {
            Sprite = new TruckSprite();
            setHeading(heading);
        }
    }
}