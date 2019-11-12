using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FroggerStarter.Extentions;
using FroggerStarter.Model;
using FroggerStarter.Model.HighScoreModel;

namespace FroggerStarter.ViewModel
{
    public class HighScoreViewModel : INotifyPropertyChanged
    {
        #region Data members

        private readonly HighScores highScores;

        private ObservableCollection<HighScore> allScores;

        #endregion

        #region Properties

        public ObservableCollection<HighScore> AllScores
        {
            get => this.allScores;
            set
            {
                this.allScores = value;
                this.OnPropertyChanged();
            }
        }

        #endregion

        #region Constructors

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

        #endregion
    }
}