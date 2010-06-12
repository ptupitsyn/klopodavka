#region Usings

using System.ComponentModel;
using KlopIfaces;

#endregion

namespace KlopModel
{
   /// <summary>
   /// Implements field cell
   /// </summary>
   public class KlopCell : IKlopCell
   {
      #region Constructors

      /// <summary>
      /// Initializes a new instance of the <see cref="KlopCell"/> class.
      /// </summary>
      /// <param name="x">The x.</param>
      /// <param name="y">The y.</param>
      public KlopCell(int x, int y)
      {
         X = x;
         Y = y;
      }

      #endregion

      #region Properties and indexers

      /// <summary>
      /// Just a flag for algorithmic purposes
      /// </summary>
      /// <value><c>true</c> if flag; otherwise, <c>false</c>.</value>
      public bool Flag { get; set; }

      #endregion

      #region IKlopCell implementation

      public event PropertyChangedEventHandler PropertyChanged;

      #endregion

      #region Public methods

      /// <summary>
      /// Clones this instance.
      /// </summary>
      /// <returns></returns>
      public KlopCell Clone()
      {
         return MemberwiseClone() as KlopCell;
      }

      #endregion

      #region IKlopCell Members

      #region Fields and Constants

      private bool _available;
      private IKlopPlayer _owner;
      private ECellState _state;

      #endregion

      /// <summary>
      /// Gets the horizontal cell position.
      /// </summary>
      /// <value>The horizontal cell position.</value>
      public int X { get; private set; }

      /// <summary>
      /// Gets the vertical cell position.
      /// </summary>
      /// <value>The vertical cell position.</value>
      public int Y { get; private set; }

      /// <summary>
      /// Gets the state.
      /// </summary>
      /// <value>The state.</value>
      public ECellState State
      {
         get { return _state; }
         set
         {
            _state = value;
            OnPropertyChanged("State");
         }
      }

      /// <summary>
      /// Gets the cell owner.
      /// </summary>
      /// <value>The owner.</value>
      public IKlopPlayer Owner
      {
         get { return _owner; }
         set
         {
            _owner = value;
            OnPropertyChanged("Owner");
         }
      }

      /// <summary>
      /// Gets a value indicating whether this <see cref="IKlopCell"/> is available for turn.
      /// </summary>
      /// <value><c>true</c> if available; otherwise, <c>false</c>.</value>
      public bool Available
      {
         get { return _available; }
         set
         {
            _available = value;
            OnPropertyChanged("Available");
         }
      }

      #endregion

      #region Private and protected methods

      protected void OnPropertyChanged(string propertyName)
      {
         if (PropertyChanged != null)
         {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
         }
      }

      #endregion
   }
}