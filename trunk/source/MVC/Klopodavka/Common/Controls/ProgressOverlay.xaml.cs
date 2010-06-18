using System.Windows;

namespace Common.Controls
{
   /// <summary>
   /// Interaction logic for ProgressOverlay.xaml
   /// </summary>
   public partial class ProgressOverlay
   {
      #region Fields and Constants

      private static readonly DependencyProperty imageSizeProperty;

      #endregion

      #region Constructors

      static ProgressOverlay()
      {
         imageSizeProperty = DependencyProperty.Register("ImageSize", typeof (double), typeof (ProgressOverlay));
      }


      /// <summary>
      /// Initializes a new instance of the <see cref="ProgressOverlay"/> class.
      /// </summary>
      public ProgressOverlay()
      {
         InitializeComponent();
         ImageSize = 30; // default value
      }

      #endregion

      #region Public properties and indexers

      /// <summary>
      /// Gets or sets the size of the image.
      /// </summary>
      /// <value>The size of the image.</value>
      public double ImageSize
      {
         get { return (double) GetValue(imageSizeProperty); }
         set { SetValue(imageSizeProperty, value); }
      }

      #endregion

      #region Event handlers

      /// <summary>
      /// Handles the SizeChanged event of the thisControl control.
      /// </summary>
      /// <param name="sender">The source of the event.</param>
      /// <param name="e">The <see cref="System.Windows.SizeChangedEventArgs"/> instance containing the event data.</param>
      private void ImageSizeChanged(object sender, SizeChangedEventArgs e)
      {
         refreshAnimation.CenterX = e.NewSize.Width/2;
         refreshAnimation.CenterY = e.NewSize.Height/2;
      }

      #endregion
   }
}