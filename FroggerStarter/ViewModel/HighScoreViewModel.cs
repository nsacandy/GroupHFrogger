using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FroggerStarter.Extentions;
using FroggerStarter.Model;
using FroggerStarter.Model.HighScoreModel;

namespace FroggerStarter.ViewModel
{
    /// <summary>Go between for highScore and scoreboard</summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public class HighScoreViewModel : INotifyPropertyChanged
    {
        #region Data members

        private readonly HighScores highScores;

        private ObservableCollection<HighScore> allScores;

        #endregion

        #region Properties

        /// <summary>Gets or sets all scores.</summary>
        /// <value>All scores.</value>
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

        /// <summary>Initializes a new instance of the <see cref="HighScoreViewModel"/> class.</summary>
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

        /// <summary>Occurs when a property value changes.</summary>
        /// <returns></returns>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>Called when [property changed].</summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}