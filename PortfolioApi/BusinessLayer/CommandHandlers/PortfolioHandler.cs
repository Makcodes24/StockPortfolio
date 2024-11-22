using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BusinessLayer.CommandHandlers
{
    public class PortfolioHandler : ICommandHandler<Position>
    {
        private Dictionary<string, int> _stocks { get; set; }
        public PortfolioHandler() 
        {
            LoadStocks();
        }

        private void LoadStocks()
        {
            _stocks = new Dictionary<string, int>()
            {
                { "AAPL", 22 },
                { "THD", 38 },
                { "CYBR", 25 },
                { "ABB", 15 }
            };
        }
        public IEnumerable<Position> Handle(List<Position> positions)
        {
            var calibratedList = CalibrateAsync(positions);
            return calibratedList.Result;
        }

        private async Task<IEnumerable<Position>> CalibrateAsync(List<Position> positions)
        {
            var positionsList = new List<Position>();

            foreach (var position in positions)
            {
                var stockPercentage = _stocks[position.Symbol];
                var stockPrice = await GetMarketDataAsync((position.Symbol));

                var desiredQuantity = await GetQuantityByPercentatgeAsync(stockPercentage, stockPrice);
                var currentQuantity = position.Quantity;

                var delta = desiredQuantity - currentQuantity;
                position.Trade = delta > 0 ? $"Buy {delta}" : $"Sell {delta}";

                positionsList.Add(position);

            }
            return positionsList;
        }

    
        public async Task<int> GetMarketDataAsync(string symbol)
        {
            int stockPrice = 0;

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            using (var client = new HttpClient(clientHandler))
            {
                client.BaseAddress = new Uri("https://localhost:7183");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    HttpResponseMessage response = await client.GetAsync($"/MarketData/{symbol}");
                    if (response.IsSuccessStatusCode)
                    {
                        stockPrice = JsonConvert.DeserializeObject<int>(await response.Content.ReadAsStringAsync());
                    }
                }
                catch(Exception ex) 
                {
                    throw;
                }
               

            }
            return stockPrice;
        }


        public async Task<decimal> GetQuantityByPercentatgeAsync(decimal stockPercentage, int stockPrice)
        {
            decimal desiredQuantity = 0;

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            using (var client = new HttpClient(clientHandler))
            {
                client.BaseAddress = new Uri("https://localhost:7037");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    HttpResponseMessage response = await client.GetAsync($"/Calibrate/{stockPercentage}/{stockPrice}");
                    if (response.IsSuccessStatusCode)
                    {
                        desiredQuantity = JsonConvert.DeserializeObject<decimal>(await response.Content.ReadAsStringAsync());
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            return desiredQuantity;
        }
    }
}
