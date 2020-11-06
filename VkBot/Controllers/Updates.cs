using System;
using System.Text.Json;
using Newtonsoft.Json;

namespace VkBot.Controllers
{
    [Serializable]
    public class Updates
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("object")]
        public JsonElement Object { get; set; }

        [JsonProperty("group_id")]
        public long GroupId { get; set; }
    }
}
