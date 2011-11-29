#region Usings

using System;
using System.Collections.Generic;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using KlopAi;
using KlopIfaces;
using KlopModel;
using KlopViewWpf.Preferences;

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
         var fieldSize = PreferencesManager.Instance.GamePreferences.GameFieldSize;
         var baseDist = PreferencesManager.Instance.GamePreferences.GameBaseDistance;
         var turnLength = PreferencesManager.Instance.GamePreferences.GameTurnLength;
         var players = new List<IKlopPlayer>
                          {
                             new KlopPlayer {BasePosX = baseDist, BasePosY = fieldSize - baseDist - 1, Color = Colors.Blue, Human = true, Name = "Player 1"},
                             new KlopPlayer {BasePosX = fieldSize - baseDist - 1, BasePosY = baseDist, Color = Colors.Red, Human = true, Name = "Player 2"},
                          };

         GameViewModel = new KlopGameViewModel(fieldSize, fieldSize, players, turnLength);
         IsMenuVisible = false;
      }

      private bool CanRestartGame()
      {
         return CanContinueGame();
      }

      private void QuickGameAgainstOne()
      {
         var fieldSize = PreferencesManager.Instance.GamePreferences.GameFieldSize;
         var baseDist = PreferencesManager.Instance.GamePreferences.GameBaseDistance;
         var turnLength = PreferencesManager.Instance.GamePreferences.GameTurnLength;
         var players = new List<IKlopPlayer>
                                {
                                   new KlopPlayer   {BasePosX = baseDist, BasePosY = fieldSize - baseDist - 1, Color = Colors.Blue, Human = true, Name = "You"},
                                   new KlopAiPlayer {BasePosX = fieldSize - baseDist - 1, BasePosY = baseDist, Color = Colors.Red, Name = "Луноход 1"},
                                };

         GameViewModel = new KlopGameViewModel(fieldSize, fieldSize, players, turnLength);
         IsMenuVisible = false;
      }

      private void QuickGameAgainstTwo()
      {
         var fieldSize = PreferencesManager.Instance.GamePreferences.GameFieldSize;
         var baseDist = PreferencesManager.Instance.GamePreferences.GameBaseDistance;
         var turnLength = PreferencesManager.Instance.GamePreferences.GameTurnLength;
         var players = new List<IKlopPlayer>
                                {
                                   new KlopPlayer   {BasePosX = baseDist, BasePosY = fieldSize / 2 - 1, Color = Colors.Blue, Human = true, Name = "You"},
                                   new KlopAiPlayer {BasePosX = fieldSize - baseDist - 1, BasePosY = baseDist, Color = Colors.Red, Name = "Луноход 1"},
                                   new KlopAiPlayer {BasePosX = fieldSize - baseDist - 1, BasePosY = fieldSize - baseDist - 1, Color = Colors.Green, Name = "Луноход 2"},
                                };

         GameViewModel = new KlopGameViewModel(fieldSize, fieldSize, players, turnLength);
         IsMenuVisible = false;
      }

      private void CustomGame()
      {
         throw new NotImplementedException();
      }

      private void ShowDemo()
      {
         var fieldSize = PreferencesManager.Instance.GamePreferences.GameFieldSize;
         var baseDist = PreferencesManager.Instance.GamePreferences.GameBaseDistance;
         var turnLength = PreferencesManager.Instance.GamePreferences.GameTurnLength;
         var players = new List<IKlopPlayer>
                                {
                                   new KlopAiPlayer {BasePosX = baseDist, BasePosY = fieldSize - baseDist - 1, Color = Colors.Red, Name = "Луноход 1", TurnDelay = TimeSpan.FromSeconds(0.3)},
                                   //new KlopAiPlayer {BasePosX = baseDist, BasePosY = baseDist, Color = Colors.Green, Name = "Луноход 2"},
                                   //new KlopAiPlayer {BasePosX = fieldSize - baseDist - 1, BasePosY = fieldSize - baseDist - 1, Color = Colors.Yellow, Name = "Луноход 3"},
                                   new KlopAiPlayer {BasePosX = fieldSize - baseDist - 1, BasePosY = baseDist, Color = Colors.Blue, Name = "Луноход 4", TurnDelay = TimeSpan.FromSeconds(0.3)}
                                };

         GameViewModel = new KlopGameViewModel(fieldSize, fieldSize, players, turnLength);
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