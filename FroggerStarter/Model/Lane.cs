using System;
using System.Collections;
using System.Collections.Generic;
using Windows.UI.Xaml;
using FroggerStarter.Model.Vehicles;

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

        #region Constructors

        public Lane(int yValue, int numVehicles, VehicleFactory.VehicleType vehicleType, Direction laneDirection,
            bool graduallyAddVehicles)
        {
            this.yValue = yValue;
            this.laneDirection = laneDirection;
            width = (double) Application.Current.Resources["AppWidth"];
            this.graduallyAddVehicles = graduallyAddVehicles;
            laneVehicles = new List<Vehicle>();
            generateVehicles(numVehicles, vehicleType);
            placeVehicles();

            VehicleOutOfBounds += resetVehicleXLocation;
            if (this.graduallyAddVehicles)
            {
                HideVehicles();
                VehicleOutOfBounds += revealNewVehicle;
            }
        }

        #endregion

        #region Properties

        public int LaneSpeed
        {
            get => LaneSpeed;
            set
            {
                foreach (var vehicle in laneVehicles) vehicle.SetSpeed(value);
            }
        }

        #endregion

        #region Data members

        private readonly int yValue;
        private readonly Direction laneDirection;
        private readonly double width;

        private readonly IList<Vehicle> laneVehicles;
        private readonly bool graduallyAddVehicles;
        private bool timeForNewVehicle;

        private event EventHandler<VehicleArgs> VehicleOutOfBounds;

        #endregion

        #region Methods

        public IEnumerator<Vehicle> GetEnumerator()
        {
            foreach (var vehicle in laneVehicles) yield return vehicle;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void generateVehicles(int numVehicles, VehicleFactory.VehicleType vehicleType)
        {
            laneVehicles.Clear();
            for (var i = 0; i < numVehicles; i++)
            {
                var heading = laneDirection.Equals(Direction.Left) ? GameObject.Heading.Left : GameObject.Heading.Right;
                var vehicle = VehicleFactory.CreateVehicle(vehicleType, heading);
                laneVehicles.Add(vehicle);
            }
        }

        public void HideVehicles()
        {
            var carPicker = new Random();
            var carIndex = carPicker.Next(laneVehicles.Count);
            var firstVisibleCar = laneVehicles[carIndex];

            foreach (var vehicle in laneVehicles)
                vehicle.Sprite.Visibility =
                    firstVisibleCar.Equals(vehicle) ? Visibility.Visible : Visibility.Collapsed;
        }

        public void CollapseAllVehicles()
        {
            foreach (var vehicle in laneVehicles) vehicle.Sprite.Visibility = Visibility.Collapsed;
        }

        private void placeVehicles()
        {
            for (var i = 0; i < laneVehicles.Count; i++)
            {
                laneVehicles[i].X = width / laneVehicles.Count * i;
                laneVehicles[i].Y = yValue;
            }
        }

        /// <summary>
        ///     Moves the vehicles on tick.
        ///     <postcondition>All vehicles x values incremented by lane speed</postcondition>
        /// </summary>
        public void MoveVehiclesOnTick()
        {
            foreach (var vehicle in laneVehicles)
            {
                vehicle.MoveVehicle();
                checkIfVehicleOutOfBounds(vehicle);
            }
        }

        private void checkIfVehicleOutOfBounds(Vehicle vehicle)
        {
            if (laneDirection.Equals(Direction.Right) && vehicle.X > width)
                OnRaiseVehicleOutOfBounds(new VehicleArgs(vehicle));

            else if (laneDirection.Equals(Direction.Left) && vehicle.X + vehicle.Width < 0)
                OnRaiseVehicleOutOfBounds(new VehicleArgs(vehicle));
        }

        protected virtual void OnRaiseVehicleOutOfBounds(VehicleArgs vehicle)
        {
            var handler = VehicleOutOfBounds;

            handler?.Invoke(this, vehicle);
        }

        private void revealNewVehicle(object sender, VehicleArgs args)
        {
            var vehicle = args.OutOfBoundsVehicle;
            if (vehicle.Sprite.Visibility.Equals(Visibility.Visible))
            {
                timeForNewVehicle = true;
            }

            else if (timeForNewVehicle)
            {
                vehicle.Sprite.Visibility = Visibility.Visible;
                timeForNewVehicle = false;
            }
        }

        private void resetVehicleXLocation(object sender, VehicleArgs args)
        {
            var vehicle = args.OutOfBoundsVehicle;
            if (laneDirection.Equals(Direction.Left)) vehicle.X = width + vehicle.Sprite.Width;

            if (laneDirection.Equals(Direction.Right)) vehicle.X = -vehicle.Sprite.Width;
        }

        public class VehicleArgs : EventArgs
        {
            #region Constructors

            public VehicleArgs(Vehicle outOfBoundsVehicle)
            {
                OutOfBoundsVehicle = outOfBoundsVehicle;
            }

            #endregion

            #region Properties

            public Vehicle OutOfBoundsVehicle { get; }

            #endregion

            #region Data members

            #endregion
        }

        #endregion
    }
}