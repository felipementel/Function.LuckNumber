using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Project.LuckyNumber.Function.Functions.Http;
using Project.LuckyNumber.Function.Middleware;
using System.Threading.Tasks;

namespace Project.LuckyNumber.Function
{
    public class Program
    {
        static async Task Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults(
                    builder =>
                    {
                        builder.UseMiddleware<CorrelationMiddleware>();
                        builder.UseMiddleware<ExceptionLoggingMiddleware>();
                    }
                )
                .ConfigureServices((services) =>
                {
                    services.AddSingleton<PingResponse>();
                })
                .Build();

            await host.RunAsync();
        }
    }
}
