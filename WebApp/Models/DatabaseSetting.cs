namespace WebApp.Models
{
    public class DatabaseSetting
    {
        public string? ConnectionString { get; set; } = string.Empty;
        public string? DatabaseName { get; set; } = string.Empty;
        public string? CollectionName { get; set; } = string.Empty;

        public DatabaseSetting(string con, string dbName, string colName)
        {
            ConnectionString = con;
            DatabaseName = dbName;
            CollectionName = colName;
        }
    }
}
