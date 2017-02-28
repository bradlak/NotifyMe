using System.Threading.Tasks;

namespace NotifyMe.App.Infrastructure
{
    public interface IAuthenticate
    {
        Task<bool> Authenticate();
    }
}
