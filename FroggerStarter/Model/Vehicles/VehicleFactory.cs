using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FroggerStarter.Model.Vehicles
{
    class VehicleFactory
    {
        public static Vehicle CreateVehicle(Vehicle.VehicleType vehicleType, Vehicle.Heading heading)
        {
            switch (vehicleType)
            {
                case Vehicle.VehicleType.Car:
                    return new Car(heading);
                case Vehicle.VehicleType.Truck:
                    return new Truck(heading);
            }

            return null;
        }
    }
}
