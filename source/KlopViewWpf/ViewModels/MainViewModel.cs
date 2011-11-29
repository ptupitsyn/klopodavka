#region Usings

using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

#endregion

namespace KlopViewWpf.ViewModels
{
   public class MainViewModel : ViewModelBase
   {
      #region Fields and Constants

      private RelayCommand _continueGameCommand;
      private RelayCommand _customGameCommand;
      private KlopGameViewModel _gameViewModel;

      private bool _isMenuVisible;
      private RelayCommand _quickGameAgainstHumanCommand;
      private RelayCommand _quickGameAgainstOneCommand;
      private RelayCommand _quickGameAgainstTwoCommand;
      private RelayCommand _restartGameCommand;
      private RelayCommand _showDemoCommand;
      private RelayCommand _showMenuCommand;

      #endregion

      #region Constructors

      public MainViewModel()
      {
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
            ContinueGameCommand.RaiseCanExecuteChanged();
            RestartGameCommand.RaiseCanExecuteChanged();
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

      public RelayCommand RestartGameCommand
      {
         get { return _restartGameCommand ?? (_restartGameCommand = new RelayCommand(RestartGame, CanRestartGame)); }
      }

      public RelayCommand ContinueGameCommand
      {
         get { return _continueGameCommand ?? (_continueGameCommand = new RelayCommand(ContinueGame, CanContinueGame)); }
      }

      public RelayCommand ShowMenuCommand
      {
         get { return _showMenuCommand ?? (_showMenuCommand = new RelayCommand(ShowMenu)); }
      }

      public RelayCommand QuickGameAgainstOneCommand
      {
         get { return _quickGameAgainstOneCommand ?? (_quickGameAgainstOneCommand = new RelayCommand(QuickGameAgainstOne)); }
      }

      public RelayCommand QuickGameAgainstTwoCommand
      {
         get { return _quickGameAgainstTwoCommand ?? (_quickGameAgainstTwoCommand = new RelayCommand(QuickGameAgainstTwo)); }
      }

      public RelayCommand QuickGameAgainstHumanCommand
      {
         get { return _quickGameAgainstHumanCommand ?? (_quickGameAgainstHumanCommand = new RelayCommand(QuickGameAgainstHuman)); }
      }

      public RelayCommand CustomGameCommand
      {
         get { return _customGameCommand ?? (_customGameCommand = new RelayCommand(CustomGame)); }
      }

      public RelayCommand ShowDemoCommand
      {
         get { return _showDemoCommand ?? (_showDemoCommand = new RelayCommand(ShowDemo)); }
      }

      #endregion

      #region Private and protected methods

      private void QuickGameAgainstHuman()
      {
         throw new NotImplementedException();
      }

      private bool CanRestartGame()
      {
         return CanContinueGame();
      }

      private void QuickGameAgainstOne()
      {
         GameViewModel = new KlopGameViewModel();
         IsMenuVisible = false;
      }

      private void QuickGameAgainstTwo()
      {
         throw new NotImplementedException();
         IsMenuVisible = false;
      }

      private void CustomGame()
      {
         throw new NotImplementedException();
      }

      private void ShowDemo()
      {
         throw new NotImplementedException();
         IsMenuVisible = false;
      }

      private bool CanContinueGame()
      {
         // TODO: Check if game finished.
         return GameViewModel != null;
      }

      private void ShowMenu()
      {
         IsMenuVisible = true;
      }

      private void ContinueGame()
      {
         IsMenuVisible = false;
      }

      private void RestartGame()
      {
         GameViewModel.ResetCommand.Execute();
         IsMenuVisible = false;
      }

      #endregion
   }
}