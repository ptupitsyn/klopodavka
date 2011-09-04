#region Usings

using GalaSoft.MvvmLight;

#endregion

namespace KlopViewWpf.ViewModels
{
   public class MainViewModel : ViewModelBase
   {
      #region Fields and Constants

      private KlopGameViewModel _gameViewModel;

      private bool _isMenuVisible;

      #endregion

      #region Constructors

      public MainViewModel()
      {
         GameViewModel = new KlopGameViewModel();
         IsMenuVisible = true;
      }

      #endregion

      #region Public properties and indexers

      public KlopGameViewModel GameViewModel
      {
         get { return _gameViewModel; }
         private set
         {
            _gameViewModel = value;
            RaisePropertyChanged("GameViewModel");
         }
      }

      public bool IsMenuVisible
      {
         get { return _isMenuVisible; }
         private set
         {
            _isMenuVisible = value;
            RaisePropertyChanged("IsMenuVisible");
         }
      }

      #endregion
   }
}