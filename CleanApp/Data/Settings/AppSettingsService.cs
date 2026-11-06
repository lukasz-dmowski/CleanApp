using System.IO;
using System.Text.Json;

namespace CleanApp.Data.Settings
{
    public class AppSettingsService
    {
        private readonly string _settingsPath;

        public AppSettingsService()
        {
            //getting the path of local app data folder on this comuter
            string localAppDataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            //creating sub-folder in local app data folder
            string appDirectory = Path.Combine(localAppDataDirectory,"CleanApp");
            Directory.CreateDirectory(appDirectory);

            //setting the path for 'settings' json file
            _settingsPath = Path.Combine(appDirectory,"settings.json");
        }


        public async Task<AppSettings> LoadAsync()
        {
            //checking if 'settings' json file exists
            //if no, use default direcotry
            if (File.Exists(_settingsPath) == false)
            {
                return new AppSettings();
            }

            
            try
            {
                //reading json file from path
                string json = await File.ReadAllTextAsync(_settingsPath);

                //getting the backup directory property with corresponding value 
                AppSettings? settings = JsonSerializer.Deserialize<AppSettings>(json);

                //using default directory also in situation when json file is null after deserialization 
                if (settings != null)
                {
                    return settings;
                }
                else
                {
                    return new AppSettings();
                }
            }
            catch
            {
                //in case of problems, just use default directory instead of crushing
                return new AppSettings();
            }
        }


        public async Task<bool> SaveAsync(AppSettings settings)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    //formatting json, without single line
                    WriteIndented = true
                };

                string json = JsonSerializer.Serialize(settings, options);

                await File.WriteAllTextAsync(_settingsPath, json);

                //save successful
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
