using Newtonsoft.Json;

namespace twilifi.unifi.Models
{
    public class Event
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("start")]
        public long Start { get; set; }

        [JsonProperty("end")]
        public long End { get; set; }

        [JsonProperty("camera")]
        public string Camera { get; set; }

        [JsonProperty("score")]
        public int Score { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("modelKey")]
        public string ModelKey { get; set; }

        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }

        public override string ToString()
        {
            return $"{nameof(Type)}: {Type}, {nameof(Start)}: {Start}, {nameof(End)}: {End}, {nameof(Camera)}: {Camera}, {nameof(Score)}: {Score}, {nameof(Id)}: {Id}, {nameof(ModelKey)}: {ModelKey}, {nameof(Thumbnail)}: {Thumbnail}";
        }
    }
}