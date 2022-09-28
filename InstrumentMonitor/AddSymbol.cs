using System.Configuration;

namespace InstrumentMonitor
{
    /// <summary>
    /// Window for adding a new symbol to the list.
    /// </summary>
    public partial class AddSymbol : Form
    {
        string[] sources;

        public AddSymbol()
        {
            InitializeComponent();
            LoadSources();
        }

        /// <summary>
        /// Initializes the form with the list of available data sources.
        /// </summary>
        private void LoadSources(){
            // Check that the configuration option is not null.
            if (ConfigurationManager.AppSettings["Sources"] != null)
            {
                listBox1.Items.Clear();
                sources = ConfigurationManager.AppSettings["Sources"].Split(';');
                listBox1.Items.AddRange(sources);
                listBox1.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Closes the window without adding the symbol to the watchlist
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonClose_Click(object sender, EventArgs e)
        {
            // No need to confirm, just close the window
            this.Close();
        }

        /// <summary>
        /// Adds the input symbol to the watchlist.
        /// 
        /// IMPORTANT: Does not verify this symbol is real for this simulation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            // Ensure the source selection is valid
            if (listBox1.SelectedIndex == -1 || listBox1.Items[0].ToString() == "No Sources")
            {
                MessageBox.Show("Please configure sources in Preferences (Edit>Sources).", "Error");
                return;
            }

            // Get the source from the listbox
            string symbol = textBoxSymbol.Text;
            string source = listBox1.SelectedItem.ToString();
            FormMain.AddSymbol(symbol.ToUpper(), source);

            this.Close();
        }
    }
}
