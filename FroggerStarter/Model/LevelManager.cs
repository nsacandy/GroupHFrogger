namespace FroggerStarter.Model
{
    /// <summary>Manages different levels</summary>
    public class LevelManager
    {
        #region Types and Delegates

        /// <summary>Level enum</summary>
        public enum GameLevel
        {
            One,
            Two,
            Three
        }

        #endregion

        #region Properties

        /// <summary>Gets and sets the current level. Starts at 1</summary>
        /// <value>The current level.</value>
        public GameLevel CurrentLevel { get; private set; } = GameLevel.One;

        #endregion

        #region Methods

        /// <summary>Moves to next level.</summary>
        public void MoveToNextLevel()
        {
            switch (this.CurrentLevel)
            {
                case GameLevel.One:
                    this.CurrentLevel = GameLevel.Two;
                    break;
                case GameLevel.Two:
                    CurrentLevel = GameLevel.Three;
                    break;
            }
        }

        #endregion
    }
}