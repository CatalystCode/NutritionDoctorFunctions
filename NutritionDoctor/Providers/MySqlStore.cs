using Microsoft.Azure.WebJobs.Host;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutritionDoctor.Providers
{
    public class MySqlStore : IDisposable
    {
        private string ConnectionString
        {
            get { return Environment.GetEnvironmentVariable("pingan-mysqlconnectionstring"); }
        }

        private readonly MySqlConnection _connection;
        private readonly TraceWriter _log;

        public MySqlStore(TraceWriter log)
        {
            _log = log;
            _connection = new MySqlConnection
            {
                ConnectionString = ConnectionString
            };
        }

        public void GetTables(string sql)
        {
                DataTable databases = _connection.GetSchema("Databases");
                foreach (DataRow database in databases.Rows)
                {
                    foreach (DataRow row in databases.Rows)
                    {
                        foreach (var item in row.ItemArray)
                        {
                            _log.Info(item.ToString());
                        }
                    }
                }
        }

        public MySqlConnection Connect()
        {
            _connection.Open();
            return _connection;
        }

        public void Dispose()
        {
            _connection.Close();
        }
    }
}
