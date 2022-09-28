using SimulationEngine.src.Models.Data;

namespace SimulationEngine.src.Models.Engines
{
    /// <summary>
    /// Represents a standard market 
    /// </summary>
    public class StdMarketEngine : IMarketEngine
    {
        private static bool _isRunning = false;
        public bool IsRunning => _isRunning;
        private string _source;
        public string Source => _source;
        internal delegate void OnDataGeneratedHandler(object sender, Dictionary<string, InstrumentDataTick> data);
        internal event OnDataGeneratedHandler? OnDataGenerated;

        /// <summary>
        /// The amount of time in milliseconds to wait between simulated data generation.
        /// </summary>
        private const int UPDATE_INTERVAL_MS = 500;

        /// <summary>
        /// Instrument data for each symbol
        /// </summary>
        private readonly Dictionary<string, InstrumentData> _marketData = new();

        /// <summary>
        /// Data that was generated is the most recent iteration.
        /// </summary>
        public Dictionary<string, InstrumentDataTick> LastData { get; private set; } = new();


        /// <summary>
        /// Basic constructor
        /// </summary>
        /// <param name="source">Source identifier</param>
        public StdMarketEngine(string source)
        {
            this._source = source;
        }

        /// <summary>
        /// Gets all market data up to the current index;
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public List<InstrumentDataTick> GetMarketData(string symbol)
        {
            if (_marketData.ContainsKey(symbol))
            {
                return _marketData[symbol].GetCurrentData();
            }
            return new List<InstrumentDataTick>();
        }

        /// <summary>
        /// Adds a new ticker to the market.
        /// </summary>
        public void AddSymbol(string symbol)
        {
            if (!_marketData.ContainsKey(symbol))
            {
                _marketData.Add(symbol, new InstrumentData(symbol));
            }
        }

        /// <summary>
        /// Create new data from a random walk.
        /// </summary>
        /// <param name="symbol">The symbol of the stock to get</param>
        public InstrumentData GenerateMarketData(string symbol)
        {
            // Get the next point from the symbol
            _marketData.TryGetValue(symbol, out InstrumentData data);
            if (data == null)
            {
                data = new InstrumentData(symbol);
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
        public void Subscribe(Action<Dictionary<string, InstrumentDataTick>> callback)
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
