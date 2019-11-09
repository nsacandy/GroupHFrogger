using System.Collections;
using System.Collections.Generic;
using FroggerStarter.Controller;
using FroggerStarter.Model.Vehicles;

namespace FroggerStarter.Model
{
    /// <summary>
    ///     Contains methods and initial values for generating and managing lanes.
    /// </summary>
    public class RoadManager : IEnumerable<Vehicle>
    {
        #region Data members

        private IList<Lane> lanes;
        private double laneWidth;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="RoadManager" /> class.
        /// </summary>
        public RoadManager(double backgroundWidth)
        {
            this.lanes = new List<Lane>();
            this.laneWidth = backgroundWidth;
            this.setLevelOneLaneValues();
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

        private void setLevelOneLaneValues()
        {
            this.setLaneOneLevelOne();
            this.setLaneTwoLevelOne();
            this.setLaneThreeLevelOne();
            this.setLaneFourLevelOne();
            this.setLaneFiveLevelOne();
        }

        private void setLevelTwoLaneValues()
        {
            this.setLaneOneLevelTwo();
            this.setLaneTwoLevelTwo();
            this.setLaneThreeLevelTwo();
            this.setLaneFourLevelTwo();
            this.setLaneFiveLevelTwo();
        }

        private void setLevelThreeLaneValues()
        {
            this.setLaneOneLevelThree();
            this.setLaneTwoLevelThree();
            this.setLaneThreeLevelThree();
            this.setLaneFourLevelThree();
            this.setLaneFiveLevelThree();
        }

        private void setLaneOneLevelOne()
        {
            var laneOne =
                new Lane(GameSettings.LaneOneLevelOneValues[0], GameSettings.LaneOneLevelOneValues[1], VehicleFactory.VehicleType.Car, Lane.Direction.Left, true) {
                    LaneSpeed = GameSettings.LaneOneLevelOneValues[2]
                };
            this.lanes.Add(laneOne);
        }

        private void setLaneTwoLevelOne()
        {
            var laneTwo =
                new Lane(GameSettings.LaneTwoLevelOneValues[0], GameSettings.LaneTwoLevelOneValues[1], VehicleFactory.VehicleType.SportsCar, Lane.Direction.Right, true) {
                    LaneSpeed = GameSettings.LaneTwoLevelOneValues[2]
                };
            this.lanes.Add(laneTwo);
        }

        private void setLaneThreeLevelOne()
        {
            var laneThree =
                new Lane(GameSettings.LaneThreeLevelOneValues[0], GameSettings.LaneThreeLevelOneValues[1], VehicleFactory.VehicleType.Car, Lane.Direction.Left, true) {
                    LaneSpeed = GameSettings.LaneThreeLevelOneValues[2]
                };
            this.lanes.Add(laneThree);
        }

        private void setLaneFourLevelOne()
        {
            var laneFour =
                new Lane(GameSettings.LaneFourLevelOneValues[0], GameSettings.LaneFourLevelOneValues[1], VehicleFactory.VehicleType.Truck, Lane.Direction.Left, true) {
                    LaneSpeed = GameSettings.LaneFourLevelOneValues[2]
                };
            this.lanes.Add(laneFour);
        }

        private void setLaneFiveLevelOne()
        {
            var laneFive =
                new Lane(GameSettings.LaneFiveLevelOneValues[0], GameSettings.LaneFiveLevelOneValues[1], VehicleFactory.VehicleType.Car, Lane.Direction.Right, true) {
                    LaneSpeed = GameSettings.LaneFiveLevelOneValues[2]
                };
            this.lanes.Add(laneFive);
        }

        private void setLaneOneLevelTwo()
        {
            var laneOne =
                new Lane(GameSettings.LaneOneLevelTwoValues[0], GameSettings.LaneOneLevelTwoValues[1], VehicleFactory.VehicleType.Car, Lane.Direction.Left, true)
                {
                    LaneSpeed = GameSettings.LaneOneLevelTwoValues[2]
                };
            this.lanes.Add(laneOne);
        }

        private void setLaneTwoLevelTwo()
        {
            var laneTwo =
                new Lane(GameSettings.LaneTwoLevelTwoValues[0], GameSettings.LaneTwoLevelTwoValues[1], VehicleFactory.VehicleType.SportsCar, Lane.Direction.Right, true)
                {
                    LaneSpeed = GameSettings.LaneTwoLevelTwoValues[2]
                };
            this.lanes.Add(laneTwo);
        }

        private void setLaneThreeLevelTwo()
        {
            var laneThree =
                new Lane(GameSettings.LaneThreeLevelTwoValues[0], GameSettings.LaneThreeLevelTwoValues[1], VehicleFactory.VehicleType.Car, Lane.Direction.Left, true)
                {
                    LaneSpeed = GameSettings.LaneThreeLevelTwoValues[2]
                };
            this.lanes.Add(laneThree);
        }

        private void setLaneFourLevelTwo()
        {
            var laneFour =
                new Lane(GameSettings.LaneFourLevelTwoValues[0], GameSettings.LaneFourLevelTwoValues[1], VehicleFactory.VehicleType.Truck, Lane.Direction.Left, true)
                {
                    LaneSpeed = GameSettings.LaneFourLevelTwoValues[2]
                };
            this.lanes.Add(laneFour);
        }

        private void setLaneFiveLevelTwo()
        {
            var laneFive =
                new Lane(GameSettings.LaneFiveLevelTwoValues[0], GameSettings.LaneFiveLevelTwoValues[1], VehicleFactory.VehicleType.Car, Lane.Direction.Right, true)
                {
                    LaneSpeed = GameSettings.LaneFiveLevelTwoValues[2]
                };
            this.lanes.Add(laneFive);
        }

        private void setLaneOneLevelThree()
        {
            var laneOne =
                new Lane(GameSettings.LaneOneLevelThreeValues[0], GameSettings.LaneOneLevelThreeValues[1], VehicleFactory.VehicleType.Car, Lane.Direction.Left, true)
                {
                    LaneSpeed = GameSettings.LaneOneLevelThreeValues[2]
                };
            this.lanes.Add(laneOne);
        }

        private void setLaneTwoLevelThree()
        {
            var laneTwo =
                new Lane(GameSettings.LaneTwoLevelThreeValues[0], GameSettings.LaneTwoLevelThreeValues[1], VehicleFactory.VehicleType.SportsCar, Lane.Direction.Right, true)
                {
                    LaneSpeed = GameSettings.LaneTwoLevelThreeValues[2]
                };
            this.lanes.Add(laneTwo);
        }

        private void setLaneThreeLevelThree()
        {
            var laneThree =
                new Lane(GameSettings.LaneThreeLevelThreeValues[0], GameSettings.LaneThreeLevelThreeValues[1], VehicleFactory.VehicleType.Car, Lane.Direction.Left, true)
                {
                    LaneSpeed = GameSettings.LaneThreeLevelThreeValues[2]
                };
            this.lanes.Add(laneThree);
        }

        private void setLaneFourLevelThree()
        {
            var laneFour =
                new Lane(GameSettings.LaneFourLevelThreeValues[0], GameSettings.LaneFourLevelThreeValues[1], VehicleFactory.VehicleType.Truck, Lane.Direction.Left, true)
                {
                    LaneSpeed = GameSettings.LaneFourLevelThreeValues[2]
                };
            this.lanes.Add(laneFour);
        }

        private void setLaneFiveLevelThree()
        {
            var laneFive =
                new Lane(GameSettings.LaneFiveLevelThreeValues[0], GameSettings.LaneFiveLevelThreeValues[1], VehicleFactory.VehicleType.Car, Lane.Direction.Right, true)
                {
                    LaneSpeed = GameSettings.LaneFiveLevelThreeValues[2]
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