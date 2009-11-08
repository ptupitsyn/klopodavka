#region Usings

using System.Windows.Input;
using System.Windows.Media;

#endregion

namespace KlopViewWpf
{
   /// <summary>
   /// Interaction logic for KlopCell.xaml
   /// </summary>
   public partial class KlopCell // : IKlopCell
   {
      #region Fields and Constants

      private bool _clopLoaded;

      #endregion

      #region Constructors

      public KlopCell()
      {
         InitializeComponent();
         MouseDown += KlopCell_MouseDown;
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
      /// Handles the MouseDown event of the KlopCell control.
      /// </summary>
      /// <param name="sender">The source of the event.</param>
      /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
      private void KlopCell_MouseDown(object sender, MouseButtonEventArgs e)
      {
         if (!_clopLoaded)
         {
            contentBox.Child = new KlopImage();
            _clopLoaded = true;
         }
      }

      #endregion
   }
}