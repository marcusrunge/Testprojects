namespace DatabaseManager.Interfaces
{
    public interface IDatabaseSettings
    {
        string TableName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        string Password { get; set; }
        string Id { get; set; }
        string Source { get; set; }
    }
}
