using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using VN.Example.Infrastructure.Database.MSSQL;
using VN.Example.Infrastructure.Database.MSSQL.Repositories;
using VN.Example.Infrastructure.Provider.MessageBus;
using VN.Example.Platform.Application.BehaviorService;
using VN.Example.Platform.Domain.BehaviorAggregation;

namespace VN.Example.Infrastructure.IoC
{
    public class ExampleModule
    {
        public ExampleModule(IConfiguration configuration)
           : this(new ServiceCollection(), configuration)
        { }

        public ExampleModule(IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            // providers
            // rabbitmq
            var rabbitMQConfiguration = new RabbitMQConfiguration();
            new ConfigureFromConfigurationOptions<RabbitMQConfiguration>(
                configuration.GetSection("RabbitMQ")).Configure(rabbitMQConfiguration);
            services.AddSingleton(rabbitMQConfiguration);
            services.AddTransient<IMessageService, MessageService>();

            // data source
            services.AddSingleton<IDataSource>(provider =>
                new DefaultDataSource(configuration, "ExampleDb"));

            // unit of work (db context)
            services.AddTransient<IUnitOfWorkAsync, ExampleContext>();

            // repositories
            services.AddTransient<IBehaviorRepository, BehaviorRepository>();

            // application services
            services.AddTransient<IBehaviorAppService, BehaviorAppService>();
        }
    }
}
