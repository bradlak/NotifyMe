using System.Threading.Tasks;

namespace NotifyMe.App.Infrastructure
{
    public interface ILoginService
    {
       Task<bool> Login();
    }
}
