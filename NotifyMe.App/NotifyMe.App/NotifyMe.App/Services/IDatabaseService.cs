using System.Collections.Generic;

namespace NotifyMe.App.Services
{
    public interface IDatabaseService
    {
        IEnumerable<T> GetAll<T>();

        void Add<T>(T obj);

        void Remove<T>(T obj);
    }
}
