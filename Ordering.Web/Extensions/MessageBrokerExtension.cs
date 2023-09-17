using MassTransit;
using Ordering.Web.MessageBroker;

namespace Ordering.Web.Extensions
{
    public static class MessageBrokerExtension
    {
        public static void AddMessageBroker(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(config =>
            {
                config.SetEndpointNameFormatter(
                    new KebabCaseEndpointNameFormatter(prefix: "user", includeNamespace: false));

                config.AddConsumers(typeof(UserConsumer).Assembly);

                config.UsingRabbitMq((context, config) =>
                {
                    config.Host(configuration["MessageBroker:Host"], "/");
                    config.ConfigureEndpoints(context);
                });
            });
        }
    }
}