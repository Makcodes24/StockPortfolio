using System;
using System.Text.Json.Serialization;

namespace WebClient.Model
{
    public class Position
    {
        [JsonPropertyName("Symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("Quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("Percentage")]
        public decimal Percentage { get; set; }

        [JsonPropertyName("Trade")]
        public string Trade { get; set; }
    }
}
