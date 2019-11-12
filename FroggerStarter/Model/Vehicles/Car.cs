using FroggerStarter.View.Sprites;
using FroggerStarter.View.Sprites.VehicleSprites;

namespace FroggerStarter.Model.Vehicles
{
    public class Car : Vehicle
    {
        public Car(Heading heading)
        {
            Sprite = new CarSprite();
            setHeading(heading);
        }
    }
}