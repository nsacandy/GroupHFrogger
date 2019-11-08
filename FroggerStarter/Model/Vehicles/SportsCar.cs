using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FroggerStarter.View.Sprites;

namespace FroggerStarter.Model.Vehicles
{
    class SportsCar : Car
    {
        public SportsCar(Heading heading):base(heading)
        {
            this.Sprite = new SportsCarSprite();
            this.setHeading(heading);

        }

    }
}
