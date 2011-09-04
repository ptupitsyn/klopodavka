namespace KlopViewWpf.Preferences
{
   public class PreferencesManager
   {
      #region Fields and Constants

      public static readonly PreferencesManager Instance = new PreferencesManager();

      #endregion

      #region Constructors

      private PreferencesManager()
      {
         //TODO: PropertyGrid for editing this
         RenderPreferences = new RenderPreferences();
         GamePreferences = new GamePreferences();
      }

      #endregion

      #region Public properties and indexers

      public RenderPreferences RenderPreferences { get; private set; }
      public GamePreferences GamePreferences { get; private set; }

      #endregion
   }
}