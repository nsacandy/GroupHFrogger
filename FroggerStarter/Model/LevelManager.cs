namespace FroggerStarter.Model
{
    public class LevelManager
    {
        #region Types and Delegates

        public enum GameLevel
        {
            One,
            Two,
            Final
        }

        #endregion

        #region Properties

        public GameLevel CurrentLevel { get; private set; } = GameLevel.One;

        #endregion

        #region Methods

        public void MoveToNextLevel()
        {
            switch (this.CurrentLevel)
            {
                case GameLevel.One:
                    this.CurrentLevel = GameLevel.Two;
                    break;
                case GameLevel.Two:
                    this.CurrentLevel = GameLevel.Final;
                    break;
            }
        }

        #endregion
    }
}