namespace VelesLibrary.DbModels;

public class Connection
{
    public Connection()
    {
    }

    public Connection(string connectionString, Group group)
    {
        ConnectionString = connectionString;
        Group = group;
    }

    public int Id { get; set; }
    public string ConnectionString { get; set; }
    public Group Group { get; set; }
}