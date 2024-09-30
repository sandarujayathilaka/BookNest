using Ecommerce.Dto;
using Ecommerce.Models;
using Ecommerce.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommercePlatform.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockController : ControllerBase
    {
        private readonly StockService _stockService;

        public StockController(StockService stockService)
        {
            _stockService = stockService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStocks()
        {
            var stocks = await _stockService.GetAllStocksAsync();
            return Ok(stocks);
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetStockByProductId(string productId)
        {
            var stock = await _stockService.GetStockByProductIdAsync(productId);
            if (stock == null)
                return NotFound();
            return Ok(stock);
        }

        //[HttpPost]
        //public async Task<IActionResult> AddOrUpdateStock([FromBody] StockDto stockDto, [FromQuery] bool isAdding)
        //{
        //    try
        //    {
        //        var stock = new Stock
        //    {
        //        ProductId = stockDto.ProductId,
        //        Quantity = stockDto.Quantity,
        //        LowStockThreshold = stockDto.LowStockThreshold,
        //        IsLowStockAlert = stockDto.IsLowStockAlert,
        //        VendorId = stockDto.VendorId

        //    };
        //    await _stockService.AddOrUpdateStockAsync(stock,isAdding);
        //    return Ok();
        //    }
        //    catch (Exception ex)
        //    {

        //        return StatusCode(500, new { message = ex.Message });
        //    }
        //}


        [HttpPost("add")]
        public async Task<IActionResult> AddStock([FromBody] StockDto stockDto)
        {
            try
            {
                var stock = new Stock
                {
                    ProductId = stockDto.ProductId,
                    Quantity = stockDto.Quantity,
                    UserId = stockDto.UserId,
                    LowStockThreshold = stockDto.LowStockThreshold,
                    IsLowStockAlert = stockDto.IsLowStockAlert,
                    VendorId = stockDto.VendorId
                };

                await _stockService.AddStockAsync(stock);
                return Ok(new { message = "Stock added successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPatch("update")]
        public async Task<IActionResult> UpdateStock([FromBody] StockDto stockDto, [FromQuery] bool isAdding)
        {
            try
            {
                await _stockService.UpdateStockAsync(stockDto.ProductId, stockDto.Quantity, isAdding);
                return Ok(new { message = "Stock updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPatch("updateLowStockStatus")]
        public async Task<IActionResult> UpdateLowStockAlertStatus([FromBody] StockDto stockDto)
        {
            try
            {
                await _stockService.UpdateLowStockAlertStatusAsync(stockDto.ProductId, stockDto.IsLowStockAlert);
                return Ok(new { message = "Low stock alert status updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveStock(string id)
        {
            try
            {
                await _stockService.RemoveStockAsync(id);
                return Ok(new { message = "Stock removed successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message }); // 409 Conflict
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message }); // 400 Bad Request
            }
        }
    }


}
