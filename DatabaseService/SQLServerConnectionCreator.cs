using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace DatabaseService
{
    public class SQLServerConnectionCreator : DbConnectionCreator
    {
        private readonly string _connectionString;  

        public SQLServerConnectionCreator(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("SQLServerConnectionString") ?? throw new ArgumentNullException("Connection string is null.", nameof(config));
        }

        public override async Task<IDbConnection> CreateAsync()
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }
    }
}
