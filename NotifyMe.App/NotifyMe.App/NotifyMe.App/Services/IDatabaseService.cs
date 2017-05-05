using Realms;
using System;
using System.Collections.Generic;

namespace NotifyMe.App.Services
{
    public interface IDatabaseService
    {
        IEnumerable<T> GetAll<T>() where T : RealmObject;

        void Add<T>(T obj) where T : RealmObject;

        void RemoveAll<T>() where T : RealmObject;
    }
}
