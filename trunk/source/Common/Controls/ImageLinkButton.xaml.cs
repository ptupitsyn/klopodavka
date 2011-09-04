using System.Windows;
using System.Windows.Media;

namespace Common.Controls
{
   /// <summary>
   /// Interaction logic for ImageLinkButton.xaml
   /// </summary>
   public partial class ImageLinkButton
   {
      #region Fields and Constants


      private static readonly DependencyProperty activeForegroundProperty;
      private static readonly DependencyProperty imageSourceProperty;
      private static readonly DependencyProperty imageSpaceProperty;


      #endregion


      #region Constructors


      /// <summary>
      /// Initializes the <see cref="ImageLinkButton"/> class.
      /// </summary>
      static ImageLinkButton()
      {
         imageSourceProperty = DependencyProperty.Register("Image", typeof (ImageSource), typeof (ImageLinkButton));
         activeForegroundProperty = DependencyProperty.Register("ActiveForeground", typeof (Brush), typeof (ImageLinkButton));
         activeForegroundProperty = DependencyProperty.Register("ImageSpace", typeof (double), typeof (ImageLinkButton));
      }


      /// <summary>
      /// Initializes a new instance of the <see cref="ImageLinkButton"/> class.
      /// </summary>
      public ImageLinkButton()
      {
         InitializeComponent();
      }


      #endregion


      #region Public properties and indexers


      /// <summary>
      /// Gets or sets the image.
      /// </summary>
      /// <value>The image.</value>
      public ImageSource Image
      {
         get { return (ImageSource) GetValue(imageSourceProperty); }
         set { SetValue(imageSourceProperty, value); }
      }


      /// <summary>
      /// Gets or sets the color of text on mouse-over.
      /// </summary>
      /// <value>The color of the active.</value>
      public Brush ActiveForeground
      {
         get { return (Brush) GetValue(activeForegroundProperty); }
         set { SetValue(activeForegroundProperty, value); }
      }


      /// <summary>
      /// Gets or sets the image space.
      /// </summary>
      /// <value>The image space.</value>
      public double ImageSpace
      {
         get { return (double) GetValue(imageSpaceProperty); }
         set { SetValue(imageSpaceProperty, value); }
      }


      #endregion
   }
}