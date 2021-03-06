namespace IvyBot.Configuration {
    internal class ConfigurationFileEditor {
        private readonly System.Configuration.Configuration file;

        internal ConfigurationFileEditor () {
            file = System.Configuration.ConfigurationManager.OpenExeConfiguration (System.Configuration.ConfigurationUserLevel.None);
        }

        internal void Save () {
            file.Save (System.Configuration.ConfigurationSaveMode.Modified);
            System.Configuration.ConfigurationManager.RefreshSection (file.AppSettings.SectionInformation.Name);
        }

        internal void WriteSetting (KeyValuePair setting) {
            if (SettingExists (setting)) {
                UpdateSetting (setting);
            } else {
                CreateSetting (setting);
            }
        }

        private void CreateSetting (KeyValuePair setting) {
            file.AppSettings.Settings.Add (setting.Key, setting.Value);
        }

        private void UpdateSetting (KeyValuePair setting) {
            file.AppSettings.Settings[setting.Key].Value = setting.Value;
        }

        private bool SettingExists (KeyValuePair setting) {
            return !(file.AppSettings.Settings[setting.Key] is null);
        }
    }
}