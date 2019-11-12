using FroggerStarter.View.Sprites;
using FroggerStarter.View.Sprites.VehicleSprites;

namespace FroggerStarter.Model.Vehicles
{
    internal sealed class Truck : Vehicle
    {
        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="Truck" /> class.</summary>
        /// <param name="heading">The heading of the Sprite</param>
        public Truck(Heading heading)
        {
            Sprite = new TruckSprite();
            SetHeading(heading);
        }

        #endregion
    }
}