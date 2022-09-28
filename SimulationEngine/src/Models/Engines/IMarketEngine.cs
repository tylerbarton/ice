using SimulationEngine.src.Models.Data;

namespace SimulationEngine.src.Models.Engines
{
    /// <summary>
    /// Represents a market data generator.
    /// </summary>
    public interface IMarketEngine
    {
        /// <summary>
        /// The source of the market data.
        /// </summary>
        string Source { get; }

        /// <summary>
        /// Starts the generation of market data from this engine.
        /// </summary>
        void Start();


        /// <summary>
        /// Halts the generation of market data from this engine.
        /// </summary>
        void Stop();

        /// <summary>
        /// Assigns a function to the delegate that is called when new data is available.
        /// </summary>
        void Subscribe(Action<Dictionary<string, TickerPoint>> callback);

        /// <summary>
        /// Remove delegates from this engine.
        /// </summary>
        void Unsubscribe();
    }
}
