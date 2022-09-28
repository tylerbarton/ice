using InstrumentMonitor.Utils;

namespace InstrumentMonitor
{
    /// <summary>
    /// Represents a ticker watchlist item
    /// </summary>
    internal class SymbolPanel : Panel
{
        public string Symbol { get; set; }
        public string Source { get; set; }

        private Label _symbolLabel;
        private Label _priceLabel;

        /// <summary>
        /// Creates a new <see cref="SymbolPanel"/> instance.
        /// </summary>
        public SymbolPanel(string symbol, string source)
        {
            Symbol = symbol;
            Source = source; 
            InitializeComponent();
        }

        /// <summary>
        /// Paints the panel.
        /// </summary>
        private void InitializeComponent()
        {
            this.BackColor = Colors.FOREGROUND_GRAY;
            
            AddSymbolLabel(Symbol);
            AddPriceLabel("0.00");
        }

        /// <summary>
        /// Adds the symbol to the left side of the panel.
        /// </summary>
        /// <param name="symbol">symbol to add</param>
        public void AddSymbolLabel(string symbol)
        {
            _symbolLabel = new Label();
            _symbolLabel.Text = Symbol;
            _symbolLabel.Location = new Point(0, 0);
            _symbolLabel.AutoSize = true;
            _symbolLabel.Font = new Font("Arial", 12, FontStyle.Bold);
            _symbolLabel.ForeColor = Color.White;
            _symbolLabel.BackColor = Color.Transparent;
            _symbolLabel.Enabled = false;
            this.Controls.Add(_symbolLabel);
        }


        /// <summary>
        /// Adds or updates the price label to the right side of the panel.
        /// </summary>
        public void AddPriceLabel(string price)
        {
            if(_priceLabel == null)
            {
                _priceLabel = new Label();
                _priceLabel.Anchor = AnchorStyles.Right;
                _priceLabel.Location = new Point(0, 0);
                _priceLabel.AutoSize = true;
                _priceLabel.Font = new Font("Arial", 10, FontStyle.Bold);
                _priceLabel.ForeColor = Color.White;
                _priceLabel.BackColor = Color.Transparent;
                _priceLabel.Enabled = false;
                this.Controls.Add(_priceLabel);
            } 
            _priceLabel.Text = price;
        }

        /// <summary>
        /// Helper method to update the price label.
        /// </summary>
        /// <param name="price">price to update</param>
        public void UpdatePrice(string price)
        {
            _priceLabel.Text = price;
        }

        /// <summary>
        /// Returns true if this panel is selected by the user.
        /// </summary>
        /// <returns>true if the panel is selected</returns>
        public Boolean Selected(){
            return this.BackColor == Colors.SELECTED_GRAY;
        }
    }
}
