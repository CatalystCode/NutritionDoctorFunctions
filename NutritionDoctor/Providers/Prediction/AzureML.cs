using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using NutritionDoctor.Models;
using Microsoft.Azure.WebJobs.Host;

namespace NutritionDoctor.Providers.Prediction
{
    public class AzureML : Predictor
    {
        private class StringTable
        {
            public string[] ColumnNames { get; set; }
            public string[,] Values { get; set; }
        }

        private string AzureMLUrl
        {
            // Should be in the form of: https://<location>.services.azureml.net/.../execute?api-version=2.0&details=true";
            get { return Environment.GetEnvironmentVariable("azureml-url"); }
        }

        private string AzureMLApiKey
        {
            get { return Environment.GetEnvironmentVariable("azureml-apikey"); }
        }

        public AzureML(TraceWriter log) : base(log)
        {
        }

        protected override async Task<ImagePrediction> PredictAsyncImpl(string imageUrl)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AzureMLApiKey);
                client.BaseAddress = new Uri(AzureMLUrl);

                var scoreRequest = new
                {
                    Inputs = new Dictionary<string, StringTable>() {
                        {
                            "input1",
                            new StringTable()
                            {
                                ColumnNames = new string[] {"imageURL"},
                                Values = new string[,] { { imageUrl } }
                            }
                        },
                    },
                    GlobalParameters = new Dictionary<string, string>() { },
                };

                HttpResponseMessage response = await client.PostAsJsonAsync("", scoreRequest).ConfigureAwait(false);

                var imagePrediction = new ImagePrediction
                {
                    Url = AzureMLUrl
                };

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    dynamic json = JsonConvert.DeserializeObject(result);

                    imagePrediction.Tag = json.Results.output1.value.Values[0][0];
                    imagePrediction.Probability = 1;
                }
                else
                {
                    _log.Error($"The request failed with status code: {response.StatusCode}.");
                    _log.Error(response.Headers.ToString());

                    string responseContent = await response.Content.ReadAsStringAsync();
                    _log.Error(responseContent);
                }

                return imagePrediction;
            }
        }
    }
}
