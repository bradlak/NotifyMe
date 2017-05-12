using NotifyMe.App.Models;

namespace NotifyMe.App
{
    public class ApplicationCache : IApplicationCache
    {
        public ApplicationUser CurrentUser { get; set; }

        public FacebookFriend SelectedFriend { get; set; }
    }
}
