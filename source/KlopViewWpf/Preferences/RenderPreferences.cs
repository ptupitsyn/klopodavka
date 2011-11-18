#region Usings

using KlopViewWpf.Properties;

#endregion

namespace KlopViewWpf.Preferences
{
   public class RenderPreferences
   {
      #region Public properties and indexers

      public bool UseCachedBrush
      {
         get { return Settings.Default.UseCachedBrush; }
         set { Settings.Default.UseCachedBrush = value; }
      }

      public bool DisableAnimation
      {
         get { return Settings.Default.DisableAnimation; }
         set { Settings.Default.DisableAnimation = value; }
      }

      #endregion
   }
}