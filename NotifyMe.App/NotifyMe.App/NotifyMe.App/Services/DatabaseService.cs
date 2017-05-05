using Realms;
using System.Collections.Generic;
using System.Linq;

namespace NotifyMe.App.Services
{
    public class DatabaseService : IDatabaseService
    {
        public void Add<T>(T obj) where T : RealmObject
        {
            var db = Realm.GetInstance();
            db.Write(() =>
            {
                db.Add<T>(obj);
            });
        }

        public IEnumerable<T> GetAll<T>() where T : RealmObject
        {
            var db = Realm.GetInstance();
            return db.All<T>().ToList();
        }

        public void RemoveAll<T>() where T : RealmObject
        {
            var db = Realm.GetInstance();
            db.Write(() =>
            {
                db.RemoveAll<T>();
            });
        }
    }
}
