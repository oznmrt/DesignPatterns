namespace WebApp.Strategy.Models
{
    public class Settings
    {
        public static string claimDBType = "databasetype";

        public EDbType DataBaseType { get; set; }

        public EDbType GetDefaultDBType => EDbType.SqlServer;
    }
}
