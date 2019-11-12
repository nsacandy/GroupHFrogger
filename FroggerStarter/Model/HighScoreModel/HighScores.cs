using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FroggerStarter.Model.HighScoreModel
{
    public class HighScores
    {

        private IList<HighScore> players;

        public IList<HighScore> PlayerHighScores => this.players;

        public HighScore this[int i]
        {
            get => this.players[i];
            set => this.players[i] = value;
        }

        public HighScores()
        {
            this.players = new List<HighScore>();

            this.players.Add(new HighScore() {Level = LevelManager.GameLevel.Two, Score = 100, Name = "Aaron"});
            this.players.Add(new HighScore() { Level = LevelManager.GameLevel.One, Score = 50, Name = "Aaron" });
            this.players.Add(new HighScore() { Level = LevelManager.GameLevel.Final, Score = 200, Name = "Aaron" });
            this.players.Add(new HighScore() { Level = LevelManager.GameLevel.Two, Score = 95, Name = "Aaron" });
            this.players.Add(new HighScore() { Level = LevelManager.GameLevel.Final, Score = 300, Name = "Aaron" });
        }

        public void Add(HighScore score)
        {
            this.players.Add(score);
        }
    }
}
