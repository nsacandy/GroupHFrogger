using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FroggerStarter.View.Sprites;

namespace FroggerStarter.Model.Vehicles
{
    public class SportsCar : Car
    {
        private int extraSpeed = 2;
        public SportsCar(Heading heading) : base(heading)
        {
            this.Sprite = new SportsCarSprite();
            this.setHeading(heading);
        }

        public override void SetSpeed(int speed)
        {
            base.SetSpeed(speed+this.extraSpeed, speed + this.extraSpeed);
        }
    }
}
