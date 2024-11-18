using Microsoft.AspNetCore.SignalR;
using StreamPlatform.Models;

namespace StreamPlatform.Hubs;

public class StreamHub: Hub
{
    private static int _connectedUsers = 0;

    public async Task JoinStream()
    {
        _connectedUsers++;
        await Clients.All.SendAsync("UpdateUserCount", _connectedUsers);
    }

    public async Task LeaveStream()
    {
        _connectedUsers--;
        await Clients.All.SendAsync("UpdateUserCount", _connectedUsers);
    }
}
