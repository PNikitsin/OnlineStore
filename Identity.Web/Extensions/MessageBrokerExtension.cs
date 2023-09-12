using MassTransit;

namespace Identity.Web.Extensions
{
    public static class MessageBrokerExtension
    {
        public static void AddMessageBroker(this IServiceCollection services)
        {
            services.AddMassTransit(config =>
            {
                config.UsingRabbitMq();
            });

            services.AddMassTransitHostedService();
        }
    }
}