using System;
using Microsoft.Cognitive.CustomVision;
using Microsoft.Cognitive.CustomVision.Models;
using System.Linq;
using System.Threading.Tasks;
using NutritionDoctor.Models;
using Microsoft.Azure.WebJobs.Host;

namespace NutritionDoctor.Providers.Prediction
{
    public class CustomVision : Predictor
    {
        private string CustomVisionUrl
        {
            // Should be in the form of: https://<location>.api.cognitive.microsoft.com/customvision/v1.0/prediction/.../image";
            get { return Environment.GetEnvironmentVariable("customvision-url"); }
        }

        private string CustomVisionPredictionKey 
        {
            get { return Environment.GetEnvironmentVariable("customvision-predictionkey"); }
        }

        private Guid CustomVisionModelId
        {
            get { return Guid.Parse(Environment.GetEnvironmentVariable("customvision-modelid")); }
        }

        private readonly PredictionEndpoint endpoint;

        public CustomVision(TraceWriter log) : base(log)
        {
            var predictionEndpointCredentials = new PredictionEndpointCredentials(CustomVisionPredictionKey);
            endpoint = new PredictionEndpoint(predictionEndpointCredentials);
        }

        protected override async Task<ImagePrediction> PredictAsyncImpl(string imageUrl)
        {
            ImageTagPrediction prediction = null;

            try
            {
                var result = await endpoint.PredictImageUrlAsync(CustomVisionModelId, new ImageUrl(imageUrl));
                prediction = result.Predictions.FirstOrDefault();
            } catch (Exception e)
            {
                this._log.Error(e.Message, e);
            }

            return new ImagePrediction
            {
                Url = imageUrl,
                Tag = prediction?.Tag,
                Probability = prediction?.Probability ?? 0.0,
            };
        }
    }
}
