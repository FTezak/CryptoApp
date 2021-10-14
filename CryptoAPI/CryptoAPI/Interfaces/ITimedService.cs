using System.Threading;
using System.Threading.Tasks;

namespace CryptoAPI.Interfaces
{
    public interface ITimedService
    {
        Task StartAsync(CancellationToken cancellationToken);
        Task StopAsync(CancellationToken cancellationToken);
        Task SomeBackgroundTask();
    }
}