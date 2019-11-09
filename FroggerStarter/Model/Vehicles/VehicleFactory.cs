namespace FroggerStarter.Model.Vehicles
{
    public class VehicleFactory
    {
        public enum VehicleType
        {
            Car,
            Truck,
            SportsCar
        }

        public static Vehicle CreateVehicle(VehicleFactory.VehicleType vehicleType, Vehicle.Heading heading)
        {
            switch (vehicleType)
            {
                case VehicleFactory.VehicleType.Car:
                    var newCar = new Car(heading);
                    return newCar;
                case VehicleFactory.VehicleType.Truck:
                    var newTruck = new Truck(heading);
                    return newTruck;
                case VehicleFactory.VehicleType.SportsCar:
                    var newSportsCar = new SportsCar(heading);
                    return newSportsCar;
            }

            return null;
        }
    }
}
