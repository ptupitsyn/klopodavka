#region Usings

using System;
using System.Windows.Media;
using KlopIfaces;

#endregion

namespace KlopModel
{
   public class KlopPlayer : IKlopPlayer
   {
      #region IKlopPlayer implementation

      public string Name { get; set; }
      public int BasePosX { get; set; }
      public int BasePosY { get; set; }
      public bool Human { get; set; }
      public Color Color { get; set; }
      
      public void SetModel(IKlopModel klopModel)
      {
      }

      #endregion
   }
}