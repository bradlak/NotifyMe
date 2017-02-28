using Newtonsoft.Json;

namespace NotifyMe.App.Models
{
    public class PictureData
    {
        [JsonProperty("data")]
        public FacebookPicture Picture { get; set; }
    }
}
