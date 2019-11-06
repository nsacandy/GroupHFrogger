using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FroggerStarter.View.Sprites;

namespace FroggerStarter.Model
{
    class Truck : Vehicle
    {

        public Truck()
        {
            this.vehicleSprite = new TruckSprite();

            this.VehicleDirection = heading;
            if (heading.Equals(Vehicle.Heading.Right))
            {
                this.flipHorizontally();
            }
        }
    }
}
