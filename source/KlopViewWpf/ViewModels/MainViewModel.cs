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
      private KlopGameViewModel _gameViewModel;

      private bool _isMenuVisible;
      private RelayCommand _newGameCommand;
      private RelayCommand _showMenuCommand;

      #endregion

      #region Constructors

      public MainViewModel()
      {
         //GameViewModel = new KlopGameViewModel();
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

      public RelayCommand NewGameCommand
      {
         get { return _newGameCommand ?? (_newGameCommand = new RelayCommand(NewGame)); }
      }

      public RelayCommand ContinueGameCommand
      {
         get { return _continueGameCommand ?? (_continueGameCommand = new RelayCommand(ContinueGame, CanContinueGame)); }
      }

      private bool CanContinueGame()
      {
         // TODO: Check if game finished.
         return GameViewModel != null;
      }

      public RelayCommand ShowMenuCommand
      {
         get { return _showMenuCommand ?? (_showMenuCommand = new RelayCommand(ShowMenu)); }
      }

      #endregion

      #region Private and protected methods

      private void ShowMenu()
      {
         IsMenuVisible = true;
      }

      private void ContinueGame()
      {
         IsMenuVisible = false;
      }

      private void NewGame()
      {
         if (GameViewModel == null)
         {
            GameViewModel = new KlopGameViewModel();
         }
         else
         {
            GameViewModel.ResetCommand.Execute();
         }
         IsMenuVisible = false;
      }

      #endregion
   }
}