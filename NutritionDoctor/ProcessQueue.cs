using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace NutritionDoctor
{
    public static class ProcessQueue
    {
        [FunctionName("QueueTriggerCSharp")]        
        public static void Run([QueueTrigger("queue-identifyjob", Connection = "")]string myQueueItem, TraceWriter log)
        {
            log.Info($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
