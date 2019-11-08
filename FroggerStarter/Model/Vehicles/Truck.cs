using FroggerStarter.View.Sprites;

namespace FroggerStarter.Model.Vehicles
{
    class Truck : Vehicle
    {

        public Truck(Heading heading)
        {
            this.Sprite = new TruckSprite();
            this.setHeading(heading);
        }
    }
}
