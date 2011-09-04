
using System.Windows.Controls.Primitives;

namespace Common.Commands
{
   /// <summary>
   /// Helper command to close pop-ups from XAML by buttons, etc.
   /// </summary>
   public class ClosePopupCommand : DelegateCommand<Popup>
   {
      #region Fields and Constants


      private static ClosePopupCommand instance;


      #endregion


      #region Constructors


      /// <summary>
      /// Initializes a new instance of the <see cref="ClosePopupCommand"/> class.
      /// </summary>
      private ClosePopupCommand() : base(ClosePopup)
      {
      }


      #endregion


      #region Public properties and indexers


      /// <summary>
      /// Gets the instance.
      /// </summary>
      /// <value>The instance.</value>
      public static ClosePopupCommand Instance
      {
         get
         {
            if (instance == null)
            {
               instance = new ClosePopupCommand();
            }
            return instance;
         }
      }


      #endregion


      #region Private and protected methods


      /// <summary>
      /// Closes the popup.
      /// </summary>
      /// <param name="popup">The popup.</param>
      private static void ClosePopup(Popup popup)
      {
         popup.IsOpen = false;
      }


      #endregion
   }
}