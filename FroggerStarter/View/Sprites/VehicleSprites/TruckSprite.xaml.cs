// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

using FroggerStarter.View.Sprites.VehicleSprites;

namespace FroggerStarter.View.Sprites
{
    public sealed partial class TruckSprite : BaseSprite, IVehicleSprite
    {
        #region Constructors

        public TruckSprite()
        {
            this.InitializeComponent();
        }

        #endregion
    }
}