using System.Data;

namespace CleanArchitectureDDD.Application.Abstractions.Data;

public interface ISqlConnectionFactory
{
    IDbConnection CreateConnection();
}