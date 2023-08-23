using MassTransit;

namespace Identity.Web.Extensions
{
    public static class MessageBrokerExtension
    {
        public static void AddMessageBroker(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(options =>
            {
                options.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(RabbitConfiguration =>
                {
                    RabbitConfiguration.Host(new Uri(configuration["MessageBroker:Host"]), host =>
                    {
                        host.Username(configuration["MessageBroker:Username"]);
                        host.Password(configuration["MessageBroker:Password"]);
                    });
                }));
            });

            services.AddMassTransitHostedService();
        }
    }
}