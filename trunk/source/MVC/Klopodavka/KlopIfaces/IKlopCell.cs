using System.ComponentModel;

namespace KlopIfaces
{
   /// <summary>
   /// Represents one cell in Klop Model
   /// </summary>
   public interface IKlopCell : INotifyPropertyChanged
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

      /// <summary>
      /// Gets a value indicating whether this <see cref="IKlopCell"/> is available for turn.
      /// </summary>
      /// <value><c>true</c> if available; otherwise, <c>false</c>.</value>
      bool Available { get; }

      /// <summary>
      /// Gets a value indicating whether this <see cref="IKlopCell"/> is highlighted. This can be used for turn suggestions.
      /// </summary>
      /// <value><c>true</c> if highlighted; otherwise, <c>false</c>.</value>
      bool Highlighted { get; set; }

      /// <summary>
      /// Gets or sets the tag (any value to display if needed).
      /// </summary>
      /// <value>The hint.</value>
      object Tag { get; set; }
   }
}