using System.Windows;
using KlopIfaces;

namespace KlopViewWpf.Views
{
   /// <summary>
   /// Interaction logic for PlayerInfo.xaml
   /// </summary>
   public partial class PlayerInfo
   {
      #region Fields and Constants

      public static readonly DependencyProperty ModelProperty =
         DependencyProperty.Register("Model", typeof (IKlopModel), typeof (PlayerInfo));


      public static readonly DependencyProperty PlayerProperty =
         DependencyProperty.Register("Player", typeof (IKlopPlayer), typeof (PlayerInfo));

      #endregion

      #region Constructors

      public PlayerInfo()
      {
         InitializeComponent();
      }

      #endregion

      #region Public properties and indexers

      public IKlopModel Model
      {
         get { return (IKlopModel) GetValue(ModelProperty); }
         set { SetValue(ModelProperty, value); }
      }

      public IKlopPlayer Player
      {
         get { return (IKlopPlayer) GetValue(PlayerProperty); }
         set { SetValue(PlayerProperty, value); }
      }

      #endregion
   }
}