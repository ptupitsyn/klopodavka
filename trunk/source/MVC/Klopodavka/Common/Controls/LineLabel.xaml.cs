using System.Windows;
using System.Windows.Ink;

namespace Common.Controls
{
   /// <summary>
   /// Interaction logic for LineLabel.xaml
   /// </summary>
   public partial class LineLabel
   {
      #region Fields and Constants


      private static readonly DependencyProperty strokeProperty;


      #endregion


      #region Constructors


      /// <summary>
      /// Initializes the <see cref="LineLabel"/> class.
      /// </summary>
      static LineLabel()
      {
         strokeProperty = DependencyProperty.Register("Stroke", typeof (Stroke), typeof (LineLabel));
      }


      /// <summary>
      /// Initializes a new instance of the <see cref="LineLabel"/> class.
      /// </summary>
      public LineLabel()
      {
         InitializeComponent();
      }


      #endregion


      #region Public properties and indexers


      /// <summary>
      /// Gets or sets the stroke.
      /// </summary>
      /// <value>The stroke.</value>
      public Stroke Stroke
      {
         get { return (Stroke) GetValue(strokeProperty); }
         set { SetValue(strokeProperty, value); }
      }


      #endregion
   }
}