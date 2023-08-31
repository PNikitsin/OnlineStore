using GreenPipes;
using MassTransit;
using Ordering.Web.MessageBroker;

namespace Ordering.Web.Extensions
{
    public static class MessageBrokerExtension
    {
        public static void AddMessageBroker(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(options =>
            {
                options.AddConsumer<UserConsumer>();
                options.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(RabbitConfiguration =>
                {
                    RabbitConfiguration.UseHealthCheck(provider);

                    RabbitConfiguration.Host(new Uri(configuration["MessageBroker:Host"]), host =>
                    {
                        host.Username(configuration["MessageBroker:Username"]);
                        host.Password(configuration["MessageBroker:Password"]);
                    });

                    RabbitConfiguration.ReceiveEndpoint(configuration["MessageBroker:Endpoint"], endpoint =>
                    {
                        endpoint.PrefetchCount = 16;
                        endpoint.UseMessageRetry(r => r.Interval(2, 100));
                        endpoint.ConfigureConsumer<UserConsumer>(provider);
                    });
                }));
            });

            services.AddMassTransitHostedService();
        }
    }
}