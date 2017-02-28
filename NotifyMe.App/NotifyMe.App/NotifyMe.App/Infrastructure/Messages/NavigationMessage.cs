namespace NotifyMe.App.Infrastructure.Messages
{
    public class NavigationMessage
    {
        public NavigationMessage(object param)
        {
            Parameter = param;
        }

        public object Parameter { get; set; }
    }
}
