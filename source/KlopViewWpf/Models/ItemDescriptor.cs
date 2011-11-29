namespace KlopViewWpf.Models
{
   /// <summary>
   /// XAML helper which represents UI information for some entity, such as Link or Button.
   /// </summary>
   public class ItemDescriptor
   {
      #region Public properties and indexers

      public string Text { get; set; }
      public string Icon { get; set; }
      public string Description { get; set; }

      #endregion
   }
}