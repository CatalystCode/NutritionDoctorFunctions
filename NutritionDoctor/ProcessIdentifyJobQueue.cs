using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using NutritionDoctor.Providers;
using NutritionDoctor.Providers.Prediction;

namespace NutritionDoctor
{
    public static class ProcessIdentifyJobQueue
    {
        private struct IdentifyJob
        {
            public string UserId { get; set; }
            public string ImageUrl { get; set; }
        }

        [FunctionName("ProcessIdentifyJobQueue")]        
        public async static void Run([QueueTrigger("queue-identifyjob", Connection = "pingan-storage")]string myQueueItem, TraceWriter log)
        {
            log.Info($"Identify Job ID: {myQueueItem}");

            IdentifyJob job = JsonConvert.DeserializeObject<IdentifyJob>(myQueueItem);
            
            var azureML = new AzureML(log);
            var azureMLPrediction = await azureML.PredictAsync(job.ImageUrl);
            log.Info($"AzureML: {azureMLPrediction}");

            var customVision = new CustomVision(log);
            var customVisionPrediction = await customVision.PredictAsync(job.ImageUrl);
            log.Info($"CustomVision: {customVision}");

            var mysql = new MySqlStore(log);

            var rowId = await mysql.InsertAsync(job.UserId, job.ImageUrl, customVisionPrediction.Tag, customVisionPrediction.Probability.ToString(), "CustomVision");
            log.Info($"CustomVision MySQL Row: {rowId}");

            rowId = await mysql.InsertAsync(job.UserId, job.ImageUrl, azureMLPrediction.Tag, azureMLPrediction.Probability.ToString(), "AzureMachineLearning");
            log.Info($"Azure Machine Learning MySQL Row: {rowId}");
        }
    }
}
