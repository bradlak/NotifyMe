using Newtonsoft.Json;

namespace NotifyMe.App.Models
{
    public class FacebookFriend
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("picture")]
        public PictureData PictureData { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonIgnore]
        public string ImageUrl
        {
            get
            {
                return PictureData.Picture.Url;
            }
        }
    }
}
