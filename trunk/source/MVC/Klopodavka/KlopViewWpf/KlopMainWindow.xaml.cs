#region Usings

using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using KlopIfaces;

#endregion

namespace KlopViewWpf
{
   /// <summary>
   /// Interaction logic for KlopMainWindow.xaml
   /// </summary>
   public partial class KlopMainWindow : Window, IKlopView
   {
      #region Constructors

      public KlopMainWindow()
      {
         InitializeComponent();
      }

      #endregion

      #region IKlopView Members

      /// <summary>
      /// Gets or sets a value indicating whether this <see cref="IKlopView"/> is locked.
      /// </summary>
      /// <value><c>true</c> if locked; otherwise, <c>false</c>.</value>
      public bool Locked
      {
         get { return false; }
         set { }
      }

      #endregion

      #region Public methods

      public Canvas LoadSpriteCanvas(string xamlPath)
      {
         Stream s = GetType().Assembly.GetManifestResourceStream(xamlPath);
         //var r = new System.IO.StreamReader(s).ReadToEnd();
         return (Canvas) XamlReader.Load(s);
      }

      #endregion

      /// <summary>
      /// Handles the Loaded event of the Window control.
      /// </summary>
      /// <param name="sender">The source of the event.</param>
      /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
      private void Window_Loaded(object sender, RoutedEventArgs e)
      {
         for (int i = 0; i < 0; i++)
         {
            var c = new RedClop();
            c.Width = 100;
            c.Height = 100;
            MainGrid.Children.Add(c);
         }

      }
   }
}