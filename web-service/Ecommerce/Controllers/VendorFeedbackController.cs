using Ecommerce.Dto;
using Ecommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.Repositories;
using Ecommerce.Services;
using System.Security.Claims;


[ApiController]
[Route("api/[controller]")]
public class VendorFeedbackController : ControllerBase
{
    private readonly IVendorFeedbackRepository _vendorFeedbackRepository;
    private readonly VendorFeedbackService _vendorfeedbackService;
    private readonly IUserRepository _userService;

    public VendorFeedbackController(IVendorFeedbackRepository vendorFeedbackRepository, VendorFeedbackService vendorfeedbackService, IUserRepository userService)
    {
        _vendorFeedbackRepository = vendorFeedbackRepository;
        _vendorfeedbackService = vendorfeedbackService;
        _userService = userService;
    }


    //Customer : Create a new feedback
    [HttpPost("addfeedback")]
    [Authorize(Roles = Roles.Customer)]
    public async Task<IActionResult> CreateFeedback([FromBody] VendorFeedbackDto vendorFeedbackDto)
    {
        // Get the CustomerUserId from the token
        var customerUserId = User.FindFirst("UserId")?.Value;

        if (customerUserId == null)
        {
            return Unauthorized("User is not authenticated.");
        }


        //// Get the email from the token 
        //var customerEmail = User.FindFirst(ClaimTypes.Name)?.Value;

        //if (string.IsNullOrEmpty(customerEmail))
        //{
        //    return BadRequest("User email is not available.");
        //}

        // Get the customer details using the email
        var customer = await _userService.GetUserById(customerUserId);

        if (customer == null)
        {
            return NotFound("Customer not found.");
        }

        var customerName = customer.FullName;



        var vendorFeedback = new VendorFeedback
        {
            VendorUserId = vendorFeedbackDto.VendorUserId,
            CustomerUserId = customerUserId,
            CustomerName = customerName,
            Comment = vendorFeedbackDto.Comment,
            Rating = vendorFeedbackDto.Rating,
            CreatedDate = DateTime.UtcNow,
        };

        await _vendorfeedbackService.AddFeedback(vendorFeedback);
        return Ok("Feedback submitted successfully.");
    }





    //Get all feedback for a vendor
    [HttpGet("getallfeedbacks/{vendorUserId}")]
    public async Task<IActionResult> GetFeedbackByVendor(string vendorUserId)
    {
        var feedbacks = await _vendorfeedbackService.GetFeedbackByVendor(vendorUserId);
        if (feedbacks == null || feedbacks.Count == 0)
        {
            return NotFound("No feedback found for this vendor.");
        }

        return Ok(feedbacks);
    }




    // Customer: Update full feedback
    [HttpPut("updatefeedback/{feedbackId}")]
    [Authorize(Roles = Roles.Customer)]
    public async Task<IActionResult> UpdateFeedback(string feedbackId, [FromBody] VendorFeedbackDto updatedFeedbackDto)
    {
        var customerUserId = User.FindFirst("UserId")?.Value;

        if (customerUserId == null)
        {
            return Unauthorized("User is not authenticated.");
        }

        // Fetch existing feedback
        var existingFeedback = await _vendorFeedbackRepository.GetFeedbackById(feedbackId);
        if (existingFeedback == null || existingFeedback.CustomerUserId != customerUserId)
        {
            return NotFound("Feedback not found or user not authorized to update this feedback.");
        }

        // Update the feedback properties
        existingFeedback.Comment = updatedFeedbackDto.Comment;
        existingFeedback.Rating = updatedFeedbackDto.Rating;
        existingFeedback.CreatedDate = DateTime.UtcNow; 

        // Call service to update feedback in repository
        await _vendorfeedbackService.UpdateFeedback(existingFeedback);


        return Ok("Feedback updated successfully.");
    }






    // Customer: Delete feedback
    [HttpDelete("removefeedback/{feedbackId}")]
    [Authorize(Roles = Roles.Customer)]
    public async Task<IActionResult> DeleteFeedback(string feedbackId)
    {
        var customerUserId = User.FindFirst("UserId")?.Value;

        if (customerUserId == null)
        {
            return Unauthorized("User is not authenticated.");
        }

        // Fetch the feedback to get the vendor's ID
        var feedback = await _vendorFeedbackRepository.GetFeedbackById(feedbackId);
        if (feedback == null || feedback.CustomerUserId != customerUserId)
        {
            return NotFound("Feedback not found or user not authorized to delete this feedback.");
        }

        // Delete the feedback and recalculate the vendor's average rating
        await _vendorfeedbackService.DeleteFeedbackAndUpdateRating(feedbackId, feedback.VendorUserId);

        return Ok("Feedback deleted and vendor's average rating updated.");
    }






}
