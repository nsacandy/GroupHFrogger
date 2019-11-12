using System;
using Windows.UI.Xaml;

namespace FroggerStarter.Controller
{
    /// <summary>
    ///     <para>Class that keeps track of various settings in the game</para>
    /// </summary>
    internal static class GameSettings
    {
        #region Data members

        public const int InitialNumLives = 4;
        public const double BottomLaneOffset = 5;
        public const double LeftBorder = 0;
        public const int LifeLengthInSeconds = 20;
        public const int TimeSpriteAppearInterval = 4;
        public const int InvincibilityAppearInterval = 5;
        public static readonly TimeSpan InvincibilityLength = new TimeSpan(0, 0, 5);

        public static readonly double TopLaneOffset = (double) Application.Current.Resources["HighRoadYLocation"];
        public static readonly double BackgroundWidth = (double) Application.Current.Resources["AppWidth"];
        public static readonly double BackgroundHeight = (double) Application.Current.Resources["AppHeight"];

        public static readonly double TopBorder = (double) Application.Current.Resources["HighRoadYLocation"] +
                                                  (double) Application.Current.Resources["RoadHeight"];

        public static readonly int[] LaneOneLevelOneValues = {305, 3, 2};
        public static readonly int[] LaneTwoLevelOneValues = {255, 2, 3};
        public static readonly int[] LaneThreeLevelOneValues = {205, 4, 4};
        public static readonly int[] LaneFourLevelOneValues = {155, 3, 5};
        public static readonly int[] LaneFiveLevelOneValues = {105, 5, 6};

        public static readonly int[] LaneOneLevelTwoValues = {305, 4, 3};
        public static readonly int[] LaneTwoLevelTwoValues = {255, 3, 4};
        public static readonly int[] LaneThreeLevelTwoValues = {205, 5, 5};
        public static readonly int[] LaneFourLevelTwoValues = {155, 4, 6};
        public static readonly int[] LaneFiveLevelTwoValues = {105, 6, 7};

        public static readonly int[] LaneOneLevelThreeValues = {305, 5, 4};
        public static readonly int[] LaneTwoLevelThreeValues = {255, 4, 5};
        public static readonly int[] LaneThreeLevelThreeValues = {205, 6, 6};
        public static readonly int[] LaneFourLevelThreeValues = {155, 5, 7};
        public static readonly int[] LaneFiveLevelThreeValues = {105, 7, 8};

        #endregion
    }
}