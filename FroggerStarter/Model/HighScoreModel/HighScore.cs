using FroggerStarter.Controller;

namespace FroggerStarter.Model.HighScoreModel
{
    public class HighScore
    {
        #region Data members

        private readonly GameManager manager;
        private readonly LevelManager currLevel;
        private int level;

        #endregion

        #region Properties

        public string Name { get; set; }
        public int Score { get; set; }

        public LevelManager.GameLevel Level { get; set; }

        #endregion

        #region Constructors

        public HighScore()
        {
            this.manager = this.manager;
            this.currLevel = this.currLevel;
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return $"{this.Name} : {this.Level} : {this.Score}";
        }

        #endregion
    }
}