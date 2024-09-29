using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly Cloudinary _cloudinary;

        public ImageController(IConfiguration config)
        {
            var account = new Account(
                config["Cloudinary:CloudName"],
                config["Cloudinary:ApiKey"],
                config["Cloudinary:ApiSecret"]
            );

            _cloudinary = new Cloudinary(account);
        }

        // POST endpoint for image upload
        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = System.IO.Path.GetExtension(file.FileName);
            if (!validExtensions.Contains(extension.ToLower()))
            {
                return BadRequest("Invalid file type. Only image files are allowed.");
            }

            using (var stream = file.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    PublicId = Guid.NewGuid().ToString(), // Generate a unique ID for the image
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return Ok(new { publicId = uploadResult.PublicId, url = uploadResult.SecureUri });
                }
                else
                {
                    return StatusCode((int)uploadResult.StatusCode, uploadResult.Error.Message);
                }
            }
        }

        // DELETE endpoint for image deletion
        [HttpDelete("delete/{publicId}")]
        public async Task<IActionResult> DeleteImage(string publicId)
        {
            if (string.IsNullOrEmpty(publicId))
            {
                return BadRequest("PublicId cannot be null or empty.");
            }

            var deletionParams = new DeletionParams(publicId);
            var deletionResult = await _cloudinary.DestroyAsync(deletionParams);

            if (deletionResult.Result == "ok")
            {
                return Ok(new { message = "Image deleted successfully" });
            }
            else
            {
                return StatusCode(500, $"Failed to delete image: {deletionResult.Error?.Message}");
            }
        }
    }
}
