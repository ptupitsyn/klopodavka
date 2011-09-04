using System.Windows.Controls.Primitives;

namespace Common.Commands
{
   /// <summary>
   /// Helper command to open pop-ups from XAML by buttons, etc.
   /// </summary>
   public class OpenPopupCommand : DelegateCommand<Popup>
   {
      #region Fields and Constants


      private static OpenPopupCommand instance;


      #endregion


      #region Constructors


      /// <summary>
      /// Initializes a new instance of the <see cref="OpenPopupCommand"/> class.
      /// </summary>
      private OpenPopupCommand()
         : base(OpenPopup)
      {
      }


      #endregion


      #region Public properties and indexers


      /// <summary>
      /// Gets the instance.
      /// </summary>
      /// <value>The instance.</value>
      public static OpenPopupCommand Instance
      {
         get
         {
            if (instance == null)
            {
               instance = new OpenPopupCommand();
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
      private static void OpenPopup(Popup popup)
      {
         popup.IsOpen = true;
      }


      #endregion
   }
}