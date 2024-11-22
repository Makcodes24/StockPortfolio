using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.QueryHandlers
{
    public class GetAllPositionsQueryHandler //: IQueryHandler<Position>
    {
        public List<Position> Handle(string query)
        {

                return new List<Position>()
                {
                    new Position()
                    {
                        Symbol = "AAPL",
                        Quantity = 100,
                        Percentage = new decimal(20),
                        Trade = "Buy"
                    },
                    new Position()
                    {
                        Symbol = "THD",
                        Quantity = 200,
                        Percentage = new decimal(20),
                        Trade = "Buy"
                    },
                    new Position()
                    {
                        Symbol = "CYBR",
                        Quantity = 150,
                        Percentage = new decimal(20),
                        Trade = "Buy"
                    },
                    new Position()
                    {
                        Symbol = "ABB",
                        Quantity = 100,
                        Percentage = new decimal(20),
                        Trade = "Buy"
                    }

                };

        }
    }
}
