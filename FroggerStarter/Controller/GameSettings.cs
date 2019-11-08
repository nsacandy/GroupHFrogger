using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace FroggerStarter.Controller
{
    static class GameSettings
    {
        public const double bottomLaneOffset = 5;
        public const double leftBorder = 0;
        public const int GameLengthInSeconds = 20;

        public static readonly double TOP_LANE_OFFSET = (double)Application.Current.Resources["HighRoadYLocation"];
        public static readonly double backgroundWidth = (double) Application.Current.Resources["AppWidth"];
        public static readonly double backgroundHeight = (double)Application.Current.Resources["AppHeight"];

        public static readonly double TopBorder = (double) Application.Current.Resources["HighRoadYLocation"] +
                                           (double) Application.Current.Resources["RoadHeight"];
    }
}