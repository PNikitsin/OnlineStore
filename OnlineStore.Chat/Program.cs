using OnlineStore.Chat.Extensions;
using OnlineStore.Chat.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAccessToken(builder.Configuration);
builder.Services.AddSignalR();
builder.Services.AddCorsPolicy();

var app = builder.Build();

app.UseCors("CORSPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<ChatHub>("/chat");

app.Run();