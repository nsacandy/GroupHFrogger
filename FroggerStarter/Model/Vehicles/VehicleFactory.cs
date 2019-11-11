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

        public static Vehicle CreateVehicle(VehicleType vehicleType, GameObject.Heading heading)
        {
            switch (vehicleType)
            {
                case VehicleType.Car:
                    var newCar = new Car(heading);
                    return newCar;
                case VehicleType.Truck:
                    var newTruck = new Truck(heading);
                    return newTruck;
                case VehicleType.SportsCar:
                    var newSportsCar = new SportsCar(heading);
                    return newSportsCar;
            }

            return null;
        }
    }
}