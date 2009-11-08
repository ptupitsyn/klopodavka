#region Usings

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#endregion

namespace KlopViewWpf
{
   internal class HitTestViewbox : Viewbox
   {
      #region Private and protected methods

      /// <summary>
      /// Implements <see cref="M:System.Windows.Media.Visual.HitTestCore(System.Windows.Media.PointHitTestParameters)"/> to supply base element hit testing behavior (returning <see cref="T:System.Windows.Media.HitTestResult"/>).
      /// </summary>
      /// <param name="hitTestParameters">Describes the hit test to perform, including the initial hit point.</param>
      /// <returns>
      /// Results of the test, including the evaluated point.
      /// </returns>
      protected override HitTestResult HitTestCore(PointHitTestParameters hitTestParameters)
      {
         var r = new Rect(new Point(), RenderSize);

         if (r.Contains(hitTestParameters.HitPoint))
         {
            return new PointHitTestResult(this, hitTestParameters.HitPoint);
         }

         return null;
      }

      #endregion
   }
}