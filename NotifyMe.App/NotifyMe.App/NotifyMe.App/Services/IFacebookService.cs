using NotifyMe.App.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotifyMe.App.Services
{
    public interface IFacebookService
    {
        Task<IEnumerable<FacebookFriend>> GetFacebookFriends();

        Task<ApplicationUser> GetCurrentApplicationUser();
    }
}
