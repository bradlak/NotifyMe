using NotifyMe.App.Models;

namespace NotifyMe.App
{
	public interface IApplicationCache
	{
		FacebookFriend SelectedFriend { get; set; }
	}
}
