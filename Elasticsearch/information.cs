using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace elasticsearch
{
    class Information
    {
        [JsonProperty(PropertyName = "objectId")]
        public string ObjectId { get; set; }
        [JsonProperty(PropertyName = "objectName")]
        public string ObjectName { get; set; }
        [JsonProperty(PropertyName = "info")]
        public string Info { get; set; }
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "publishingDate")]
        public string PublishingDate { get; set; }
        [JsonProperty(PropertyName = "viewableUntil")]
        public string ViewableUntil { get; set; }
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }
        [JsonProperty(PropertyName = "creationDate")]
        public string CreationDate { get; set; }
        [JsonProperty(PropertyName = "creatorId")]
        public string CreatorID { get; set; }

    }
}
