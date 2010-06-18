#region Copyright (C) 1994-2009, Johnson & Johnson PRD, LLC.

//---------------------------------------------------------------------------*
//
//    LineLabel.xaml.cs: Interaction logic for LineLabel.xaml.
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
using System.Windows.Ink;


namespace Jnj.ThirdDimension.WPFControls
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