#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;

#endregion

namespace KlopIfaces
{
   /// <summary>
   /// Defines Model part of MVC pattern
   /// </summary>
   public interface IKlopModel : INotifyPropertyChanged
   {
      /// <summary>
      /// Gets or sets the width of the field.
      /// </summary>
      /// <value>The width of the field.</value>
      int FieldWidth { get; }

      /// <summary>
      /// Gets the height of the field.
      /// </summary>
      /// <value>The height of the field.</value>
      int FieldHeight { get; }

      /// <summary>
      /// Gets the players.
      /// </summary>
      /// <value>The players.</value>
      IList<IKlopPlayer> Players { get; }

      /// <summary>
      /// Gets the current player.
      /// </summary>
      /// <value>The current player.</value>
      IKlopPlayer CurrentPlayer { get; }

      /// <summary>
      /// Gets the available klop count for each turn
      /// </summary>
      /// <value>The length of the turn.</value>
      int TurnLength { get; }

      /// <summary>
      /// Gets the remaining klops.
      /// </summary>
      /// <value>The remaining klops.</value>
      int RemainingKlops { get; }

      /// <summary>
      /// Gets the <see cref="KlopIfaces.IKlopCell"/> with the specified position
      /// </summary>
      /// <value></value>
      IKlopCell this[int x, int y] { get; }

      /// <summary>
      /// Gets the cells.
      /// </summary>
      /// <value>The cells.</value>
      IEnumerable<IKlopCell> Cells { get; }

      /// <summary>
      /// Resets the game to initial state
      /// </summary>
      void Reset();

      /// <summary>
      /// Makes the turn - put klop to specified position
      /// </summary>
      /// <param name="x">The x.</param>
      /// <param name="y">The y.</param>
      void MakeTurn(int x, int y);

      /// <summary>
      /// Makes the turn to the specified cell.
      /// </summary>
      /// <param name="cell">The cell.</param>
      void MakeTurn(IKlopCell cell);

      /// <summary>
      /// Undoes the previous turn.
      /// </summary>
      void UndoTurn();

      /// <summary>
      /// Gets the neighbor cells.
      /// </summary>
      IEnumerable<IKlopCell> GetNeighborCells(IKlopCell cell);
   }
}