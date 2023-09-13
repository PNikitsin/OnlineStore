using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace OnlineStore.Chat.Hubs
{
    [Authorize(Roles = "Administrator")]
    public class ChatHub : Hub
    {
        public async Task Send(string message, string userName)
        {
            await Clients.All.SendAsync("Receive", message, userName);
        }
    }
}