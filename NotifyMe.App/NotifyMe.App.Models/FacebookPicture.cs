using Newtonsoft.Json;

namespace NotifyMe.App.Models
{
    public class FacebookPicture
    {
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
