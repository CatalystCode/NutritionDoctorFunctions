using Microsoft.Azure.WebJobs.Host;
using MySql.Data.MySqlClient;
using System;
using System.Threading.Tasks;

namespace NutritionDoctor.Providers
{
    public class MySqlStore
    {
        private string ConnectionString
        {
            get { return Environment.GetEnvironmentVariable("pingan-mysqlconnectionstring"); }
        }

        private const string UserFoodTable = "user_food_tbl";
        private readonly TraceWriter _log;

        public MySqlStore(TraceWriter log)
        {
            _log = log;
        }

        public async Task<long> InsertAsync(string userId, string photoUrl, string detectedFood, string detectedFoodProbability, string source)
        {
            try
            {
                using (var connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();

                    var sqlCommand = $"INSERT INTO {UserFoodTable}(USER_ID, PHOTO_URL, DETECTED_FOOD, DETECTED_FOOD_PR, DETECTED_FOOD_SOURCE) VALUES (@userId, @photoUrl, @detectedFood, @detectedFoodPr, @source)";
                    using (var command = new MySqlCommand(sqlCommand, connection))
                    {
                        command.Parameters.AddWithValue("@userId", userId);
                        command.Parameters.AddWithValue("@photoUrl", photoUrl);
                        command.Parameters.AddWithValue("@detectedFood", detectedFood);
                        command.Parameters.AddWithValue("@detectedFoodPr", detectedFoodProbability);
                        command.Parameters.AddWithValue("@source", source);
                        await command.ExecuteNonQueryAsync();
                        return command.LastInsertedId;
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex);
                return 0;
            }
        }
    }
}
