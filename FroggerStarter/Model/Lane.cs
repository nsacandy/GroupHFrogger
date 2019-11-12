using System;
using System.Collections;
using System.Collections.Generic;
using Windows.UI.Xaml;
using FroggerStarter.Model.Vehicles;

namespace FroggerStarter.Model
{
    /// <summary>Lane class for keeping track of vehicles</summary>
    /// <seealso cref="System.Collections.Generic.IEnumerable{FroggerStarter.Model.Vehicles.Vehicle}" />
    public class Lane : IEnumerable<Vehicle>
    {
        #region Types and Delegates

        /// <summary>The direction that dictates vehicle heading</summary>
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

        /// <summary>Gets or sets for all vehicles in the lane</summary>
        /// <value>The lane speed.</value>
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

        /// <summary>Initializes a new instance of the <see cref="Lane" /> class.</summary>
        /// <param name="yValue">The y value.</param>
        /// <param name="numVehicles">The number vehicles.</param>
        /// <param name="vehicleType">Type of the vehicle.</param>
        /// <param name="laneDirection">The lane direction.</param>
        /// <param name="graduallyAddVehicles">if set to <c>true</c> [gradually add vehicles].</param>
        public Lane(int yValue, int numVehicles, VehicleFactory.VehicleType vehicleType, Direction laneDirection,
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
                this.HideRandomVehicles();
                this.VehicleOutOfBounds += this.revealNewVehicle;
            }
        }

        #endregion

        #region Methods

        /// <summary>Returns an enumerator that iterates through the collection of vehicles.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
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

        private void generateVehicles(int numVehicles, VehicleFactory.VehicleType vehicleType)
        {
            this.laneVehicles.Clear();
            for (var i = 0; i < numVehicles; i++)
            {
                var heading = this.laneDirection.Equals(Direction.Left)
                    ? GameObject.Heading.Left
                    : GameObject.Heading.Right;
                var vehicle = VehicleFactory.CreateVehicle(vehicleType, heading);
                this.laneVehicles.Add(vehicle);
            }
        }

        /// <summary>Hides the random vehicles when generated .</summary>
        public void HideRandomVehicles()
        {
            var carPicker = new Random();
            var carIndex = carPicker.Next(this.laneVehicles.Count);
            var firstVisibleCar = this.laneVehicles[carIndex];

            foreach (var vehicle in this.laneVehicles)
            {
                vehicle.Sprite.Visibility =
                    firstVisibleCar.Equals(vehicle) ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        /// <summary>Collapses all vehicles.</summary>
        public void CollapseAllVehicles()
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

        /// <summary>
        ///     <para>Specialized args for passing a car out of bounds</para>
        /// </summary>
        /// <seealso cref="System.EventArgs" />
        public class VehicleArgs : EventArgs
        {
            #region Properties

            /// <summary>Gets the out of bounds vehicle.</summary>
            /// <value>The out of bounds vehicle.</value>
            public Vehicle OutOfBoundsVehicle { get; }

            #endregion

            #region Constructors

            /// <summary>Initializes a new instance of the <see cref="VehicleArgs" /> class.</summary>
            /// <param name="outOfBoundsVehicle">The out of bounds vehicle.</param>
            public VehicleArgs(Vehicle outOfBoundsVehicle)
            {
                this.OutOfBoundsVehicle = outOfBoundsVehicle;
            }

            #endregion
        }

        #endregion
    }
}