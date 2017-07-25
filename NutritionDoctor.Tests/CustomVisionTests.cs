using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NutritionDoctor.Providers.Prediction;
using Microsoft.Azure.WebJobs.Host;
using System.Diagnostics;

namespace NutritionDoctor.Tests
{
    public class MyTraceWriter : TraceWriter
    {
        public MyTraceWriter() : this(TraceLevel.Off)
        {
        }

        protected MyTraceWriter(TraceLevel level) : base(level)
        {
        }

        public override void Trace(TraceEvent traceEvent)
        {
        }
    }

    [TestClass]
    public class CustomVisionTests
    {
        [TestMethod]
        public void ChickenChowMeinTest()
        {
            var customVision = new CustomVision(new MyTraceWriter());
            var prediction = customVision.PredictAsync("https://cdn5.norecipes.com/wp-content/uploads/2015/05/23053212/recipechicken-chow-mein.1024x1024-4.jpg").Result;
        }
    }
}
