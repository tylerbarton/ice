namespace SimulationEngine.src.Models.Data
{
    /// <summary>
    /// Represents market data tick for a given instrument.
    /// </summary>
    public class TickerPoint
    {
        public string? Symbol { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime Time { get; set; }
    }
}
