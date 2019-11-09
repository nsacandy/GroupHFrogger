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
        public const int NunberOfLevels = 3;

        public static readonly double TOP_LANE_OFFSET = (double)Application.Current.Resources["HighRoadYLocation"];
        public static readonly double backgroundWidth = (double) Application.Current.Resources["AppWidth"];
        public static readonly double backgroundHeight = (double)Application.Current.Resources["AppHeight"];

        public static readonly double TopBorder = (double) Application.Current.Resources["HighRoadYLocation"] +
                                           (double) Application.Current.Resources["RoadHeight"];

        public static readonly int[] LaneOneLevelOneValues = { 305, 3, 2 };
        public static readonly int[] LaneTwoLevelOneValues = { 255, 2, 3 };
        public static readonly int[] LaneThreeLevelOneValues = { 205, 4, 4 };
        public static readonly int[] LaneFourLevelOneValues = { 155, 3, 5 };
        public static readonly int[] LaneFiveLevelOneValues = { 105, 5, 6 };

        public static readonly int[] LaneOneLevelTwoValues = { 305, 4, 3 };
        public static readonly int[] LaneTwoLevelTwoValues = { 255, 3, 4 };
        public static readonly int[] LaneThreeLevelTwoValues = { 205, 5, 5 };
        public static readonly int[] LaneFourLevelTwoValues = { 155, 4, 6 };
        public static readonly int[] LaneFiveLevelTwoValues = { 105, 6, 7 };

        public static readonly int[] LaneOneLevelThreeValues = { 305, 5, 4 };
        public static readonly int[] LaneTwoLevelThreeValues = { 255, 4, 5 };
        public static readonly int[] LaneThreeLevelThreeValues = { 205, 6, 6 };
        public static readonly int[] LaneFourLevelThreeValues = { 155, 5, 7 };
        public static readonly int[] LaneFiveLevelThreeValues = { 105, 7, 8 };
    }
}