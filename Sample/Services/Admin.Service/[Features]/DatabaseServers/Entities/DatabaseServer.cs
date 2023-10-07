using Admin.ServiceBase.DatabaseServers;

using Yaver.Db;

namespace Admin.Service.DatabaseServers.Entities
{
  public class DatabaseServer(
    string name,
    string host,
    int port,
    DatabaseServerStatus status,
    string connectionStringFormat,
    bool isDefault = false) : AuditableEntity
  {

    public string Name { get; set; } = name;
    public string Host { get; set; } = host;
    public int Port { get; set; } = port;
    public DatabaseServerStatus Status { get; set; } = status;
    public string ConnectionStringFormat { get; set; } = connectionStringFormat;
    public bool IsDefault { get; set; } = isDefault;
  }
}
