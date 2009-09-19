namespace KlopIfaces
{
   /// <summary>
   /// Represents one cell in Klop Model
   /// </summary>
   public interface IKlopCell
   {
      /// <summary>
      /// Gets the horizontal cell position.
      /// </summary>
      /// <value>The horizontal cell position.</value>
      int X { get; }

      /// <summary>
      /// Gets the vertical cell position.
      /// </summary>
      /// <value>The vertical cell position.</value>
      int Y { get; }

      /// <summary>
      /// Gets the state.
      /// </summary>
      /// <value>The state.</value>
      ECellState State { get; }

      /// <summary>
      /// Gets the cell owner.
      /// </summary>
      /// <value>The owner.</value>
      IKlopPlayer Owner { get; }
   }
}