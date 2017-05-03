using Realms;

namespace NotifyMe.App.Models.Entities
{
    public class SentMessage : RealmObject
    {
        public string Body { get; set; }

        public string Receiver { get; set; }

        public string Date { get; set; }
    }
}
