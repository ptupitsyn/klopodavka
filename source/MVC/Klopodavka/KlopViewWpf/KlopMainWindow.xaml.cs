#region Usings

using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using KlopIfaces;

#endregion

namespace KlopViewWpf
{
   /// <summary>
   /// Interaction logic for KlopMainWindow.xaml
   /// </summary>
   public partial class KlopMainWindow : Window, IKlopView
   {
      #region Fields and Constants

      private readonly List<UIElement> _clopCells = new List<UIElement>();
      private AutoResetEvent _cellsCreated = new AutoResetEvent(false);

      #endregion

      #region Constructors

      public KlopMainWindow()
      {
         InitializeComponent();
         PopulateGrid();
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

      #region Private and protected methods

      /// <summary>
      /// Populates the grid.
      /// </summary>
      private void PopulateGrid()
      {
         for (int i = 0; i < MainGrid.Rows * MainGrid.Columns; i++)
         {
            AddClop();
         }
      }

      private void AddClop()
      {
         MainGrid.Children.Add(new KlopCell());
      }


      #endregion

   }
}