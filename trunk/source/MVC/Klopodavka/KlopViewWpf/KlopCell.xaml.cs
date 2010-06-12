#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using KlopIfaces;

#endregion

namespace KlopViewWpf
{
   /// <summary>
   /// Interaction logic for KlopCell.xaml
   /// </summary>
   public partial class KlopCell
   {

      #region Constructors

      public KlopCell()
      {
         InitializeComponent();
      }

      #endregion

      #region Private and protected methods

      protected override GeometryHitTestResult HitTestCore(GeometryHitTestParameters hitTestParameters)
      {
         return new GeometryHitTestResult(this, IntersectionDetail.FullyContains);
      }


      #endregion

   }
}