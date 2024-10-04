using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Ecommerce.Services
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            Console.WriteLine("GetUserId called");
            // Extract the vendorId or userId from the connection's Claims (or other sources)
           

            var userId =  connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //var userId1 = "GENPB33224";
           
            //var userId3 = connection.User?.FindFirst(ClaimTypes.Name)?.Value; // If you have "Email" in `ClaimTypes.Name`
            //var userId4 = connection.User?.FindFirst("VendorId")?.Value;


            Console.WriteLine($"Extracted UserId: {userId} ");
            return userId;
        }
    }
}
