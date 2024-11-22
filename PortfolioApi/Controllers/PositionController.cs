using BusinessLayer.CommandHandlers;
using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.QueryHandlers;
using BusinessLayer;

namespace PortfolioApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PositionController : ControllerBase
    {

        private ICommandHandler<Position> _positionHandler;


        private readonly ILogger<PositionController> _logger;

        public PositionController(ICommandHandler<Position> positionHandler)
        {
            _positionHandler = positionHandler;
        }

        [HttpPost]
        public IEnumerable<Position> CalibratePortfolio(List<Position> positions)
        {
            var calibratedPositions = _positionHandler.Handle(positions);

            return calibratedPositions;
        }

        [HttpGet]
        public IEnumerable<Position> GetAllCurrentPositions(string query)
        {
            var queryHandler = new GetAllPositionsQueryHandler();
            var allPositions = queryHandler.Handle(query);

            return allPositions;
        }
    }
}
