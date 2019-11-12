namespace FroggerStarter.Model.Vehicles
{
    /// <summary>A factory class for creating different Vehicle objects</summary>
    public class VehicleFactory
    {
        #region Types and Delegates

        /// <summary>Which Vehicle to instantiate</summary>
        public enum VehicleType
        {
            Car,
            Truck,
            SportsCar
        }

        #endregion

        #region Methods

        /// <summary>Creates the a new Vehicle.</summary>
        /// <param name="vehicleType">Type of the vehicle.</param>
        /// <param name="heading">The heading.</param>
        /// <returns>A new Vehicle</returns>
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

        #endregion
    }
}