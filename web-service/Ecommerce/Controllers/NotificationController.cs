using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Ecommerce.Hubs;
using Ecommerce.Models;
using Ecommerce.Repositories;
using MongoDB.Driver;

[Route("api/[controller]")]
public class NotificationController : ControllerBase
{
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly INotificationRepository _notificationRepository;

    public NotificationController(IHubContext<NotificationHub> hubContext, INotificationRepository notificationRepository)
    {
        _hubContext = hubContext;
        _notificationRepository = notificationRepository;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendNotification([FromBody] string message)
    {
        Console.WriteLine($"SendNotification called with message: {message}");

        var notification = new Notification
        {
            Message = message,
            UserId = null,
            Timestamp = DateTime.UtcNow,
            IsRead = false
        };

        await _notificationRepository.AddNotificationAsync(notification);
        Console.WriteLine("Notification saved to database.");

        await _hubContext.Clients.All.SendAsync("ReceiveNotification", notification);
        Console.WriteLine("Notification sent to all clients.");

        return Ok(new { Message = "Notification sent and saved." });
    }

    [HttpPost("sendToVendor")]
    public async Task<IActionResult> SendNotificationToVendor(string vendorId, [FromBody] string message)
    {
        Console.WriteLine($"SendNotificationToVendor called for vendorId: {vendorId} with message: {message}");
        
        var notification = new Notification
        {
            Message = message,
            UserId = vendorId,
            Timestamp = DateTime.UtcNow,
            IsRead = false
        };

        await _notificationRepository.AddNotificationAsync(notification);
        Console.WriteLine($"Notification saved for vendor {vendorId}.");
        
        // Send the entire notification object, not just the message string
        await _hubContext.Clients.User(vendorId).SendAsync("ReceiveNotification", notification);
        Console.WriteLine($"Notification sent to vendor {vendorId}.");

        return Ok(new { Message = "Notification sent to vendor and saved." });
    }


   

    [HttpGet("getNotificationsForUser")]
    public async Task<IActionResult> GetNotificationsForUser(string userId)
    {
        var notifications = await _notificationRepository.GetNotificationsForUserAsync(userId);

        foreach (var notification in notifications)
        {
            // If it's a broadcast notification and the user hasn't marked it as read
            if (notification.UserId == null && !notification.ReadBy.Contains(userId))
            {
                notification.IsRead = false; // Mark as unread for this user
            }
            else if (notification.UserId == null && notification.ReadBy.Contains(userId))
            {
                notification.IsRead = true; // Mark as read if user has seen it
            }
        }

        return Ok(notifications);
    }



    [HttpGet("getAll")]
    public async Task<IActionResult> GetAllNotifications()
    {
        var notifications = await _notificationRepository.GetAllNotificationsAsync(); // Modify this method according to your repository
        return Ok(notifications);
    }

 


    [HttpPatch("markAsRead/{userid}")]
    public async Task<IActionResult> MarkAsRead(string userid, [FromBody] Notification updatedNotification)
    {
        Console.WriteLine($"MarkAsRead called for Notification ID: {userid}");
        Console.WriteLine($"UserId from request: ,{updatedNotification.Id}");

        var filter = Builders<Notification>.Filter.Eq(n => n.Id, updatedNotification.Id);
        var existingNotification = await _notificationRepository.FindOneAsync(filter);

        // Check if the notification exists
        if (existingNotification == null)
        {
            Console.WriteLine("Notification not found.");
            return NotFound("Notification not found.");
        }

        // Optionally, log the existing notification data
        Console.WriteLine($"Existing Notification: {existingNotification.Id}");
        if (existingNotification.UserId == null) // Check if it's a broadcast notification
        {
            Console.WriteLine($"Processing as a broadcast notification. UserId from request: {userid}");

            // Ensure UserId is not already in the ReadBy list
            var update = Builders<Notification>.Update.AddToSet(n => n.ReadBy, userid); // Use the passed userId
            var result = await _notificationRepository.UpdateOneAsync(filter, update);

            Console.WriteLine($"Update result: ModifiedCount = {result.ModifiedCount}");

            if (result.ModifiedCount > 0)
            {
                Console.WriteLine($"User {userid} marked broadcast notification as read.");
                return Ok();
            }
            else
            {
                Console.WriteLine("Notification not found or already marked as read.");
            }
        }

        else // Unicast messages
        {
            Console.WriteLine("Processing as a unicast notification.");
            var update = Builders<Notification>.Update.Set(n => n.IsRead, updatedNotification.IsRead);
            var result = await _notificationRepository.UpdateOneAsync(filter, update);

            Console.WriteLine($"Update result: ModifiedCount = {result.ModifiedCount}");

            if (result.ModifiedCount > 0)
            {
                Console.WriteLine($"Notification {updatedNotification.Id} marked as read.");
                return Ok();
            }
            else
            {
                Console.WriteLine("Notification not found or already marked as read.");
            }
        }

        return BadRequest("Notification not found or already marked as read.");
    }


    

}


