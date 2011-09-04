using System;
using System.Windows.Media;
using KlopIfaces;

namespace KlopViewWpf.Overlays
{
   public interface ICellOverlay
   {
      void RenderOverlay(IKlopCell klopCell,  DrawingContext drawingContext);
      //event EventHandler 
   }
}