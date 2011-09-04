using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KlopIfaces;

namespace KlopViewWpf.ViewModels
{
   public class KlopCellViewModel
   {
      private readonly IKlopCell _klopCell;


      public KlopCellViewModel(IKlopCell klopCell)
      {
         _klopCell = klopCell;

         // TODO: Presentation logic like highlighting, overlay.
         // TODO: Model accessor. KlopCell view should need only this class.
      }
   }
}
