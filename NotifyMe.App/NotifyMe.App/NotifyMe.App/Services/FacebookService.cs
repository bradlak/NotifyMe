using System;
using System.Collections.Generic;
using NotifyMe.App.Models;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using NotifyMe.App.Infrastructure;
using System.Linq;

namespace NotifyMe.App.Services
{
    public class FacebookService : IFacebookService
    {
        public async Task<ApplicationUser> GetCurrentApplicationUser()
        {
            var providers = await Initialize();
            var result = new ApplicationUser();

            var idClaim = providers[0].UserClaims.FirstOrDefault(z => z.Type.Contains("nameidentifier"));
            var nameClaim = providers[0].UserClaims.FirstOrDefault(z => z.Value.Contains(" "));

            if(idClaim != null && nameClaim != null)
            {
                result.Id = idClaim.Value;
                result.Name = nameClaim.Value;
            }

            return result;
        }

        public async Task<IEnumerable<FacebookFriend>> GetFacebookFriends()
        {
            var providers = await Initialize();
            string path = @"https://graph.facebook.com/v2.8/me/friends?fields=id,name,picture&access_token=" + providers[0].AccessToken;
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(path),
                    Method = HttpMethod.Get,
                };
                var data = await client.SendAsync(request);

                var response = await data.Content.ReadAsStringAsync();
                var deserialized = JsonConvert.DeserializeObject<FriendsResponse>(response);

                return deserialized.Friends;
            }
        }

        private async Task<ProvidersResponse[]> Initialize()
        {
            var data = await MobileServiceClientWrapper.Instance.Client.InvokeApiAsync("/.auth/me");
            var datastr = data.ToString();
            var response = JsonConvert.DeserializeObject<ProvidersResponse[]>(datastr);
            return response;
        }
    }
}
