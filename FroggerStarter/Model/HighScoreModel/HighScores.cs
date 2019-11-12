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
            this.players = new List<HighScore>();
        }

        #endregion

        #region Methods

        /// <summary>Adds the specified score.</summary>
        /// <param name="score">The score.</param>
        public void Add(HighScore score)
        {
            this.players.Add(new HighScore(level, score, name));
        }

        #endregion
    }
}