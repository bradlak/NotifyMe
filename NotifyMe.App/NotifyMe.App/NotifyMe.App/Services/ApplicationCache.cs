using NotifyMe.App.Models;

namespace NotifyMe.App
{
	public class ApplicationCache : IApplicationCache
	{
		public FacebookFriend SelectedFriend { get; set; }
	}
}
