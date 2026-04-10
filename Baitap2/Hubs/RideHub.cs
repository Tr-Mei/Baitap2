using Microsoft.AspNetCore.SignalR;

namespace Baitap2.Hubs
{
    public class RideHub : Hub
    {
        public async Task ThamGiaChuyen(string Id)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, Id);
        }

        public async Task GuiViTri(string Id, double Vido, double KinhDo)
        {
            await Clients.Group(Id)
                .SendAsync("CapNhatViTri", Vido, KinhDo);
        }
    }
}