using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http.Json;
using Newtonsoft.Json;
using WebClient.Model;
using System.Text;
using System.Net.Http.Headers;

namespace WebClient.ViewModel
{
    public class PositionService : IPositionService
    {

        private readonly HttpClient httpClient;
        private ITokenService TokenService;
        public PositionService(HttpClient _httpClient, ITokenService _tokenService)
        {
            httpClient = _httpClient;
            TokenService = _tokenService;
        }

        public IEnumerable<Position>? position { get; set; }
        public async Task<IEnumerable<Position>> GetAllPositionsAsync()
        {
            IEnumerable<Position>? position = null;
            var request = new HttpRequestMessage(HttpMethod.Get,"http://localhost/PortfolioApi/Position");

            try
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenService.AccessToken);
                
                var response = await httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    position = JsonConvert.DeserializeObject<IEnumerable<Position>>(await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return position;
        }

        async Task<IEnumerable<Position>> IPositionService.CalibratePortfolioAsync(List<Position> positions)
        {
            IEnumerable<Position>? position = null;

            try
            {
            
                var positionList = new List<Position>()
                {
                    new Position()
                    {
                        Symbol = "AAPL",
                        Quantity = 50,
                        Percentage = new decimal(22),
                        Trade = "Buy"
                    },
                    new Position()
                    {
                        Symbol = "THD",
                        Quantity = 200,
                        Percentage = new decimal(38),
                        Trade = "Buy"
                    },
                    new Position()
                    {
                        Symbol = "CYBR",
                        Quantity = 150,
                        Percentage = new decimal(25),
                        Trade = "Buy"
                    },
                    new Position()
                    {
                        Symbol = "ABB",
                        Quantity = 900,
                        Percentage = new decimal(15),
                        Trade = "Buy"
                    }

                };

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenService.AccessToken);
                var bodyContent = JsonConvert.SerializeObject(positionList);
                var uri = new Uri("http://localhost/PortfolioApi/Position");
                var response = await httpClient.PostAsync(uri, new StringContent(bodyContent, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    position = JsonConvert.DeserializeObject<IEnumerable<Position>>(await response.Content.ReadAsStringAsync());
                }
            }
            catch(Exception ex)
            {
                throw;
            }
            return position;
        }

        async Task<IEnumerable<Position>> IPositionService.GetDesiredPositionAsync(Position position)
        {
            throw new NotImplementedException();
        }
    }
}
