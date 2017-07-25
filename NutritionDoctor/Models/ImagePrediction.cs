using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutritionDoctor.Models
{
    public class ImagePrediction
    {
        public string Url { get; set; }

        public string Tag { get; set; }

        public double Probability { get; set; }

        public override string ToString()
        {
            return $"{Url}: {Tag} @ {Probability}";
        }
    }
}
