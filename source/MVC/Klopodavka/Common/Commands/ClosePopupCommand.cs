#region Copyright (C) 1994-2009, Johnson & Johnson PRD, LLC.

//---------------------------------------------------------------------------*
//
//    ClosePopupCommand.cs: Helper command to close pop-ups from XAML by buttons, etc.
//
//---
//
//    Copyright (C) 1994-2009, Johnson & Johnson PRD, LLC.
//    All Rights Reserved.
//
//    Pavel Tupitsin, 12/2009
//
//---------------------------------------------------------------------------*/

#endregion

using System.Windows.Controls.Primitives;
using System.Windows.Input;


namespace Jnj.ThirdDimension.WPFControls.Commands
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