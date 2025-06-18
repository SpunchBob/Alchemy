using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Alchemy.Code
{
    public class ChemicalElement // Блять так сложно указать модификатор доступа?
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("number")]
        public int? Number { get; set; }

        [JsonPropertyName("mass")]
        public double? Mass { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("how_to_get")]
        public string How_to_get { get; set; }

        [JsonPropertyName("using")]
        public string Using { get; set; }

        [JsonPropertyName("image_path")]
        public string Image_path { get; set; }
    }
}
