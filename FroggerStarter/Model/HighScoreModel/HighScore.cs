using FroggerStarter.Controller;

namespace FroggerStarter.Model.HighScoreModel
{
    /// <summary>Keeps track of high score</summary>
    public class HighScore
    {
        #region Data members

        

        #endregion

        #region Properties

        /// <summary>Gets or sets the name of the scorer.</summary>
        /// <value>The name.</value>
        public string Name { get; set; }
        /// <summary>Gets or sets the high score achieved.</summary>
        /// <value>The score.</value>
        public int Score { get; set; }

        /// <summary>Gets or sets the level achieved.</summary>
        /// <value>The level.</value>
        public LevelManager.GameLevel Level { get; set; }

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="HighScore"/> class.</summary>
        public HighScore(LevelManager.GameLevel level, int score, string name)
        {
            this.Level = level;
            this.Score = score;
            this.Name = name;
        }

        #endregion

        #region Methods

        /// <summary>Converts to string.</summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return $"{this.Name} : {this.Level} : {this.Score}";
        }

        #endregion
    }
}