using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace NotifyMe.App.Models
{
    public class ProvidersResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expires_on")]
        public DateTime ExpiresOn { get; set; }

        [JsonProperty("provider_name")]
        public string ProviderName { get; set; }

        [JsonProperty("user_claims")]
        public List<UserClaim> UserClaims { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }
    }
}
