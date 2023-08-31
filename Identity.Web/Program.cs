using Identity.Infrastructure.Data;
using Identity.Web;
using Serilog;
using Identity.Web.Configurations;
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

    if (args.Contains("/seed"))
    {
        Log.Information("Seeding database...");
        await ApplicationDbContextSeed.SeedEssentialsAsync(app.Services);
        Log.Information("Done seeding database. Exiting.");
        return;
    }

    app.Run();
}
catch (Exception ex) when (ex.GetType().Name is not "StopTheHostException" && ex.GetType().Name is not "HostAbortedException")
{
    Log.Fatal(ex, "Unhandled exception");
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