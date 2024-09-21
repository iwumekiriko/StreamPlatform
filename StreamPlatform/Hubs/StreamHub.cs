using Microsoft.AspNetCore.SignalR;

namespace StreamPlatform.Hubs;

public class StreamHub : Hub
{
    public async Task JoinStream(string streamId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, streamId);
    }

    public async Task LeaveStream(string streamId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, streamId);
    }

    public async Task SendStreamMessage(string streamId, string message)
    {
        await Clients.Group(streamId).SendAsync("ReceiveMessage", message);
    }
}