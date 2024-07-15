using MeDirect_Currency_Exchange_API.Exceptions;
using MeDirect_Currency_Exchange_API.Interfaces;
using MeDirect_Currency_Exchange_API.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace MeDirect_Currency_Exchange_API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyExchangeController : ControllerBase {
        private readonly IExchangeService _exchangeService;

        public CurrencyExchangeController(IExchangeService exchangeService) {
            _exchangeService = exchangeService;
        }
        [HttpGet("rate")]
        public async Task<IActionResult> GetRateAsync([FromQuery] string fromCurrency, [FromQuery] string toCurrency) {
            if(string.IsNullOrEmpty(fromCurrency) || string.IsNullOrEmpty(toCurrency)) {
                return BadRequest("Both 'fromCurrency' and 'toCurrency' query parameters are required.");
            }

            try {
                var rate = await _exchangeService.GetRateAsync(fromCurrency, toCurrency);

                if(rate == null) {
                    return NotFound("Rate not found.");
                }

                return Ok(rate);
            }
            catch(ApiException ex) {
                return StatusCode(500, new { message = ex.Message });
            }
            catch(Exception ex) {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        [HttpPost("trade")]
        public async Task<IActionResult> CreateTradeAsync([FromBody] TradeRequest tradeRequest) {
            if(tradeRequest == null) {
                return BadRequest("Trade request cannot be null.");
            }

            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            try {
                var trade = await _exchangeService.CreateTradeAsync(tradeRequest);

                if(trade == null) {
                    return BadRequest("Trade creation failed.");
                }
                return Ok(trade.ExchangedAmount);
            }
            catch(ApiException ex) {
                // Handle known API exceptions
                return StatusCode(ex.ErrorCode, new { message = $"code:{ex.ErrorCode} message:{ex.Message}"});
            }
            catch(Exception ex) {
                // Handle other unexpected exceptions
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }
    }
}
