using System.Collections;
using System.Collections.Generic;

namespace FroggerStarter.Model
{
    /// <summary>
    ///     Contains methods and initial values for generating and managing lanes.
    /// </summary>
    public class RoadManager : IEnumerable<Vehicle>
    {
        #region Data members

        private static readonly int[] LaneOneValues = {305, 3, 2};
        private static readonly int[] LaneTwoValues = {255, 2, 3};
        private static readonly int[] LaneThreeValues = {205, 4, 4};
        private static readonly int[] LaneFourValues = {155, 3, 5};
        private static readonly int[] LaneFiveValues = {105, 5, 6};

        private IList<Lane> lanes;
        private double laneWidth;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="RoadManager" /> class.
        /// </summary>
        public RoadManager(double backgroundWidth)
        {
            this.laneWidth = backgroundWidth;
            this.setInitialLaneValues();
        }

        #endregion

        #region Methods

        public IEnumerator<Vehicle> GetEnumerator()
        {
            foreach (var lane in this.lanes)
            {
                foreach (var vehicle in lane)
                {
                    yield return vehicle;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private void setInitialLaneValues()
        {
            this.lanes = new List<Lane>();
            this.setInitialLaneOne();
            this.setInitialLaneTwo();
            this.setInitialLaneThree();
            this.setInitialLaneFour();
            this.getInitialLaneFive();
        }

        private void setInitialLaneOne()
        {
            var laneOne =
                new Lane(LaneOneValues[0], LaneOneValues[1], Vehicle.VehicleType.Car, Lane.Direction.Left, true) {
                    LaneSpeed = LaneOneValues[2]
                };
            this.lanes.Add(laneOne);
        }

        private void setInitialLaneTwo()
        {
            var laneTwo =
                new Lane(LaneTwoValues[0], LaneTwoValues[1], Vehicle.VehicleType.Truck, Lane.Direction.Right, true) {
                    LaneSpeed = LaneTwoValues[2]
                };
            this.lanes.Add(laneTwo);
        }

        private void setInitialLaneThree()
        {
            var laneThree =
                new Lane(LaneThreeValues[0], LaneThreeValues[1], Vehicle.VehicleType.Car, Lane.Direction.Left, true) {
                    LaneSpeed = LaneThreeValues[2]
                };
            this.lanes.Add(laneThree);
        }

        private void setInitialLaneFour()
        {
            var laneFour =
                new Lane(LaneFourValues[0], LaneFourValues[1], Vehicle.VehicleType.Truck, Lane.Direction.Left, true) {
                    LaneSpeed = LaneFourValues[2]
                };
            this.lanes.Add(laneFour);
        }

        private void getInitialLaneFive()
        {
            var laneFive =
                new Lane(LaneFiveValues[0], LaneFiveValues[1], Vehicle.VehicleType.Car, Lane.Direction.Right, true) {
                    LaneSpeed = LaneFiveValues[2]
                };
            this.lanes.Add(laneFive);
        }

        /// <summary>
        ///     Moves all vehicles at lane speed.
        /// </summary>
        public void moveAllVehicles()
        {
            foreach (var lane in this.lanes)
            {
                lane.MoveVehiclesOnTick();
            }
        }

        public void resetNumVehicles()
        {
            foreach (var lane in this.lanes)
            {
                lane.HideVehicles();
            }
        }

        public void RemoveAllVehicles()
        {
            foreach (var lane in this.lanes)
            {
                lane.RemoveAllVehicles();
            }
        }

        #endregion
    }
}