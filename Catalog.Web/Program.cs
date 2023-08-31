using Catalog.Web.Configurations;
using Serilog;

SerilogConfiguration.ConfigureLogging();

Log.Information("Starting up");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();

    var app = builder
        .ConfigureServices()
        .ConfigurePipeline();

    app.Run();
}
catch (HostAbortedException ex)
{
    Log.Fatal(ex.Message);
}
catch (Exception ex)
{
    Log.Fatal(ex.Message);
    Log.Information("Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}