using KlopViewWpf.Properties;


namespace KlopViewWpf.Preferences
{
   public class GamePreferences
   {
      #region Public properties and indexers

      public int GameFieldSize
      {
         get { return Settings.Default.GameFieldSize; }
         set { Settings.Default.GameFieldSize = value; }
      }

      public int GameTurnLength
      {
         get { return Settings.Default.GameTurnLength; }
         set { Settings.Default.GameTurnLength = value; }
      }

      public int GameBaseDistance
      {
         get { return Settings.Default.GameBaseDistance; }
         set { Settings.Default.GameBaseDistance = value; }
      }

      public int PlayerCount
      {
         get { return Settings.Default.PlayerCount; }
         set { Settings.Default.PlayerCount = value; }
      }

      #endregion
   }
}