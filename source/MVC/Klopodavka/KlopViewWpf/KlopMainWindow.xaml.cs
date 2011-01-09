#region Usings

#endregion

#region Usings

using System.Windows.Input;
using KlopIfaces;

#endregion

namespace KlopViewWpf
{
   /// <summary>
   /// Interaction logic for KlopMainWindow.xaml
   /// </summary>
   public partial class KlopMainWindow
   {
      #region Fields and Constants

      private readonly KlopGameViewModel _viewModel = new KlopGameViewModel();

      #endregion

      #region Constructors

      public KlopMainWindow()
      {
         InitializeComponent();
      }

      #endregion

      #region Event handlers

      /// <summary>
      /// Handles the MouseLeftButtonUp event of the KlopCell control.
      /// </summary>
      /// <param name="sender">The source of the event.</param>
      /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
      private void KlopCell_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
      {
         _viewModel.MakeTurnCommand.Execute(((KlopCell2) sender).Cell);
      }

      #endregion

      private void ImageLinkButton_Click(object sender, System.Windows.RoutedEventArgs e)
      {
         DataContext = _viewModel;
      }
   }
}