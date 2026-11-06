namespace CleanApp.Data.Settings
{
    public class AppSettings
    {
        //default backup directory is local data folder
        //this gets new value, only when default directory is changed
        public string BackupDirectory { get; set; } = string.Empty;
    }
}
