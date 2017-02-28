using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotifyMe.App.Models
{
    public class FriendsResponse
    {
        [JsonProperty("data")]
        public List<FacebookFriend> Friends { get; set; }

        [JsonProperty("summary")]
        public Summary Summary { get; set; }
    }
}
