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

        private readonly MySqlConnection connection;

        public MySqlStore()
        {
            connection = new MySqlConnection
            {
                ConnectionString = ConnectionString
            };
        }

        public void GetTables(string sql)
        {
            using (var connection = new MySqlStore().Connect())
            {
                DataTable databases = connection.GetSchema("Databases");
                foreach (DataRow database in databases.Rows)
                {
                    foreach (DataRow row in databases.Rows)
                    {
                        foreach (var item in row.ItemArray)
                        {
                            Console.Write("{0} ", item);
                        }
                        Console.WriteLine();
                    }
                }
            }
        }

        public MySqlConnection Connect()
        {
            connection.Open();
            return connection;
        }

        public void Dispose()
        {
            connection.Close();
        }
    }
}
