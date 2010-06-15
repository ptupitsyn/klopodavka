#region Usings

using System.Windows.Input;
using System.Windows.Media;
using KlopIfaces;

#endregion

namespace KlopViewWpf
{
   /// <summary>
   /// Interaction logic for KlopCell.xaml
   /// </summary>
   public partial class KlopCell
   {
      #region Constructors

      public KlopCell()
      {
         InitializeComponent();
      }

      #endregion

      #region Private and protected methods

      protected override GeometryHitTestResult HitTestCore(GeometryHitTestParameters hitTestParameters)
      {
         return new GeometryHitTestResult(this, IntersectionDetail.FullyContains);
      }

      #endregion

      #region Event handlers

      /// <summary>
      /// Handles the MouseEnter event of the Button control.
      /// </summary>
      /// <param name="sender">The source of the event.</param>
      /// <param name="e">The <see cref="System.Windows.Input.MouseEventArgs"/> instance containing the event data.</param>
      private void Button_MouseEnter(object sender, MouseEventArgs e)
      {
         var dc = DataContext as IKlopCell;
         if (dc != null) dc.Highlighted = true;
      }

      /// <summary>
      /// Handles the MouseLeave event of the Button control.
      /// </summary>
      /// <param name="sender">The source of the event.</param>
      /// <param name="e">The <see cref="System.Windows.Input.MouseEventArgs"/> instance containing the event data.</param>
      private void Button_MouseLeave(object sender, MouseEventArgs e)
      {
         var dc = DataContext as IKlopCell;
         if (dc != null) dc.Highlighted = false;
      }

      #endregion
   }
}