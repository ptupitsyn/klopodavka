#region Copyright (C) 1994-2009, Johnson & Johnson PRD, LLC.


//---------------------------------------------------------------------------*
//
//    MultiSelectComboBox.xaml.cs: Interaction logic for MultiSelectComboBox.xaml
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


using System.Windows;
using System.Windows.Input;


namespace Jnj.ThirdDimension.Workflow.WorkflowManagementControls.Controls
{
   /// <summary>
   /// Interaction logic for MultiSelectComboBox.xaml
   /// </summary>
   public partial class MultiSelectComboBox
   {
      #region Fields and Constants


      private static readonly DependencyProperty enumSelectorProperty;


      #endregion


      #region Constructors


      static MultiSelectComboBox()
      {
         enumSelectorProperty = DependencyProperty.Register("EnumSelector", typeof (EnumSelector), typeof (MultiSelectComboBox));
      }


      /// <summary>
      /// Initializes a new instance of the <see cref="MultiSelectComboBox"/> class.
      /// </summary>
      public MultiSelectComboBox()
      {
         InitializeComponent();
      }


      #endregion


      #region Public properties and indexers


      /// <summary>
      /// Gets or sets the value.
      /// </summary>
      /// <value>The value.</value>
      public EnumSelector EnumSelector
      {
         get { return (EnumSelector) GetValue(enumSelectorProperty); }
         set { SetValue(enumSelectorProperty, value); }
      }


      #endregion


      #region Event handlers


      /// <summary>
      /// Handles the MouseUp event of the thisControl control.
      /// </summary>
      /// <param name="sender">The source of the event.</param>
      /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
      private void thisControl_MouseUp(object sender, MouseButtonEventArgs e)
      {
         IsDropDownOpen = true;
      }


      #endregion
   }
}