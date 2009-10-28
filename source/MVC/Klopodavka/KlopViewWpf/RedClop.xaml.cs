#region Usings

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;


#endregion

namespace KlopViewWpf
{
   /// <summary>
   /// Interaction logic for RedClop.xaml
   /// </summary>
   public partial class RedClop : UserControl
   {
      DropShadowEffect effect = new DropShadowEffect();

      #region Constructors

      public RedClop()
      {
         InitializeComponent();
         MouseEnter += new System.Windows.Input.MouseEventHandler(RedClop_MouseEnter);
         MouseLeave += new System.Windows.Input.MouseEventHandler(RedClop_MouseLeave);
      }

      void RedClop_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
      {
         //Background = Brushes.White;
         //ClopCanvas.Effect = null;
      }

      void RedClop_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
      {
         //Background = Brushes.Red;
         //ClopCanvas.Effect = effect;
      }

      #endregion

      protected override GeometryHitTestResult HitTestCore(GeometryHitTestParameters hitTestParameters)
      {
         //Point pt = hitTestParameters.HitGeometry.;

         // Perform custom actions during the hit test processing,
         // which may include verifying that the point actually
         // falls within the rendered content of the visual.

         // Return hit on bounding rectangle of visual object.
         return new GeometryHitTestResult(this, IntersectionDetail.FullyContains);
      }
   }
}