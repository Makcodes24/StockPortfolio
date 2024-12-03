using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace MarketData.Controllers
{
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class MarketDataController : ControllerBase
    {
        private readonly ILogger<MarketDataController> _logger;

        public MarketDataController(ILogger<MarketDataController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{symbol}")]
        public int Get(string symbol)
        {
            var price = 0;
            switch (symbol)
            {
                case "AAPL":
                    price = 145;
                    break;
                case "THD":
                    price = 30;
                    break;
                case "CYBR":
                    price = 45;
                    break;
                case "ABB":
                    price = 175;
                    break;
                default:
                    return price;
            }

            return price;
        }
    }
}
