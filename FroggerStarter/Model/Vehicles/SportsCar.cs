using FroggerStarter.View.Sprites;

namespace FroggerStarter.Model.Vehicles
{
    /// <summary>A slightly faster car extended from the Car class</summary>
    /// <seealso cref="FroggerStarter.Model.Vehicles.Car" />
    public sealed class SportsCar : Car
    {
        #region Data members

        private readonly int extraSpeed = 2;

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="SportsCar" /> class.</summary>
        /// <param name="heading">The heading of the sprite</param>
        public SportsCar(Heading heading) : base(heading)
        {
            Sprite = new SportsCarSprite();
            SetHeading(heading);
        }

        #endregion

        #region Methods

        /// <summary>Sets the speed faster than a normal Car object would go.</summary>
        /// <param name="speed">The speed before being augmented.</param>
        public override void SetSpeed(int speed)
        {
            base.SetSpeed(speed + this.extraSpeed, speed + this.extraSpeed);
        }

        #endregion
    }
}