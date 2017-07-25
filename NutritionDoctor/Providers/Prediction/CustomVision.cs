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
        private const string url = "http://southcentralus.api.cognitive.microsoft.com/customvision/v1.0/prediction/588e539b-b5ec-4bdb-8fbb-b0545df48926/image";
        private const string predictionKey = "eea4e6bb615f4bdab5c0d6f36f3127bd";
        private static readonly Guid modelId = new Guid("588e539b-b5ec-4bdb-8fbb-b0545df48926");
        private readonly PredictionEndpoint endpoint;

        public CustomVision(TraceWriter log) : base(log)
        {
            var predictionEndpointCredentials = new PredictionEndpointCredentials(predictionKey);
            endpoint = new PredictionEndpoint(predictionEndpointCredentials);
        }

        protected override async Task<ImagePrediction> PredictAsyncImpl(string imageUrl)
        {
            ImageTagPrediction prediction = null;

            try
            {
                var result = await endpoint.PredictImageUrlAsync(modelId, new ImageUrl(imageUrl));
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
