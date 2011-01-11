#region Usings

using System.Windows.Media;

#endregion


namespace KlopIfaces
{
   /// <summary>
   /// Describes game player (human or CPU)
   /// </summary>
   public interface IKlopPlayer
   {
      #region Public properties and indexers

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

      #endregion


      #region Public methods

      /// <summary>
      /// Sets the IKlopModel.
      /// </summary>
      /// <param name="klopModel"></param>
      void SetModel(IKlopModel klopModel);

      #endregion
   }
}