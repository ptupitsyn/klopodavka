#region Usings

using System.Windows;
using System.Windows.Media;

#endregion

namespace KlopViewWpf
{
   /// <summary>
   /// Interaction logic for KlopImage.xaml
   /// </summary>
   public partial class KlopImage
   {
      #region Fields and Constants

      /// <summary>
      /// Dependency property for KlopColor
      /// </summary>
      public static DependencyProperty KlopColorProperty = DependencyProperty.Register("KlopColor", typeof (Color), typeof (KlopImage));

      #endregion

      #region Constructors

      /// <summary>
      /// Initializes a new instance of the <see cref="KlopImage"/> class.
      /// </summary>
      public KlopImage()
      {
         InitializeComponent();
         KlopColor = Colors.Blue;
      }

      #endregion

      #region Public properties and indexers

      /// <summary>
      /// Gets or sets the color of the klop.
      /// </summary>
      /// <value>The color of the klop.</value>
      public Color KlopColor
      {
         get { return (Color)GetValue(KlopColorProperty); }
         set { SetValue(KlopColorProperty, value); }
      }

      #endregion

      #region Private and protected methods

      /// <summary>
      /// Implements <see cref="M:System.Windows.Media.Visual.HitTestCore(System.Windows.Media.GeometryHitTestParameters)"/> to supply base element hit testing behavior (returning <see cref="T:System.Windows.Media.GeometryHitTestResult"/>).
      /// </summary>
      /// <param name="hitTestParameters">Describes the hit test to perform, including the initial hit point.</param>
      /// <returns>
      /// Results of the test, including the evaluated geometry.
      /// </returns>
      protected override GeometryHitTestResult HitTestCore(GeometryHitTestParameters hitTestParameters)
      {
         return new GeometryHitTestResult(this, IntersectionDetail.FullyContains);
      }

      #endregion
   }
}