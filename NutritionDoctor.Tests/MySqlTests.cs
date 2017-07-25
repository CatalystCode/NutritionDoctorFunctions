using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NutritionDoctor.Providers.Prediction;
using Microsoft.Azure.WebJobs.Host;
using System.Diagnostics;
using NutritionDoctor.Providers;

namespace NutritionDoctor.Tests
{
    [TestClass]
    public class MySqlTests
    {
        [TestMethod]
        public void MySql_Tests()
        {
            Environment.SetEnvironmentVariable("pingan-mysqlconnectionstring", "Database=pingandb;Data Source=us-cdbr-azure-east-c.cloudapp.net;User Id=b8639718fe5ad6;Password=2cd7b667");
            var mysql = new MySqlStore(new MyTraceWriter());
            var id = mysql.InsertAsync("userId", "photoUrl", "detectedFood", "detectedFoodPr").Result;
            Assert.AreNotEqual(0, id);
        }
    }
}
