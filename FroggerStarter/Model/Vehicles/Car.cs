using FroggerStarter.View.Sprites;

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