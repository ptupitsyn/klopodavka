using System.Windows;

namespace KlopViewWpf
{
   /// <summary>
   /// Interaction logic for App.xaml
   /// </summary>
   public partial class App
   {
      /// <summary>
      /// Handles the DispatcherUnhandledException event of the Application control.
      /// </summary>
      /// <param name="sender">The source of the event.</param>
      /// <param name="e">The <see cref="System.Windows.Threading.DispatcherUnhandledExceptionEventArgs"/> instance containing the event data.</param>
      private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
      {
         MessageBox.Show(e.Exception.ToString(), "Unhandled exception", MessageBoxButton.OK, MessageBoxImage.Error);
         e.Handled = true;
      }
   }
}
