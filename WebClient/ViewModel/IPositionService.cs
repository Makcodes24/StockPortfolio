using WebClient.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebClient.ViewModel
{
    public interface IPositionService
    {
        Task<IEnumerable<Position>> CalibratePortfolioAsync(List<Position> positions);
        Task<IEnumerable<Position>> GetDesiredPositionAsync(Position position);
    }
}