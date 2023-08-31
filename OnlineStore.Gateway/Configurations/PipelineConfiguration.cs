using Ocelot.Middleware;

namespace OnlineStore.Gateway.Configurations
{
    public static class PipelineConfiguration
    {
        public static WebApplication ConfigurePipeline(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.UseOcelot();

            return app;
        }
    }
}