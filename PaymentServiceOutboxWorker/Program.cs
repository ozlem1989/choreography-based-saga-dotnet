using EasyNetQ;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PaymentService.Infra;

namespace PaymentServiceOutboxWorker
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddDbContext<AppDbContext>(x => x.UseSqlServer(hostContext.Configuration.GetConnectionString("PaymentDB")));

                    var bus = RabbitHutch.CreateBus(hostContext.Configuration["RabbitMQ:ConnectionString"]);

                    services.AddSingleton<IBus>(bus);

                    services.AddHostedService<Worker>();
                });

    }
}
