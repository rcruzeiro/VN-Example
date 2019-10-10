using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using VN.Example.Infrastructure.Database.Couchbase;
using VN.Example.Infrastructure.Database.MSSQL;
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
            services.AddScoped<IDataSource>(provider =>
                new DefaultDataSource(configuration, "ExampleDb"));

            // unit of work (db context)
            services.AddTransient<IUnitOfWorkAsync, ExampleContext>();

            // couchbase options
            services.AddScoped(provider =>
                new CouchbaseOptions(configuration.GetValue<string>("CouchbaseOptions:Host"),
                                     configuration.GetValue<string>("CouchbaseOptions:Bucket"),
                                     configuration.GetValue<string>("CouchbaseOptions:Port"),
                                     configuration.GetValue<string>("CouchbaseOptions:Username"),
                                     configuration.GetValue<string>("CouchbaseOptions:Password")));

            // repositories
            services.AddTransient<Database.MSSQL.Repositories.BehaviorRepository>();
            services.AddTransient<Database.Couchbase.Repositories.BehaviorRepository>();

            services.AddTransient<BehaviorRepositoryResolver>(provider => key =>
            {
                return key switch
                {
                    "MSSQL" => provider.GetService<Database.MSSQL.Repositories.BehaviorRepository>(),
                    "Couch" => provider.GetService<Database.Couchbase.Repositories.BehaviorRepository>(),
                    _ => throw new KeyNotFoundException(),
                };
            });

            // application services
            services.AddTransient<IBehaviorAppService, BehaviorAppService>();
        }
    }
}
