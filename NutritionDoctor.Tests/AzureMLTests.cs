using Microsoft.VisualStudio.TestTools.UnitTesting;
using NutritionDoctor.Providers.Prediction;

namespace NutritionDoctor.Tests
{
    [TestClass]
    public class AzureMLTests
    {
        [TestMethod]
        public void AzureML_ChickenChowMeinTest()
        {
            var customVision = new AzureML(new MyTraceWriter());
            var prediction = customVision.PredictAsync("https://upload.wikimedia.org/wikipedia/commons/thumb/c/c5/Roujiamo.jpg/200px-Roujiamo.jpg").Result;
        }
    }
}
