﻿#region Copyright (C) 1994-2009, Johnson & Johnson PRD, LLC.


//---------------------------------------------------------------------------*
//
//    ClosePopupCommand.cs: Helper command to open pop-ups from XAML by buttons, etc.
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


namespace Jnj.ThirdDimension.WPFControls.Commands
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