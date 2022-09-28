using ScottPlot;    // For data generation

namespace SimulationEngine.src.Models.Data
{
    /// <summary>
    /// Represents market data tick for a given instrument.
    /// </summary>
    public class TickerData
    {
        public string Symbol {get;set;}
        public List<TickerPoint> Data {get;set;}
        private decimal _startPrice;
        private Random _random = new Random();
        private int _curIndex = 0;

        /// <summary>
        /// Create a new object with all fields initialized.
        /// </summary>
        public TickerData(string symbol)
        {
            Symbol = symbol;
            _startPrice = (decimal)_random.NextDouble() * 190 + 10;
            Data = this.GenerateDate(12*60);
        }

        /// <summary>
        /// Gets a next data point.
        /// </summary>
        public TickerPoint GetNext(){
            return Data[_curIndex++];
        }

        /// <summary>
        /// Gets the data to the current index;
        /// </summary>
        public List<TickerPoint> GetCurrentData(){
            return Data.GetRange(0, _curIndex);
        }

        /// <summary>
        /// Create new data from a random walk.
        /// </summary>
        private List<TickerPoint> GenerateDate(int count)
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

            List<TickerPoint> data = new List<TickerPoint>();
            for (int i = 0; i < prices.Length; i++)
            {
                data.Add(new TickerPoint()
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