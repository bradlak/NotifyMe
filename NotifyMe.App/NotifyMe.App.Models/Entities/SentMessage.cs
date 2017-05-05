using Realms;

namespace NotifyMe.App.Models.Entities
{
    public class SentMessage : RealmObject
    {
        public SentMessage()
        {
        }

        public SentMessage(string receiver, string body, string date)
        {
            Receiver = receiver;
            Body = body;
            Date = date;
        }

        public string Body { get; set; }

        public string Receiver { get; set; }

        public string Date { get; set; }
    }
}
