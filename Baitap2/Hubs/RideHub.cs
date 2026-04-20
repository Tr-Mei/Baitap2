using Microsoft.AspNetCore.SignalR;

namespace Baitap2.Hubs
{
    public class RideHub : Hub
    {
        
        public async Task ThamGiaChuyen(int chuyenId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chuyenId.ToString());
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.GetHttpContext().Session.GetInt32("UserId");

            if (userId != null)
            {
                // 🔥 dùng để gửi cuốc riêng cho từng tài xế
                await Groups.AddToGroupAsync(Context.ConnectionId, userId.ToString());
            }

            await base.OnConnectedAsync();
        }

        public async Task GuiViTri(int chuyenId, double lat, double lng)
        {
            await Clients.Group(chuyenId.ToString())
                .SendAsync("CapNhatViTri", new
                {
                    chuyenId = chuyenId,
                    vido = lat,
                    kinhDo = lng
                });
        }
    }

}


