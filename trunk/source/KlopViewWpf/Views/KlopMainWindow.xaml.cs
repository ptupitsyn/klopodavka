#region Usings

using KlopViewWpf.ViewModels;

#endregion

namespace KlopViewWpf.Views
{
   /// <summary>
   /// Interaction logic for KlopMainWindow.xaml
   /// </summary>
   public partial class KlopMainWindow
   {
      #region Fields and Constants

      private static readonly MainViewModel ViewModel = new MainViewModel();

      #endregion

      #region Constructors

      public KlopMainWindow()
      {
         DataContext = ViewModel;
         InitializeComponent();
      }

      #endregion
   }
}