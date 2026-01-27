using Microsoft.Data.SqlClient;
using System.Data;


namespace Kaira.WebUI.Context
{
    public class AppDbContext
    {

        private readonly string _connectionString;

        public AppDbContext(IConfiguration _configuration)
        {
            _connectionString = _configuration.GetConnectionString("DefaultConnection");

        }

        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);

    }
}
