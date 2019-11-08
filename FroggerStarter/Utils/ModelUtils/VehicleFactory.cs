using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FroggerStarter.Model;
using FroggerStarter.View.Sprites;

namespace FroggerStarter.Utils.ModelUtils
{
    class VehicleFactory
    {
        public Vehicle CreateVehicle(Vehicle.VehicleType vehicle, Vehicle.Heading heading)
        {
            Vehicle newVehicle = null;
            if (vehicleType.Equals(Vehicle.VehicleType.Car))
            {
                newVehicle = new Car(heading);
            }

            if (vehicleType.Equals(Vehicle.VehicleType.Truck))
            {
                newVehicle = new Truck(heading);
            }

            this.VehicleDirection = heading;
            if (heading.Equals(Vehicle.Heading.Right))
            {
                this.flipHorizontally();
            }
            return 
        }
    }
}
