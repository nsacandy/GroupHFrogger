using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using FroggerStarter.Extentions;
using FroggerStarter.Model;
using FroggerStarter.Model.HighScoreModel;

namespace FroggerStarter.ViewModel
{
    public class HighScoreViewModel : INotifyPropertyChanged
    {
        private HighScores highScores;

        private ObservableCollection<HighScore> allScores;

        public ObservableCollection<HighScore> AllScores
        {
            get => this.allScores;
            set
            {
                this.allScores = value;
                this.OnPropertyChanged();
            }
        }

        public HighScoreViewModel()
        {
            this.highScores = new HighScores();
            this.AllScores = this.highScores.PlayerHighScores.ToObservableCollection();
        }

        public void AddPlayerToHighScore(LevelManager.GameLevel level, int score, string name)
        {
            this.highScores.Add(level, score, name);
            this.AllScores = this.highScores.PlayerHighScores.ToObservableCollection();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
