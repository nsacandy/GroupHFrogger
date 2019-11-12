// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

using FroggerStarter.View.Sprites.VehicleSprites;

namespace FroggerStarter.View.Sprites
{
    public partial class CarSprite : BaseSprite, IVehicleSprite

    {
        #region Constructors

        public CarSprite()
        {
            this.InitializeComponent();
        }

        #endregion
    }
}