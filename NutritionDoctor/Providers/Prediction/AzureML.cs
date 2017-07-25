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

        private const string url = "https://ussouthcentral.services.azureml.net/workspaces/6eb1c3c48c174fa782b3e6a48a27de84/services/20293eed6bcd422f927ea8ac484bdc30/execute?api-version=2.0&details=true";
        private const string apiKey = "lbd8wyiCjjQtcmfrNtR2L+uRE5mGLRUQ3iKrtDkHsR++8w10FxMiNh8rF5jxDF87TeLbmg9YzhuvFi+Z3nA8Ig==";

        public AzureML(TraceWriter log) : base(log)
        {
        }

        protected override async Task<ImagePrediction> PredictAsyncImpl(string imageUrl)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                client.BaseAddress = new Uri(url);

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
                    Url = url
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
