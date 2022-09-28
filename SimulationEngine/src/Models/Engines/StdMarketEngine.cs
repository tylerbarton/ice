using SimulationEngine.src.Models.Data;

namespace SimulationEngine.src.Models.Engines
{
    /// <summary>
    /// Represents a standard market 
    /// </summary>
    public class StdMarketEngine : IMarketEngine
    {
        /// <summary>
        /// The amount of time in milliseconds to wait between simulated data generation.
        /// </summary>
        private const int UPDATE_INTERVAL_MS = 500;

        private static bool _isRunning = false;
        public bool IsRunning => _isRunning;
        private readonly Dictionary<string, TickerData> _marketData = new();
        public Dictionary<string, TickerPoint> LastData { get; private set; } = new();
        public string Source;
        string IMarketEngine.Source => Source;
        internal delegate void OnDataGeneratedHandler(object sender, Dictionary<string, TickerPoint> data);
        internal event OnDataGeneratedHandler OnDataGenerated;

        public StdMarketEngine(string source)
        {
            this.Source = source;
        }

        /// <summary>
        /// Gets all market data up to the current index;
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public List<TickerPoint> GetMarketData(string symbol)
        {
            if (_marketData.ContainsKey(symbol))
            {
                return _marketData[symbol].GetCurrentData();
            }
            return new List<TickerPoint>();
        }

        /// <summary>
        /// Adds a new ticker to the market.
        /// </summary>
        public void AddSymbol(string symbol)
        {
            if (!_marketData.ContainsKey(symbol))
            {
                _marketData.Add(symbol, new TickerData(symbol));
            }
        }

        /// <summary>
        /// Create new data from a random walk.
        /// </summary>
        /// <param name="symbol">The symbol of the stock to get</param>
        public TickerData GenerateMarketData(string symbol)
        {
            // Get the next point from the symbol
            _marketData.TryGetValue(symbol, out TickerData data);
            if (data == null)
            {
                data = new TickerData(symbol);
                _marketData.Add(symbol, data);
            }

            return data;
        }

        public void Start()
        {
            _isRunning = true;
            // Create a new thread to generate market data.
            Task.Run(() =>
            {
                while (IsRunning)
                {
                    // Generate a new market data tick for each instrument.
                    foreach (var tickerSymbol in _marketData.Keys)
                    {
                        // This shouldn't be a shared resource, so no need to lock it.
                        var data = GenerateMarketData(tickerSymbol);
                        LastData[tickerSymbol] = data.GetNext();
                        
                    }
                    OnDataGenerated?.Invoke(this, LastData);

                    // Wait for the next update.
                    Task.Delay(UPDATE_INTERVAL_MS).Wait();
                }
            });
        }

        /// <summary>
        /// Stops the market engine.
        /// </summary>
        public void Stop()
        {
            _isRunning = false;
        }

        /// <summary>
        /// Subscribes to the OnDataGenerated event.
        /// </summary>
        /// <param name="callback">The function to call on the new data.</param>
        public void Subscribe(Action<Dictionary<string, TickerPoint>> callback)
        {
            OnDataGenerated += (sender, data) => callback(LastData);
        }

        /// <summary>
        /// Removes the OnDataGenerated event.
        /// </summary>
        public void Unsubscribe()
        {
            OnDataGenerated = null;
        }
    }
}
