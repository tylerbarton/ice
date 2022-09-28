using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimulationEngine.src.Models.Engines;
using SimulationEngine.src.Models.Data;

namespace SimulationEngine.src.Models.Engines.Tests
{
    [TestClass()]
    public class StdMarketEngineTests
    {
        [TestMethod()]
        public void UnsubscribeTest()
        {
            int count = 0;
            var engine = new StdMarketEngine("NYSE");
            Action<Dictionary<string, TickerPoint>> callback = (data) => { count++;  };
            engine.Subscribe(callback);
            engine.Start();
            System.Threading.Thread.Sleep(1000);
            engine.Stop();
            Assert.IsTrue(count > 0, "Count is unchanged after subscribe.");

            // Reset to compare with Unsubscribe
            engine.Unsubscribe(callback);
            count = 0;
            engine.Start();
            System.Threading.Thread.Sleep(1000);
            engine.Stop();
            Assert.IsTrue(count == 0, "Count changed after unsubscribe.");
        }

        [TestMethod()]
        public void SubscribeTest()
        {
            int count = 0;
            var engine = new StdMarketEngine("NYSE");
            Action<Dictionary<string, TickerPoint>> callback = (data) => count++;
            engine.Subscribe(callback);

            engine.Start();
            System.Threading.Thread.Sleep(1000);
            engine.Stop();
            Assert.IsTrue(count > 0, "Count is unchanged after subscribe.");
        }

        [TestMethod()]
        public void StopTest()
        {
            // Start a new engine
            var engine = new StdMarketEngine("NYSE");
            engine.AddSymbol("AAPL");
            engine.Start();
            Assert.IsTrue(engine.IsRunning);
            int count = engine.GetMarketData("AAPL").Count;

            // Stop the engine
            engine.Stop();
            Assert.IsFalse(engine.IsRunning);
            Assert.AreEqual(count, engine.GetMarketData("AAPL").Count);
        }
    }
}