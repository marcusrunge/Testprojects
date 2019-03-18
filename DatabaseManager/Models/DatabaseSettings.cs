using DatabaseManager.Interfaces;

namespace DatabaseManager.Models
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public string Source { get; set; }
        public string TableName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string Id { get; set; }
        public string Password { get; set; }
    }
}
