using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Store.Preview.InstallControl;
using FroggerStarter.Controller;

namespace FroggerStarter.Model.HighScoreModel
{
    public class HighScore
    {
        public string Name { get; private set; }
        public int Score { get; private set; }
        public LevelManager.GameLevel Level { get; private set; }

        public HighScore(LevelManager.GameLevel level, int score, string name)
        {
            this.Level = level;
            this.Score = score;
            this.Name = name;
        }

        public override string ToString()
        {
            return $"{this.Name} : {this.Level} : {this.Score}";
        }
    }
}
