using KlopViewWpf.Properties;


namespace KlopViewWpf.Preferences
{
   public class GamePreferences
   {
      #region Public properties and indexers

      public int GameFieldSize
      {
         get { return (int) Settings.Default["GameFieldSize"]; }
         set { Settings.Default["GameFieldSize"] = value; }
      }

      public int GameTurnLength
      {
         get { return (int) Settings.Default["GameTurnLength"]; }
         set { Settings.Default["GameTurnLength"] = value; }
      }

      public int GameBaseDistance
      {
         get { return (int) Settings.Default["GameBaseDistance"]; }
         set { Settings.Default["GameBaseDistance"] = value; }
      }

      #endregion
   }
}