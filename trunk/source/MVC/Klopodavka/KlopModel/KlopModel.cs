#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using KlopIfaces;

#endregion

namespace KlopModel
{
   /// <summary>
   /// Implements Model
   /// </summary>
   public class KlopModel : ModelBase, IKlopModel
   {
      #region Fields and Constants

      private readonly KlopCell[,] _cells;
      private readonly Stack<KlopCell> _history;
      private readonly object _syncroot = new object();
      private int _currentPlayerIndex;
      private int _remainingKlops;
      private int _turnLength;

      #endregion

      #region Private and protected properties and indexers

      private int CurrentPlayerIndex
      {
         get { return _currentPlayerIndex; }
         set
         {
            _currentPlayerIndex = value;
            OnPropertyChanged("CurrentPlayer");
         }
      }

      #endregion

      #region Constructors

      /// <summary>
      /// Initializes a new instance of the <see cref="KlopModel"/> class.
      /// </summary>
      /// <param name="width">The width.</param>
      /// <param name="height">The height.</param>
      /// <param name="players">The players.</param>
      /// <param name="turnLenght">The turn lenght.</param>
      public KlopModel(int width, int height, IList<IKlopPlayer> players, int turnLenght)
      {
         FieldWidth = width;
         FieldHeight = height;
         Players = players;
         TurnLength = turnLenght; 

         if (width < 10 || height < 10)
         {
            throw new ArgumentException("Width and height must be greater than 9");
         }

         if (players == null || players.Count < 2)
         {
            throw new ArgumentException("Need two or more players");
         }

         if (players.Any(player => !CheckCoordinates(player.BasePosX, player.BasePosY)))
         {
            throw new ArgumentException("Player base is outside of field!");
         }

         // initialize field
         _cells = new KlopCell[width,height];
         _history = new Stack<KlopCell>();
         Reset();
      }

      #endregion

      #region Private/protected/internal methods

      private void SwitchTurn()
      {
         if (RemainingKlops == 0)
         {
            RemainingKlops = TurnLength;

            if (CurrentPlayerIndex == Players.Count - 1)
            {
               CurrentPlayerIndex = 0;
            }
            else
            {
               CurrentPlayerIndex++;
            }

            _history.Clear(); // cannot undo after turn switch

            // Reset availability
            foreach (KlopCell cell in _cells)
            {
               cell.Available = false;
            }
         }
      }

      /// <summary>
      /// Checks the coordinates.
      /// </summary>
      /// <param name="x">The x.</param>
      /// <param name="y">The y.</param>
      /// <returns></returns>
      private bool CheckCoordinates(int x, int y)
      {
         return x >= 0 && x < FieldWidth && y >= 0 && y < FieldHeight;
      }

      /// <summary>
      /// Mark cells where turn is possible as available
      /// </summary>
      private void FindAvailableCells()
      {
         foreach (KlopCell cell in _cells)
         {
            cell.Flag = false;
         }

         var avail = FindAvailableCells(_cells[CurrentPlayer.BasePosX, CurrentPlayer.BasePosY]).ToDictionary(cell => cell);

         // TODO: Bad thing that model knows of UI dispatcher..
         UiDispatcherInvoke(() =>
                               {
                                  foreach (KlopCell cell in _cells)
                                  {
                                     cell.Available = avail.ContainsKey(cell);
                                  }
                               });
      }

      /// <summary>
      /// Mark cells where turn is possible as available
      /// </summary>
      /// <param name="baseCell">The base cell.</param>
      private IEnumerable<IKlopCell> FindAvailableCells(KlopCell baseCell)
      {
         if (baseCell.Owner != CurrentPlayer || baseCell.Flag)
            yield break;

         baseCell.Flag = true;

         foreach (KlopCell cell in GetNeighborCells(baseCell))
         {
            if (cell.Flag) continue;

            if (cell.Owner == CurrentPlayer)
            {
               // Continue tree search
               foreach (var c in FindAvailableCells(cell))
               {
                  yield return c;
               }
               continue;
            }

            if (cell.State != ECellState.Free && cell.State != ECellState.Alive) continue;
            
            // Can go to free cell or eat enemy klop
            cell.Flag = true;
            yield return cell;
         }
      }

      /// <summary>
      /// Gets the neighbor cells.
      /// </summary>
      /// <param name="cell">The cell.</param>
      /// <returns></returns>
      public IEnumerable<IKlopCell> GetNeighborCells(IKlopCell cell)
      {
         var dx = new[] {-1, -1, -1, 1, 1, 1, 0, 0};
         var dy = new[] {-1, 0, 1, -1, 0, 1, -1, 1};

         for (int i = 0; i < dx.Length; i++)
         {
            var x = cell.X + dx[i];
            var y = cell.Y + dy[i];
            if (CheckCoordinates(x, y))
               yield return _cells[x, y];
         }
      }

