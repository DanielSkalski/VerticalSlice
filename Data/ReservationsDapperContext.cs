using System.Data;
using Microsoft.Data.SqlClient;

namespace VerticalSlice.Data
{
    public class ReservationsDapperContext
    {
        private readonly string _connectionString;

        public ReservationsDapperContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ReservationsContext");
        }

        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}
