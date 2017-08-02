using Microsoft.Azure.WebJobs.Host;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NutritionDoctor.Providers.Prediction;
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
            var prediction = customVision.PredictAsync("https://upload.wikimedia.org/wikipedia/commons/thumb/c/c5/Roujiamo.jpg/200px-Roujiamo.jpg").Result;
        }
    }
}
