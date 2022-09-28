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

        /// <summary>
        /// The market engines used to generate simulated data.
        /// </summary>
        private static List<IMarketEngine> marketEngines = new();

        /// <summary>
        /// Constructor for the main form.
        /// </summary>
        public FormMain()
        {
            Instance = this;
            InitializeComponent();
            this.FormClosed += new FormClosedEventHandler(MainWindow_FormClosed);
        }

        /// <summary>
        /// Displays application creator information
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
        private void SymbolPanel_Update(Dictionary<string, InstrumentDataTick> e)
        {
            // Update the text price
            foreach (SymbolPanel panel in flowLayoutPanelSymbols.Controls)
            {
                if (e.ContainsKey(panel.Symbol))
                {
                    panel.Invoke((MethodInvoker)delegate
                    {
                        panel.UpdatePrice(e[panel.Symbol].Price.ToString("C2"));

                    });
                }
            }

            // Update the chart
            pictureBoxPlot.Invoke((MethodInvoker)delegate
            {
                pictureBox1_Update();
            });
        }

        /// <summary>
        /// Called when the user selects a new symbol from the panel
        /// </summary>
        /// <param name="sender">Used to identify the panel that triggered this event.</param>
        private void SymbolPanel_Click(object sender, EventArgs e)
        {
            // Change the color of the panel to indicate it is selected
            SymbolPanel panel = (SymbolPanel)sender;
            panel.BackColor = Colors.SELECTED_GRAY;

            // Change the color of the other panels to indicate they are not selected
            foreach (Control control in flowLayoutPanelSymbols.Controls)
            {
                if (control != panel)
                {
                    control.BackColor = Colors.FOREGROUND_GRAY;
                }
            }

            // Update the plot
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
        /// Helper method for configuration of a Scottplot.
        /// </summary>
        /// <param name="plot">The plot to modify</param>
        /// <param name="data">The data used to dynamically configure the chart</param>
        private void ConfigurePlot(ref Plot plot, List<InstrumentDataTick> data)
        {
            // Basic Styling
            plot.Style(ScottPlot.Style.Gray1);
            plot.XAxis.TickLabelStyle(fontSize: 30);
            plot.XAxis.Label("Time");
            plot.YAxis.Label("Price");
            plot.YAxis.TickLabelStyle(fontSize: 30);

            // Set Axis Limits (NYSE Hours)
            plot.AxisAuto(0, 0);
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
            plot.XAxis.ManualTickPositions(positions, labels);
            plot.YAxis.ManualTickSpacing(0.1);

            // Frame the plot
            double start = (double)data[0].Price;
            double end = (double)data[data.Count - 1].Price;
            double max = (double)data.Max(x => x.Price);
            double min = (double)data.Min(x => x.Price);
            plot.SetAxisLimits(0, 7*60, Math.Min(start * 0.99,min), Math.Max(start * 1.01, max));

            // Plot the data
            double[] prices = data.Select(x => (double)x.Price).ToArray();
            double[] times = DataGen.Consecutive(prices.Length, 1.0);
            Color color = start < end ? Colors.POSITIVE_GREEN : Colors.NEGATIVE_RED;
            plot.PlotScatter(times, prices, lineWidth: 2, color: color);
            plot.PlotHLine(start, color: Color.Black, lineStyle: LineStyle.Dash, lineWidth: 2);
        }

        /// <summary>
        /// Updates the plot image with the latest data.
        /// Note that this method is very dirty and should would definitely be refactored with more time.
        /// </summary>
        private void pictureBox1_Update()
        {
            int width = pictureBoxPlot.Width;
            int height = pictureBoxPlot.Height;
            var plot = new ScottPlot.Plot(width, height);
    
            // Get the data based on the selected symbol
            SymbolPanel panel = flowLayoutPanelSymbols.Controls.OfType<SymbolPanel>().FirstOrDefault(x => x.Selected());
            if (panel == null)
            {
                return;
            }
            IMarketEngine marketEngine = marketEngines.First(x => x.Source == panel.Source);
            var data = (marketEngine as StdMarketEngine).GetMarketData(panel.Symbol);
            if (data == null || data.Count == 0)
            {
                return;
            }

            // Generate & print the plot
            ConfigurePlot(ref plot, data);
            pictureBoxPlot.Image = plot.GetBitmap();
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
            pictureBoxPlot.Image = null;
        }
    }
}