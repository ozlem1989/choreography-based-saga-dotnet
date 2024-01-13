using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using PaymentService.Infra.Models;
using PaymentService.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentServiceOutboxWorker
{
    public class Worker : BackgroundService
    {

        private readonly IServiceScopeFactory _scopeFactory;

        public Worker(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await PublishOutboxMessages(stoppingToken);
            }
        }

        private async Task PublishOutboxMessages(CancellationToken stoppingToken)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                await using var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                IBus bus = scope.ServiceProvider.GetRequiredService<IBus>();

                List<OutboxMessage> messages = appDbContext.OutboxMessages.Where(om => !om.IsSent).ToList();

                foreach (OutboxMessage outboxMessage in messages)
                {
                    try
                    {
                        var messageType = Type.GetType(outboxMessage.EventType);
                        var message = JsonConvert.DeserializeObject(outboxMessage.EventPayload, messageType!);

                        await bus.PubSub.PublishAsync(message, messageType);

                        outboxMessage.IsSent = true;
                        outboxMessage.SentDate = DateTime.UtcNow;

                        appDbContext.OutboxMessages.Update(outboxMessage);
                        await appDbContext.SaveChangesAsync();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }

    }
}
