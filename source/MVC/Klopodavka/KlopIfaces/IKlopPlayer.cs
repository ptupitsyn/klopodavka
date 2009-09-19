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
   }
}