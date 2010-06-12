#region Usings



#endregion

namespace KlopViewWpf
{
   /// <summary>
   /// Interaction logic for KlopMainWindow.xaml
   /// </summary>
   public partial class KlopMainWindow
   {
      #region Constructors

      public KlopMainWindow()
      {
         DataContext = new KlopGameViewModel();
         InitializeComponent();
      }

      #endregion
   }
}