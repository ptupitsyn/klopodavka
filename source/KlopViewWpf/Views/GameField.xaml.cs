#region Usings

using System.Windows.Input;
using KlopViewWpf.ViewModels;

#endregion

namespace KlopViewWpf.Views
{
   /// <summary>
   /// Interaction logic for GameField.xaml
   /// </summary>
   public partial class GameField
   {
      #region Constructors

      public GameField()
      {
         InitializeComponent();
      }

      #endregion

      #region Private properties and indexers

      private KlopGameViewModel ViewModel
      {
         get { return (KlopGameViewModel) DataContext; }
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
         if (ViewModel.Model.CurrentPlayer.Human)
         {
            ViewModel.MakeTurnCommand.Execute(((KlopCell2) sender).Cell);
         }
      }

      /// <summary>
      /// Handles the MouseEnter event of the KlopCell control.
      /// </summary>
      /// <param name="sender">The source of the event.</param>
      /// <param name="e">The <see cref="System.Windows.Input.MouseEventArgs"/> instance containing the event data.</param>
      private void KlopCell_MouseEnter(object sender, MouseEventArgs e)
      {
         ViewModel.SetActiveCellCommand.Execute(((KlopCell2) sender).Cell);
      }

      #endregion
   }
}