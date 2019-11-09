using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


        public LevelManager()
        {
            
        }

        public void MoveToNextLevel()
        {
            switch (CurrentLevel)
            {
                case GameLevel.One:
                    this.CurrentLevel = GameLevel.Two;
                    break;
                case GameLevel.Two:
                    this.CurrentLevel = GameLevel.Final;
                    break;
            }
        }
        
    }
}
