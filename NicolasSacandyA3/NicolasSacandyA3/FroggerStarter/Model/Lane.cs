using System.Collections.Generic;
using FroggerStarter.Controller;

namespace FroggerStarter.Model
{

    public class Lane
    {
        private readonly int yValue;
        private IList<Vehicle> laneVehicles;
        public enum Direction{Left,Right};

        private readonly Direction laneDirection;
        private readonly double width;
        public double LaneSpeed { get; set; }

        public Lane(int yValue, int numVehicles, Vehicle.VehicleType vehicleType, Direction laneDirection, double laneWidth)
        {
            this.yValue = yValue;
            this.laneDirection = laneDirection;
            this.width = laneWidth;
            this.populateLaneWithVehicles(numVehicles, vehicleType);
        }

        private void populateLaneWithVehicles(int numVehicles, Vehicle.VehicleType vehicleType)
        {
            this.laneVehicles = new List<Vehicle>();

            if (vehicleType.Equals(Vehicle.VehicleType.Car))
            {
                
                for (var i = 0; i < numVehicles; i++)
                {
                    var car = new Vehicle(Vehicle.VehicleType.Car,this.laneDirection){X = (this.width/numVehicles) * i,Y=this.yValue};
                    this.laneVehicles.Add(car);
                }
            }
            else if (vehicleType.Equals(Vehicle.VehicleType.Truck))
            {
                for (var i = 0; i < numVehicles; i++)
                {
                    var truck = new Vehicle(Vehicle.VehicleType.Truck,this.laneDirection) { X = (this.width / numVehicles) * i, Y=this.yValue};
                    this.laneVehicles.Add(truck);
                }
            }
        }

        /// <summary>
        /// Gets the vehicles.
        /// </summary>
        /// <returns>List of vehicles</returns>
        public IList<Vehicle> GetVehicles()
        {
            return this.laneVehicles;
        }

        /// <summary>
        /// Moves the vehicles on tick.
        /// <postcondition>All vehicles x values incremented by lane speed</postcondition>
        /// </summary>
        public void MoveVehiclesOnTick()
        {
            switch (this.laneDirection)
            {
                case Direction.Left:
                    this.moveVehiclesLeft();
                    break;
                case Direction.Right:
                    this.moveVehiclesRight();
                    break;
            }
        }

        private void moveVehiclesRight()
        {
            foreach (var vehicle in this.laneVehicles)
            {
                vehicle.X += this.LaneSpeed;
                
                if (vehicle.X > this.width)
                {
                    vehicle.X = -vehicle.Sprite.Width;
                }
            }
        }

        private void moveVehiclesLeft()
        {
            foreach (var vehicle in this.laneVehicles)
            {
                vehicle.X -= this.LaneSpeed;
                if ((vehicle.X + vehicle.Sprite.Width) < 0)
                {
                    vehicle.X = this.width + vehicle.Sprite.Width;
                }

            }
        }
    }
}
