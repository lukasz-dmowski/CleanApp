using CleanApp.Data.Settings;
using Microsoft.Data.Sqlite;
using System.IO;

namespace CleanApp.Data
{
    public class DatabaseBackupService
    {
        // max number of backup files in backup folder
        // on new backup creation, these files are valuated 
        private const int MaxBackupCount = 10;

        private readonly string _databasePath;
        private readonly AppSettings _appSettings;

        public DatabaseBackupService(string databasePath, AppSettings appSettings)
        {
            _databasePath = databasePath;
            _appSettings = appSettings;
        }

        public bool CreateBackup()
        {
            string backupDirectory = _appSettings.BackupDirectory;

            if (string.IsNullOrWhiteSpace(backupDirectory))
            {
                //if backup directory from settings json file is empty, use default directory
                backupDirectory = DatabasePathProvider.BackupDirectory;
            }

            string? backupPath = null;

            try
            {
                Directory.CreateDirectory(backupDirectory);

                string fileName = $"CleanApp_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.db";

                backupPath = Path.Combine(backupDirectory, fileName);

                using SqliteConnection source = new SqliteConnection($"Data Source={_databasePath}");
                using SqliteConnection destination = new SqliteConnection($"Data Source={backupPath}");

                source.Open();
                destination.Open();

                source.BackupDatabase(destination);

                DeleteOldBackups(backupDirectory);

                return true;
            }
            catch
            {
                if (backupPath != null && File.Exists(backupPath))
                {
                    try
                    {
                        File.Delete(backupPath);
                    }
                    catch
                    {
                    }
                }

                return false;
            }
        }

        private void DeleteOldBackups(string backupDirectory)
        {
            List<FileInfo> backups = new DirectoryInfo(backupDirectory).GetFiles("CleanApp_*.db")
                                                                       .OrderByDescending(file => file.CreationTimeUtc)
                                                                       .ToList();

            foreach (FileInfo backup in backups.Skip(MaxBackupCount))
            {
                backup.Delete();
            }
        }
    }
}
