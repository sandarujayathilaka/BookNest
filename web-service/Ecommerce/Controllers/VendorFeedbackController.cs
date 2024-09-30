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




    // Customer: Get all feedback for a vendor
    [HttpGet("{vendorUserId}")]
    public async Task<IActionResult> GetFeedbackByVendor(string vendorUserId)
    {
        var feedbacks = await _vendorfeedbackService.GetFeedbackByVendor(vendorUserId);
        if (feedbacks == null || feedbacks.Count == 0)
        {
            return NotFound("No feedback found for this vendor.");
        }

        return Ok(feedbacks);
    }

    // Customer: Update feedback comment
    [HttpPut("{feedbackId}")]
    [Authorize(Roles = Roles.Customer)]
    public async Task<IActionResult> UpdateFeedbackComment(string feedbackId, [FromBody] string newComment)
    {
        var customerUserId = User.FindFirst("UserId")?.Value;

        if (customerUserId == null)
        {
            return Unauthorized("User is not authenticated.");
        }

        var feedback = await _vendorFeedbackRepository.GetFeedbackById(feedbackId);
        if (feedback == null || feedback.CustomerUserId != customerUserId)
        {
            return NotFound("Feedback not found or user not authorized to update this feedback.");
        }

        await _vendorfeedbackService.UpdateComment(feedbackId, newComment);
        return Ok("Feedback comment updated successfully.");
    }

    // Customer: Delete feedback
    [HttpDelete("{feedbackId}")]
    [Authorize(Roles = Roles.Customer)]
    public async Task<IActionResult> DeleteFeedback(string feedbackId)
    {
        var customerUserId = User.FindFirst("UserId")?.Value;

        if (customerUserId == null)
        {
            return Unauthorized("User is not authenticated.");
        }

        var feedback = await _vendorFeedbackRepository.GetFeedbackById(feedbackId);
        if (feedback == null || feedback.CustomerUserId != customerUserId)
        {
            return NotFound("Feedback not found or user not authorized to delete this feedback.");
        }

        await _vendorFeedbackRepository.DeleteFeedback(feedbackId);
        return Ok("Feedback deleted successfully.");
    }




}
