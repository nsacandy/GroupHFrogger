using System.Collections.Generic;

namespace FroggerStarter.Model.HighScoreModel
{
    public class HighScores
    {
        #region Properties

        public IList<HighScore> PlayerHighScores { get; }

        public HighScore this[int i]
        {
            get => this.PlayerHighScores[i];
            set => this.PlayerHighScores[i] = value;
        }

        #endregion

        #region Constructors

        public HighScores()
        {
            this.PlayerHighScores = new List<HighScore>();

            this.PlayerHighScores.Add(new HighScore {Level = LevelManager.GameLevel.Two, Score = 100, Name = "Aaron"});
            this.PlayerHighScores.Add(new HighScore {Level = LevelManager.GameLevel.One, Score = 50, Name = "Aaron"});
            this.PlayerHighScores.Add(new HighScore
                {Level = LevelManager.GameLevel.Final, Score = 200, Name = "Aaron"});
            this.PlayerHighScores.Add(new HighScore {Level = LevelManager.GameLevel.Two, Score = 95, Name = "Aaron"});
            this.PlayerHighScores.Add(new HighScore
                {Level = LevelManager.GameLevel.Final, Score = 300, Name = "Aaron"});
        }

        #endregion

        #region Methods

        public void Add(HighScore score)
        {
            this.PlayerHighScores.Add(score);
        }

        #endregion
    }
}