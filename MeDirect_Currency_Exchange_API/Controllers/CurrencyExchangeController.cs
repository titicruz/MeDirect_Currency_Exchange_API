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
        [HttpGet("GetRate")]
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

        [HttpPost("ExchangeTrade")]
        public async Task<IActionResult> CreateTradeAsync([FromBody] TradeRequest tradeRequest) {
            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            try {
                var trade = await _exchangeService.CreateTradeAsync(tradeRequest);

                if(trade == null) {
                    return BadRequest("Trade creation failed.");
                }
                TradeOutput tradeOutput = new TradeOutput() {
                    ID = trade.ID,
                    ID_Client = trade.ID_Client,
                    FromCurrency = trade.FromCurrency,
                    ToCurrency = trade.ToCurrency,
                    Amount = trade.Amount,
                    ExchangedAmount = trade.ExchangedAmount,
                    Rate = trade.Rate
                };
                return Ok(tradeOutput);
            }
            catch(ApiException ex) {
                // Handle known API exceptions
                return StatusCode(500, ex.ErrorResponse);
            }
            catch(Exception ex) {
                // Handle other unexpected exceptions
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }
    }
}
