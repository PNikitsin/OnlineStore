using IdentityService.Infrastructure.Data;
using IdentityService.Web;
using Serilog;

Log.Information("Starting up");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, configuration) =>
    {
        configuration.ReadFrom.Configuration(context.Configuration);
    });

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
catch (Exception ex) when (ex.GetType().Name is not "StopTheHostException" && ex.GetType().Name is not "HostAbortedException")
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}