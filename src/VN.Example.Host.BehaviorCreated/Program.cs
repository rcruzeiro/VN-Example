using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using VN.Example.Infrastructure.IoC;
using VN.Example.Infrastructure.Provider.MessageBus;
using VN.Example.Platform.Application.BehaviorService;
using VN.Example.Platform.Application.BehaviorService.DTOs;
using VN.Example.Platform.Domain.BehaviorAggregation.Events;

namespace VN.Example.Host.BehaviorCreated
{
    class Program
    {
        private static IServiceCollection services = new ServiceCollection();
        private static IConfiguration configuration;
        private static readonly string queueName = "behavior_created";
        private static readonly AutoResetEvent _waitHandle =
            new AutoResetEvent(false);

        static void Main(string[] args)
        {
            // start DI container
            InitializeDIContainer();

            // create a provider containing all services which exists whithin IServiceCollection instance.i
            var serviceProvider = services.BuildServiceProvider();

            using (IMessageService messageService = new MessageService(serviceProvider.GetService<RabbitMQConfiguration>()))
            {
                // consume the queue from RabbitMQ
                Task.Run(() => messageService.ConsumeAsync(queueName, (s, e) =>
                {
                    try
                    {
                        var content = Encoding.UTF8.GetString(e.Body);
                        var result = JsonConvert.DeserializeObject<BehaviorCreatedEvent>(content);

                        // persists the new behavior inside every database with the domain application service
                        IBehaviorAppService behaviorAppService = serviceProvider.GetService<IBehaviorAppService>();
                        var createBehaviorDto = new CreateBehaviorDto
                        {
                            IP = result.IP,
                            PageName = result.PageName,
                            UserAgent = result.UserAgent,
                            PageParameters = result.PageParameters
                        };

                        Task.Run(() =>
                        {
                            try
                            {
                                behaviorAppService.CreateBehaviorAsync(createBehaviorDto);

                                Console.WriteLine($"New behavior created for IP: {result.IP}.");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error when creating behavior: {ex.Message}");
                            }
                        });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error when pulling from queue: {ex.Message}");
                    }
                }));

                Console.WriteLine("Waiting for created behaviors..");

                Console.CancelKeyPress += (o, e) =>
                {
                    Console.WriteLine("Leaving..");

                    _waitHandle.Set();
                    e.Cancel = true;
                };

                _waitHandle.WaitOne();
            }
        }

        private static void InitializeDIContainer()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            configuration = builder.Build();

            var module = new ExampleModule(services, configuration);
        }
    }
}
