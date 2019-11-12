using FroggerStarter.View.Sprites;

namespace FroggerStarter.Model.Vehicles
{
    /// <summary>A vehicle class for binding with a car sprite</summary>
    /// <seealso cref="FroggerStarter.Model.Vehicles.Vehicle" and
    ///     cref="FroggerStarter.View.VehicleSprites.CarSprite" />
    public class Car : Vehicle
    {
        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="Car" /> class.</summary>
        /// <param name="heading">The heading of the sprite</param>
        public Car(Heading heading)
        {
            Sprite = new CarSprite();
            setHeading(heading);
        }

        #endregion
    }
}