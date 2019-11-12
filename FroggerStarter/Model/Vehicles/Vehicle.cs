using Windows.Foundation;
using Windows.UI.Xaml.Media;

namespace FroggerStarter.Model.Vehicles
{
    /// <summary>
    ///     Abstract class for defining basic Vehicle functionality
    /// </summary>
    public abstract class Vehicle : GameObject
    {
        #region Methods

        /// <summary>Sets the heading of the spite.</summary>
        /// <param name="heading">The heading.</param>
        public override void SetHeading(Heading heading)
        {
            CurrentHeading = heading;
            switch (CurrentHeading)
            {
                case Heading.Right:
                    this.HeadRight();
                    break;
                case Heading.Left:
                    break;
            }
        }

        /// <summary>Heads the Vehicle right</summary>
        protected void HeadRight()
        {
            Sprite.RenderTransformOrigin = new Point(0.5, 0.5);
            Sprite.RenderTransform = new ScaleTransform {ScaleX = -1};
        }

        /// <summary>
        ///   <para>
        ///  Moves the vehicle in the direction of the heading
        /// </para>
        /// </summary>
        public void MoveVehicle()
        {
            switch (CurrentHeading)
            {
                case Heading.Left:
                    X -= SpeedX;
                    break;
                case Heading.Right:
                    X += SpeedX;
                    break;
                case Heading.Down:
                    Y += SpeedY;
                    break;
                case Heading.Up:
                    Y -= SpeedY;
                    break;
            }
        }

        /// <summary>Sets the speed of the Vehicle</summary>
        /// <param name="speed">The speed.</param>
        public virtual void SetSpeed(int speed)
        {
            base.SetSpeed(speed, speed);
        }

        #endregion
    }
}