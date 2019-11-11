using FroggerStarter.View.Sprites;

namespace FroggerStarter.Model.Vehicles
{
    public class SportsCar : Car
    {
        private readonly int extraSpeed = 2;

        public SportsCar(Heading heading) : base(heading)
        {
            Sprite = new SportsCarSprite();
            setHeading(heading);
        }

        public override void SetSpeed(int speed)
        {
            base.SetSpeed(speed + extraSpeed, speed + extraSpeed);
        }
    }
}