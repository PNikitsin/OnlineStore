using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Net;

namespace Ordering.Web.Extensions
{
    public static class GrpcServerExtension 
    {
        public static WebApplicationBuilder AddGrpcService(this WebApplicationBuilder builder)
        {
            builder.Services.AddGrpc();

            builder.WebHost.ConfigureKestrel(options =>
            {
                options.Listen(IPAddress.Any, 80, options =>
                {
                    options.Protocols = HttpProtocols.Http1;
                });
                options.Listen(IPAddress.Any, 5005, options =>
                {
                    options.Protocols = HttpProtocols.Http2;

                });
            });

            return builder;
        }
    }
}