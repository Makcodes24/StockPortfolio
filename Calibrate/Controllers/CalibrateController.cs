using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace Calibrate.Controllers
{
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CalibrateController : ControllerBase
    {
        private readonly ILogger<CalibrateController> _logger;

        public CalibrateController(ILogger<CalibrateController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{percentage}/{stockPrice}")]
        public decimal Get(decimal percentage, int stockPrice)
        {
            var portfolioValue = 10000; //$ get total asset value

            var availableDollar = ((decimal)portfolioValue / 100) * percentage;

            var desiredQuantity = GetQuantityByDollarAmount(availableDollar, stockPrice);

            return desiredQuantity;
        }

        private decimal GetQuantityByDollarAmount(decimal amount, int stockPrice)
        {
            var desiredQuantity = amount / stockPrice;
            return desiredQuantity;
        }
    }
}
