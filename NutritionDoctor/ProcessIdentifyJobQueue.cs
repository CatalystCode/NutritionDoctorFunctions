using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using NutritionDoctor.Providers;

namespace NutritionDoctor
{
    public static class ProcessIdentifyJobQueue
    {
        [FunctionName("ProcessIdentifyJobQueue")]        
        public static void Run([QueueTrigger("queue-identifyjob", Connection = "pingan-storage")]string myQueueItem, TraceWriter log)
        {
            log.Info($"C# Queue trigger function processed: {myQueueItem}");
            new MySqlStore().GetTables("");
        }
    }
}
