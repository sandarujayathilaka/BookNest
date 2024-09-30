using Microsoft.AspNetCore.SignalR;

namespace Ecommerce.Hubs
{
    public class NotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier; // This should now reflect the UserId claim
            var connectionId = Context.ConnectionId;

            Console.WriteLine($"User connected with Identifier: {userId} and ConnectionId: {connectionId}");

            if (Context.User != null)
            {
                foreach (var claim in Context.User.Claims)
                {
                    Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
                }
            }

            await base.OnConnectedAsync();
        }



        public async Task SendNotification(string message)
        {
            await Clients.All.SendAsync("ReceiveNotification", message);
        }

        public async Task SendNotificationToVendor(string vendorId, string message)
        {
            await Clients.User(vendorId).SendAsync("ReceiveNotification", message); // This line is correct
        }
    }
}


