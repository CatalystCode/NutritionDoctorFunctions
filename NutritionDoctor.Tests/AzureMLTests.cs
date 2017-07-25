using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NutritionDoctor.Providers.Prediction;
using Microsoft.Azure.WebJobs.Host;
using System.Diagnostics;

namespace NutritionDoctor.Tests
{
    [TestClass]
    public class AzureMLTests
    {
        [TestMethod]
        public void AzureML_ChickenChowMeinTest()
        {
            var customVision = new AzureML(new MyTraceWriter());
            var prediction = customVision.PredictAsync("https://pinganhackfest2017.blob.core.windows.net/roujiamo/download%20copy%202.jpg").Result;
        }
    }
}
