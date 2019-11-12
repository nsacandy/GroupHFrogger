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
        }

        public void Add(LevelManager.GameLevel level, int score, string name)
        {
            this.players.Add(new HighScore(level, score, name));
        }
    }
}
