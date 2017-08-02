using Microsoft.VisualStudio.TestTools.UnitTesting;
using NutritionDoctor.Providers;

namespace NutritionDoctor.Tests
{
    [TestClass]
    public class MySqlTests
    {
        [TestMethod]
        public void MySql_Tests()
        {
            var mysql = new MySqlStore(new MyTraceWriter());
            var id = mysql.InsertAsync("userId", "photoUrl", "detectedFood", "detectedFoodPr", "source").Result;
            Assert.AreNotEqual(0, id);
        }
    }
}
