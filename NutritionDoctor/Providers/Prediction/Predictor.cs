using Microsoft.Azure.WebJobs.Host;
using NutritionDoctor.Models;
using System;
using System.Threading.Tasks;

namespace NutritionDoctor.Providers.Prediction
{
    public abstract class Predictor
    {
        protected readonly TraceWriter _log;

        public Predictor(TraceWriter log)
        {
            _log = log;
        }

        public async Task<ImagePrediction> PredictAsync(string imageUrl)
        {
            if (String.IsNullOrEmpty(imageUrl))
            {
                throw new ArgumentNullException(nameof(imageUrl));
            }

            try
            {
                var prediction = await PredictAsyncImpl(imageUrl);
                _log.Info(prediction.ToString());
                return prediction;
            }
            catch (Exception e)
            {
                _log.Error($"Error attempting to retrieve pridction for {imageUrl}", e);
                throw;
            }
        }

        protected abstract Task<ImagePrediction> PredictAsyncImpl(string imageUrl);
    }
}
