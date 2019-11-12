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
        private GameManager manager;
        private LevelManager currLevel;
        private int level;

        public string Name { get; set; }
        public int Score { get; set; }

        public LevelManager.GameLevel Level
        {
            get;
            set;
        }

        public HighScore()
        {
            this.manager = manager;
            this.currLevel = currLevel;
        }

        public override string ToString()
        {
            return $"{this.Name} : {this.Level} : {this.Score}";
        }
    }
}
