using System;

namespace Contracts
{
    public class Position : ICommand
    {
        public string Symbol { get; set; }
 
        public int Quantity { get; set; }
 
        public decimal Percentage { get; set; }

        public string Trade { get; set; }
    }
}
