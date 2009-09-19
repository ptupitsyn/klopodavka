using System.Windows.Media;

namespace KlopIfaces
{
   /// <summary>
   /// Describes game player (human or CPU)
   /// </summary>
   public interface IKlopPlayer
   {
      /// <summary>
      /// Gets the name.
      /// </summary>
      /// <value>The name.</value>
      string Name { get; }

      /// <summary>
      /// Gets the base position X.
      /// </summary>
      /// <value>The base position X.</value>
      int BasePosX { get; }

      /// <summary>
      /// Gets the base position Y.
      /// </summary>
      /// <value>The base position Y.</value>
      int BasePosY { get; }

      /// <summary>
      /// Gets a value indicating whether this <see cref="IKlopPlayer"/> is human player or CPU.
      /// </summary>
      /// <value><c>true</c> if human; otherwise, <c>false</c>.</value>
      bool Human { get; }

      /// <summary>
      /// Gets the color.
      /// </summary>
      /// <value>The color.</value>
      Color Color { get; }
   }
}