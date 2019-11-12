// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

using FroggerStarter.View.Sprites.VehicleSprites;

namespace FroggerStarter.View.Sprites
{
    /// <summary>Creates a new carSprite</summary>
    /// <seealso cref="FroggerStarter.View.Sprites.BaseSprite" />
    /// <seealso cref="FroggerStarter.View.Sprites.VehicleSprites.IVehicleSprite" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public partial class CarSprite : BaseSprite, IVehicleSprite

    {
        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="CarSprite" /> class.</summary>
        public CarSprite()
        {
            this.InitializeComponent();
        }

        #endregion
    }
}