using IdentityService.Infrastructure.Data;
using IdentityService.Web.Configurations;
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

    if (args.Contains("/seed"))
    {
        Log.Information("Seeding database...");
        await ApplicationDbContextSeed.SeedEssentialsAsync(app.Services);
        Log.Information("Done seeding database. Exiting.");
        return;
    }
}
catch (HostAbortedException ex) 
{
    Log.Fatal(ex.Message);
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}