#region Usings

using System;
using System.Collections.Generic;
using KlopIfaces;

#endregion

namespace KlopModel
{
   /// <summary>
   /// Implements Model
   /// </summary>
   public class KlopModel : IKlopModel
   {
      #region Fields and constants

      private readonly KlopCell[,] _cells;
      private Stack<KlopCell> _history;
      private int _currentPlayerIndex;

      #endregion

      #region Constructors

      /// <summary>
      /// Initializes a new instance of the <see cref="KlopModel"/> class.
      /// </summary>
      /// <param name="width">The width.</param>
      /// <param name="height">The height.</param>
      /// <param name="players">The players.</param>
      public KlopModel(int width, int height, IList<IKlopPlayer> players)
      {
         if (width < 10 || height < 10)
         {
            throw new ArgumentException("Width and height must be greater than 9");
         }

         if (players == null || players.Count < 2)
         {
            throw new ArgumentException("Need two or more players");
         }

         foreach (IKlopPlayer player in Players)
         {
            if (!CheckCoord(player.BasePosX, player.BasePosY))
            {
               throw new ArgumentException("Player base is outside of field!");
            }
         }


         FieldWidth = width;
         FieldHeight = height;
         Players = players;
         TurnLength = 10; // default

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
            _currentPlayerIndex++;

            if (_currentPlayerIndex >= Players.Count)
            {
               _currentPlayerIndex = 0;
            }

            CurrentPlayerChanged(this, new EventArgs());

            _history.Clear(); // cannot undo after turn switch
         }
      }

      /// <summary>
      /// Checks the coordinates.
      /// </summary>
      /// <param name="x">The x.</param>
      /// <param name="y">The y.</param>
      /// <returns></returns>
      private bool CheckCoord(int x, int y)
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
            cell.Available = false;
            cell.Flag = false;
         }

         FindAvailableCells(_cells[CurrentPlayer.BasePosX, CurrentPlayer.BasePosY]);
      }

      /// <summary>
      /// Mark cells where turn is possible as available
      /// </summary>
      /// <param name="baseCell">The base cell.</param>
      private void FindAvailableCells(KlopCell baseCell)
      {
         if (baseCell.Owner != CurrentPlayer)
            return;

         baseCell.Flag = true;

         foreach (KlopCell cell in GetNeighborCells(baseCell))
         {
            if (cell.State == ECellState.Free || (cell.State == ECellState.Alive && cell.Owner != CurrentPlayer))
            {
               // Can go to free cell or eat enemy klop
               cell.Available = true;
            }

            if ((cell.State == ECellState.Alive && cell.Owner == CurrentPlayer))
            {
               // Continue tree search
               FindAvailableCells(cell);
            }
         }
      }

      /// <summary>
      /// Gets the neighbor cells.
      /// </summary>
      /// <param name="cell">The cell.</param>
      /// <returns></returns>
      private IEnumerable<KlopCell> GetNeighborCells(IKlopCell cell)
      {
         var dx = new [] { -1, -1, -1, 1, 1, 1, 0, 0 };
         var dy = new [] { -1, 0, 1, -1, 0, 1, -1, 1 };

         for (int i = 0; i < dx.Length; i++)
         {
            var x = cell.X + dx[i];
            var y = cell.Y + dy[i];
            if (CheckCoord(x, y))
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
         get
         {
            return Players[_currentPlayerIndex];
         }
      }

      /// <summary>
      /// Gets the available klop count for each turn
      /// </summary>
      /// <value>The length of the turn.</value>
      public int TurnLength { get; set; }

      /// <summary>
      /// Gets the remaining klops.
      /// </summary>
      /// <value>The remaining klops.</value>
      public int RemainingKlops { get; private set; }

      /// <summary>
      /// Gets the <see cref="KlopIfaces.IKlopCell"/> with the specified position
      /// </summary>
      /// <value></value>
      public IKlopCell this[int x, int y]
      {
         get
         {
            if (!CheckCoord(x, y))
            {
               throw new ArgumentException("Coordinates must be within field");
            }

            return _cells[x, y];
         }
      }

      /// <summary>
      /// Resets the game to initial state
      /// </summary>
      public void Reset()
      {
         for (int x = 0; x < FieldWidth; x++)
         {
            for (int y = 0; y < FieldHeight; y++)
            {
               _cells[x, y] = new KlopCell(x, y);
               CellsChanged(_cells[x, y], new EventArgs()); //TODO: think
            }
         }

         foreach(IKlopPlayer player in Players)
         {
            var cell = _cells[player.BasePosX, player.BasePosY];
            cell.Owner = player;
            cell.State = ECellState.Base;
            CellsChanged(cell, new EventArgs()); //TODO: think
         }

         _currentPlayerIndex = 0;
         RemainingKlops = TurnLength;
         FindAvailableCells();
         CurrentPlayerChanged(this, new EventArgs());
      }

      /// <summary>
      /// Makes the turn - put klop to specified position
      /// </summary>
      /// <param name="x">The x.</param>
      /// <param name="y">The y.</param>
      public void MakeTurn(int x, int y)
      {
         if (!CheckCoord(x, y))
            return;

         var cell = _cells[x, y];

         if (!cell.Available)
            return;

         _history.Push(cell.Clone());

         cell.Owner = CurrentPlayer;

         switch(cell.State)
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
         FindAvailableCells();
      }

      /// <summary>
      /// Undoes the previous turn.
      /// </summary>
      public void UndoTurn()
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

      /// <summary>
      /// Occurs when [current player changed].
      /// </summary>
      public event EventHandler CurrentPlayerChanged;

      /// <summary>
      /// Occurs when [cells changed].
      /// </summary>
      public event EventHandler CellsChanged;

      #endregion


   }
}