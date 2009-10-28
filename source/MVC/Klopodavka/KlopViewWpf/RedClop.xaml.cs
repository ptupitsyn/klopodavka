#region Usings

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
   }
}