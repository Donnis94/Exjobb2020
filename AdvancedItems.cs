using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Text.Json.Serialization;

namespace testExjobb
{
    class AdvancedItems
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id = Guid.NewGuid();
        [JsonProperty(PropertyName = "placeId")]
        public string PlaceId {get;set;}
        [JsonProperty(PropertyName = "mediumId")]
        public string MediumId { get; set; }
        [JsonProperty(PropertyName = "videoId")]
        public string VideoId { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        [JsonProperty(PropertyName = "ipAdress")]
        public string IPadress { get; set; }
        [JsonProperty(PropertyName = "country")]
        public string Country { get; set; }
        [JsonProperty(PropertyName = "device")]
        public string Device { get; set; }
        [JsonProperty(PropertyName = "platform")]
        public string Platform { get; set; }
        [JsonProperty(PropertyName = "operatingSystem")]
        public string OperatingSystem { get; set; }
        [JsonProperty(PropertyName = "rate")]
        public int Rate { get; set; }
        [JsonProperty(PropertyName = "userId")]
        public string UserID { get; set; }
        [JsonProperty(PropertyName = "startTime")]
        public string StartTime { get; set; }
        [JsonProperty(PropertyName = "pauseTime")]
        public string PauseTime { get; set; }
    }
}



