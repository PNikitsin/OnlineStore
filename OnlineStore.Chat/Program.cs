using OnlineStore.Chat.Extensions;
using OnlineStore.Chat.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAccessToken(builder.Configuration);
builder.Services.AddSignalR();

var app = builder.Build();

app.UseDefaultFiles();

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<ChatHub>("/chat");

app.Run();