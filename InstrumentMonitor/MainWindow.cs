using ScottPlot;
using System.Configuration;
using InstrumentMonitor.Utils;
using SimulationEngine.src.Models.Engines;
using SimulationEngine.src.Models.Data;

namespace InstrumentMonitor
{
    /// <summary>
    /// The main window for the application.
    /// </summary>
    public partial class FormMain : Form
    {
        /// <summary>
        /// Singleton instance of the main form.
        /// Used in the AddSymbol form to update the watchlist.
        /// </summary>
        public static FormMain Instance { get; private set; }

        private static List<IMarketEngine> marketEngines = new();

        public FormMain()
        {
            Instance = this;
            InitializeComponent();
            this.FormClosed += new FormClosedEventHandler(MainWindow_FormClosed);
        }

        /// <summary>
        /// Displays creation information
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Created by: Tyler Barton\nhttps://github.com/tylerbarton", "About");
        }

        /// <summary>
        /// Add Ticker Option
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addTickerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddSymbol addSymbol = new AddSymbol();
            addSymbol.ShowDialog();
        }

        /// <summary>
        /// Double clicking the panel opens the add ticker form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void flowLayoutPanelSymbols_DoubleClick(object sender, EventArgs e)
        {
            addTickerToolStripMenuItem_Click(sender, e);
        }

        /// <summary>
        /// Draws the selected symbol chart
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            return;
            // TODO: Remove this method

            int width = pictureBox1.Width;
            int height = pictureBox1.Height;
            var chart = new ScottPlot.Plot(width, height);
            chart.Style(ScottPlot.Style.Gray1);

            // Set the x-axis to display hours from 9 to 5 (technically 9:30 to 4)
            string[] xLabels = { "9:00", "10:00", "11:00", "12:00", "1:00", "2:00", "3:00", "4:00", "5:00" }; 
            double[] xPositions = { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            chart.XAxis.ManualTickPositions(xPositions, xLabels);
            chart.YAxis.Label("Price");
            chart.YAxis.TickLabelStyle(fontSize: 10);


            // Generate some random data
            Random rand = new Random();
            double[] prices = DataGen.RandomWalk(rand, 12 * 60, 100);
            double[] times = DataGen.Consecutive(12 * 60, 1.0 / 60);

            // Plot the data
            chart.PlotScatter(times, prices, lineWidth: 2, color: Color.FromArgb(46, 192, 79));
            pictureBox1.Image = chart.GetBitmap();
        }

        private void AddSymbolPanel(string symbol, string source)
        {
            // Check if the placeholder text is still visible and remove it if so
            if (flowLayoutPanelSymbols.Controls.Count == 1 && flowLayoutPanelSymbols.Controls[0].Text.Contains("Double-click"))
            {
                flowLayoutPanelSymbols.Controls.RemoveAt(0);
            }            

            // Add a new panel
            int width = flowLayoutPanelSymbols.Width;
            int height = 100;
            SymbolPanel panel = new SymbolPanel(symbol, source);
            panel.Size = new Size(width, height);
            panel.Click += new EventHandler(SymbolPanel_Click);

            // If the source is not already in the list, add it and start a new engine.
            if (!marketEngines.Any(x => x.Source == source))
            {
                IMarketEngine marketEngine = MarketEngineFactory.CreateMarketEngine(source);
                marketEngines.Add(marketEngine);
                marketEngine.Subscribe(SymbolPanel_Update);
                marketEngine.Start();
            }
        
            // Check if the symbol is already in the list
            if (!flowLayoutPanelSymbols.Controls.OfType<SymbolPanel>().Any(x => x.Symbol == symbol))
            {
                // Get the market engine for this source
                IMarketEngine marketEngine = marketEngines.First(x => x.Source == source);
                (marketEngine as StdMarketEngine).AddSymbol(symbol);
            }

            flowLayoutPanelSymbols.Controls.Add(panel);
        }

        /// <summary>
        /// Updates the symbol panels with the latest data
        /// </summary>
        private void SymbolPanel_Update(Dictionary<string, TickerPoint> e)
        {
            // Update the text price
            foreach (SymbolPanel panel in flowLayoutPanelSymbols.Controls)
            {
                if (e.ContainsKey(panel.Symbol))
                {
                    panel.Invoke((MethodInvoker)delegate
                    {
                        // Running on the UI thread
                        panel.UpdatePrice(e[panel.Symbol].Price.ToString("C2"));

                    });
                }
            }

            // Update the chart
            pictureBox1.Invoke((MethodInvoker)delegate
            {
                // Running on the UI thread
                pictureBox1_Update();
            });
        }

        /// <summary>
        /// Updates the symbol panels with the latest data.
        /// Note that this method is very dirty and should would definitely be refactored with more time.
        /// </summary>
        private void pictureBox1_Update()
        {
            int width = pictureBox1.Width;
            int height = pictureBox1.Height;
            var chart = new ScottPlot.Plot(width, height);
            chart.Style(ScottPlot.Style.Gray1);

            // Try to get the first panel where Selected() is true. If none exists, return.
            SymbolPanel panel = flowLayoutPanelSymbols.Controls.OfType<SymbolPanel>().FirstOrDefault(x => x.Selected());
            if (panel == null)
            {
                return;
            }

            IMarketEngine marketEngine = marketEngines.First(x => x.Source == panel.Source);
            var data = (marketEngine as StdMarketEngine).GetMarketData(panel.Symbol);
            // If the data is null or empty, return
            if (data == null || data.Count == 0)
            {
                return;
            }

            // Disable autoaxis 
            chart.AxisAuto(0, 0);


            // Set the limits of the chart so x is 60*12 with labels from 9am to 4pm in increments of 10 minutes
            double[] positions = new double[8];
            string[] labels = new string[8];
            for (int i = 0; i < 8; i++)
            {
                positions[i] = i * 60;

                // Hacky way to Set the label to the corresponding am/pm time
                labels[i] = (9 + i).ToString() + ":00 am";
                if (i > 3)
                {
                    labels[i] = (i - 3).ToString() + ":00 pm";
                }
                
            }
            chart.XAxis.ManualTickPositions(positions, labels);

            // Zoom out to see the whole day & dynamically resize the chart (sort of)
            double start = (double)data[0].Price;
            double end = (double)data[data.Count - 1].Price;
            double max = (double)data.Max(x => x.Price);
            double min = (double)data.Min(x => x.Price);
            chart.SetAxisLimits(0, 7*60, Math.Min(start * 0.99,min), Math.Max(start * 1.01, max));

            // Set y intervals to 0.1
            chart.YAxis.ManualTickSpacing(0.1);

            double[] prices = data.Select(x => (double)x.Price).ToArray();
            double[] times = DataGen.Consecutive(prices.Length, 1.0);

            // Plot the data
            Color color = start < end ? Colors.POSITIVE_GREEN : Colors.NEGATIVE_RED;
            chart.PlotScatter(times, prices, lineWidth: 2, color: color);
            chart.PlotHLine(start, color: Color.Black, lineStyle: LineStyle.Dash, lineWidth: 2);

            pictureBox1.Image = chart.GetBitmap();
        }

        /// <summary>
        /// Handles the click event for the symbol panels
        /// </summary>
        private void SymbolPanel_Click(object sender, EventArgs e)
        {
            // Change the color of the panel to indicate it is selected
            SymbolPanel panel = (SymbolPanel)sender;
            panel.BackColor = Color.FromArgb(52, 50, 56);

            // Change the color of the other panels to indicate they are not selected
            foreach (Control control in flowLayoutPanelSymbols.Controls)
            {
                if (control != panel)
                {
                    control.BackColor = Colors.FOREGROUND_GRAY;
                }
            }

            pictureBox1_Update();
        }

        /// <summary>
        /// Adds a symbol to the watchlist 
        /// </summary>
        /// <param name="symbol">symbol to add</param>
        /// <param name="source">source to get the symbol from</param>
        public static void AddSymbol(string symbol, string source)
        {
            // This approach is used to save time and avoid having to use a delegate
            Instance.AddSymbolPanel(symbol, source);
        }

        /// <summary>
        /// Opens the sources configuration options.
        /// 
        /// IMPORTANT: This method takes a shortcut to auto-configure the sources for the sake of time.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sourcesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Check if sender is the sources menu item
            if (sender is ToolStripMenuItem && ((ToolStripMenuItem)sender).Name == "sourcesToolStripMenuItem")
            {
                MessageBox.Show("Sources have now been auto-configured for sake of simplicity.", "Sources");
            }

            // Add the sources to the configuration file if they don't exist
            if (ConfigurationManager.AppSettings["Sources"] == null)
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings.Add("Sources", "NASDAQ;NYSE;AMEX");
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
        }

        /// <summary>
        /// Handles the close event of the form.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Reset the appsetting sources for repeatable demo purposes.
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Remove("Sources");
            config.Save(ConfigurationSaveMode.Modified);
        }

        /// <summary>
        /// FOR DEMO PURPOSES ONLY
        /// 
        /// Sets the application into a functioning state immediately.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void executeDebugModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sourcesToolStripMenuItem_Click(sender, e);

            // Add some symbols to the watchlist
            AddSymbol("AAPL", "NASDAQ");
            AddSymbol("MSFT", "NASDAQ");
            AddSymbol("AMZN", "NASDAQ");
            AddSymbol("GOOG", "NASDAQ");
            AddSymbol("FB", "NASDAQ");
        }

        /// <summary>
        /// Clears the list of current tickers.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearTickersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            flowLayoutPanelSymbols.Controls.Clear();

            // Clear the chart
            pictureBox1.Image = null;
        }
    }
}