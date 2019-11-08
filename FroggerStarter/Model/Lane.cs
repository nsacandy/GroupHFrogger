using System;
using System.Collections;
using System.Collections.Generic;
using Windows.UI.Xaml;

namespace FroggerStarter.Model
{
    public class Lane : IEnumerable<Vehicle>
    {
        #region Types and Delegates

        public enum Direction
        {
            Left,
            Right
        }

        #endregion

        #region Data members

        private readonly int yValue;
        private readonly Direction laneDirection;
        private readonly double width;

        private readonly IList<Vehicle> laneVehicles;
        private readonly bool graduallyAddVehicles;
        private bool timeForNewVehicle;

        #endregion

        #region Properties

        public int LaneSpeed
        {
            get => this.LaneSpeed;
            set
            {
                foreach (var vehicle in this.laneVehicles)
                {
                    vehicle.SetSpeed(value);
                }
            }
        }

        #endregion

        #region Constructors

        public Lane(int yValue, int numVehicles, Vehicle.VehicleType vehicleType, Direction laneDirection,
            bool graduallyAddVehicles)
        {
            this.yValue = yValue;
            this.laneDirection = laneDirection;
            this.width = (double) Application.Current.Resources["AppWidth"];
            this.graduallyAddVehicles = graduallyAddVehicles;
            this.laneVehicles = new List<Vehicle>();
            this.generateVehicles(numVehicles, vehicleType);
            this.placeVehicles();

            this.VehicleOutOfBounds += this.resetVehicleXLocation;
            if (this.graduallyAddVehicles)
            {
                this.HideVehicles();
                this.VehicleOutOfBounds += this.revealNewVehicle;
            }
        }

        #endregion

        #region Methods

        public IEnumerator<Vehicle> GetEnumerator()
        {
            foreach (var vehicle in this.laneVehicles)
            {
                yield return vehicle;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private event EventHandler<VehicleArgs> VehicleOutOfBounds;

        private void generateVehicles(int numVehicles, Vehicle.VehicleType vehicleType)
        {
            for (var i = 0; i < numVehicles; i++)
            {
                var heading = this.laneDirection.Equals(Direction.Left) ? Vehicle.Heading.Left : Vehicle.Heading.Right;
                var vehicle = this.createVehicle(vehicleType, heading);
                this.laneVehicles.Add(vehicle);
            }
        }

        private Vehicle createVehicle(Vehicle.VehicleType type, Vehicle.Heading heading)
        {
            switch (type)
            {
                case Vehicle.VehicleType.Truck:
                    return new Truck(heading);
                case Vehicle.VehicleType.Car:
                    return new Car(heading);
                default:
                    return null;
            }
        }

        public void HideVehicles()
        {
            var carPicker = new Random();
            var carIndex = carPicker.Next(this.laneVehicles.Count);
            var firstVisibleCar = this.laneVehicles[carIndex];

            foreach (var vehicle in this.laneVehicles)
            {
                vehicle.Sprite.Visibility =
                    (firstVisibleCar.Equals(vehicle)) ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public void RemoveAllVehicles()
        {
            foreach (var vehicle in this.laneVehicles)
            {
                vehicle.Sprite.Visibility = Visibility.Collapsed;
            }
        }

        private void placeVehicles()
        {
            for (var i = 0; i < this.laneVehicles.Count; i++)
            {
                this.laneVehicles[i].X = this.width / this.laneVehicles.Count * i;
                this.laneVehicles[i].Y = this.yValue;
            }
        }

        /// <summary>
        ///     Gets the vehicles.
        /// </summary>
        /// <returns>List of vehicles</returns>
        public IList<Vehicle> GetVehicles()
        {
            return this.laneVehicles;
        }

        /// <summary>
        ///     Moves the vehicles on tick.
        ///     <postcondition>All vehicles x values incremented by lane speed</postcondition>
        /// </summary>
        public void MoveVehiclesOnTick()
        {
            foreach (var vehicle in this.laneVehicles)
            {
                vehicle.MoveVehicle();
                this.checkIfVehicleOutOfBounds(vehicle);
            }
        }

        private void checkIfVehicleOutOfBounds(Vehicle vehicle)
        {
            if (this.laneDirection.Equals(Direction.Right) && vehicle.X > this.width)
            {
                this.OnRaiseVehicleOutOfBounds(new VehicleArgs(vehicle));
            }

            else if (this.laneDirection.Equals(Direction.Left) && vehicle.X + vehicle.Width < 0)
            {
                this.OnRaiseVehicleOutOfBounds(new VehicleArgs(vehicle));
            }
        }

        protected virtual void OnRaiseVehicleOutOfBounds(VehicleArgs vehicle)
        {
            var handler = this.VehicleOutOfBounds;

            handler?.Invoke(this, vehicle);
        }

        private void revealNewVehicle(object sender, VehicleArgs args)
        {
            var vehicle = args.OutOfBoundsVehicle;
            if (vehicle.Sprite.Visibility.Equals(Visibility.Visible))
            {
                this.timeForNewVehicle = true;
            }

            else if (this.timeForNewVehicle)
            {
                vehicle.Sprite.Visibility = Visibility.Visible;
                this.timeForNewVehicle = false;
            }
        }

        private void resetVehicleXLocation(object sender, VehicleArgs args)
        {
            var vehicle = args.OutOfBoundsVehicle;
            if (this.laneDirection.Equals(Direction.Left))
            {
                vehicle.X = this.width + vehicle.Sprite.Width;
            }

            if (this.laneDirection.Equals(Direction.Right))
            {
                vehicle.X = -vehicle.Sprite.Width;
            }
        }

        public class VehicleArgs : EventArgs
        {
            #region Data members

            #endregion

            #region Properties

            public Vehicle OutOfBoundsVehicle { get; }

            #endregion

            #region Constructors

            public VehicleArgs(Vehicle outOfBoundsVehicle)
            {
                this.OutOfBoundsVehicle = outOfBoundsVehicle;
            }

            #endregion
        }

        #endregion
    }
}