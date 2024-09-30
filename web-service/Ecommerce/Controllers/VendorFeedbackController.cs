using Ecommerce.Dto;
using Ecommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.Repositories;
using Ecommerce.Services;


[ApiController]
[Route("api/[controller]")]
public class VendorFeedbackController : ControllerBase
{
    private readonly IVendorFeedbackRepository _vendorFeedbackRepository;
    private readonly VendorFeedbackService _vendorfeedbackService;

    public VendorFeedbackController(IVendorFeedbackRepository vendorFeedbackRepository, VendorFeedbackService vendorfeedbackService)
    {
        _vendorFeedbackRepository = vendorFeedbackRepository;
        _vendorfeedbackService = vendorfeedbackService;
    }


    //Customer : Create a new feedback
    [HttpPost]
    [Authorize(Roles = Roles.Customer)]
    public async Task<IActionResult> CreateFeedback([FromBody] VendorFeedbackDto vendorFeedbackDto)
    {
        // Get the CustomerUserId from the logged-in user's claims
        var customerUserId = User.FindFirst("UserId")?.Value; 

        if (customerUserId == null)
        {
            return Unauthorized("User is not authenticated.");
        }

        var vendorFeedback = new VendorFeedback
        {
            VendorUserId = vendorFeedbackDto.VendorUserId,
            CustomerUserId = customerUserId,
            Comment = vendorFeedbackDto.Comment,
            Rating = vendorFeedbackDto.Rating,
            CreatedDate = DateTime.UtcNow,

        };

        await _vendorfeedbackService.AddFeedback(vendorFeedback);
        return Ok("Feedback submitted successfully.");
    }
}
