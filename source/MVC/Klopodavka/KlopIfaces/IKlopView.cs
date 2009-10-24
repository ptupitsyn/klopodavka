namespace KlopIfaces
{
   /// <summary>
   /// Defines View part of MVC pattern
   /// </summary>
   public interface IKlopView
   {
      /// <summary>
      /// Gets or sets a value indicating whether this <see cref="IKlopView"/> is locked.
      /// </summary>
      /// <value><c>true</c> if locked; otherwise, <c>false</c>.</value>
      bool Locked { get; set; }
   }
}