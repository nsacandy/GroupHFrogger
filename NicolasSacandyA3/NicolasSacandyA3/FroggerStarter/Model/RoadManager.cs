using System.Collections.Generic;

namespace FroggerStarter.Model
{
    /// <summary>
    /// Contains methods and initial values for generating and managing lanes.
    /// </summary>
    public class RoadManager
    {
        private static readonly int[] LanesYValues = new int[] {305,255,205,155,105};
        private static readonly int[] InitialLaneSpeeds = new int[] {2, 3, 4, 5, 6};
        private static readonly int[] NumVehiclesInEachLane = new int[]{2,3,3,2,3};
        public enum LaneNumber
        {
            One,
            Two,
            Three,
            Four,
            Five
        };

        private Dictionary<LaneNumber, Lane> lanes;
        private double laneWidth;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoadManager"/> class.
        /// </summary>
        public RoadManager(double backgroundWidth)
        {
            this.laneWidth = backgroundWidth;
            this.setInitialLaneValues();
        }

        private void setInitialLaneValues()
        {
            this.lanes = new Dictionary<LaneNumber, Lane> {
                {LaneNumber.One, getInitialLaneOne()},
                {LaneNumber.Two, getInitialLaneTwo()},
                {LaneNumber.Three, getInitialLaneThree()},
                {LaneNumber.Four, getInitialLaneFour()},
                {LaneNumber.Five, this.getInitialLaneFive()}
            };
        }

        private Lane getInitialLaneOne()
        {
            var laneOne =
                new Lane(LanesYValues[0], NumVehiclesInEachLane[0], Vehicle.VehicleType.Car, Lane.Direction.Left, true) {
                    LaneSpeed = InitialLaneSpeeds[0]
                };
            return laneOne;
        }

        private Lane getInitialLaneTwo()
        {
            var laneTwo =
                new Lane(LanesYValues[1], NumVehiclesInEachLane[1], Vehicle.VehicleType.Truck, Lane.Direction.Right,true) {
                    LaneSpeed = InitialLaneSpeeds[1]
                };
            return laneTwo;
        }

        private Lane getInitialLaneThree()
        {
            var laneThree =
                new Lane(LanesYValues[2], NumVehiclesInEachLane[2], Vehicle.VehicleType.Car, Lane.Direction.Left, true) {
                    LaneSpeed = InitialLaneSpeeds[2]
                };
            return laneThree;
        }


        private Lane getInitialLaneFour()
        {
            var laneFour = 
                new Lane(LanesYValues[3], NumVehiclesInEachLane[3], Vehicle.VehicleType.Truck, Lane.Direction.Left, true) {
                    LaneSpeed = InitialLaneSpeeds[3]
                };
            return laneFour;
        }


        private Lane getInitialLaneFive()
        {
            var laneFive =
                new Lane(LanesYValues[4], NumVehiclesInEachLane[4], Vehicle.VehicleType.Car, Lane.Direction.Right, true) {
                    LaneSpeed = InitialLaneSpeeds[4]
                };
            return laneFive;
        }

        /// <summary>
        /// Gets all vehicle objects across all lanes.
        /// </summary>
        /// <returns>IList of Vehicles</returns>
        public IList<Vehicle> getVehicles()
        {
            IList<Vehicle> vehicles = new List<Vehicle>();
            foreach (var lane in this.lanes.Values)
            {
                foreach (var vehicle in lane)
                {
                    vehicles.Add(vehicle);
                }

            }
            return vehicles;
        }

        /// <summary>
        /// Moves all vehicles at lane speed.
        /// </summary>
        public void moveAllVehicles()
        {
            foreach (var lane in this.lanes.Values)
            {
                lane.MoveVehiclesOnTick();
            }
        }

        public void resetNumVehicles()
        {
            throw new System.NotImplementedException();
        }
    }
}
