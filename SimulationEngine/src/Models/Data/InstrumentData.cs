using ScottPlot;    // For data generation

namespace SimulationEngine.src.Models.Data
{
    /// <summary>
    /// Represents a collection of market data. Generation is handled here.
    /// </summary>
    public class InstrumentData
    {
        
        public string Symbol {get;set;}
        public List<InstrumentDataTick> Data {get;set;}
        private decimal _startPrice;
        private Random _random = new Random();
        private int _curIndex = 0;

        /// <summary>
        /// Create a new object with all fields initialized.
        /// </summary>
        public InstrumentData(string symbol)
        {
            Symbol = symbol;
            _startPrice = GenerateStartPrice();
            Data = this.GenerateDate(12*60);
        }

        /// <summary>
        /// Helper method to generate a starting price for the data's random walk.
        /// </summary>
        /// <returns>A starting price for the instrument</returns>
        private decimal GenerateStartPrice()
        {
            return (decimal)_random.NextDouble() * 190 + 10;
        }

        /// <summary>
        /// Gets a next data point.
        /// </summary>
        public InstrumentDataTick GetNext(){
            return Data[_curIndex++];
        }

        /// <summary>
        /// Gets the data to the current index;
        /// </summary>
        public List<InstrumentDataTick> GetCurrentData(){
            return Data.GetRange(0, _curIndex);
        }

        /// <summary>
        /// Create new data from a random walk.
        /// </summary>
        private List<InstrumentDataTick> GenerateDate(int count)
        {
            double price = ((double)_startPrice);
            double mult = 0.1;
            double[] prices = DataGen.RandomWalk(_random, count, mult, price);

            // This is a dirty hack to prevent negative prices.
            // If a price is negative, add the difference to the last price instead to keep it positive.
            for (int i = 0; i < prices.Length; i++)
            {
                if (prices[i] < 0)
                {
                    double difference = prices[i] - prices[i - 1];
                    prices[i] = prices[i - 1] + difference;
                }
            }

            // Compile the data to return
            List<InstrumentDataTick> data = new List<InstrumentDataTick>();
            for (int i = 0; i < prices.Length; i++)
            {
                data.Add(new InstrumentDataTick()
                {
                    Symbol = Symbol,
                    Price = (decimal)prices[i],
                    Quantity = _random.Next(100, 1000),
                    Time = DateTime.Now.AddSeconds(i)
                });
            }

            return data;
        }
    }
}