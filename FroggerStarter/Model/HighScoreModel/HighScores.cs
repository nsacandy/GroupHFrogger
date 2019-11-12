using System.Collections.Generic;

namespace FroggerStarter.Model.HighScoreModel
{
    /// <summary>Keeps up with high scores</summary>
    public class HighScores
    {
        #region Properties

        /// <summary>Gets the player high scores.</summary>
        /// <value>The player high scores.</value>
        public IList<HighScore> PlayerHighScores { get; }

        /// <summary>Gets or sets the <see cref="HighScore"/> at the specified index.</summary>
        /// <param name="i">The index that holds the score.</param>
        /// <value>The <see cref="HighScore"/>.</value>
        /// <returns></returns>
        public HighScore this[int i]
        {
            get => this.PlayerHighScores[i];
            set => this.PlayerHighScores[i] = value;
        }

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="HighScores"/> class.</summary>
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

        /// <summary>Adds the specified score.</summary>
        /// <param name="score">The score.</param>
        public void Add(HighScore score)
        {
            this.PlayerHighScores.Add(score);
        }

        #endregion
    }
}