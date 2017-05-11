using NotifyMe.App.Enumerations;
using System;

namespace NotifyMe.App.Services
{
    public interface IMobileCenterLogger
    {
        void TrackEvent(string userName, EventType type);

        void TrackCrash(Exception ex);
    }
}
