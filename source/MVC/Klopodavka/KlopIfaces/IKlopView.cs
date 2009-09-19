namespace KlopIfaces
{
   /// <summary>
   /// Defines View part of MVC pattern
   /// </summary>
   public interface IKlopView
   {
      /// <summary>
      /// Gets or sets a value indicating whether this <see cref="IKlopView"/> is enabled.
      /// </summary>
      /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
      bool Enabled { get; set; }
   }
}