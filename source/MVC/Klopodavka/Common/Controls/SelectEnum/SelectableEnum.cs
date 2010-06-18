#region Copyright (C) 1994-2009, Johnson & Johnson PRD, LLC.


//---------------------------------------------------------------------------*
//
//    SelectableEnum.cs: SPECIFY COMMENT.
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


using System;
using System.ComponentModel;


namespace Jnj.ThirdDimension.Workflow.WorkflowManagementControls.Controls
{
   /// <summary>
   /// 
   /// </summary>
   public class SelectableEnum : INotifyPropertyChanged
   {
      #region Fields and Constants


      private bool isSelected;

      private EnumSelector parent;
      private Enum value;


      #endregion


      #region Constructors


      /// <summary>
      /// Initializes a new instance of the <see cref="SelectableEnum"/> class.
      /// </summary>
      /// <param name="parentSelector">The parent selector.</param>
      public SelectableEnum(EnumSelector parentSelector)
      {
         parent = parentSelector;
      }


      #endregion


      #region INotifyPropertyChanged implementation


      /// <summary>
      /// Occurs when a property value changes.
      /// </summary>
      public event PropertyChangedEventHandler PropertyChanged;


      #endregion


      #region Public properties and indexers


      /// <summary>
      /// Gets or sets a value indicating whether this instance is selected.
      /// </summary>
      /// <value>
      /// 	<c>true</c> if this instance is selected; otherwise, <c>false</c>.
      /// </value>
      public bool IsSelected
      {
         get { return isSelected; }
         set
         {
            isSelected = value;
            OnPropertyChanged("IsSelected");
         }
      }

      /// <summary>
      /// Gets or sets the value.
      /// </summary>
      /// <value>The value.</value>
      public Enum Value
      {
         get { return value; }
         set
         {
            this.value = value;
            OnPropertyChanged("Value");
         }
      }


      #endregion


      #region Public methods


      /// <summary>
      /// Returns a <see cref="System.String"/> that represents this instance.
      /// </summary>
      /// <returns>
      /// A <see cref="System.String"/> that represents this instance.
      /// </returns>
      public override string ToString()
      {
         return parent.ToString();
      }


      #endregion


      #region Private and protected methods


      /// <summary>
      /// Called when [property changed].
      /// </summary>
      /// <param name="propertyName">Name of the property.</param>
      protected void OnPropertyChanged(string propertyName)
      {
         PropertyChangedEventHandler handler = PropertyChanged;
         if (handler != null)
         {
            handler(this, new PropertyChangedEventArgs(propertyName));
         }
      }


      #endregion
   }
}