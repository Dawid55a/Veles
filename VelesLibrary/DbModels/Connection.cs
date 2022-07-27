namespace VelesLibrary.DbModels
{
    public class Connection
    {
        public Connection(){}
        public Connection(string connectionId, Group group)
        {
            ConnectionId = connectionId;
            Group = group;
        }

        public string ConnectionId { get; set; }
        public Group Group { get; set; }
    }
}
