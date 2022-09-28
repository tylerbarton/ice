using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimulationEngine.src.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationEngine.src.Models.Data.Tests
{
    [TestClass()]
    public class TickerDataTests
    {
        [TestMethod()]
        public void TickerDataTest()
        {
            var data = new InstrumentData("AAPL");
            Assert.AreEqual("AAPL", data.Symbol);
            Assert.IsTrue(DateTime.Now.Subtract(data.Data[0].Time).TotalSeconds < 1);
            Assert.AreEqual(12 * 60, data.Data.Count);
        }

        [TestMethod()]
        public void GetNextTest()
        {
            var data = new InstrumentData("AAPL");
            Assert.AreEqual("AAPL", data.Symbol);

            var next = data.GetNext();
            Assert.AreEqual("AAPL", next.Symbol);
            Assert.IsTrue(next.Price > 0);
            Assert.IsTrue(next.Quantity > 0);
        }
    }
}