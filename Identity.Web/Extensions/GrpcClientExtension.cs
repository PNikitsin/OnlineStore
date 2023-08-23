using Identity.Application.Grpc;
using Identity.Application.Grpc.Protos;

namespace Identity.Web.Extensions
{
    public static class GrpcClientExtension
    {
        public static void AddGrpcClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddGrpcClient<GrpcUser.GrpcUserClient>(options =>
            {
                options.Address = new Uri(configuration["GrpcHost"]);
            });

            services.AddScoped<GrpcUserClient>();
        }
    }
}