      #endregion

      #region IKlopModel Members


      /// <summary>
      /// Gets or sets the width of the field.
      /// </summary>
      /// <value>The width of the field.</value>
      public int FieldWidth { get; private set; }

      /// <summary>
      /// Gets the height of the field.
      /// </summary>
      /// <value>The height of the field.</value>
      public int FieldHeight { get; private set; }

      /// <summary>
      /// Gets the players.
      /// </summary>
      /// <value>The players.</value>
      public IList<IKlopPlayer> Players { get; private set; }

      /// <summary>
      /// Gets the current player.
      /// </summary>
      /// <value>The current player.</value>
      public IKlopPlayer CurrentPlayer
      {
         get { return Players[CurrentPlayerIndex]; }
      }

      /// <summary>
      /// Gets the available klop count for each turn
      /// </summary>
      /// <value>The length of the turn.</value>
      public int TurnLength
      {
         get { return _turnLength; }
         set
         {
            _turnLength = value;
            OnPropertyChanged("TurnLength");
         }
      }

      /// <summary>
      /// Gets the remaining klops.
      /// </summary>
      /// <value>The remaining klops.</value>
      public int RemainingKlops
      {
         get { return _remainingKlops; }
         private set
         {
            _remainingKlops = value;
            OnPropertyChanged("RemainingKlops");
         }
      }

      /// <summary>
      /// Gets the <see cref="KlopIfaces.IKlopCell"/> with the specified position
      /// </summary>
      /// <value></value>
      public IKlopCell this[int x, int y]
      {
         get
         {
            return CheckCoordinates(x, y) ? _cells[x, y] : null;
         }
      }

      /// <summary>
      /// Gets the cells.
      /// </summary>
      /// <value>The cells.</value>
      public IEnumerable<IKlopCell> Cells
      {
         get
         {
            //return _cells.OfType<IKlopCell>();
            for (int y = 0; y < FieldHeight; y++)
            {
               for (int x = 0; x < FieldWidth; x++)
               {
                  yield return this[x, y];
               }
            }
         }
      }

      /// <summary>
      /// Resets the game to initial state
      /// </summary>
      public void Reset()
      {
         lock (_syncroot)
         {
            for (int x = 0; x < FieldWidth; x++)
            {
               for (int y = 0; y < FieldHeight; y++)
               {
                  if (_cells[x, y] == null)
                  {
                     _cells[x, y] = new KlopCell(x, y);
                  }
                  var cell = _cells[x, y];
                  cell.Available = false;
                  cell.Flag = false;
                  cell.Owner = null;
                  cell.State = ECellState.Free;
                  cell.Highlighted = false;
                  cell.Tag = null;
               }
            }

            foreach (IKlopPlayer player in Players)
            {
               var cell = _cells[player.BasePosX, player.BasePosY];
               cell.Owner = player;
               cell.State = ECellState.Base;
            }

            CurrentPlayerIndex = 0;
            RemainingKlops = TurnLength;
            FindAvailableCells();
         }
      }

      /// <summary>
      /// Makes the turn - put klop to specified position
      /// </summary>
      /// <param name="x">The x.</param>
      /// <param name="y">The y.</param>
      public void MakeTurn(int x, int y)
      {
         lock (_syncroot)
         {
            if (!CheckCoordinates(x, y))
               return;

            var cell = _cells[x, y];

            if (!cell.Available)
               return;

            _history.Push(cell.Clone());

            cell.Owner = CurrentPlayer;

            switch (cell.State)
            {
               case ECellState.Alive:
                  cell.State = ECellState.Dead;
                  break;
               case ECellState.Free:
                  cell.State = ECellState.Alive;
                  break;
            }

            RemainingKlops--;
            SwitchTurn();
            //TODO: GameOver condition
            FindAvailableCells();
         }
      }

      /// <summary>
      /// Makes the turn to the specified cell.
      /// </summary>
      /// <param name="cell">The cell.</param>
      public void MakeTurn(IKlopCell cell)
      {
         MakeTurn(cell.X, cell.Y);
      }

      /// <summary>
      /// Undoes the previous turn.
      /// </summary>
      public void UndoTurn()
      {
         lock (_syncroot)
         {
            if (_history.Count == 0)
               return;

            var oldCell = _history.Pop();
            var cell = _cells[oldCell.X, oldCell.Y];

            cell.State = oldCell.State;
            cell.Owner = oldCell.Owner;

            RemainingKlops++;
            FindAvailableCells();
         }
      }
      

      #endregion
   }
}