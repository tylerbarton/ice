namespace SimulationEngine.src.Models.Engines
{
    /// <summary>
    /// Factory class for creating market engines
    /// </summary>
    public class MarketEngineFactory
    {
        /// <summary>
        /// Creates a new market engine.
        /// </summary>
        /// <param name="symbol">The source the data supposedly comes from.</param>
        public static IMarketEngine CreateMarketEngine(string source)
        {
            return new StdMarketEngine(source);
        }
    }
}
