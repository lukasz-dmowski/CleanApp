using System.IO;

namespace CleanApp.Data
{
    public static class DatabasePathProvider
    {
        public static string DatabaseDirectory
        {
            get
            {
                //getting the path of LocalAppData folder on this compter 
                string localAppDataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                //creating sub-folders in LocalAppData
                string databaseDirectory = Path.Combine(localAppDataDirectory, "CleanApp", "Data");

                return databaseDirectory;
            }
        }
        public static string BackupDirectory
        {
            get
            {
                //getting the path of LocalAppData folder on this compter
                string localAppDataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                //creating sub-folders in LocalAppData
                string backupDirectory = Path.Combine(localAppDataDirectory, "CleanApp", "Backups");

                return backupDirectory;
            }
        }

        public static string DatabasePath
        {
            get
            {   
                //creating full path to database with database name
                string fullDatabasePath = Path.Combine(DatabaseDirectory,"CleanApp.db");
                return fullDatabasePath;
            }
        }

        public static void EnsureDirectoryExists()
        {
            Directory.CreateDirectory(DatabaseDirectory);
        }
    }
}
