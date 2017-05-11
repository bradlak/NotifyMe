using Microsoft.Azure.Mobile.Analytics;
using NotifyMe.App.Enumerations;
using System;
using System.Collections.Generic;

namespace NotifyMe.App.Services
{
    public class MobileCenterLogger : IMobileCenterLogger
    {
        public void TrackEvent(string userName, EventType type)
        {
            var name = Enum.GetName(typeof(EventType), type);
            var properties = new Dictionary<string, string>();
            properties.Add(nameof(userName), userName);
            Analytics.TrackEvent(name, properties);
        }

        public void TrackCrash(Exception ex)
        {
            var properties = new Dictionary<string, string>();
            properties.Add("Message", ex.Message);
            Analytics.TrackEvent("Crash" ,properties);
        }
    }
}
