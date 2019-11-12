namespace FroggerStarter.Model
{
    public class LevelManager
    {
        public enum GameLevel
        {
            One,
            Two,
            Final
        }


        public GameLevel CurrentLevel { get; private set; } = GameLevel.One;

        public void MoveToNextLevel()
        {
            switch (CurrentLevel)
            {
                case GameLevel.One:
                    CurrentLevel = GameLevel.Two;
                    break;
                case GameLevel.Two:
                    CurrentLevel = GameLevel.Final;
                    break;
            }
        }
    }
}