using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace KlopViewWpf.Views
{
   /// <summary>
   /// Interaction logic for MainMenu.xaml
   /// </summary>
   public partial class MainMenu
   {
      public MainMenu()
      {
         InitializeComponent();
      }

      /// <summary>
      /// Handles the Click event of the Button control.
      /// </summary>
      /// <param name="sender">The source of the event.</param>
      /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
      private void Button_Click(object sender, RoutedEventArgs e)
      {
         // First call to BeginGame can take some time - show cursor.
         Application.Current.MainWindow.Cursor = Cursors.Wait;
         Dispatcher.BeginInvoke((Action) (() => { Application.Current.MainWindow.Cursor = null; }), DispatcherPriority.SystemIdle);
      }
   }
}
